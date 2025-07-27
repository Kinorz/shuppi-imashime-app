import type { RouteRecordRaw } from 'vue-router';

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    component: () => import('layouts/MainLayout.vue'),
    children: [{ path: '', component: () => import('pages/TopPage.vue') }],
    meta: { requiresAuth: true },
  },
  {
    path: '/compose',
    component: () => import('layouts/BlankLayout.vue'),
    children: [{ path: 'expense', component: () => import('pages/ExpenseFormPage.vue') }],
    meta: { requiresAuth: true },
  },
  {
    path: '/login',
    component: () => import('layouts/BlankLayout.vue'),
    children: [{ path: '', component: () => import('pages/LoginForm.vue') }],
  },
  // Always leave this as last one,
  // but you can also remove it
  {
    path: '/:catchAll(.*)*',
    component: () => import('pages/ErrorNotFound.vue'),
  },
];

export default routes;
