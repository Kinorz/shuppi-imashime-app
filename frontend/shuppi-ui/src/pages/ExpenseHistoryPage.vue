<!-- src/pages/ExpenseHistoryPage.vue -->
<template>
  <q-page>
    <div class="q-px-md q-py-sm sticky-filters">
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
        @update:model-value="onSelect"
      />

      <div class="row items-center q-mb-sm">
        <div class="col">
          <DateInput
            v-model="from"
            :isClearable="true"
          />
        </div>
        <div class="q-mx-xs">〜</div>
        <div class="col">
          <DateInput
            v-model="to"
            :isClearable="true"
          />
        </div>
      </div>

      <div class="text-right text-weight-bold">合計：¥{{ totalAmount.toLocaleString() }}</div>
    </div>

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
      :disable="isLoading || !hasMore"
      v-if="hasMore"
    >
      <div class="row justify-center q-my-md">
        <q-spinner-dots size="40px" />
      </div>
    </q-infinite-scroll>
  </q-page>
</template>

<style scoped>
.sticky-filters {
  position: sticky;
  top: 0;
  z-index: 1;
  background: rgba(255, 255, 255, 0.8);
  backdrop-filter: blur(6px);
  -webkit-backdrop-filter: blur(6px);
  border-bottom: 1px solid rgba(0, 0, 0, 0.06);
}
</style>

<script setup lang="ts">
import { ref, watch } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { api } from 'boot/axios';
import { useCategoryStore } from 'src/stores/CategoryStore';
import { computed } from 'vue';
import DateInput from 'src/components/forms/DateInput.vue';
import { ymd } from 'src/utils/date';

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
  totalAmount: number;
}

const route = useRoute();
const router = useRouter();

const items = ref<ExpenseItem[]>([]);
const totalAmount = ref(0);
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

// 期間指定
const today = new Date();
const first = new Date(today.getFullYear(), today.getMonth(), 1);
const last = new Date(today.getFullYear(), today.getMonth() + 1, 0);

const from = ref<string>(typeof route.query.from === 'string' ? route.query.from : ymd(first));
const to = ref<string>(typeof route.query.to === 'string' ? route.query.to : ymd(last));

const fetchPage = async (): Promise<boolean> => {
  if (isLoading.value || !hasMore.value) return false;
  isLoading.value = true;
  try {
    const { data } = await api.get<PageDto>('expenses', {
      params: {
        page: page.value,
        pageSize,
        categoryId: categoryId.value ?? undefined,
        tagIds: selectedTagIds.value.length ? selectedTagIds.value : undefined,
        from: from.value || undefined,
        to: to.value || undefined,
      },
    });
    totalAmount.value = data.totalAmount ?? 0;
    items.value.push(...data.items);
    hasMore.value = data.hasMore;
    return data.items.length > 0;
  } finally {
    isLoading.value = false;
  }
};

const loadMore = async (_: number, done: () => void) => {
  const ok = await fetchPage();
  if (ok) page.value++;
  done();
};

const onSelect = async (val: number | null) => {
  await router.replace({
    path: '/expenses',
    query: val ? { categoryId: String(val) } : {},
  });
};

watch(
  () => route.query.categoryId,
  async (q) => {
    categoryId.value = q ? Number(q) : null;
    items.value = [];
    page.value = 1;
    hasMore.value = true;
    const ok = await fetchPage();
    if (ok) page.value++;
  },
  { immediate: true },
);

// 期間変更で再読込
watch([from, to], async () => {
  // from > to のとき入れ替える
  if (from.value && to.value && from.value > to.value) {
    [from.value, to.value] = [to.value, from.value];
  }
  await router.replace({
    path: '/expenses',
    query: {
      ...(categoryId.value ? { categoryId: String(categoryId.value) } : {}),
      from: from.value,
      to: to.value,
    },
  });
  items.value = [];
  totalAmount.value = 0;
  page.value = 1;
  hasMore.value = true;
  const ok = await fetchPage();
  if (ok) page.value++;
});
</script>
