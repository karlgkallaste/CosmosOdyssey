<script lang="ts">
import {defineComponent} from 'vue'
import {api} from "../../apiClients.generated.ts";
import ReservationModal from '../components/ReservationModal.vue';

export default defineComponent({
  name: "Legs",
  components: {
    ReservationModal
  },
  props: {
    from: {
      type: String,
      required: true,
    },
    to: {
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
    new api.LegClient().legs(this.$props.from, this.$props.to, undefined, undefined).then(routes => {
      this.routes = routes;
    })
  },
  methods: {
    // navigateToReservation(route: api.RouteListItemModel){
    //   this.$router.push({name: 'Reservation', params:{fare: route}})
    // },
    createReservation(route: api.RouteListItemModel) {
      (this.$refs.modal as any).open(route);
    },
  }
})
</script>

<template>
  <div v-for="(route, index) in routes" :key="index" class="flex items-center justify-center p-5">
    <div class="p-6 bg-gray-100 border border-gray-100 rounded-lg shadow-lg w-full max-w-6xl">
      <h2 class="text-xl text-indigo-400 font-mono font-bold mb-1 text-left">Option {{ index + 1 }}</h2>
      <p class="text-xl text-indigo-400 font-mono mb-2 text-left">Number of transfers: {{ route.routes?.length || 0 }}</p>

      <!-- Stack Legs Vertically -->
      <div v-for="(leg, legIndex) in route.routes" :key="legIndex"
           class="bg-gray-200 border border-gray-400 rounded-lg p-3 shadow-md mb-4">
        <div class="flex items-center justify-between">
          <!-- From - To -->
          <p class="text-gray-600 font-semibold text-sm">
            <span class="text-indigo-400">{{ leg.from?.name }}</span> - <span class="text-indigo-600">{{
              leg.to?.name
            }}</span>
          </p>
        </div>

        <!-- Accordion for Provider Details -->
        <Accordion value="0" expandIcon="pi pi-plus" collapseIcon="pi pi-minus">
          <AccordionPanel value="2">
            <AccordionHeader>
              <span class="flex items-center gap-2 w-full text-indigo-400">
                <span class="font-bold font-mono whitespace-nowrap">PROVIDERS AND PRICES</span>
            </span>
            </AccordionHeader>
            <AccordionContent class="">
              <div v-if="leg.providers?.length" class="mt-3 bg">
                <div v-for="(provider, providerIndex) in leg.providers" :key="providerIndex"
                     class="flex justify-between border-b  text-xs">
                  <p><strong>{{ provider.company?.name }}</strong></p>
                  <p>Start:{{ provider.flightStart?.toLocaleDateString('en-GB') }}</p>
                  <p>End: {{ provider.flightEnd?.toLocaleDateString('en-GB') }}</p>
                  <p class="font-semibold">{{ provider.price }}</p>
                </div>
              </div>
            </AccordionContent>
          </AccordionPanel>
        </Accordion>
      </div>
      <Button @click="createReservation(route)" type="button" label="Reserve" icon="pi pi-ticket"
              class="w-full bg-indigo-400 font-mono font-bold mt-5 h-10 text-white hover:bg-indigo-700"/>

    </div>
  </div>
  <reservation-modal ref="modal"></reservation-modal>
</template>
<style scoped>

</style>