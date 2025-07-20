<template>
  <router-view />
</template>

<script setup lang="ts">
import { onMounted } from 'vue';
import { useCategoryStore } from 'src/stores/useCategoryStore';

const categoryStore = useCategoryStore();

// カテゴリ初期化
onMounted(async () => {
  await categoryStore.fetchCategories();
});
</script>
