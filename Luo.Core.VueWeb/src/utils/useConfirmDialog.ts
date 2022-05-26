import confirmDialog from '@/components/ConfirmDialog.vue' 
import { h, render, VNode } from 'vue'

export type dialogProps = {
    title: string
    msg: string,
    isDialogShow: Boolean
}


type MessageFnc = (message: string) => void
type MessageProp = ((message: string | dialogProps) => void) & {
    success: MessageFnc
    warning: MessageFnc
    info: MessageFnc
    error: MessageFnc
}




const messageBoxId = 'MESSAGE-ID'

const renderMessage = (): VNode => {
    const props: dialogProps = {
        title: "aasdfas",
        msg:"123",
        isDialogShow:true
    }
    const container = document.createElement('div')
    container.id = messageBoxId
   
    // 创建 虚拟dom
    const messageVNode = h(confirmDialog, props)
    // 将虚拟dom渲染到 container dom 上
    render(messageVNode, container)
    // 最后将 container 追加到 body 上
    document.body.appendChild(container)

    return messageVNode
}






export default renderMessage