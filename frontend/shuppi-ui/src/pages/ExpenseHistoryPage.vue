<!-- src/pages/ExpenseHistoryPage.vue -->
<template>
  <q-page class="q-pa-md">
    <q-select
      v-model="categoryId"
      :options="categoryOptions"
      option-value="id"
      option-label="name"
      emit-value
      map-options
      clearable
      label="カテゴリーで絞り込み"
      class="q-mb-md"
      @update:model-value="reload"
    />
    <q-list
      bordered
      separator
    >
      <q-item
        v-for="e in items"
        :key="e.id"
      >
        <q-item-section>
          <div class="text-subtitle1">
            {{ nameOf(e.categoryId) }} ¥{{ e.amount.toLocaleString() }}
          </div>
          <div class="text-caption text-grey-7">{{ e.date.slice(0, 10) }} {{ e.note }}</div>
        </q-item-section>
      </q-item>
    </q-list>

    <q-infinite-scroll
      @load="loadMore"
      :offset="100"
      v-if="hasMore"
    >
      <div class="text-center q-my-md">読み込み中…</div>
    </q-infinite-scroll>
  </q-page>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { api } from 'boot/axios';
import { useCategoryStore } from 'src/stores/CategoryStore';
import { computed } from 'vue';

interface ExpenseItem {
  id: number;
  amount: number;
  date: string;
  categoryId: number;
  note?: string;
}
interface PageDto {
  items: ExpenseItem[];
  hasMore: boolean;
}

const route = useRoute();
const router = useRouter();

const items = ref<ExpenseItem[]>([]);
const page = ref(1);
const pageSize = 20;
const hasMore = ref(true);
const isLoading = ref(false);

const selectedTagIds = ref<number[]>([]);

const categoryStore = useCategoryStore();
const categoryNameMap = computed(() =>
  Object.fromEntries(categoryStore.categories.map((c) => [c.id, c.name])),
);
const nameOf = (id: number) => categoryNameMap.value[id] ?? '未分類';

const categoryId = ref<number | null>(
  route.query.categoryId ? Number(route.query.categoryId) : null,
);

const categoryOptions = computed(() =>
  categoryStore.categories.map((c) => ({ id: c.id, name: c.name })),
);

const fetchPage = async () => {
  if (isLoading.value || !hasMore.value) return;
  isLoading.value = true;
  try {
    const { data } = await api.get<PageDto>('expenses', {
      params: {
        page: page.value,
        pageSize,
        categoryId: categoryId.value ?? undefined,
        tagIds: selectedTagIds.value.length ? selectedTagIds.value : undefined,
      },
    });

    const exists = new Set(items.value.map((i) => i.id));
    const dedup = data.items.filter((i) => !exists.has(i.id));

    items.value.push(...dedup);
    hasMore.value = data.hasMore;
  } finally {
    isLoading.value = false;
  }
};

const loadMore = async (_: number, done: () => void) => {
  await fetchPage();
  page.value++;
  done();
};

const reload = async () => {
  await router.replace({
    path: '/expenses',
    query: categoryId.value ? { categoryId: String(categoryId.value) } : {},
  });
  items.value = [];
  page.value = 1;
  hasMore.value = true;
  await fetchPage();
  page.value++;
};

watch(
  () => route.query.categoryId,
  async (q) => {
    categoryId.value = q ? Number(q) : null;
    await router.replace({
      path: '/expenses',
      query: categoryId.value ? { categoryId: String(categoryId.value) } : {},
    });
    items.value = [];
    page.value = 1;
    hasMore.value = true;
    await fetchPage();
    page.value++;
  },
  { immediate: true },
);
</script>
