using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ShuppiApi.Data;
using ShuppiApi.Services;

var builder = WebApplication.CreateBuilder(args);

// CORS をサービスに追加
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .SetIsOriginAllowed(origin => true) // デバッグ用にすべてのOriginを許可
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// サービス登録
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext をサービスに追加
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddSingleton<TokenService>();

// 認証の設定を追加
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT_ISSUER"],
            ValidAudience = builder.Configuration["JWT_AUDIENCE"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JWT_SECRET"]!)
            )
        };
    });

// 認可（Authorize）を使えるように
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.

// 開発環境でのみ Swagger を有効にする
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// CORS を有効化
app.UseCors();

app.UseAuthentication();   // 認証
app.UseAuthorization();    // 認可

app.MapControllers(); 

app.Run();
