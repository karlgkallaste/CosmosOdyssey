<script lang="ts">
import {defineComponent, PropType} from 'vue'
import {api} from "../../apiClients.generated.ts";

export default defineComponent({
  name: "Reservation",
  props: {
    fare: {
      type: Object as PropType<api.RouteListItemModel>, // Declare the prop type
      required: true,
    },
  },
})
</script>

<template>

</template>

<style scoped>

</style>