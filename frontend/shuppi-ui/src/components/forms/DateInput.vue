<template>
  <q-input
    v-model="formatted"
    v-bind="labelAttrs"
    readonly
    dense
    class="hide-indicator text-h6"
    input-class="text-center cursor-pointer"
    @click="showPicker = true"
  >
  </q-input>

  <q-dialog v-model="showPicker">
    <q-date
      v-model="internalValue"
      mask="YYYY-MM-DD"
      @update:model-value="onDateSelected"
    />
  </q-dialog>
</template>

<script setup lang="ts">
import { ref, watch, computed } from 'vue';

const props = defineProps<{
  modelValue: string;
  label?: string;
}>();

const labelAttrs = computed(() => {
  return props.label ? { label: props.label } : {};
});

const emit = defineEmits<{
  (e: 'update:modelValue', value: string): void;
}>();

const showPicker = ref(false);
const internalValue = ref(props.modelValue);

watch(
  () => props.modelValue,
  (val) => {
    internalValue.value = val;
  },
);

const formatted = computed(() => {
  const date = new Date(internalValue.value);
  if (isNaN(date.getTime())) return '';
  const days = ['日', '月', '火', '水', '木', '金', '土'];
  const weekday = days[date.getDay()];
  return `${date.getFullYear()}/${date.getMonth() + 1}/${date.getDate()} (${weekday})`;
});

function onDateSelected(val: string) {
  internalValue.value = val;
  emit('update:modelValue', val);
  showPicker.value = false;
}
</script>
