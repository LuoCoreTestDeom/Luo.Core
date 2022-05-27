import confirmDialog from '@/components/ConfirmDialog.vue' 
import { h, Ref, render, VNode } from 'vue'

export type dialogProps = {
    title: string
    msg: string,
    isDialogShow: Ref<Boolean>
}

const renderMessage = (props: dialogProps): VNode => {
    const container = document.createElement('div')
    container.id = "asd123";
    // 创建 虚拟dom
    const messageVNode = h(confirmDialog, {
        title: props.title,
        msg: props.msg,
        isDialogShow: props.isDialogShow,
        onCloseDialog:(e:boolean)=>{
            props.isDialogShow.value=e;
        }
    });
    // 将虚拟dom渲染到 container dom 上
    render(messageVNode, container)
    // 最后将 container 追加到 body 上
    document.body.appendChild(container)
    return messageVNode;
};
export default renderMessage