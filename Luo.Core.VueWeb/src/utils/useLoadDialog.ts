import LoadDialog from '@/components/LoadDialog.vue' 
import { h, Ref,ref, render, VNode } from 'vue'

let _isDialogShow:Ref<boolean>=ref(false);

const renderMessage = (isState:boolean): VNode => {
    _isDialogShow.value=isState;
    const container = document.createElement('div')
    // 创建 虚拟dom
    const messageVNode = h(LoadDialog,{isDialogShow:_isDialogShow});
    // 将虚拟dom渲染到 container dom 上
    render(messageVNode, container)
    // 最后将 container 追加到 body 上
    document.body.appendChild(container)
    return messageVNode
}
renderMessage.ShowLoad=()=>renderMessage(true);
renderMessage.CloseLoad=()=>renderMessage(false);
export default renderMessage