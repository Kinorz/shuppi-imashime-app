<!-- src/pages/TopPage.vue -->
<template>
  <q-page class="column no-scroll">
    <div class="row items-center justify-center q-pt-md q-py-sm">
      <q-btn
        dense
        flat
        icon="chevron_left"
        @click="moveMonth(-1)"
      />
      <div class="text-h6 q-mx-sm">
        {{ year }}年 {{ month }}月
        <span class="q-ml-sm">(¥{{ totalAmount.toLocaleString() }})</span>
      </div>
      <q-btn
        dense
        flat
        icon="chevron_right"
        @click="moveMonth(1)"
      />
    </div>

    <div class="col row no-wrap">
      <div
        class="column q-pa-sm"
        :style="leftStyle"
      >
        <!-- <div class="text-center">+¥200,000</div> -->
        <div class="text-center"></div>
        <div class="col row">
          <div class="bg-grey col-11"></div>
        </div>
      </div>
      <div
        class="column q-pa-sm"
        :style="rightStyle"
      >
        <!-- <div class="text-left">-¥55,000</div> -->
        <div class="text-left"></div>
        <div class="col row">
          <div class="bg-grey col-3"></div>
          <div class="col-9 column">
            <q-list class="">
              <q-slide-item
                v-for="category in categoryStore.categories"
                :key="category.id"
                @right="onRight(category.id, $event)"
                @click="goToForm(category.id)"
                v-ripple
              >
                <template v-slot:right>
                  <q-icon name="history" />
                </template>
                <q-item class="">
                  <q-item-section avatar>
                    <q-avatar
                      :style="{ backgroundColor: category.color, color: 'white' }"
                      text-color="white"
                      :icon="category.icon"
                    />
                  </q-item-section>
                  <q-item-section>
                    <div class="text-">{{ category.name }}</div>
                    <div class="text-right">
                      ¥{{ (categorySummaryMap[category.id] ?? 0).toLocaleString() }}
                    </div>
                  </q-item-section>
                </q-item>
              </q-slide-item>
            </q-list>
          </div>
        </div>
      </div>
    </div>

    <!-- <div class="q-mt-md">
      <q-btn
        label="左を広く"
        @click="setLayout('left-wide')"
      />
      <q-btn
        label="右を広く"
        @click="setLayout('right-wide')"
        class="q-ml-sm"
      />
      <q-btn
        label="均等"
        @click="setLayout('even')"
        class="q-ml-sm"
      />
    </div> -->
  </q-page>
</template>

<script setup lang="ts">
import { ref, computed, onBeforeUnmount, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { useCategoryStore } from 'src/stores/CategoryStore';
import { api } from 'boot/axios';

const totalAmount = ref(0);
const categorySummaryMap = ref<Record<number, number>>({});
const now = new Date();
const year = ref(now.getFullYear());
const month = ref(now.getMonth() + 1);

interface CategorySummary {
  categoryId: number;
  amount: number;
}

async function moveMonth(delta: number) {
  const newDate = new Date(year.value, month.value - 1 + delta);
  year.value = newDate.getFullYear();
  month.value = newDate.getMonth() + 1;
  await fetchData();
}

async function fetchData() {
  try {
    const res = await api.get('/dashboard/monthly-summary', {
      params: { year: year.value, month: month.value },
    });
    totalAmount.value = res.data.totalAmount;
    categorySummaryMap.value = Object.fromEntries(
      res.data.categorySummaries.map((c: CategorySummary) => [c.categoryId, c.amount]),
    );
  } catch (err) {
    console.error('取得失敗', err);
  }
}

onMounted(async () => {
  await fetchData();
});

const layoutMode = ref<'even' | 'left-wide' | 'right-wide'>('right-wide');
const router = useRouter();
const categoryStore = useCategoryStore();

// 記録ページへ遷移
function goToForm(categoryId: number) {
  router
    .push({
      path: '/compose/expense',
      query: { categoryId: categoryId.toString() },
    })
    .catch((err) => {
      console.error('Navigation failed:', err);
    });
}

// 履歴ページへ遷移
const goHistory = (id?: number) => {
  router.push({ path: '/expenses', query: id ? { categoryId: String(id) } : {} }).catch((err) => {
    console.error('Navigation failed:', err);
  });
};

// 左右幅の比率
const leftWidth = computed(() => {
  switch (layoutMode.value) {
    case 'left-wide':
      return 75;
    case 'right-wide':
      return 25;
    default:
      return 50;
  }
});

const rightWidth = computed(() => 100 - leftWidth.value);

const leftStyle = computed(() => ({
  flexBasis: leftWidth.value + '%',
  transition: 'flex-basis 0.3s ease',
}));

const rightStyle = computed(() => ({
  flexBasis: rightWidth.value + '%',
  transition: 'flex-basis 0.3s ease',
}));

// const setLayout = (mode: typeof layoutMode.value) => {
//   layoutMode.value = mode;
// };

// スライドで履歴表示
let timer: ReturnType<typeof setTimeout>;

function finalize(reset: () => void) {
  timer = setTimeout(() => {
    reset();
  }, 500);
}

onBeforeUnmount(() => {
  clearTimeout(timer);
});

function onRight(categoryId: number, { reset }: { reset: () => void }) {
  finalize(reset);
  goHistory(categoryId); // ★ 該当カテゴリで履歴へ
}
</script>
