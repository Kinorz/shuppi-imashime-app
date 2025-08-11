import type { RouteRecordRaw } from 'vue-router';

const routes: RouteRecordRaw[] = [
  {
    path: '/login',
    component: () => import('layouts/BlankLayout.vue'),
    children: [{ path: '', component: () => import('pages/LoginForm.vue') }],
  },
  {
    path: '/',
    component: () => import('layouts/MainLayout.vue'),
    children: [
      { path: '', component: () => import('pages/TopPage.vue') },
      { path: 'expenses', component: () => import('pages/ExpenseHistoryPage.vue') },
      { path: 'settings', component: () => import('pages/SettingsPage.vue') },
    ],
    meta: { requiresAuth: true },
  },
  {
    path: '/compose',
    component: () => import('layouts/MainLayout.vue'),
    children: [{ path: 'expense', component: () => import('pages/ExpenseFormPage.vue') }],
    meta: { requiresAuth: true },
  },
  // Always leave this as last one,
  // but you can also remove it
  {
    path: '/:catchAll(.*)*',
    component: () => import('pages/ErrorNotFound.vue'),
  },
];

export default routes;
