<script setup lang="ts">
    import { ref, reactive, toRefs, watch, defineProps } from 'vue';
    const props = defineProps({
      isShow: Boolean
    });
    const {  isShow } = toRefs(props);
    watch(isShow, (newValue, oldValue) => {
      console.log(newValue, oldValue);
    });
   
  </script>
<template>
    <div class="modal fade modal-blur show" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel"
        role="dialog" :style="{'display':isShow?'block':'none'}">
        <div class="modal-dialog  modal-dialog-centered" role="document">
          <div class="spinner-grow modal-content" style="width: 5rem; height: 5rem;" role="status">
            <span class="visually-hidden">Loading...</span>
        </div>
        </div>
    </div>
</template>