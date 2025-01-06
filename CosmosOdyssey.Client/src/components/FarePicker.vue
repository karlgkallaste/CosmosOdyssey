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
  <div class="flex items-center justify-center mt-10 space-x-4">
    <div class="w-full max-w-xs md:w-56">
      <Select v-model="from" :options="locations" key="id" optionLabel="name" placeholder="Select a City"
              class="w-full"/>
    </div>
    <div class="w-full max-w-xs md:w-56">
      <Select v-model="to" :options="locations" optionLabel="name" placeholder="Select a City" class="w-full"/>
    </div>
  </div>
</template>

<style scoped>

</style>