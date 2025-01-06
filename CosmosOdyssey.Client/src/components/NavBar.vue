<template>
  <div class="card">
    <Menubar :model="items"/>
  </div>
</template>

<script>
import {defineComponent} from 'vue';
import Menubar from 'primevue/menubar';
import {RouterLink} from "vue-router"; // Import Menubar from PrimeVue

export default defineComponent({
  name: 'Navbar',
  components: {
    Menubar,
    RouterLink
  },
  data() {
    return {
      items: [
        {
          label: 'Home',
          icon: 'pi pi-home',
          command: () => {
            this.$router.push('/');
          }
        },
        {
          label: 'About',
          icon: 'pi pi-star',
          command: () => {
            this.$router.push('/about');
          }
        },
      ]
    };
  }
});
</script>

<style scoped>
/* Optionally, you can add custom styles for your navbar */
</style>
