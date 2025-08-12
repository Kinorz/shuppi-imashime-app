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

      <q-input
        dense
        v-model="note"
        label="note"
        autogrow
        class="q-mb-md"
      />

      <q-btn
        v-for="tag in filteredTags"
        :key="tag.id"
        :label="`#${tag.name}`"
        @click="appendTagToText(tag.name)"
        size="sm"
        color="grey-4"
        text-color="grey-8"
        unelevated
        rounded
        class="q-mb-xs q-mr-xs"
      />

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
import { computed, nextTick, onMounted, ref, watch } from 'vue';
import MoneyInput from '../components/forms/MoneyInput.vue';
import DateInput from '../components/forms/DateInput.vue';
import { useRoute, useRouter } from 'vue-router';
import { useCategoryStore } from 'src/stores/CategoryStore';
import { useTagStore } from 'src/stores/TagStore';
import type { Tag } from 'src/stores/TagStore';
import { useExpenseApi } from 'src/composables/useExpenseApi';
import { ymd } from 'src/utils/date';

const route = useRoute();
const router = useRouter();
const selectedCategoryId = ref<number | null>(null);
const categoryStore = useCategoryStore();
const selectedDate = ref('');
const amount = ref<number>(0);
const moneyInputRef = ref<{ focus: () => void } | null>(null);
const tagStore = useTagStore();
const filteredTags = ref<Tag[]>([]);
const note = ref('');
const { createExpense } = useExpenseApi();

const appendTagToText = (name: string) => {
  const current = note.value
    .trim()
    .split(/\s+/)
    .map((t) => t.replace(/^#/, ''));
  if (!current.includes(name)) {
    note.value += (note.value.endsWith(' ') || note.value === '' ? '' : ' ') + `#${name}`;
  }
};

const parseTags = (): string[] => {
  return note.value
    .trim()
    .split(/\s+/)
    .filter((s) => s.startsWith('#'))
    .map((s) => s.slice(1))
    .filter((s) => s.length > 0);
};

watch(
  [selectedCategoryId, () => tagStore.tags],
  ([categoryId]) => {
    if (categoryId != null) {
      filteredTags.value = tagStore.getTagsByCategory(categoryId);
    } else {
      filteredTags.value = [];
    }
  },
  { immediate: true },
);

// 初期表示
onMounted(async () => {
  const id = route.query.categoryId;
  selectedCategoryId.value = id ? parseInt(id as string, 10) : null;
  void nextTick(() => {
    void nextTick(() => {
      moneyInputRef.value?.focus();
    });
  });
  // タグ取得
  await tagStore.fetchTags();
  if (selectedCategoryId.value !== null) {
    tagStore.getTagsByCategory(selectedCategoryId.value);
  }
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

selectedDate.value = ymd();

async function submitExpense() {
  if (!selectedCategoryId.value || !selectedDate.value || amount.value == null) {
    console.warn('未入力項目があります');
    return;
  }

  const expenseData = {
    categoryId: selectedCategoryId.value,
    date: selectedDate.value,
    amount: amount.value,
    tags: parseTags(),
    note: note.value,
  };

  try {
    await createExpense(expenseData);
    goToTop();
  } catch (error) {
    console.error('登録に失敗しました', error);
  }
}
</script>
