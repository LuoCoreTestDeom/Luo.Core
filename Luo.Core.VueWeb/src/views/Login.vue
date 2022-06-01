<template>

  <div class=" border-top-wide border-primary d-flex flex-column">
    <div class="page page-center">
      <div class="container-tight py-4">
        <div class="text-center mb-4">
          <a href="." class="navbar-brand navbar-brand-autodark"><img src="../assets/logo.png" height="36" alt=""></a>
        </div>
        <form class="card card-md" action="." method="get" autocomplete="off">
          <div class="card-body">
            <h2 class="card-title text-center mb-4">会员中心</h2>
            <div class="mb-3">
              <label class="form-label">用户名</label>
              <input type="email" class="form-control" v-model="account" placeholder="用户名" autocomplete="off">
            </div>
            <div class="mb-2">
              <label class="form-label">
                密码
                <span class="form-label-description">
                  <a href="./forgot-password.html">我忘记了密码</a>
                </span>
              </label>
              <div class="input-group input-group-flat">
                <input type="密码" class="form-control" placeholder="密码" autocomplete="off" v-model="password">
                <span class="input-group-text">
                  <a href="#" class="link-secondary" title="Show password" data-bs-toggle="tooltip">
                  </a>
                </span>
              </div>
            </div>
            <div class="mb-2">
              <label class="form-check">
                <input type="checkbox" class="form-check-input" />
                <span class="form-check-label">记住密码</span>
              </label>
            </div>
            <div class="form-footer">
              <button type="button" class="btn btn-primary w-100" @click="BtnSubmit()">登录</button>
            </div>
          </div>

        </form>
        <div class="text-center text-muted mt-3">
          还没有账号? <a href="./sign-up.html" tabindex="-1">注册</a>
        </div>




      </div>
    </div>

  </div>

</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRoute, useRouter } from 'vue-router';
import { AxiosError } from 'axios';
import { useStore } from "vuex";
import confirmDialog from '@/utils/useConfirmDialog'
import httpRequest from '@/network/httpRequest';
import JwtResponseDto from '@/models/memberLogin'


//显示弹框
const ShowConfirmDialog = (msg: string, title: string = "温馨提示") => {
  confirmDialog({
    title: title,
    msg: msg,
    isDialogShow: ref(true)
  });
}


//获取当前路由
const route = useRoute();//获取路由参数
const router = useRouter();//跳转
const store = useStore();

let account = ref("");
let password = ref("");
//点击登录
function BtnSubmit() {

 var sss= store.state.token;

  var reqData = JSON.stringify({
    "account": account.value,
    "password": password.value,
    "token": "string"
  });

  httpRequest.request<JwtResponseDto>({
    url: 'https://localhost:7096/api/v1/Member/Login',
    method: 'Post',
    headers: {
      "Content-Type": "application/json"
    },
    data: reqData,
    showLoading: true,
    interceptors: {
      requestInterceptor: (config) => {
        console.log('单独的请求的config拦截')
        return config
      },
      responseInterceptor: (res) => {
        console.log('单独响应的response')
        return res
      }
    }
  }).then((res) => {
    console.log("成功：" + res);
    if (res instanceof AxiosError) {
      ShowConfirmDialog("请求发生异常：" + res.message);
    }
    else {
      debugger;
      if (!res.profile) {
        ShowConfirmDialog("请求失败：" + res.access);
      }
      else{
         store.commit("setToken",res.access);
        router.push('/');
      }
    }
  }).catch(res => {
    console.log("失败：" + res);
    ShowConfirmDialog("请求发生异常：" + res);
  });




}
</script>