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
    navigateToLegsList(){
      this.$router.push({
        name: 'Legs',
        params: {from: this.from, to: this.to}
      });
    }
  }
})
</script>

<template>
  <div class="grid place-items-center m-10">
    <div class="text-center mt-10 px-4">
      <h1 class="text-5xl font-extrabold text-amber-600 bg-amber-950 mb-6">Your Intergalactic Journey Awaits</h1>
      <p class="text-lg text-gray-500 max-w-2xl mx-auto">
        Welcome aboard <span class="font-bold text-amber-600">Space Odyssey</span>, the ultimate app for travelers
        seeking to explore the wonders of the universe. Whether you’re a seasoned space traveler or a curious wanderer,
        our platform offers seamless journeys between planets, solar systems, and galaxies. With just a few taps, you’ll
        be able to plan your voyage, book your travel, and explore the endless beauty of space.
      </p>
    </div>
    <div class="grid grid-cols-3 gap-4 mt-10 ">
      <div class="col-span-3">
        <fare-picker :locations="options.locations" @values-ready="handleValuesReady" class="w-full"/>
      </div>

      <div class="col-span-3">
        <Button @click="navigateToLegsList" type="button" label="Search" icon="pi pi-search"
                class="w-full bg-amber-950 h-10 text-white hover:bg-amber-600"/>
      </div>
    </div>
  </div>

</template>

<style scoped>

</style>