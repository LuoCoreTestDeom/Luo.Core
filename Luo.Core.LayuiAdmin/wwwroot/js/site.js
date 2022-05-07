// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.



layui.config({
    base: '/layui_extend/' //静态资源所在路径
}).use('index', function () {
    layui.admin.deleteLoader(1);
})

Date.prototype.format = function (format) {
    var args = {
        "M+": this.getMonth() + 1,
        "d+": this.getDate(),
        "h+": this.getHours(),
        "m+": this.getMinutes(),
        "s+": this.getSeconds(),
        "q+": Math.floor((this.getMonth() + 3) / 3),  //quarter
        "S": this.getMilliseconds()
    };
    if (/(y+)/.test(format))
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var i in args) {
        var n = args[i];
        if (new RegExp("(" + i + ")").test(format))
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? n : ("00" + n).substr(("" + n).length));
    }
    return format;
};

function LuoAjax(reqUrl, reqType, reqData, funSuccess, reqAsync=true,resDataType=null) {

    layui.use('layer', function () {
        let $ = layui.jquery;
        var msgDialogIndex = layer.msg('正在执行，请稍等.....', { shade: 0.3, icon: 16 });
        if (resDataType === null || resDataType === "") {
            $.ajax({
                type: reqType,
                url: reqUrl,
                async: reqAsync,
                data: reqData,
                success: function (res) {
                    funSuccess(res);
                    layer.close(msgDialogIndex);
                },
                error: function (jqXHR) {
                    layer.close(msgDialogIndex);
                    layer.msg("发生错误：" + jqXHR.status, {
                        time: 0,  //不自动关闭
                        btn: ['确定'],
                        yes: function (index) {
                            layer.close(index);
                        }
                    });
                }
            });
        }
        else {
            $.ajax({
                type: reqType,
                url: reqUrl,
                async: reqAsync,
                data: reqData,
                dataType:resDataType,
                success: function (res) {
                    funSuccess(res);
                    layer.close(msgDialogIndex);
                },
                error: function (jqXHR) {
                    layer.close(msgDialogIndex);
                    layer.msg("发生错误：" + jqXHR.status, {
                        time: 0,  //不自动关闭
                        btn: ['确定'],
                        yes: function (index) {
                            layer.close(index);
                        }
                    });
                }
            });
        }


    });

    
}


