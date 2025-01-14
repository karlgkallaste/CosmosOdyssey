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
          query: {id: response}
        });
      }).catch(e => {
        if (e.status == 400) {
          const parsedResponse = JSON.parse(e.response);
          parsedResponse.forEach((error: { errorMessage: string }) => {
            this.$toast.add({
              severity: 'error',
              summary: 'Validation Error',
              detail: error.errorMessage, // Display each error message in the toast
              life: 3000, // Toast duration
            });
          });
        } else {
          throw e;
        }
      });
    },
  },
})
</script>


<template>
  <div class="flex flex-col sm:flex-row gap-4">
    <div class="flex-1 p-4 bg-gray-100 rounded-lg shadow-md">
      <route-list @create-reservation="(selectedRoute, id) => setRouteInfo(selectedRoute, id)"
                  :to="to" :from="from"
                  :depart-date="departDate.toISOString()"></route-list>
    </div>
    <div class="p-4 bg-gray-100 rounded-lg shadow-md">
      <reservation-tab @reservation-confirmed="(reservation) => createReservation(reservation)"
                       :priceListId="priceListId" :route="route"></reservation-tab>

    </div>
  </div>
</template>


<style scoped>

</style>