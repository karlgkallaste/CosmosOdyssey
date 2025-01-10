<script lang="ts">
import {defineComponent, PropType} from 'vue'
import {api} from "../../apiClients.generated.ts";

export default defineComponent({
  name: "Reservation",
  props: {
    id: {
      type: String, // Declare the prop type
      required: true,
    },
  },
  mounted() {
    console.log(this.$props.id);
  }
})
</script>

<template>
  
  <h1>reservation</h1>
</template>

<style scoped>

</style>