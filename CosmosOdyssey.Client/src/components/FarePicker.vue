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
    }
  },
  mounted() {
    this.$watch(
        () => [this.from, this.to],
        ([newFrom, newTo]) => {
          if (newFrom && newTo) {
            this.$emit('values-ready', {from: (newFrom as LocationModel).name, to: (newTo as LocationModel).name});
          }
        },
        {immediate: false}
    );
  }
})
</script>

<template>
  <div class="flex flex-col sm:flex-row items-center justify-center mt-10 space-y-4 sm:space-y-0 sm:space-x-4">
    <!-- "From" Select Component -->
    <div class="relative w-full sm:w-auto">
      <Select
          v-model="from"
          :options="locations"
          optionLabel="name"
          class="w-full sm:w-[300px] bg-indigo-800 font-extrabold text-indigo-100 
           rounded-lg px-4 py-2 shadow-lg hover:bg-indigo-400 
           focus:outline-none focus:ring-2 focus:ring-indigo-500"
      >
        <!-- Custom Label Slot -->
        <template #value="slotProps">
          <span v-if="!slotProps.value" class="text-white">Where from?</span>
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
          class="w-full sm:w-[300px] bg-indigo-800 font-extrabold text-indigo-100 
           rounded-lg px-4 py-2 shadow-lg hover:bg-indigo-400 
           focus:outline-none focus:ring-2 focus:ring-indigo-500"
      >
        <!-- Custom Label Slot -->
        <template #value="slotProps">
          <span v-if="!slotProps.value" class="text-white">Where to?</span>
          <span v-else class="font-bold text-white">{{ slotProps.value.name }}</span>
        </template>
      </Select>
    </div>
  </div>
</template>

<style scoped>

</style>