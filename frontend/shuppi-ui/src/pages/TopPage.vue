<!-- src/pages/TopPage.vue -->
<template>
  <q-page class="column q-pa-md no-scroll">
    <div class="row items-center justify-center">
      <!-- <DateInput v-model="startDate" />
      <span class="q-mx-sm">-</span>
      <DateInput v-model="endDate" /> -->
      <div class="text-h6"><span>7月</span><span class="q-ml-sm">(+¥145,000)</span></div>
    </div>
    <div class="col row no-wrap">
      <div
        class="column q-pa-sm"
        :style="leftStyle"
      >
        <div class="text-center">+¥200,000</div>
      </div>
      <div
        class="column q-pa-sm"
        :style="rightStyle"
      >
        <div class="text-left">-¥55,000</div>
        <div class="col row">
          <div class="bg-red col-3"></div>
          <div class="col-9 column">
            <q-list class="">
              <q-slide-item
                @right="onRight"
                @click="goToForm"
                v-ripple
              >
                <template v-slot:right>
                  <q-icon name="history" />
                </template>
                <q-item class="">
                  <q-item-section avatar>
                    <q-avatar
                      color="primary"
                      text-color="white"
                      icon="restaurant"
                    />
                  </q-item-section>
                  <q-item-section>
                    <div class="text-">食費</div>
                    <div class="text-right">¥15,000</div>
                  </q-item-section>
                </q-item>
              </q-slide-item>
              <q-separator inset="item" />
              <q-slide-item
                @left="onLeft"
                @right="onRight"
              >
                <template v-slot:right>
                  <q-icon name="history" />
                </template>
                <q-item>
                  <q-item-section avatar>
                    <q-avatar
                      color="primary"
                      text-color="white"
                      icon="sym_o_cleaning"
                    />
                  </q-item-section>
                  <q-item-section>
                    <div>日用品</div>
                    <div class="text-right">¥15,000</div>
                  </q-item-section>
                </q-item>
              </q-slide-item>
              <q-separator inset="item" />
              <q-slide-item
                @left="onLeft"
                @right="onRight"
              >
                <template v-slot:right>
                  <q-icon name="history" />
                </template>
                <q-item>
                  <q-item-section avatar>
                    <q-avatar
                      color="primary"
                      text-color="white"
                      icon="sym_o_apparel"
                    />
                  </q-item-section>
                  <q-item-section>
                    <div>衣類・美容</div>
                    <div class="text-right">¥15,000</div>
                  </q-item-section>
                </q-item>
              </q-slide-item>
              <q-separator inset="item" />
              <q-slide-item
                @left="onLeft"
                @right="onRight"
              >
                <template v-slot:right>
                  <q-icon name="history" />
                </template>
                <q-item>
                  <q-item-section avatar>
                    <q-avatar
                      color="primary"
                      text-color="white"
                      icon="star"
                    />
                  </q-item-section>
                  <q-item-section>
                    <div>娯楽費</div>
                    <div class="text-right">¥15,000</div>
                  </q-item-section>
                </q-item>
              </q-slide-item>
              <q-separator inset="item" />
              <q-slide-item
                @left="onLeft"
                @right="onRight"
              >
                <template v-slot:right>
                  <q-icon name="history" />
                </template>
                <q-item>
                  <q-item-section avatar>
                    <q-avatar
                      color="primary"
                      text-color="white"
                      icon="house"
                    />
                  </q-item-section>
                  <q-item-section>
                    <div>固定費</div>
                    <div class="text-right">¥15,000</div>
                  </q-item-section>
                </q-item>
              </q-slide-item>
              <q-separator inset="item" />
              <q-slide-item
                @left="onLeft"
                @right="onRight"
              >
                <template v-slot:right>
                  <q-icon name="history" />
                </template>
                <q-item>
                  <q-item-section avatar>
                    <q-avatar
                      color="primary"
                      text-color="white"
                      icon="more_horiz"
                    />
                  </q-item-section>
                  <q-item-section>
                    <div>その他</div>
                    <div class="text-right">¥10,000,000</div>
                  </q-item-section>
                </q-item>
              </q-slide-item>
            </q-list>
          </div>
        </div>
      </div>
    </div>

    <div class="q-mt-md">
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
    </div>
  </q-page>
</template>

<script setup lang="ts">
import { ref, computed, onBeforeUnmount } from 'vue';
import { useRouter } from 'vue-router';

const layoutMode = ref<'even' | 'left-wide' | 'right-wide'>('right-wide');
const router = useRouter();

// 記録ページへ遷移
function goToForm() {
  router.push('/expenses/new').catch((err) => {
    console.error('Navigation failed:', err);
  });
}

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

const setLayout = (mode: typeof layoutMode.value) => {
  layoutMode.value = mode;
};

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

function onLeft({ reset }: { reset: () => void }) {
  finalize(reset);
}

function onRight({ reset }: { reset: () => void }) {
  finalize(reset);
}
</script>
