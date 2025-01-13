<script lang="ts">
import {defineComponent, type PropType} from 'vue'
import {api} from "../../apiClients.generated.ts";
import LocationModel = api.LocationModel;

export default defineComponent({
  name: "FarePicker",
  emits: ["values-ready"],
  props: {
    locations: {
      type: Array as PropType<LocationModel[]>,
      required: false,
    }
  },
  data() {
    return {
      from: null,
      to: null,
      departDate: null,
    }
  },
  mounted() {
    this.$watch(
        () => [this.from, this.to, this.departDate],
        ([newFrom, newTo, newDepartDate]) => {
          if (newFrom && newTo && newDepartDate) {
            this.$emit('values-ready', {
              from: (newFrom as LocationModel).name,
              to: (newTo as LocationModel).name,
              departDate: (newDepartDate as Date)
            });
          }
        },
        {immediate: false}
    );
  }
})
</script>

<template>
  <div class="flex flex-col sm:flex-row items-center justify-center mt-1 space-y-6 sm:space-y-0 sm:space-x-6">
    <!-- "From" Select Component -->
    <div class="relative w-full sm:w-auto">
      <Select
          v-model="from"
          :options="locations"
          optionLabel="name"
          class="w-full sm:w-[320px] bg-gradient-to-r from-indigo-600 to-indigo-700 text-white font-semibold 
           rounded-lg px-5 py-3 shadow-xl hover:from-indigo-500 hover:to-indigo-600 
           focus:outline-none focus:ring-4 focus:ring-indigo-400 transition-all duration-300 ease-in-out"
      >
        <!-- Custom Label Slot -->
        <template #value="slotProps">
          <span v-if="!slotProps.value" class="text-indigo-200">Where from?</span>
          <span v-else class="font-bold text-white">{{ slotProps.value.name }}</span>
        </template>
      </Select>
    </div>

    <!-- "To" Select Component -->
    <div class="relative w-full sm:w-auto">
      <Select
          v-model="to"
          :options="locations"
          optionLabel="name"
          class="w-full sm:w-[320px] bg-gradient-to-r from-indigo-600 to-indigo-700 text-white font-semibold 
           rounded-lg px-5 py-3 shadow-xl hover:from-indigo-500 hover:to-indigo-600 
           focus:outline-none focus:ring-4 focus:ring-indigo-400 transition-all duration-300 ease-in-out"
      >
        <!-- Custom Label Slot -->
        <template #value="slotProps">
          <span v-if="!slotProps.value" class="text-indigo-200">Where to?</span>
          <span v-else class="font-bold text-white">{{ slotProps.value.name }}</span>
        </template>
      </Select>
    </div>
  </div>

  <!-- New Row for Calendar -->
  <div class="w-full mt-6 flex justify-center">
    <div class="relative w-full sm:w-auto">
      <Calendar
          v-model="departDate"
          class="w-full sm:w-[320px] bg-gradient-to-r from-indigo-600 to-indigo-700 text-white font-semibold 
        rounded-lg px-5 py-3 shadow-xl hover:from-indigo-500 hover:to-indigo-600 
        focus:outline-none focus:ring-4 focus:ring-indigo-400 transition-all duration-300 ease-in-out"
          placeholder="Select Date and Time"
      />
    </div>
  </div>
</template>

<style scoped>

</style>