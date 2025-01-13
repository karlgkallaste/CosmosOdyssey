<script setup lang="ts">
import Navbar from "./components/NavBar.vue";
import Toast from "primevue/toast";
</script>

<template>
  <div id="app">
    <Navbar />
    <div class="content">
      <Toast />
      <router-view />
    </div>
    
  </div>
</template>

<style scoped>
#app {
  display: flex;
  flex-direction: column;
  min-height: 100vh; /* Full height of the screen */
}

.content {
  flex: 1; /* Take remaining space for content */
  padding: 20px;
}

body {
  margin: 0; /* Remove any default margin */
  font-family: Avenir, Helvetica, Arial, sans-serif;
}
</style>
