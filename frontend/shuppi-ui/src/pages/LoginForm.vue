<template>
  <q-page class="q-pa-md flex flex-center">
    <q-card style="width: 400px">
      <q-card-section>
        <div class="text-h6">ログイン</div>
      </q-card-section>

      <q-card-section>
        <q-input
          v-model="email"
          label="メールアドレス"
          type="email"
        />
        <q-input
          v-model="password"
          label="パスワード"
          type="password"
        />
      </q-card-section>

      <q-card-actions align="right">
        <q-btn
          label="ログイン"
          color="primary"
          @click="login"
          :loading="loading"
        />
      </q-card-actions>
    </q-card>
  </q-page>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { api } from 'boot/axios';
import { useAuthStore } from 'src/stores/useAuthStore';

const email = ref('');
const password = ref('');
const loading = ref(false);
const router = useRouter();
const authStore = useAuthStore();

const login = async () => {
  loading.value = true;
  try {
    const response = await api.post('/users/login', {
      email: email.value,
      password: password.value,
    });
    const token = response.data.token;
    authStore.setToken(token);
    api.defaults.headers.common['Authorization'] = `Bearer ${token}`;
    router.push('/').catch((err) => {
      console.error('Navigation failed:', err);
    });
  } catch (err) {
    console.error('ログイン失敗', err);
    alert('ログインに失敗しました。');
  } finally {
    loading.value = false;
  }
};
</script>
