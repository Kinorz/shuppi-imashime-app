<template>
  <q-field
    :label="label"
    filled
  >
    <template v-slot:control="{ id }">
      <div class="row items-center no-wrap full-width">
        <input
          :id="id"
          v-model="inputText"
          @focus="selectOnFocus"
          @input="handleInput"
          v-money="moneyFormatForDirective"
          class="q-field__input text-right col-grow"
          inputmode="numeric"
        />
        <span class="q-ml-sm">円</span>
      </div>
    </template>
  </q-field>
</template>

<script setup lang="ts">
import { defineProps, defineEmits, watch, ref } from 'vue';

const props = defineProps<{
  modelValue: number | null;
  label: string;
}>();

const emit = defineEmits<{
  (e: 'update:modelValue', value: number): void;
}>();

const inputText = ref('');

const moneyFormatForDirective = {
  decimal: '.',
  thousands: ',',
  prefix: '',
  suffix: '',
  precision: 0,
  masked: false,
};

// 外部のmodelValue → inputTextへ反映
watch(
  () => props.modelValue,
  (val) => {
    inputText.value = val !== null ? val.toLocaleString() : '';
  },
  { immediate: true },
);

function handleInput(event: Event) {
  const input = event.target as HTMLInputElement;
  const value = input.value;

  // カンマを除去して数値に変換
  const numeric = parseInt(value.replace(/,/g, ''), 10) || 0;

  emit('update:modelValue', numeric);
}

function selectOnFocus(event: FocusEvent) {
  const target = event.target as HTMLInputElement;
  setTimeout(() => {
    target.select();
  }, 5);
}
</script>
