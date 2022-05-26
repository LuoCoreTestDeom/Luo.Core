<script setup lang="ts">
    import { ref, reactive, toRefs, watch, defineProps } from 'vue';
    const props = defineProps({
      isDialogShow: Boolean
    });
    const {  isDialogShow } = toRefs(props);
    watch(isDialogShow, (newValue, oldValue) => {
      console.log(newValue, oldValue);
    });
   
  </script>
<template>
    <div class="modal fade modal-blur show" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel"
        role="dialog" :style="{'display':isDialogShow?'block':'none'}">
        <div class="modal-dialog  modal-dialog-centered" role="document">
          <div class="box">
            <div class="spinner-grow box" style="width: 5rem; height: 5rem;" role="status">
              <span class="visually-hidden">Loading...</span>
          </div>
          </div>
         
        </div>
    </div>
</template>
<style scoped>
  .box{
    width: 100%;
    -webkit-display:flex;
    display:flex;
    -webkit-align-items:center;
    align-items:center;
    -webkit-justify-content:center;
    justify-content:center;
}
</style>