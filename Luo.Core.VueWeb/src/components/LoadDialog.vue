<script setup lang="ts">
import {  Ref,toRefs, watch } from 'vue';
  
    const props = defineProps<{isDialogShow:Ref<Boolean>}>();
    const {  isDialogShow } = toRefs(props);
    console.log("加載等待值："+isDialogShow.value);
    watch(isDialogShow, (newValue, oldValue) => {
      debugger;
      console.log("加載等待值：","新值："+newValue, "旧值："+oldValue);
    });
   
  </script>
<template>
    <div class="modal fade modal-blur show" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel"
        role="dialog" :style="{'display':isDialogShow?'block':'none'}">
        <div class="modal-dialog  modal-dialog-centered" role="document">
          <div class="box">
            <div class="spinner-grow box" style="width: 5rem; height: 5rem;" role="status">
              <span class="visually-hidden">Loading...+{{isDialogShow}}</span>
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