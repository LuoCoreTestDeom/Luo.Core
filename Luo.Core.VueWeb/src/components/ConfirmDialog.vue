<script setup lang="ts">
  import {  Ref,toRefs, watch } from 'vue';

  const props = defineProps<{title:String,msg:String,isDialogShow: Ref<Boolean>}>();
  const { title, msg, isDialogShow } = toRefs(props);
  watch(isDialogShow, (newValue, oldValue) => {
    console.log(newValue, oldValue);
  });



  const emit = defineEmits<{(event:"CloseDialog",args:Boolean): void}>()

  const CloseDialog = () => {
   
    emit('CloseDialog',false)
  }
</script>

<template>
  <div class="modal fade modal-blur show" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel"
    role="dialog" :style="{'display':isDialogShow?'block':'none'}">
    <div class="modal-dialog  modal-dialog-centered" role="document">
      <div class="modal-content" style="box-shadow: 2px 2px 20px #888888;">
        <div class="modal-header">
          <h5 class="modal-title" id="exampleModalLabel">{{title}}</h5>
          <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"
            @click="CloseDialog"></button>
        </div>
        <div class="modal-body">
          {{msg}}
        </div>
        <div class="modal-footer" style="justify-content: center;">
          <button type="button" class="btn btn-primary btn-lg" @click="CloseDialog">确认</button>
        </div>
      </div>
    </div>
  </div>
</template>