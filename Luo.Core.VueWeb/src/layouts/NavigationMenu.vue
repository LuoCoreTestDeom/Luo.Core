 <script setup lang="ts">
 import { useRoute,useRouter } from 'vue-router';
 //获取当前路由
const route = useRoute();
const router = useRouter();
let menuDataList=router.options.routes;
debugger;
 </script>

 <template>
    <div class="navbar-expand-md">
        <div class="collapse navbar-collapse" id="navbar-menu">
            <div class="navbar navbar-light">
                <div class="container-xl">

                    <!--菜单-->
                    <ul class="navbar-nav">
                        <li class="nav-item active">

                            <a class="nav-link" href="/">
                                <span class="nav-link-icon d-md-none d-lg-inline-block">
                                    <i class="ti ti-home icon" width="24" height="24"></i>
                                </span>
                                <span class="nav-link-title">
                                    集运首页
                                </span>
                            </a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="./MyPackage">
                                <span class="nav-link-icon d-md-none d-lg-inline-block">
                                    <i class="ti ti-package icon" width="24" height="24"></i>
                                </span>
                                <span class="nav-link-title">
                                    我的包裹
                                </span>
                            </a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="./form-elements.html">
                                <span class="nav-link-icon d-md-none d-lg-inline-block">
                                    <i class="ti ti-checklist icon" width="24" height="24"></i>
                                </span>
                                <span class="nav-link-title">
                                    我的订单
                                </span>
                            </a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="./form-elements.html">
                                <span class="nav-link-icon d-md-none d-lg-inline-block">
                                    <i class="ti ti-address-book icon" width="24" height="24"></i>
                                </span>
                                <span class="nav-link-title">
                                    地址管理
                                </span>
                            </a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="./form-elements.html">
                                <span class="nav-link-icon d-md-none d-lg-inline-block">
                                    <i class="ti ti-map-search icon" width="24" height="24"></i>
                                </span>
                                <span class="nav-link-title">
                                    物流追踪
                                </span>
                            </a>
                        </li>


                        <li class="nav-item">
                            <a class="nav-link" href="./icons.html">
                                <span class="nav-link-icon d-md-none d-lg-inline-block">
                                    <i class="ti ti-report-money icon" width="24" height="24"></i>
                                </span>
                                <span class="nav-link-title">
                                    我的钱包
                                </span>
                            </a>
                        </li>

                        <li class="nav-item">
                            <a class="nav-link" href="./icons.html">
                                <span class="nav-link-icon d-md-none d-lg-inline-block">
                                    <i class="ti ti-user icon" width="24" height="24"></i>
                                </span>
                                <span class="nav-link-title">
                                    个人信息
                                </span>
                            </a>
                        </li>


                        <li class="nav-item">
                            <a class="nav-link" href="./icons.html">
                                <span class="nav-link-icon d-md-none d-lg-inline-block">
                                    <i class="ti ti-share icon" width="24" height="24"></i>
                                </span>
                                <span class="nav-link-title">
                                    分享推广
                                </span>
                            </a>
                        </li>

                        <li class="nav-item">
                            <a class="nav-link" href="./icons.html">
                                <span class="nav-link-icon d-md-none d-lg-inline-block">
                                    <i class="ti ti-ad-2 icon" width="24" height="24"></i>
                                </span>
                                <span class="nav-link-title">
                                    系统公告
                                </span>
                            </a>
                        </li>

                    </ul>
                    <!--菜单-->


                </div>
            </div>
        </div>
    </div>

</template>