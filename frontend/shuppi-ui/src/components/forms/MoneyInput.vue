<template>
  <q-field
    v-bind="labelAttrs"
    dense
  >
    <template v-slot:control="{ id }">
      <div class="row no-wrap full-width items-baseline">
        <input
          :id="id"
          ref="inputRef"
          v-model="inputText"
          @input="handleInput"
          v-money="moneyFormatForDirective"
          class="q-field__input text-right col-grow text-h4"
          style="height: auto; line-height: 1.5; padding-top: 8px"
          inputmode="numeric"
        />
      </div>
    </template>
  </q-field>
</template>

<script setup lang="ts">
import { defineProps, defineEmits, watch, ref, computed } from 'vue';

const props = defineProps<{
  modelValue: number | null;
  label?: string;
}>();

const labelAttrs = computed(() => {
  return props.label ? { label: props.label } : {};
});

const emit = defineEmits<{
  (e: 'update:modelValue', value: number): void;
}>();

const inputText = ref('');

const moneyFormatForDirective = {
  decimal: '.',
  thousands: ',',
  prefix: '¥',
  suffix: '',
  precision: 0,
  masked: false,
};

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

// 親コンポーネントからfocus()を呼び出し可能にする
const inputRef = ref<HTMLInputElement | null>(null);

defineExpose({
  focus: () => {
    inputRef.value?.focus();
  },
});
</script>
