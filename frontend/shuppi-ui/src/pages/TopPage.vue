<!-- src/pages/TopPage.vue -->
 <template>
  <q-page class="q-pa-md">
    <h1>Shuppiへようこそ！</h1>
    <p>これはあなたの支出を振り返るアプリです。</p>
    <div class="text-h6">天気予報</div>
    <q-list bordered separator>
      <q-item v-for="(item, index) in forecast" :key="index">
        <q-item-section>
          <div>{{ item.date }} - {{ item.summary }}（{{ item.temperatureC }}℃）</div>
        </q-item-section>
      </q-item>
    </q-list>
  </q-page>
 </template>

 <script setup lang="ts">
import { ref, onMounted } from 'vue';
import { api } from 'boot/axios';

interface WeatherForecast {
  date: string;
  temperatureC: number;
  summary: string
}

const forecast = ref<WeatherForecast[]>([]);

onMounted(async () => {
  try {
    const response = await api.get<WeatherForecast[]>('/weatherforecast');
    forecast.value = response.data;
  } catch (error) {
    console.error('API fetch error:', error);
  }
});
</script>