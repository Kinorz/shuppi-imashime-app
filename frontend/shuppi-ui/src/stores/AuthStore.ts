import { defineStore } from 'pinia';

export const useAuthStore = defineStore('auth', {
  state: () => ({
    token: null as string | null,
  }),
  actions: {
    setToken(token: string) {
      this.token = token;
      localStorage.setItem('token', token);
    },
    loadToken() {
      const token = localStorage.getItem('token');
      if (token) this.token = token;
    },
    logout() {
      this.token = null;
      localStorage.removeItem('token');
    },
  },
});
