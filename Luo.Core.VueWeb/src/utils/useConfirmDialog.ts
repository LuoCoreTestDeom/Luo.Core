import confirmDialog from '@/components/ConfirmDialog.vue'
import { h,ref, Ref, render, VNode } from 'vue'

type CloseDialogClick = (isShowState: Boolean) => void
export type dialogProps = {
    title: String
    msg: String,
    isDialogShow: Ref<Boolean>
}


const renderMessage = (args: dialogProps): VNode => {
    const container = document.createElement('div')
    container.id = "confirm";
    // 创建 虚拟dom
    const messageVNode = h(confirmDialog,
        {
            title:args.title,
            msg:args.msg,
            isDialogShow:args.isDialogShow,
            onCloseDialog:(e:Boolean)=>{
                args.isDialogShow.value=e;
            }
        });
    // 将虚拟dom渲染到 container dom 上
    render(messageVNode, container)
    // 最后将 container 追加到 body 上
    document.body.appendChild(container)
    return messageVNode;
};
export default renderMessage