import LoadDialog from '@/components/LoadDialog.vue';
import { h, Ref,ref, render, VNode } from 'vue';

class LoadingFrame {
   protected showLoading: Ref<Boolean>=ref(false);

    constructor(){
        this.renderMessage()
        this.ShowLoad();
    }
    renderMessage = (): VNode => {
        const container = document.createElement('div')
        container.id = "loading";
        let ddd=ref(true);
        // 创建 虚拟dom
        const CreateVNode = h(LoadDialog,{isDialogShow: this.showLoading});

        // 将虚拟dom渲染到 container dom 上
        render(CreateVNode, container)
        // 最后将 container 追加到 body 上
        document.body.appendChild(container)
        return CreateVNode
    }
    ShowLoad = () => {
        this.showLoading.value =true;
    }
    CloseLoad = () => {
        this.showLoading.value=false;
    }
}



export default LoadingFrame;