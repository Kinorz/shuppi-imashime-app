import { defineStore } from 'pinia';
import { ref } from 'vue';
import { api } from 'boot/axios';

export interface Category {
  id: number;
  name: string;
  color: string;
  icon: string;
}

export const useCategoryStore = defineStore('category', () => {
  const categories = ref<Category[]>([]);

  async function fetchCategories() {
    if (categories.value.length > 0) return; // すでに取得済み
    try {
      const response = await api.get('/categories');
      categories.value = response.data;
    } catch (error) {
      console.error('カテゴリの取得に失敗しました', error);
    }
  }

  return { categories, fetchCategories };
});
