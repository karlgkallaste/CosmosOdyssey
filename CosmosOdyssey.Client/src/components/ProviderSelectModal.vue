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
      new api.LegClient().providers(this.legId, this.priceListId, filters.priceBelow, filters.arriveBy, filters.companyName, filters.sortBy).then(x => {
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
    formatDate(date: Date | undefined) {
      return date ? date.toLocaleString('en-US', {
            weekday: 'short', // Abbreviated day of the week (e.g., "Mon")
            year: 'numeric',
            month: 'short', // Abbreviated month (e.g., "Jan")
            day: 'numeric',
            hour: '2-digit',
            minute: '2-digit',
          })
          : "-";
    }
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
  <Dialog v-model:visible="visible" modal header="Provider selection" :style="{ width: '70rem' }"
          :breakpoints="{ '1199px': '75vw', '575px': '90vw' }">

    <div class="container mx-auto p-6">
      <div class="mb-6 space-y-6 sm:space-y-0 sm:grid sm:grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        <!-- Search Box -->
        <div>
          <label class="text-gray-400" for="companyName">Search by company name</label>
          <input
              v-model="filters.companyName"
              type="text"
              id="companyName"
              class="w-full p-3 text-black border border-gray-300 bg-white rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-indigo-500 transition-all duration-200"
          />
        </div>

        <!-- Price Below -->
        <div>
          <label class="text-gray-400" for="priceBelow">Search by price</label>
          <input
              v-model="filters.priceBelow"
              type="number"
              id="priceBelow"
              placeholder="Price limit"
              class="w-full p-3 text-black border border-gray-300 bg-white rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-indigo-500 transition-all duration-200"
          />
        </div>

        <!-- Calendar -->
        <div>
          <label class="text-gray-400" for="arriveBy">Arrive by</label>
          <Calendar
              v-model="filters.arriveBy"
              inputId="arriveBy"
              showIcon
              iconDisplay="input"
              variant="filled"
              input-class="w-full text-black p-3 border border-gray-300 bg-white rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-indigo-500 transition-all duration-200"
          />
        </div>

        <!-- Sort By Dropdown -->
        <div>
          <label class="text-gray-400" for="sortBy">Sort by</label>
          <select
              v-model="filters.sortBy"
              id="sortBy"
              class="w-full p-3 text-black border border-gray-300 bg-white rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-indigo-500 transition-all duration-200">
            <option value="" disabled selected hidden>-</option>
            <option value="price">Sort by lowest price</option>
            <option value="flightstart">Sort by company A-Z</option>
            <option value="flightend">Sort by flight start</option>
          </select>
        </div>
      </div>

      <div v-if="providers.length" v-for="provider in providers" :key="provider.id"
           class="bg-white shadow-lg rounded-lg p-3 mb-3">
        <div class="flex flex-col sm:flex-row sm:space-x-4 justify-between items-start mb-4">
          <div class="flex flex-col items-start space-y-3">
            <p class="text-xl border rounded-3xl p-3 bg-indigo-600 text-white font-semibold">{{
                provider.company?.name
              }}</p>
            <div class="flex space-x-6">
              <div>
                <p class="text-gray-500">Price</p>
                <p class="font-semibold text-indigo-600">${{ provider.price!.toFixed(2) }}</p>
              </div>

              <div>
                <p class="text-gray-500">Start</p>
                <p class="font-semibold text-indigo-600">{{ formatDate(provider.flightStart!) }}</p>
              </div>

              <div>
                <p class="text-gray-500">End</p>
                <p class="font-semibold text-indigo-600">{{ formatDate(provider.flightEnd!) }}</p>
              </div>
            </div>
          </div>


          <div class="mt-4 sm:mt-0 sm:ml-4 w-full sm:w-auto flex justify-end">
            <Button @click="selectProvider(provider)" type="button" label="Select" icon="pi pi-ticket"
                    class="w-full sm:w-auto text-white bg-indigo-600 font-semibold rounded-lg h-14 sm:h-16 px-6 sm:px-8 text-lg sm:text-xl hover:bg-indigo-700 transition duration-200 ease-in-out transform hover:scale-105"/>
          </div>
        </div>

      </div>
      <div v-else>
        <div class="flex items-center justify-center m-2 lg:m-2 h-screen">
          <p class="text-lg text-gray-500">No results</p>
        </div>
      </div>
    </div>
  </Dialog>
</template>

<style scoped>

</style>