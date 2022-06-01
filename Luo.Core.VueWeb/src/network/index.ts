import axios from 'axios'
import type { AxiosInstance } from 'axios'
import { RequestInterceptors, RequestConfig } from './type'
import LoadingFrame from '@/utils/useLoadDialog';

// import {LoadingOptionsResolved} from 'element-plus/lib/components/loading/src/types'
const DEFAULT_LOADING = true
class ApiRequest {
    instance: AxiosInstance
    interceptors?: RequestInterceptors
    showLoading: boolean
    loading?: LoadingFrame

    constructor(config: RequestConfig) {
        //1.创建axios实例
        this.instance = axios.create(config)
        //2.保存基本信息
        this.interceptors = config.interceptors
        this.showLoading = config.showLoading ?? DEFAULT_LOADING

        //3.使用拦截器
        //3.1.从config中取出的拦截器是对应的实例的拦截器
        this.instance.interceptors.request.use(
            this.interceptors?.requestInterceptor,
            this.interceptors?.requestInterceptorCatch
        )
        this.instance.interceptors.response.use(
            this.interceptors?.responseInterceptor,
            this.interceptors?.responseInterceptorCatch
        )

        //3.2.添加所有实例的拦截器
        this.instance.interceptors.request.use(
            (config) => {
                console.log('所有实例都有的拦截:请求拦截成功')
                if (this.showLoading) {
                    this.loading =new LoadingFrame();
                    this.loading.ShowLoad()
                    console.log('请求加载中：加载中...')
                }
                return config
            },
            (err) => {
                console.log('所有实例都有的拦截：请求响应失败')
                return err
            }
        )
        this.instance.interceptors.response.use(
            (res) => {
                console.log('所有实例都有的拦截：请求响应成功')
                this.loading?.CloseLoad()
                const data = res.data
   
                if(data===undefined){
                    return res
                }
                if (data.returnCode === '-1001') {
                    console.log('请求失败~, 错误信息')
                } else {
                    return data
                }
            },
            (err) => {
                console.log('所有实例都有的拦截：请求响应失败')
                // 例子: 判断不同的HttpErrorCode显示不同的错误信息
                this.loading?.CloseLoad()
                if (err.response.status === 404) {
                    console.log('404的错误~')
                }
                return err
            }
        )
    }
    request<T>(config: RequestConfig<T>): Promise<T> {
        return new Promise((resolve, reject) => {
            //1.判断是否需要显示loading
            if (config.showLoading === false) {
                this.showLoading = config.showLoading
            }
            //2.单个请求对config的处理
            if (config.interceptors?.requestInterceptor) {
                config = config.interceptors.requestInterceptor(config)
            }
            this.instance
                .request<any, T>(config)
                .then((res) => {
                    //1.单个请求对数据的处理
                    if (config.interceptors?.responseInterceptor) {
                        res = config.interceptors.responseInterceptor(res)
                    }

                    //2.将loading恢复默认值，避免影响下个请求
                    this.showLoading = DEFAULT_LOADING

                    //3.返回结果
                    resolve(res)
                })
                .catch((err) => {
                    //3.将loading恢复默认值，避免影响下个请求
                    this.showLoading = DEFAULT_LOADING
                    reject(err)
                })
        })
    }

    get<T>(config: RequestConfig<T>): Promise<T> {
        return this.request<T>({ ...config, method: 'GET' })
    }
    post<T>(config: RequestConfig<T>): Promise<T> {
        return this.request<T>({ ...config, method: 'POST' })
    }
    delete<T>(config: RequestConfig<T>): Promise<T> {
        return this.request<T>({ ...config, method: 'DELETE' })
    }
    patch<T>(config: RequestConfig<T>): Promise<T> {
        return this.request<T>({ ...config, method: 'PATCH' })
    }
}

export default ApiRequest

