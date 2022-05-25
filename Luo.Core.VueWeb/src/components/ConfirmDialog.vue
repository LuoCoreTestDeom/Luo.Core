<script setup lang="ts">
  import { ref, reactive, toRefs, watch, defineProps, defineEmits } from 'vue';
  const props = defineProps({
    title: String,
    msg: String,
    isShow: Boolean
  });
  const { title, msg, isShow } = toRefs(props);
  watch(isShow, (newValue, oldValue) => {
    console.log(newValue, oldValue);
  });



  const emit = defineEmits(['CloseDialog'])

  function closeDialog() {

    emit('CloseDialog', false);
  }
</script>

<template>
  <div class="modal fade modal-blur show" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel"
    role="dialog" :style="{'display':isShow?'block':'none'}">
    <div class="modal-dialog  modal-dialog-centered" role="document">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="exampleModalLabel">{{title}}</h5>
          <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"
            @click="closeDialog"></button>
        </div>
        <div class="modal-body">
          {{msg}}
        </div>
        <div class="modal-footer" style="justify-content: center;">
          <button type="button" class="btn btn-primary btn-lg" @click="closeDialog">确认</button>
        </div>
      </div>
    </div>
  </div>
</template>