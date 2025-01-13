<script lang="ts">
import {defineComponent} from 'vue'
import {api} from "../../apiClients.generated";
import ProviderSelectModal from "./ProviderSelectModal.vue";

export default defineComponent({
  name: "Reservation",
  components: {ProviderSelectModal},
  props: {
    route: {
      type: api.RouteListItemModel,
      required: true,
    },
    priceListId: {
      type: String,
      required: true,
    },
  },
  data() {
    return {
      request: {} as api.CreateReservationRequest,
      route: {} as api.RouteListItemModel,
    }
  },
  created() {
    this.createEmptyRequest();
    this.$watch(() => this.$props.route, (newRoute) => {
      this.route = newRoute;
      this.request.routes = [];
    })
  },
  methods: {
    createEmptyRequest() {
      this.request = new api.CreateReservationRequest();
      this.request.name = new api.PersonNameModel({firstName: "", lastName: ""})
      this.request.routes = [];

    },
    selectFlight(legId: string) {
      return (this.$refs.modal as any).open(legId, this.$props.priceListId);
    },
    setRouteInfo(route: api.RouteListItemModel) {
      this.route = route;
    },
    getProviderInfo(legId: string): api.ProviderInfoModel | undefined {
      if (this.request.routes == undefined) {
        return undefined;
      }
      let companyId = this.request.routes.find(x => x.legId === legId)?.companyId;
      const providerInfo = this.route.routes
          ?.flatMap(route => route.providers) // Flatten all providers across routes
          ?.find(provider => provider?.company?.id === companyId);

      return providerInfo;

    },

    confirmReservation() {
      this.request.priceListId = this.$props.priceListId;
      this.$emit('reservation-confirmed', this.request.clone());
    },

    setLegProvider(legId: string, provider: api.ProviderInfoModel) {
      if (this.request.routes == undefined) {
        this.request.routes = [];
      }

      // Check if a route with the given legId exists
      const routeProvider = this.request.routes.find(x => x.legId === legId);

      if (routeProvider !== undefined) {
        // If it exists, remove it
        this.request.routes = this.request.routes.filter(x => x.legId !== legId);
      }

      // Add the new route
      this.request.routes.push(new api.ReservationRouteModel({
        legId: legId,
        companyId: provider.company?.id,
        arrival: provider.flightStart,
        departure: provider.flightEnd,
        price: provider.price
      }));
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
  computed: {
    totalPrice() {
      return this.request.routes!.reduce((total, route) => {
        return total + (route.price || 0); // Adding price, defaulting to 0 if route.price is undefined or null
      }, 0)
    }
  }
})
</script>

<template>
  <transition name="resize-transition">
    <div :class="{
    'p-10': route.priceListId,
    'p-4': !route.priceListId
  }" class="transition-all duration-500 ease-out">

      <!-- Content here -->
      <div v-if="route.priceListId" class="p-6 sm:p-8 lg:p-10 space-y-8 max-w-full sm:max-w-lg lg:max-w-xl mx-auto">
        <!-- Header -->
        <h2 class="text-3xl font-bold text-gray-800 tracking-wide mb-6">Reservation</h2>

        <!-- User Details Form -->
        <div class="bg-white p-6 shadow-lg rounded-lg">
          <div class="grid grid-cols-1 sm:grid-cols-2 gap-6">
            <!-- First Name Input -->
            <div class="flex flex-col">
              <label for="firstName" class="text-gray-700 font-medium">First Name</label>
              <InputText
                  id="firstName"
                  v-model="request.name!.firstName"
                  class="border border-gray-300 bg-white text-black rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-indigo-600 focus:border-indigo-600"
                  placeholder="First Name"
              />
            </div>

            <!-- Last Name Input -->
            <div class="flex flex-col">
              <label for="lastName" class="text-gray-700 font-medium">Last Name</label>
              <InputText
                  id="lastName"
                  v-model="request.name!.lastName"
                  class="border border-gray-300 bg-white text-black rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-indigo-600 focus:border-indigo-600"
                  placeholder="Last Name"
              />
            </div>
          </div>
        </div>

        <!-- Route Details -->
        <div v-if="route.routes" class="space-y-6 mt-6">
          <div v-for="(leg, legIndex) in route.routes" :key="legIndex"
               class="bg-white shadow-lg rounded-lg p-6 transition-transform duration-300">
            <!-- Route Overview -->
            <div class="flex justify-between items-center border-b pb-4 mb-4">
              <div>
                <p class="text-lg text-gray-700 font-medium">
                  <span class="text-indigo-600 font-bold">{{ leg.from?.name }}</span>
                  →
                  <span class="text-indigo-600 font-bold">{{ leg.to?.name }}</span>
                </p>
                <p class="text-md text-gray-500">Price: ${{ getProviderInfo(leg.id!)?.price ?? "-" }}</p>
                <p class="text-md text-gray-500">Company: {{ getProviderInfo(leg.id!)?.company?.name ?? "-" }}</p>
                <p class="text-md text-gray-500">Depart at: {{ formatDate(getProviderInfo(leg.id!)?.flightStart) }}</p>
                <p class="text-md text-gray-500">Arrive at: {{ formatDate(getProviderInfo(leg.id!)?.flightEnd) }}</p>
              </div>
            </div>
            <Button
                type="button"
                @click="selectFlight(leg.id!)"
                label="Select Flight"
                class="mt-4 w-full bg-indigo-600 text-white py-3 rounded-lg hover:bg-indigo-700 transition"
            />
          </div>
        </div>

        <!-- Total Price and Confirm Button -->
        <div class="bg-white shadow-lg rounded-lg p-6 text-center mt-6">
          <h1 class="text-lg font-semibold text-gray-700 mb-4">
            Total: <span class="text-indigo-600 font-bold">${{ totalPrice.toFixed(2) }}</span>
          </h1>
          <Button
              type="button"
              @click="confirmReservation()"
              label="Confirm"
              class="mt-4 w-full bg-indigo-600 text-white py-3 rounded-lg hover:bg-indigo-700 transition"
          />
        </div>
      </div>

      <!-- No Route Selected -->
      <div v-else>
        <div class="flex items-center justify-center m-2 lg:m-2 h-screen">
          <p class="text-lg text-gray-500">Select a flight</p>
        </div>
      </div>

    </div>
  </transition>
  <provider-select-modal @provider-select="(e, e2) => setLegProvider(e, e2)" ref="modal"></provider-select-modal>
</template>

<style scoped>

</style>