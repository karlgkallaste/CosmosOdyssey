<script lang="ts">
import {defineComponent, PropType} from 'vue'
// @ts-ignore
import {api} from "../../apiClients.generated.ts";

export default defineComponent({
  name: "Reservation",

  data() {
    return {
      reservationId: "",
      reservation: {} as api.ReservationDetailsModel
    }
  },
  created() {
    this.reservationId = this.$route.query.id as string;
    this.fetchReservation(this.reservationId)
  },
  methods: {
    fetchReservation(id: string) {
      new api.ReservationClient().get(id).then(response => {
        this.reservation = response
      })
    }
  },
  computed:{
    totalPrice(){
      return this.reservation.routes ? this.reservation.routes.reduce((total, route) => {
        return total + (Number.parseFloat(route.price!.toString()) || 0); // Adding price, defaulting to 0 if route.price is undefined or null
      }, 0) : 0
    }
  }
})
</script>

<template>
  <div class="flex flex-col items-center py-10 px-6 sm:px-12 lg:px-20 rounded-xl shadow-lg text-white">
    <!-- Title -->
    <h1 class="text-4xl sm:text-5xl lg:text-6xl font-extrabold tracking-tight text-white mb-6">
      Reservation
    </h1>

    <!-- Customer Information -->
    <p class="text-lg sm:text-xl font-medium text-indigo-100 mb-4">
      {{ reservation.customer?.firstName }} {{ reservation.customer?.lastName }}
    </p>

    <!-- Routes Section -->
    <div v-if="reservation.routes" class="w-full max-w-4xl space-y-4 mt-6">
      <div
          v-for="(route, index) in reservation.routes"
          :key="index"
          class="flex justify-between items-center bg-indigo-600/80 px-6 py-4 rounded-lg shadow-sm hover:shadow-lg transition-shadow"
      >
        <span class="text-base sm:text-lg font-semibold">{{ route.from }} → {{ route.to }}</span>
        <span class="text-sm sm:text-base text-gray-200">{{ route.time?.toFixed(0) }} hours</span>
        <span class="text-base sm:text-lg font-bold">${{ route.price!.toFixed(2) }}</span>
      </div>
    </div>

    <!-- Total Price -->
    <div class="mt-8 text-center">
      <p class="text-2xl sm:text-3xl font-extrabold text-white">
        Total: ${{ totalPrice.toFixed(2) }}
      </p>
    </div>
  </div>
</template>

<style scoped>

</style>