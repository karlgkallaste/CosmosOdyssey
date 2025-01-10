<script lang="ts">
import {defineComponent} from 'vue'
import {api} from "../../apiClients.generated.ts";

export default defineComponent({
  name: "ReservationModal",
  data() {
    return {
      visible: false,
      fare: {} as api.RouteListItemModel,
      request: {} as api.CreateReservationRequest,
    }
  },


  methods: {
    open(route: api.RouteListItemModel) {
      this.fare = route;
      this.createEmptyRequest();
      this.visible = true;
    },

    createEmptyRequest() {
      this.request = new api.CreateReservationRequest();
      this.request.name = new api.PersonNameModel({firstName: "", lastName: ""})
      this.request.priceListId = this.fare.priceListId;
      this.request.routes = [];

    },

    confirmReservation() {
      new api.ReservationClient().create(this.request).then(response => {
        this.$router.push({
          name: 'Reservation',
          params: {id: response}
        });
      }).catch(e => {
        if (e.status == 400) {
          console.error(e.response);
          console.log(e)
        } else {
          throw e;
        }
      });
    },

    selectProvider(legIndex: number, companyId: string, legId: string) {
      if (this.request.routes == undefined){
        this.request.routes = [];
      }
      this.request.routes[legIndex] = new api.ReservationRouteModel({legId: legId, companyId: companyId});
    },
    getProviderInfo(legId: string) : api.ProviderInfoModel | undefined {
      if (this.request.routes == undefined){
        return undefined;
      }
      let companyId = this.request.routes.find(x => x.legId === legId)?.companyId;
      const providerInfo = this.fare.routes
          ?.flatMap(route => route.providers) // Flatten all providers across routes
          ?.find(provider => provider?.company?.id === companyId);

      return providerInfo;
      
    }

  },
})
</script>

<template>
  <Dialog v-model:visible="visible" modal header="Select Providers" :style="{ width: '70rem' }"
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

    <div class="flex items-center justify-center p-5">
      <div class="p-6 rounded-lg shadow-lg w-full max-w-4xl">


        <!-- Stack Legs Vertically -->
        <div v-for="(leg, legIndex) in fare.routes" :key="legIndex"
             class="bg-gray-200 border border-gray-400 rounded-lg p-3 shadow-md mb-4">
          <div class="flex items-center justify-between">
            <!-- From - To -->
            <p class="text-gray-600 font-semibold text-sm">
              <span class="text-amber-500">{{ leg.from?.name }}</span> - <span class="text-amber-600">{{
                leg.to?.name
              }}</span>
            </p>
            <p class="text-gray-600 font-semibold text-sm">Price:{{ getProviderInfo(leg.id!)?.price ?? "1" }}</p>
            <p class="text-gray-600 font-semibold text-sm">Company: {{ getProviderInfo(leg.id!)?.company?.name ?? "2" }}</p>
          </div>

          <!-- Accordion for Provider Details -->
          <Accordion value="1" expandIcon="pi pi-plus" collapseIcon="pi pi-minus">
            <AccordionPanel value="2">
              <AccordionHeader>
              <span class="flex items-center gap-2 w-full text-amber-600">
                <span class="font-bold whitespace-nowrap">Providers</span>
            </span>
              </AccordionHeader>
              <AccordionContent class="">
                <DataTable :value="leg.providers" tableStyle="min-width: 20rem">
                  <Column field="company.name" header="Company"></Column>
                  <Column field="flightStart" header="Start">
                    <template v-slot:body="slotProps">
                      {{ new Date(slotProps.data.flightStart).toLocaleDateString('en-US') }}
                    </template>
                  </Column>

                  <Column field="flightEnd" header="Arrival">
                    <template v-slot:body="slotProps">
                      {{ new Date(slotProps.data.flightEnd).toLocaleDateString('en-US') }}
                    </template>
                  </Column>
                  <Column field="price" header="Price">
                    <template v-slot:body="slotProps">
                      ${{ slotProps.data.price.toFixed(2) }}
                    </template>
                  </Column>
                  <Column class="w-24 !text-end">
                    <template #body="{ data }">
                      <Button icon="pi pi-check-circle" @click="selectProvider(legIndex, data.company.id, leg.id!)" severity="secondary"
                              rounded></Button>
                    </template>
                  </Column>
                </DataTable>


              </AccordionContent>
            </AccordionPanel>
          </Accordion>
        </div>
      </div>
    </div>
    <h1>Total: $ totalPrice </h1>
    <Button @click="confirmReservation" type="button" label="Confirm"
            class="w-full bg-amber-950 h-10 text-white hover:bg-amber-600"/>
  </Dialog>

</template>

<style scoped>

</style>