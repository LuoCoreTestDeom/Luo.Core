import axios from 'axios'

axios.defaults.timeout=10000;

const Service=axios.create({
    baseURL:'/'
})

axios.get("",{
    params:{}
})
.then(function(res){
    //当前请求完成时候执行then回调函数
    console.log(res);
})
.catch(resp=>{
    console.log("请求失败："+resp.status+","+resp.statusText);
})