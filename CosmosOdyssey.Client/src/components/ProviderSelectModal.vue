<script lang="ts">
import {defineComponent} from 'vue'
import {api} from "../../apiClients.generated";
import {debounce} from 'vue-debounce';

export default defineComponent({
  name: "ProviderSelectModal",
  data() {
    return {
      visible: false,
      providers: [] as api.ProviderInfoModel[],
      filters: new SearchFiltersModel(),
      legId: "",
      priceListId: ""
    }
  },

  created() {
    this.$watch(
        'filters',
        (newValue) => {
          this.filters = newValue;
          this.fetchProviders(this.filters)
        }, {deep: true}
    );
  },
  methods: {
    fetchProviders(filters: SearchFiltersModel) {
      new api.LegClient().legProvidersList(this.legId, this.priceListId, filters.priceBelow, filters.arriveBy, filters.companyName, filters.sortBy).then(x => {
        this.providers = x;
      })
    },
    
    open(legId: string, priceListId: string) {
      this.visible = true;
      this.legId = legId;
      this.priceListId = priceListId;
      this.fetchProviders(this.filters)
    },
    selectProvider(provider: api.ProviderInfoModel) {
    this.filters = new SearchFiltersModel();
    this.$emit("provider-select", this.legId, provider);
    this.visible = false;
    },
  },

})

class SearchFiltersModel {
  companyName: string = "";
  arriveBy: Date | undefined = undefined;
  priceBelow: number | undefined = undefined;
  sortBy: string = "";
}
</script>

<template>
  <Dialog v-model:visible="visible" modal header="Select Providers" :style="{ width: '70rem' }"
          :breakpoints="{ '1199px': '75vw', '575px': '90vw' }">

    <div class="container mx-auto p-6">
      <!-- Search Box -->
      <div class="mb-6 space-y-6 sm:space-y-0 sm:grid sm:grid-cols-2 sm:gap-6  lg:grid-cols-3">
        <!-- Search Box -->
        <div>
          <input
              v-model="filters.companyName"
              type="text"
              placeholder="Search by company name..."
              class="w-full p-3 border border-gray-300 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-indigo-500 transition-all duration-200"
          />
        </div>

        <!-- Price Below -->
        <div>
          <input
              v-model="filters.priceBelow"
              type="number"
              placeholder="Search by price below..."
              class="w-full p-3 border border-gray-300 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-indigo-500 transition-all duration-200"
          />
        </div>

        <!-- Calendar -->
        <div>
          <Calendar
              v-model="filters.arriveBy"
              class="w-full sm:w-[320px] bg-gradient-to-r from-indigo-600 to-indigo-700 text-white font-semibold 
        rounded-lg px-5 py-3 shadow-xl hover:from-indigo-500 hover:to-indigo-600 
        focus:outline-none focus:ring-4 focus:ring-indigo-400 transition-all duration-300 ease-in-out"
              placeholder="Select Date and Time"
          />
        </div>

        <!-- Sort By Dropdown -->
        <div>
          <select
              v-model="filters.sortBy"
              class="w-full p-3 border border-gray-300 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-indigo-500 transition-all duration-200">
            <option value="price">Sort by lowest price</option>
            <option value="flightstart">Sort by company A-Z</option>
            <option value="flightend">Sort by flight start</option>
          </select>
        </div>
      </div>
      <!-- Filtered Providers List -->
      <div v-for="provider in providers" :key="provider.id" class="bg-white shadow-lg rounded-lg p-6 mb-4">
        <!-- Company Name -->
        <p class="text-xl font-semibold text-gray-800">{{ provider.company?.name }}</p>

        <!-- Flight Details -->
        <div class="flex justify-between text-sm text-gray-600 mt-2">
          <div>
            <p class="text-gray-500">Price</p>
            <p class="font-semibold text-indigo-600">${{ provider.price!.toFixed(2) }}</p>
          </div>
          <div>
            <p class="text-gray-500">Start</p>
            <p class="font-semibold">{{ new Date(provider.flightStart!).toLocaleDateString("en-US") }}</p>
          </div>
          <div>
            <p class="text-gray-500">End</p>
            <p class="font-semibold">{{ new Date(provider.flightEnd!).toLocaleDateString("en-US") }}</p>
          </div>
        </div>

        <!-- Button -->
        <div class="mt-4">
          <Button
              label="Select"
              icon="pi pi-check"
              @click="selectProvider(provider)"
              class="w-full bg-indigo-600 text-white hover:bg-indigo-700 focus:ring-2 focus:ring-indigo-500 rounded-lg shadow-lg transition-all duration-200"
          />
        </div>
      </div>
    </div>
  </Dialog>
</template>

<style scoped>

</style>