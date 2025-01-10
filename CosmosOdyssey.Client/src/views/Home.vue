<script lang="ts">
import {defineComponent} from 'vue'
import FarePicker from "../components/FarePicker.vue";
import {api} from "../../apiClients.generated.ts";
import LegListFilterOptionsModel = api.LegListFilterOptionsModel;

export default defineComponent({
  name: "Home",
  components: {FarePicker},
  data() {
    return {
      from: "",
      to: "",
      options: new LegListFilterOptionsModel,
    }
  },

  mounted() {
    new api.LegClient().listFilters().then(data => {
      this.options = data;
    })
  },
  methods: {
    handleValuesReady(payload: { from: string, to: string }) {
      this.from = payload.from;
      this.to = payload.to;
      // Navigate to the 'Legs' route with the from and to parameters from the payload
    },
    navigateToLegsList() {
      this.$router.push({
        name: 'Legs',
        params: {from: this.from, to: this.to}
      });
    }
  }
})
</script>

<template>
  <div class="grid place-items-center m-1 sm:m-1 lg:m-5">
    <div class="relative text-center mt-10 sm:mt-20 px-4 sm:px-6">
      <!-- Blur Background -->
      <div class="absolute inset-0 bg-white opacity-10 blur-3xl -z-10 sm:scale-150 scale-100"></div>

      <!-- Foreground Content -->
      <h1 class="text-3xl sm:text-4xl lg:text-5xl font-extrabold text-indigo-400 mb-6 font-mono">
        Your Journey Awaits
      </h1>
      <hr class="mb-6 sm:mb-10">
      <p class="text-sm sm:text-base lg:text-lg text-gray-100 max-w-xl mx-auto font-mono">
        Welcome aboard <span class="font-bold text-indigo-200">Space Odyssey</span>, the ultimate app for travelers
        seeking to explore the wonders of the universe. Whether you’re a seasoned space traveler or a curious wanderer,
        our platform offers seamless journeys between planets, solar systems, and galaxies. With just a few taps, you’ll
        be able to plan your voyage, book your travel, and explore the endless beauty of space.
      </p>
      <hr class="mt-6 sm:mt-10">
    </div>

    <div class="grid grid-cols-1 gap-4 sm:grid-cols-3 mt-6 sm:mt-10 w-full px-4">
      <!-- Fare Picker with Button below it -->
      <div class="col-span-3 sm:col-span-3 flex flex-col gap-4 w-full mx-auto">
        <fare-picker
            :locations="options.locations"
            @values-ready="handleValuesReady"
            class="w-full font-mono"
        />
        <!-- Button directly under the fare-picker, constrained to the same width -->
        <Button
            @click="navigateToLegsList"
            type="button"
            label="Find fares"
            icon="pi pi-search"
            iconPos="top"
            class="w-full sm:w-full max-w-[500px] bg-indigo-400 font-mono font-bold mt-5 h-14 sm:h-16 lg:h-20 text-white hover:bg-indigo-700 mx-auto"
        />
      </div>
    </div>
  </div>

</template>

<style scoped>

</style>