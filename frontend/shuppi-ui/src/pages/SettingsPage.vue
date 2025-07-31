<template>
  <q-page class="q-pa-md">
    <div class="text-h6 q-mb-md">設定</div>
    <q-btn
      label="ログアウト"
      color="negative"
      @click="logout"
    />
  </q-page>
</template>

<script setup lang="ts">
import { useAuthStore } from 'src/stores/useAuthStore';
import { useRouter } from 'vue-router';

const authStore = useAuthStore();
const router = useRouter();

const logout = () => {
  authStore.logout();
  router.push('/login').catch((err) => {
    console.error('Navigation failed:', err);
  });
};
</script>
