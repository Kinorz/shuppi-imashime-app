import { defineStore } from 'pinia';
import { api } from 'boot/axios';

export interface Tag {
  id: number;
  name: string;
  isHidden: boolean;
  categoryIds: number[];
}

export const useTagStore = defineStore('tag', {
  state: () => ({
    tags: [] as Tag[],
  }),
  actions: {
    async fetchTags() {
      const res = await api.get('/tags'); // ユーザーのタグ取得
      this.tags = res.data;
    },
    getTagsByCategory(categoryId: number) {
      return this.tags.filter((tag) => !tag.isHidden && tag.categoryIds.includes(categoryId));
    },
  },
});
