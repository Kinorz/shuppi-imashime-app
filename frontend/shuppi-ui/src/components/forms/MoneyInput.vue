<template>
  <q-field
    :label="label"
    filled
  >
    <template v-slot:control="{ id }">
      <div class="row items-center no-wrap full-width">
        <input
          :id="id"
          :value="formattedValue"
          @focus="selectOnFocus"
          @input="handleInput"
          v-money="moneyFormatForDirective"
          class="q-field__input text-right col-grow"
        />
        <span class="q-ml-sm">円</span>
      </div>
    </template>
  </q-field>
</template>

<script setup lang="ts">
import { defineProps, defineEmits, computed } from 'vue';

const props = defineProps<{
  modelValue: number | null;
  label: string;
}>();

const emit = defineEmits<{
  (e: 'update:modelValue', value: number): void;
}>();

const moneyFormatForDirective = {
  decimal: '.',
  thousands: ',',
  prefix: '',
  suffix: '',
  precision: 0,
  masked: false,
};

// フォーマット済みの文字列を表示専用として作成
const formattedValue = computed(() => {
  if (props.modelValue === null || isNaN(props.modelValue)) return '';
  return props.modelValue.toLocaleString(); // 例: 123456 → "123,456"
});

function selectOnFocus(event: FocusEvent) {
  const target = event.target as HTMLInputElement;
  setTimeout(() => {
    target.select();
  }, 5);
}

function handleInput(event: Event) {
  const input = event.target as HTMLInputElement;
  const value = input.value;

  // 全角数字 → 半角数字に変換
  const converted = value.replace(/[０-９]/g, (s) => String.fromCharCode(s.charCodeAt(0) - 0xfee0));

  // 数値化してemit
  const numeric = parseInt(converted.replace(/,/g, ''), 10) || 0;
  emit('update:modelValue', numeric);
}
</script>
