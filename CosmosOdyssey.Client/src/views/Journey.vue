<script lang="ts">
import {defineComponent} from 'vue'
import {api} from "../../apiClients.generated";
import RouteList from "../components/RouteList.vue";
import ReservationTab from "../components/ReservationTab.vue";

export default defineComponent({
  name: "Journey",
  components: {ReservationTab, RouteList},
  data() {
    return {
      to: "",
      from: "",
      departDate: {} as Date,
      route: {} as api.RouteListItemModel,
      priceListId: "",
    }
  },

  created() {
    this.to = this.$route.query.to as string;
    this.from = this.$route.query.from as string;

    const departDateString = this.$route.query.departDate as string | undefined;
    if (departDateString) {
      this.departDate = new Date(departDateString);
    }

  },
  methods: {
    setRouteInfo(route: api.RouteListItemModel, priceListId: string) {
      this.route = route;
      this.priceListId = priceListId;
    },
    createReservation(request: api.CreateReservationRequest) {
      new api.ReservationClient().create(request).then(response => {
        this.$router.push({
          name: 'Reservation',
          params: {id: response}
        });
      }).catch(e => {
        if (e.status == 400) {
          console.error(e.response.data as string);
          console.log(e)
        } else {
          throw e;
        }
      });
    },
  }
})
</script>


<template>
  <div class="flex flex-col sm:flex-row gap-4">
    <div class="flex-1 p-4 bg-gray-100 rounded-lg shadow-md">
      <h2 class="text-xl font-semibold text-gray-700">Available routes</h2>
      <route-list @create-reservation="(selectedRoute, priceListId) => setRouteInfo(selectedRoute, priceListId)" :to="to" :from="from"
                  :depart-date="departDate.toISOString()"></route-list>
    </div>
    <div class="flex-1 p-4 bg-gray-100 rounded-lg shadow-md">
      <reservation-tab @reservation-confirmed="(reservation) => createReservation(reservation)" :priceListId="priceListId" :route="route"></reservation-tab>

    </div>
  </div>
</template>


<style scoped>

</style>