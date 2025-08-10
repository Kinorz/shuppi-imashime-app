using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShuppiApi.Data;
using ShuppiApi.Services;
using System.Diagnostics;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ===== CORS =====
// ALLOWED_ORIGINS="https://xxx.pages.dev,https://your.domain"
var origins = (builder.Configuration["ALLOWED_ORIGINS"] ?? "")
  .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

builder.Services.AddCors(o =>
{
    if (origins.Length > 0)
    {
        o.AddPolicy("pages",
            p => p.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod());
    }
    else
    {
        // ローカル開発用（compose時）
        o.AddPolicy("pages",
            p => p.SetIsOriginAllowed(_ => true).AllowAnyHeader().AllowAnyMethod());
    }
});

// ===== DB =====
string? cs =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? builder.Configuration.GetConnectionString("Default")
    ?? builder.Configuration["ConnectionStrings:DefaultConnection"]
    ?? builder.Configuration["ConnectionStrings:Default"];

if (string.IsNullOrWhiteSpace(cs))
    throw new InvalidOperationException("DB connection string not found.");

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseNpgsql(cs, o => o.CommandTimeout(60));
});

// ===== AuthN/AuthZ =====
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = builder.Configuration["JWT_ISSUER"],
            ValidAudience = builder.Configuration["JWT_AUDIENCE"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JWT_SECRET"]!))
        };
    });

builder.Services.AddAuthorization();

// ===== MVC / Swagger =====
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<TokenService>();

var app = builder.Build();

// 逆プロキシ(X-Forwarded-*)対応（Render/Cloudflare経由）
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedFor
});

// ★ perf: 全リクエストの総処理時間を記録
app.Use(async (ctx, next) =>
{
    var sw = Stopwatch.StartNew();
    await next();
    ctx.Response.OnCompleted(() =>
    {
        var log = ctx.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("Perf");
        log.LogInformation(
            "req {Method} {Path} status={Status} total={Total}ms trace={TraceId}",
            ctx.Request.Method, ctx.Request.Path, ctx.Response.StatusCode,
            sw.ElapsedMilliseconds, ctx.TraceIdentifier);
        return Task.CompletedTask;
    });
});

// DevだけSwaggerとHTTPSリダイレクト
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
}

// CORS
app.UseCors("pages");

// Auth
app.UseAuthentication();
app.UseAuthorization();

// ヘルスチェック
app.MapGet("/healthz", () => Results.Ok("ok"));

app.MapControllers();

app.Run();
