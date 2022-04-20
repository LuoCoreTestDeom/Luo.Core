// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

layui.config({
    base: '/layui_extend/' //静态资源所在路径
}).use('index', function () {

    layui.admin.deleteLoader(1);
})