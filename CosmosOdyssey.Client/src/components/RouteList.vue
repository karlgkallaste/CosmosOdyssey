<script lang="ts">
import {defineComponent} from 'vue'
import {api} from "../../apiClients.generated";
import {useToast} from "primevue";

interface CustomErrorResponse {
  errorMessage: string;
  [key: string]: any;
}

export default defineComponent({
  name: "RouteList",
  props: {
    from: {
      type: String,
      required: true,
    },
    to: {
      type: String,
      required: true,
    },
    departDate: {
      type: String,
      required: true,
    },
  },
  data() {
    return {
      routes: [] as api.RouteListItemModel[]
    }
  },
  mounted() {
    new api.LegClient().legs(this.$props.from, this.$props.to, new Date(this.$props.departDate)).then(routes => {
      this.routes = routes;
    }).catch((error) => { // Use custom error type if not using axios
      // Handle the error as needed
      this.$toast.add({
        severity: 'error',
        summary: 'Error',
        detail: "Test",
        life: 3000,
      });
    });
  },
  methods: {
    createReservation(route: api.RouteListItemModel) {
      this.$emit("createReservation", route, route.priceListId);
    },
  }
})

</script>

<template>
  <div class="p-6 space-y-8">
    <!-- Header -->
    <h2 class="text-3xl font-bold text-gray-800 tracking-wide">Flight Options</h2>

    <div v-for="(route, index) in routes" :key="index" class="flex justify-center p-4">
      <div class="bg-white w-full max-w-6xl p-6 transition-transform duration-300 ease-in-out hover:scale-105 shadow-lg rounded-lg">

        <!-- Option and Number of Transfers (Left-aligned) -->
        <div class="flex flex-col sm:flex-row justify-between items-center mb-4">
          <div class="flex flex-col sm:flex-row sm:space-x-6 items-start w-full">
            <h3 class="text-2xl font-semibold text-gray-700">Option {{index + 1}}</h3>
            <p class="text-lg text-gray-600">Number of transfers: {{ route.routes?.length || 0 }}</p>
          </div>

          <!-- Reserve Button (Right-aligned) -->
          <div class="mt-4 sm:mt-0 sm:ml-4 w-full sm:w-auto flex justify-end">
            <Button @click="createReservation(route)" type="button" label="Select" icon="pi pi-ticket"
                    class="w-full sm:w-auto text-white bg-indigo-600 font-semibold rounded-lg h-14 sm:h-16 px-6 sm:px-8 text-lg sm:text-xl hover:bg-indigo-700 transition duration-200 ease-in-out transform hover:scale-105"/>
          </div>
        </div>

        <!-- Stacked Legs (From -> To) with smooth arrow icon -->
        <Timeline :value="route.routes" layout="horizontal" align="alternate">
          <template #opposite>&nbsp;</template>
          <template #content="slotProps">
            <span class="text-indigo-600 font-mono">{{ slotProps.item.to?.name }}</span>
          </template>
          <template #marker="slotProps">
            <!-- Custom marker color -->
            <span class="w-4 h-4 rounded-full bg-indigo-600 flex items-center justify-center font-mono"
                  :title="slotProps.item.to?.name"></span>
          </template>
        </Timeline>

      </div>
    </div>
  </div>
</template>

<style scoped>

</style>