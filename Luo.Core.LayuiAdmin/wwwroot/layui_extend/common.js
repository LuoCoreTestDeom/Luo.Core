/**

 @Name：layuiAdmin 公共业务
 @Author：贤心
 @Site：http://www.layui.com/admin/
 @License：LPPL
    
 */
 
layui.define(function(exports){
  var $ = layui.$
  ,layer = layui.layer
  ,laytpl = layui.laytpl
  ,setter = layui.setter
  ,view = layui.view
  ,admin = layui.admin
  
  //公共业务的逻辑处理可以写在此处，切换任何页面都会执行
  //……
  
  
  
  //退出
    admin.events.logout = function () {
        debugger;
        var msgDialogIndex = layer.msg('正在执行退出，请稍等.....', { shade: 0.3, icon: 16 });
    //执行退出接口
        $.ajax({
            type: "get",
            url: "/User/LoginOut",
            async: false,
            success: function (res) {
                layer.close(msgDialogIndex);
                window.location.href = res;
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
  };

  
  //对外暴露的接口
  exports('common', {});
});