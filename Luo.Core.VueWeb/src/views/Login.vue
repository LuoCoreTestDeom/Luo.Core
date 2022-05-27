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




        <!--加载框-->
        <loadDialog :isDialogShow="dialogLoadIsShow">
        </loadDialog>
      </div>
    </div>

  </div>

</template>

<script setup lang="ts">
import { ref, reactive } from 'vue'
import { useRoute, useRouter } from 'vue-router';
import axios from 'axios';
import { useStore } from "vuex";
import loadDialog from '@/components/LoadDialog.vue'

import userConfirmDialog from '@/utils/useConfirmDialog';
import userLaodDialog from '@/utils/useLoadDialog';

//获取当前路由
const route = useRoute();//获取路由参数
const router = useRouter();//跳转
const store = useStore();

let account = ref("");
let password = ref("");
//点击登录
function BtnSubmit() {



  debugger;

  const tokenValue = localStorage.getItem('token')

  userLaodDialog.ShowLoad();
  var reqData = JSON.stringify({
    "account": account.value,
    "password": password.value,
    "token": "string"
  });
  debugger;
  axios({
    method: "post",
    headers: {
      "Content-Type": "application/json"
    },
    url: "https://localhost:7096/api/v1/Member/Login",
    data: reqData,
  }).then(res => {

    userLaodDialog.CloseLoad();
    console.log(res);
    if (res.data.profile == null) {
      userConfirmDialog({
        title: "请求失败",
        msg: "错误消息：" + res.data.access,
        isDialogShow: ref(true)
      });
    }
    else {
      debugger;
      store.commit('setToken', res.data.access);
      router.push('/')
    }
  }).catch(result => {
    debugger;
    userLaodDialog.CloseLoad();
    userConfirmDialog({
      title: "请求失败",
      msg: "错误消息：" + result.message + "，错误代码：" + result.response.status + ",返回错误信息：" + result.response.statusText,
      isDialogShow: ref(true)
    });
  })

}
</script>