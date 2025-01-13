<template>
    <Menubar :model="items" class="font-mono">
      <template #end>
        <div class="flex items-center gap-3 sm:gap-5">
          <h1 class="text-xl sm:text-2xl font-extrabold font-mono text-indigo-400 bg-indigo-950 p-2 rounded-lg">
            Space Odyssey
          </h1>
        </div>
      </template>
    </Menubar>
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
        {
          label: 'Reservations',
          icon: 'pi pi-list',
          command: () => {
            this.$router.push('/reservations');
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
