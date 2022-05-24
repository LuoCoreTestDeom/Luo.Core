<template>
    
    <div  class=" border-top-wide border-primary d-flex flex-column">
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
                  <input type="email" class="form-control" placeholder="用户名" autocomplete="off">
                </div>
                <div class="mb-2">
                  <label class="form-label">
                    密码
                    <span class="form-label-description">
                      <a href="./forgot-password.html">我忘记了密码</a>
                    </span>
                  </label>
                  <div class="input-group input-group-flat">
                    <input type="密码" class="form-control"  placeholder="密码"  autocomplete="off">
                    <span class="input-group-text">
                      <a href="#" class="link-secondary" title="Show password" data-bs-toggle="tooltip">
                      </a>
                    </span>
                  </div>
                </div>
                <div class="mb-2">
                  <label class="form-check">
                    <input type="checkbox" class="form-check-input"/>
                    <span class="form-check-label">记住密码</span>
                  </label>
                </div>
                <div class="form-footer">
                  <button type="button" class="btn btn-primary w-100" @click="plusOne()">登录</button>
                </div>
              </div>
              
            </form>
            <div class="text-center text-muted mt-3">
              还没有账号? <a href="./sign-up.html" tabindex="-1">注册{{isShow}}</a>
            </div>
            <h1 v-show="v-show">是否展示</h1>
           <SmallDialog :title="dialogTitle" :isShow="isShow"></SmallDialog>
          </div>
        </div>
  
    </div>

</template>

<script setup lang="ts">
  import {ref,reactive} from 'vue'
  
  import axios from 'axios';

 import SmallDialog from '@/components/SmallDialog.vue'

 let dialogTitle=ref<String>("传值");
  let  isShow=ref<Boolean>(false);
    
  function plusOne(){
    isShow.value=true;
    dialogTitle.value="111阿斯弗";
    //debugger;
  //  const reqData={account: 'zhangsan', password: '123456789'};
  //   axios({
  //     method:"post",
  //     headers:{
  //       "Content-Type":"application/json"
  //     },
  //     url:"https://localhost:7096/token",
  //     data:reqData,
  //   })
  //   .then(res=>{
  //     debugger;
  //     console.log(res);
  //   })
  //   .catch(result=>{
  //     debugger;
  //       console.log("请求失败："+result.response.status+","+result.response.statusText);
  //   })
    
  }
</script>