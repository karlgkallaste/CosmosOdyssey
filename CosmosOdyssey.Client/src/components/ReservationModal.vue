<script lang="ts">
import {defineComponent} from 'vue'
import {api} from "../../apiClients.generated.ts";

export default defineComponent({
  name: "ReservationModal",
  data() {
    return {
      visible: false,
      route: {} as api.RouteListItemModel,
      request: {} as api.CreateReservationRequest
    }
  },

  created() {
    this.request = new api.CreateReservationRequest();
    this.request.name = new api.PersonNameModel({firstName: "", lastName: ""})
    //this.request.routes = [] as api.RouteListItemModel[];

  },
  methods: {
    open(route: api.RouteListItemModel) {
      console.log(route.priceListId)
      this.request.priceListId = route.priceListId;
      // this.request.routes = (route.routes || []).map((leg) => {
      //   return {
      //     legId: leg.to?.id, // Map the leg ID from the route
      //     companyId: undefined, // Initialize companyId with null
      //   } as api.ReservationRouteModel;
      // });
      this.visible = true;
    },
    confirmReservation() {
      console.log(this.request.priceListId)
      new api.ReservationClient().create(this.request).then(response => {

      });
    }
  }
})
</script>

<template>
  <Dialog v-model:visible="visible" modal header="Select Providers" :style="{ width: '50rem' }"
          :breakpoints="{ '1199px': '75vw', '575px': '90vw' }">
    <div class="flex gap-4">
      <!-- Username Input -->
      <div class="flex flex-col gap-2 w-1/2">
        <label for="firstName">First name</label>
        <InputText id="username" v-model="request.name!.firstName" aria-describedby="firstName-help"/>
        <Message size="small" severity="secondary" variant="simple">Enter your first name.</Message>
      </div>

      <!-- Last Name Input -->
      <div class="flex flex-col gap-2 w-1/2">
        <label for="lastName">Last name</label>
        <InputText id="lastname" v-model="request.name!.lastName" aria-describedby="lastname-help"/>
        <Message size="small" severity="secondary" variant="simple">Enter your last name.</Message>
      </div>
    </div>

<!--    <div class="flex items-center justify-center p-5">-->
<!--      <div class="p-6 rounded-lg shadow-lg w-full max-w-4xl">-->

<!--        &lt;!&ndash; Stack Legs Vertically &ndash;&gt;-->
<!--        <div v-for="(leg, legIndex) in route.routes" :key="legIndex"-->
<!--             class="bg-gray-200 border border-gray-400 rounded-lg p-3 shadow-md mb-2">-->
<!--          <div class="flex items-start justify-between">-->
<!--            &lt;!&ndash; From - To &ndash;&gt;-->
<!--            <p class="text-gray-600 font-semibold text-sm">-->
<!--              <span class="text-amber-500">{{ leg.from?.name }}</span> - <span-->
<!--                class="text-amber-600">{{ leg.to?.name }}</span>-->
<!--            </p>-->

<!--            <p class="text-amber-500 font-semibold text-sm">Price</p>-->
<!--          </div>-->
<!--          <Select v-model="request.routes[legIndex].companyId" :options="leg.providers" optionLabel="company.name"-->
<!--                  placeholder="Select a City" class="bg-gray-200 border border-gray-400 rounded-xl p-3 shadow-md mb-2"/>-->

<!--        </div>-->
<!--      </div>-->
<!--    </div>-->
    <Button @click="confirmReservation" type="button" label="Confirm"
            class="w-full bg-amber-950 h-10 text-white hover:bg-amber-600"/>
  </Dialog>

</template>

<style scoped>

</style>