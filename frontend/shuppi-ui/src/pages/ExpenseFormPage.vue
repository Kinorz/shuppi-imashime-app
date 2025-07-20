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

    <q-form class="q-px-md">
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
const amount = ref<number | null>(null);
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

// async function onDialogShow() {
//   await nextTick();
//   moneyInputRef.value?.focus();
// }

// function openDialogAndFocus() {
//   void nextTick(() => {
//     void nextTick(() => {
//       moneyInputRef.value?.focus();
//     });
//   });
// }

// type CategoryOption = { label: string; value: number };
// const $q = useQuasar();

// const mainCategories: CategoryOption[] = [
//   { label: '食費', value: 0 },
//   { label: '生活費', value: 1 },
//   { label: 'その他', value: 2 },
// ];

// const subCategoryOptions: Record<number, CategoryOption[]> = {
//   0: [
//     { label: '未分類', value: 0 },
//     { label: '食材', value: 1 },
//     { label: '外食', value: 2 },
//   ],
//   1: [
//     { label: '未分類', value: 0 },
//     { label: '日用品', value: 1 },
//     { label: '衣類', value: 2 },
//   ],
//   2: [
//     { label: '未分類', value: 0 },
//     { label: '嗜好品', value: 1 },
//     { label: '家賃', value: 2 },
//     { label: '光熱費', value: 3 },
//     { label: '水道料金', value: 4 },
//   ],
// };

// const selectedDate = ref('');
// const selectedMainCategory = ref<CategoryOption | null>(null);
// const selectedSubCategory = ref<CategoryOption | null>(null);
// const amount = ref<number | null>(null);
// const memo = ref<string>('');

// async function submitExpense() {
//   if (
//     !selectedDate.value ||
//     !selectedMainCategory.value ||
//     !selectedSubCategory.value ||
//     !amount.value
//   ) {
//     $q.notify({
//       type: 'negative',
//       message: 'すべての必須項目を入力してください。',
//     });

//     return;
//   }

//   console.log({
//     selectedDate: selectedDate,
//     selectedMainCategory: selectedMainCategory.value,
//     selectedSubCategory: selectedSubCategory.value,
//     amount: amount.value,
//     memo: memo.value,
//   });

//   try {
//     const payload = {
//       date: selectedDate.value,
//       mainCategory: selectedMainCategory.value?.value,
//       subCategory: selectedSubCategory.value?.value,
//       amount: amount.value,
//       memo: memo.value,
//     };

//     await api.post('/expenses', payload);

//     // 成功したら通知やフォームリセットも検討
//     console.log('登録成功');

//     // $q.notify({
//     //   type: 'positive',
//     //   message: '登録しました',
//     // });
//     const dismiss = Notify.create({
//       group: false, // ← 個別にスタイルを指定したいなら false
//       type: 'positive',
//       message: '登録しました',
//       timeout: 0,
//       classes: 'fast-fade',
//       actions: [
//         {
//           label: 'OK',
//           color: 'white',
//           handler: () => {
//             dismiss();
//           },
//         },
//       ],
//     });
//   } catch (error) {
//     console.error('登録失敗:', error);
//     $q.notify({
//       type: 'negative',
//       message: '登録に失敗しました',
//     });
//   }
// }

// const currentSubCategories = computed(() => {
//   const key = selectedMainCategory.value?.value;
//   return typeof key === 'number' ? subCategoryOptions[key] : [];
// });

// watch(selectedMainCategory, (val) => {
//   if (val !== null && currentSubCategories.value != null && currentSubCategories.value.length > 0) {
//     selectedSubCategory.value = currentSubCategories.value[0] as CategoryOption;
//   } else {
//     selectedSubCategory.value = null;
//   }
// });

// const today = new Date().toISOString().slice(0, 10); // e.g. "2025-06-29"
// selectedDate.value = today;
</script>
