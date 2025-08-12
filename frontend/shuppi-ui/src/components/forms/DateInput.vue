<template>
  <div class="di-wrap">
    <q-input
      v-model="display"
      v-bind="labelAttrs"
      dense
      filled
      readonly
      class="hide-indicator"
      input-class="text-center cursor-pointer"
    >
      <template
        #append
        v-if="props.isClearable && !!internalValue"
      >
        <q-icon
          name="cancel"
          class="q-field__focusable-action cursor-pointer"
          role="button"
          aria-label="クリア"
          tabindex="0"
          @click.stop="clearDate"
          @keydown.enter.prevent="clearDate"
          @keydown.space.prevent="clearDate"
        />
      </template>
    </q-input>

    <button
      type="button"
      class="di-cover"
      :style="coverStyle"
      @click="showPicker = true"
    />
  </div>

  <q-dialog v-model="showPicker">
    <q-date
      :model-value="internalValue || null"
      mask="YYYY-MM-DD"
      @update:model-value="onDateSelected"
    />
  </q-dialog>
</template>

<style scoped>
.di-wrap {
  position: relative;
}
.di-cover {
  position: absolute;
  background: transparent;
  border: 0;
  padding: 0;
  margin: 0;
}
.di-cover:focus {
  outline: none;
}
</style>

<script setup lang="ts">
import { ref, watch, computed } from 'vue';

const props = withDefaults(
  defineProps<{
    modelValue?: string;
    label?: string;
    isClearable?: boolean;
  }>(),
  {
    modelValue: '',
    isClearable: false,
  },
);

// ボタン用のスタイル
const showClear = computed(() => props.isClearable && !!internalValue.value);
const coverStyle = computed(() => ({
  inset: `0 ${showClear.value ? '40px' : '0'} 0 0`,
}));

const emit = defineEmits<{ (e: 'update:modelValue', value: string): void }>();

const showPicker = ref(false);
const internalValue = ref<string>(props.modelValue);

watch(
  () => props.modelValue,
  (v) => {
    internalValue.value = v ?? '';
  },
);

const labelAttrs = computed(() => (props.label ? { label: props.label } : {}));

type Ymd = { y: number; mo: number; d: number };
const pad = (n: number) => String(n).padStart(2, '0');

// "YYYY-MM-DD" / "YYYY/M/D" を安全に分解して実在日付か検証
function parseYmdString(val: string): Ymd | null {
  const s = val.trim().replace(/\//g, '-');
  const parts = s.split('-');
  if (parts.length !== 3) return null;

  const [yStr, moStr, dStr] = parts;
  const y = Number(yStr),
    mo = Number(moStr),
    d = Number(dStr);
  if (!Number.isInteger(y) || !Number.isInteger(mo) || !Number.isInteger(d)) return null;

  // 実在チェック（TZ影響を避けるためUTCで）
  const dt = new Date(Date.UTC(y, mo - 1, d));
  if (dt.getUTCFullYear() !== y || dt.getUTCMonth() + 1 !== mo || dt.getUTCDate() !== d)
    return null;

  return { y, mo, d };
}

function toYmd(val: unknown): string {
  if (val == null) return '';
  if (val instanceof Date) {
    return `${val.getFullYear()}-${pad(val.getMonth() + 1)}-${pad(val.getDate())}`;
  }
  if (typeof val === 'string') {
    const r = parseYmdString(val);
    return r ? `${r.y}-${pad(r.mo)}-${pad(r.d)}` : '';
  }
  return '';
}

function formatDisplay(ymd: string): string {
  const r = parseYmdString(ymd);
  if (!r) return '';
  const { y, mo, d } = r;
  const dt = new Date(Date.UTC(y, mo - 1, d));
  const days = ['日', '月', '火', '水', '木', '金', '土'];
  return `${y}/${mo}/${d} (${days[dt.getUTCDay()]})`;
}

const display = computed<string>({
  get: () => (internalValue.value ? formatDisplay(internalValue.value) : ''),
  set: (v) => {
    if (!v) {
      internalValue.value = '';
      emit('update:modelValue', '');
    }
  },
});

function clearDate() {
  internalValue.value = '';
  emit('update:modelValue', '');
}

function onDateSelected(val: unknown) {
  const ymd = toYmd(val); // ★ 正規化（string/Date/null対応）
  if (!ymd) {
    showPicker.value = false;
    return;
  }
  // 同日再選択でも値はそのまま維持（emitはしてOK）
  internalValue.value = ymd;
  emit('update:modelValue', ymd);
  showPicker.value = false;
}
</script>
