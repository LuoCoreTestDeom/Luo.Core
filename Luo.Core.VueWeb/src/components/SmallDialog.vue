<script setup lang="ts">
    import { ref, reactive, toRefs, watch, defineProps } from 'vue';
    const props = defineProps({
        title: String,
        msg: String,
        isShow: Boolean
    });
    const isShow=toRefs(props).isShow;
    watch(isShow, (newValue, oldValue) => {
        console.log(newValue, oldValue);
    });
</script>
<template>

<div class="modal modal-blur fade show" id="modal-small" tabindex="-1" style="display: block;" aria-modal="true" role="dialog">
      <div class="modal-dialog modal-sm modal-dialog-centered" role="document">
        <div class="modal-content">
          <div class="modal-body">
            <div class="modal-title">Are you sure?</div>
            <div>If you proceed, you will lose all your personal data.</div>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-link link-secondary me-auto" data-bs-dismiss="modal">Cancel</button>
            <button type="button" class="btn btn-danger" data-bs-dismiss="modal">Yes, delete all my data</button>
          </div>
        </div>
      </div>
    </div>

    <div class="modal modal-blur fade show" tabindex="-1" role="dialog" aria-modal="true" :style="{'display':isShow?'block':'none'}">
        <div class="modal-dialog modal-sm modal-dialog-centered" role="document">
            <div class="modal-content">
                <div class="modal-body">
                    <div class="modal-title">{{title}}</div>
                    <div>{{msg}}</div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-link link-secondary me-auto"
                        data-bs-dismiss="modal">Cancel</button>
                </div>
            </div>
        </div>
    </div>




</template>