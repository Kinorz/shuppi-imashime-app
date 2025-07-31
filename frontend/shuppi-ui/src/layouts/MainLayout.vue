<template>
  <q-layout view="lHh Lpr lFf">
    <!-- <q-header class="bg-white text-black">
      <q-toolbar>
        <q-btn
          flat
          dense
          round
          icon="menu"
          aria-label="Menu"
          @click="toggleLeftDrawer"
        />
        <q-toolbar-title></q-toolbar-title>
      </q-toolbar>
      <q-separator />
    </q-header> -->

    <!-- <q-drawer
      v-model="leftDrawerOpen"
      show-if-above
      bordered
    >
      <q-list>
        <q-item-label header> Essential Links </q-item-label>
      </q-list>
    </q-drawer> -->

    <q-page-container>
      <router-view />
    </q-page-container>

    <q-footer class="bg-grey-2 text-black">
      <q-tabs
        align="justify"
        active-color="primary"
        dense
        class="text-grey-8"
        no-caps
      >
        <q-route-tab
          to="/"
          label="ホーム"
          icon="home"
        />
        <q-route-tab
          to="/settings"
          label="設定"
          icon="settings"
        />
      </q-tabs>
    </q-footer>
  </q-layout>
</template>

<script setup lang="ts">
// import { ref } from 'vue';
// const leftDrawerOpen = ref(false);

// function toggleLeftDrawer() {
//   leftDrawerOpen.value = !leftDrawerOpen.value;
// }
</script>
