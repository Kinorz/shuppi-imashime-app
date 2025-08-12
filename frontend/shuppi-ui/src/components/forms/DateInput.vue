<template>
  <q-input
    v-model="display"
    v-bind="labelAttrs"
    dense
    filled
    :clearable="props.isClearable"
    class="hide-indicator"
    input-class="text-center cursor-pointer"
    @click="showPicker = true"
  />
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

const props = withDefaults(
  defineProps<{
    modelValue: string;
    label?: string;
    isClearable?: boolean;
  }>(),
  {
    isClearable: false,
  },
);

const emit = defineEmits<{ (e: 'update:modelValue', value: string): void }>();
const showPicker = ref(false);
const internalValue = ref(props.modelValue);

watch(
  () => props.modelValue,
  (val) => {
    internalValue.value = val;
  },
);

const labelAttrs = computed(() => (props.label ? { label: props.label } : {}));

// 表示用（YYYY/M/D (曜)）— クリア時だけ setter で '' を親に返す
const display = computed<string>({
  get() {
    const date = new Date(internalValue.value);
    if (isNaN(date.getTime())) return '';
    const days = ['日', '月', '火', '水', '木', '金', '土'];
    return `${date.getFullYear()}/${date.getMonth() + 1}/${date.getDate()} (${days[date.getDay()]})`;
  },
  set(v: string) {
    // clearボタンで '' が入る → 内部状態と親をクリア
    if (!v) {
      internalValue.value = '';
      emit('update:modelValue', '');
    }
  },
});

function onDateSelected(val: string) {
  internalValue.value = val;
  emit('update:modelValue', val);
  showPicker.value = false;
}
</script>
