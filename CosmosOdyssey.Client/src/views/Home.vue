<script lang="ts">
import {defineComponent} from 'vue'
import FarePicker from "../components/FarePicker.vue";
// @ts-ignore
import {api} from "../../apiClients.generated.ts";
import LegListFilterOptionsModel = api.LegListFilterOptionsModel;

export default defineComponent({
  name: "Home",
  components: {FarePicker},
  data() {
    return {
      from: "",
      to: "",
      departDate: {} as Date | undefined,
      options: new LegListFilterOptionsModel,
    }
  },

  mounted() {
    new api.LegClient().listFilters().then(data => {
      this.options = data;
    })
  },
  methods: {
    handleValuesReady(payload: { from: string, to: string, departDate: Date }) {
      this.from = payload.from;
      this.to = payload.to;
      this.departDate = payload.departDate;
      // Navigate to the 'Legs' route with the from and to parameters from the payload
    },
    navigateToLegsList() {
      if (this.from === "" || this.to === "" || this.departDate == undefined){
        return this.$toast.add({severity: 'info', summary: 'Info', detail: "Journey incomplete", life: 3000});
      }
      this.$router.push({
        name: 'Journey',
        query: {from: this.from, to: this.to, departDate: this.departDate ? this.departDate.toISOString() : undefined},
      });
    }
  }
})
</script>

<template>
  <div class="grid place-items-center m-2 lg:m-2">
    <!-- Header Section -->
    <div class="relative text-center mt-5 sm:mt-24 px-6 sm:px-10 lg:px-14">
      <!-- Blur Background -->
      <div class="absolute inset-0 bg-gradient-to-r from-indigo-500 via-purple-500 to-indigo-500 opacity-20 blur-3xl -z-10 scale-125"></div>

      <!-- Foreground Content -->
      <h1 class="text-4xl sm:text-5xl lg:text-6xl font-extrabold text-indigo-600 tracking-tight mb-8 font-sans">
        Your Journey Awaits
      </h1>
      <p class="text-base sm:text-lg lg:text-xl text-gray-200 max-w-2xl mx-auto leading-relaxed font-sans">
        Welcome aboard <span class="font-semibold text-indigo-300">Space Odyssey</span>, the ultimate app for travelers
        seeking to explore the wonders of the universe. Whether you’re a seasoned space traveler or a curious wanderer,
        our platform offers seamless journeys between planets, solar systems, and galaxies. Plan your voyage, book your
        travel, and discover the endless beauty of space.
      </p>
      <hr class="border-t border-indigo-600 opacity-50 my-8 sm:my-12 w-1/2 mx-auto">
    </div>

    <!-- Fare Picker Section -->
    <div class="grid grid-cols-2 gap-2 sm:grid-cols-3 mt-5 sm:mt-20 w-full px-8">
      <div class="col-span-3 flex flex-col gap-8 items-center">
        <!-- Fare-picker container with white background -->
        <fare-picker
            :locations="options.locations"
            @values-ready="handleValuesReady"
            class="w-full max-w-3xl bg-white rounded-lg shadow-xl p-8 font-sans"
            style="overflow: hidden;"
        />
        <Button
            @click="navigateToLegsList"
            type="button"
            label="Find Fares"
            icon="pi pi-search"
            iconPos="left"
            class="w-full max-w-3xl bg-indigo-600 text-white text-lg font-semibold py-5 rounded-lg shadow-lg hover:bg-indigo-700 hover:scale-105 transform transition-transform duration-200 ease-in-out"
        />
      </div>
    </div>
  </div>
</template>

<style scoped>

</style>