<script lang="ts">
import {defineComponent} from 'vue'
import {api} from "../../apiClients.generated.ts";
import FarePicker from "../components/FarePicker.vue";

export default defineComponent({
  name: "ReservationList",
  components: {FarePicker},
  data() {
    return {
      searchQuery: '',
      reservations: [] as api.ReservationListItemModel[]
    }
  },
  mounted() {
    if (this.$route.query.search) {
      this.searchQuery = this.$route.query.search as string;
      this.fetchReservations(this.searchQuery)
    }
  },
  methods: {
    fetchReservations(query: string | undefined) {
      this.$router.push({query: {search: query}});
      new api.ReservationClient().list(query).then(response => {
        this.reservations = response
      })
    },
    navigateToReservation(id: string) {
      return this.$router.push({
        name: 'Reservation',
        query: {id: id}
      });
    }
  },
})
</script>

<template>
  <div class="flex flex-col items-center p-4 lg:p-8 space-y-8">
    <!-- Search Box Section -->
    <div class="w-full sm:w-2/3 md:w-1/2">
      <input
          v-model="searchQuery"
          type="text"
          placeholder="Search by last name"
          class="w-full p-4 border border-gray-300 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 transition-all duration-300 ease-in-out"
      />
    </div>

    <!-- Reservations Section -->
    <div v-if="reservations.length" class="relative text-center my-8 sm:my-12 w-full">
      <h2 class="text-2xl sm:text-3xl font-bold text-indigo-600 mb-6">Reservations</h2>

      <!-- DataTable for Reservations -->
      <DataTable :value="reservations" size="large" tableStyle="min-width: 50rem">
        <Column field="name.firstName" header="First name"></Column>
        <Column field="name.lastName" header="Last name"></Column>
        <Column field="from" header="From"></Column>
        <Column field="to" header="To"></Column>
        <Column class="w-24 !text-end">
          <template #body="{ data }">
            <Button icon="pi pi-search" @click="navigateToReservation(data.id)" severity="secondary" rounded></Button>
          </template>
        </Column>
      </DataTable>
    </div>

    <!-- Fare Picker Section (Button) -->
    <div class="w-full sm:w-2/3 md:w-1/2">
      <Button
          @click="fetchReservations(searchQuery)"
          type="button"
          label="Find Reservations"
          icon="pi pi-search"
          iconPos="left"
          class="w-full bg-indigo-600 text-white text-lg font-semibold py-4 rounded-lg shadow-md hover:bg-indigo-700 hover:scale-105 transform transition-all duration-300 ease-in-out"
      />
    </div>
  </div>
</template>

<style scoped>

</style>