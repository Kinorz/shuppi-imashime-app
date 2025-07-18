import { defineStore } from 'pinia';

export const useCategoryStore = defineStore('category', () => {
  const categories = [
    { id: 1, name: '食費', color: '#f28b82', icon: 'sym_o_restaurant' },
    { id: 2, name: '日用品', color: '#fbbc04', icon: 'sym_o_shopping_basket' },
    { id: 3, name: '衣類・美容', color: '#c58af9', icon: 'sym_o_styler' },
    { id: 4, name: '娯楽費', color: '#81d4fa', icon: 'sym_o_sports_esports' },
    { id: 5, name: '固定費', color: '#9aa0a6', icon: 'sym_o_home' },
    { id: 6, name: 'その他', color: '#d7ccc8', icon: 'sym_o_category' },
  ];

  return { categories };
});
