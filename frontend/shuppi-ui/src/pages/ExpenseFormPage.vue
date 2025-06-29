<template>
  <q-page class="q-pa-md">
    <q-form @submit.prevent="submitExpense">
      <DateInput
        v-model="selectedDate"
        label="日付"
      />

      <q-select
        v-model="category"
        label="カテゴリ"
        :options="categories"
        filled
        class="q-mb-md"
      />

      <MoneyInput
        v-model="amount"
        label="支出"
        class="q-mb-md"
      />

      <q-btn
        label="追加"
        type="submit"
        color="primary"
      />
    </q-form>
  </q-page>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import MoneyInput from '../components/forms/MoneyInput.vue';
import DateInput from '../components/forms/DateInput.vue';

const categories = ['食費', '生活費', 'その他'];

const selectedDate = ref('');
const category = ref('');
const amount = ref<number | null>(null);

function submitExpense() {
  console.log({
    selectedDate: selectedDate,
    category: category.value,
    amount: amount.value,
  });
}

const today = new Date().toISOString().slice(0, 10); // e.g. "2025-06-29"
selectedDate.value = today;
</script>
