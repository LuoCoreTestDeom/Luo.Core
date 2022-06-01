import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router'
import Layout from '@/layouts/index.vue'
const routes = [
  {
    path: "/login",
    name: "Login",
    component: () => import('@/views/Login.vue'),
  },
  {
    path: '/',
    name: "首页",
    component:Layout,
    meta: {
      requireAuth: true, // 判断是否需要登录
    },
    redirect:'/dashboard',
    children:[
      {
        path:'/dashboard',
        component: () => import('@/views/dashboard/index.vue'),
        name:'dashboard',
        meta:{
            title:'首页',
            icon:'ti-home',
            requireAuth: true,
        }
      }
    ]
  },
  {
    component:Layout,
    path:'/MyPackage',
    name:'我的包裹',
    meta:{
        title:'我的包裹',
        icon:'ti-package',
        roles:['sys:manage'],
        parentId:0,
        requireAuth: true,
    },
    redirect:'/MyPackage',
    children:[
      {
        path:'/MyPackage',
        component: () => import('@/views/MyPackage.vue'),
        name:'MyPackage',
        meta:{
            title:'我的包裹',
            icon:'ti-package',
            requireAuth: true,
        }
      }
    ]
}
  
]

// 3. 创建路由实例
const router = createRouter({
  //history: createWebHashHistory(), // 表示使用 hash 模式，即 url 会有 # 前缀
  history: createWebHistory(),
  routes
})


// 4. 你还可以监听路由拦截，比如权限验证。
router.beforeEach((to, from, next) => {

  // 1. 每个条件执行后都要跟上 next() 或 使用路由跳转 api 否则页面就会停留一动不动
  // 2. 要合理的搭配条件语句，避免出现路由死循环。
  const token = localStorage.getItem('token')

  if (to.meta.requireAuth) {
    debugger;
    if (!token || token === "undefined") {
      if (to.path != "/login") {
        next("/login");
        return;
      }
    }
    next();
  } else {
    next();
  }
})



export default router;
