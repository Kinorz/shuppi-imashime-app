<template>
  <q-page class="q-pa-md">
    <q-header :style="{ backgroundColor: category?.color, color: 'white' }">
      <q-toolbar>
        <q-btn
          dense
          flat
          icon="arrow_back"
          size="md"
          @click="goToTop"
        />
        <q-icon
          :name="category?.icon"
          size="md"
        ></q-icon>
        <q-toolbar-title>{{ category?.name }}</q-toolbar-title>
      </q-toolbar>
      <q-separator />
    </q-header>

    <q-form
      class="q-px-md"
      @submit.prevent="submitExpense"
    >
      <div class="row justify-center">
        <DateInput v-model="selectedDate" />
      </div>
      <div class="row justify-center">
        <q-space />
        <MoneyInput
          ref="moneyInputRef"
          v-model="amount"
          class="q-mb-md col-6"
        />
        <q-space />
      </div>

      <div class="q-mt-md row justify-center">
        <q-btn
          label="記録する"
          type="submit"
          :style="{ backgroundColor: category?.color, color: 'white' }"
        />
      </div>
    </q-form>
  </q-page>
</template>

<script setup lang="ts">
import { computed, nextTick, onMounted, ref } from 'vue';
// import { Notify, useQuasar } from 'quasar';
import MoneyInput from '../components/forms/MoneyInput.vue';
import DateInput from '../components/forms/DateInput.vue';
import { useRoute, useRouter } from 'vue-router';
import { useCategoryStore } from 'src/stores/useCategoryStore';
// import { api } from 'src/boot/axios';

const route = useRoute();
const router = useRouter();
const selectedCategoryId = ref<number | null>(null);
const categoryStore = useCategoryStore();
const selectedDate = ref('');
const amount = ref<number>(0);
const moneyInputRef = ref<{ focus: () => void } | null>(null);

// 初期表示
onMounted(() => {
  const id = route.query.categoryId;
  selectedCategoryId.value = id ? parseInt(id as string, 10) : null;
  void nextTick(() => {
    void nextTick(() => {
      moneyInputRef.value?.focus();
    });
  });
});

// カテゴリを取得する計算プロパティ
const category = computed(() => {
  return categoryStore.categories.find((c) => c.id === selectedCategoryId.value);
});

// トップページへ遷移
function goToTop() {
  router.push('/').catch((err) => {
    console.error('Navigation failed:', err);
  });
}

const today = new Date().toISOString().slice(0, 10); // e.g. "2025-06-29"
selectedDate.value = today;

// async function submitExpense() {
function submitExpense() {
  if (!selectedCategoryId.value || !selectedDate.value || amount.value == null) {
    console.warn('未入力項目があります');
    return;
  }

  const expenseData = {
    categoryId: selectedCategoryId.value,
    date: selectedDate.value,
    amount: amount.value,
    tags: [], // 今後追加予定
  };

  try {
    // await api.post('/expenses', expenseData);
    console.log('登録内容', expenseData);
    goToTop();
  } catch (error) {
    console.error('登録に失敗しました', error);
  }
}
</script>
