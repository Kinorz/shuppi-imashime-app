<template>
  <q-page class="q-pa-md">
    <q-form @submit.prevent="submitExpense">
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

const amount = ref<number | null>(null);
const category = ref('');
const categories = ['食費', '交通費', '趣味', '交際費', 'その他'];

function submitExpense() {
  console.log({
    category: category.value,
    amount: amount.value,
  });
}
</script>
