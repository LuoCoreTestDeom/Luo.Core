<script setup lang="ts">
    import { ref, reactive, toRefs } from 'vue';
    defineProps({
        Title: String, // 提示信息内容
        Content: String, // 提示信息内容
        visibleDialog: Boolean
    });
    
</script>
<template>
    <div class="modal modal-blur fade show" tabindex="-1" role="dialog" aria-modal="true" v-show="visibleDialog">
        <div class="modal-dialog modal-sm modal-dialog-centered" role="document">
            <div class="modal-content">
                <div class="modal-body">
                    <div class="modal-title">{{Title}}</div>
                    <div>{{Content}}</div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-link link-secondary me-auto"
                        data-bs-dismiss="modal">Cancel</button>
                    <button type="button" class="btn btn-danger" data-bs-dismiss="modal">Yes, delete all my
                        data</button>
                </div>
            </div>
        </div>
    </div>
</template>