<template>
  <q-page class="q-pa-md">
    <q-form @submit.prevent="submitExpense">
      <DateInput v-model="selectedDate" />

      <q-select
        v-model="selectedMainCategory"
        :options="mainCategories"
        label="大ぶんるい"
        option-value="value"
        option-label="label"
        filled
        class="q-mb-md"
      />

      <q-select
        v-model="selectedSubCategory"
        :options="currentSubCategories"
        label="小ぶんるい"
        option-value="value"
        option-label="label"
        filled
        :disable="selectedMainCategory === null"
        class="q-mb-md"
      />

      <MoneyInput
        v-model="amount"
        class="q-mb-md"
      />

      <q-input
        v-model="memo"
        filled
        autogrow
        label="メモ"
        class="q-mb-md"
      />

      <q-btn
        label="ついかする"
        type="submit"
        color="primary"
      />
    </q-form>
  </q-page>
</template>

<script setup lang="ts">
import { computed, ref, watch } from 'vue';
import MoneyInput from '../components/forms/MoneyInput.vue';
import DateInput from '../components/forms/DateInput.vue';
type CategoryOption = { label: string; value: number };

const mainCategories: CategoryOption[] = [
  { label: '食費', value: 0 },
  { label: '生活費', value: 1 },
  { label: 'その他', value: 2 },
];

const subCategoryOptions: Record<number, CategoryOption[]> = {
  0: [
    { label: '未分類', value: 0 },
    { label: '食材費', value: 1 },
    { label: '外食費', value: 2 },
  ],
  1: [
    { label: '未分類', value: 0 },
    { label: '日用品', value: 1 },
    { label: '衣類', value: 2 },
  ],
  2: [
    { label: '未分類', value: 0 },
    { label: '嗜好品', value: 1 },
    { label: '家賃', value: 2 },
    { label: '光熱費', value: 3 },
    { label: '水道料金', value: 4 },
  ],
};

const selectedDate = ref('');
const selectedMainCategory = ref<CategoryOption | null>(null);
const selectedSubCategory = ref<CategoryOption | null>(null);
const amount = ref<number | null>(null);
const memo = ref<string>('');

function submitExpense() {
  console.log({
    selectedDate: selectedDate,
    selectedMainCategory: selectedMainCategory.value,
    selectedSubCategory: selectedSubCategory.value,
    amount: amount.value,
    memo: memo.value,
  });
}

const currentSubCategories = computed(() => {
  const key = selectedMainCategory.value?.value;
  return typeof key === 'number' ? subCategoryOptions[key] : [];
});

watch(selectedMainCategory, (val) => {
  if (val !== null && currentSubCategories.value != null && currentSubCategories.value.length > 0) {
    selectedSubCategory.value = currentSubCategories.value[0] as CategoryOption;
  } else {
    selectedSubCategory.value = null;
  }
});

const today = new Date().toISOString().slice(0, 10); // e.g. "2025-06-29"
selectedDate.value = today;
</script>
