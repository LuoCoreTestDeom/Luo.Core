

layui.use(['form', 'table', 'laydate', 'layer'], function () {
    var $ = layui.jquery,
        form = layui.form,
        table = layui.table,
        laydate = layui.laydate;

    
    const nowData = new Date();

    laydate.render({
        elem: '#TimeStart',
        type: 'datetime',
        format: 'yyyy-MM-dd HH:mm:ss',
        value: nowData.format("yyyy-MM-dd hh:mm:ss")
    });
    const nowDateEnd =new Date(nowData.setDate(nowData.getDate() + 1));

    laydate.render({
        elem: '#TimeEnd',
        type: 'datetime',
        format: 'yyyy-MM-dd HH:mm:ss',
        value: nowDateEnd.format("yyyy-MM-dd hh:mm:ss")
    });
    form.render();

    const tempDataTable = table.render({
        elem: '#currentTable',
        toolbar: '#toolbarEvent',
        defaultToolbar: ['filter'],
        cellMinWidth: 80,
        loading: false,

        cols: [[
            { type: "checkbox", width: '5%' },
            { field: 'userId', width: '10%', title: 'ID', sort: true },
            { field: 'userName', width: '15%', title: '用户名' },
            { field: 'createTime', width: '30%', title: '创建时间', sort: true },
            { field: 'createName', width: '20%', title: '创建人', sort: true },
            { title: '操作', width: '20%', toolbar: '#currentTableBar', align: "center" }
        ]],

        limits: [10, 15, 20, 25, 50, 100],
        limit: 15,
        page: true,
        skin: 'row',

        text: {
            none: '暂无相关数据' //默认：无数据。
        },
        request: {
            pageName: 'PageIndex',//页码的参数名称，默认：page
            limitName: 'PageCount' //每页数据量的参数名，默认：limit
        },
        response: {
            statusCode: 200 //规定成功的状态码，默认：0
        }
    });


    //头工具栏事件
    table.on('toolbar(currentTableFilter)', function (obj) {
        var checkStatus = table.checkStatus(obj.config.id);
        switch (obj.event) {
            case 'btnQuery':
                getDataTable();
                break;
            case 'btnAdd':
                layer.open({
                    title:"添加用户",
                    area: ['450px', '500px'],
                    anim: 1,//弹出动画
                    resize: false,//是否允许拉伸
                    type: 2,
                    content: '/SystemConfig/UserInfo',
                    success: function (layero, index) {
                        //对加载后的iframe进行宽高度自适应
                        layer.iframeAuto(index)
                    }
                });
                break;
            case 'btnDelete':
                if (checkStatus.data.length <= 0) {
                    layer.msg('最少选择一行');
                    return;
                }
                layer.confirm('真的删除行么', function (index) {
                    let userIds = [];
                    checkStatus.data.forEach(function (item) {
                        userIds.push(item.userId)
                    });
                    deleteUser(userIds);
                    layer.close(index);

                    let tableDT = table.cache.currentTable
                    tableDT.filter(x => x.LAY_CHECKED).splice(1);
                    table.reload("currentTable", {
                        data: tableDT   // 将新数据重新载入表格
                    });
                });
                break;
        };
    });
    //监听行工具事件
    table.on('tool(currentTableFilter)', function (obj) {
        var data = obj.data;
        if (obj.event === 'delete') {
            layer.confirm('真的删除行么', function (index) {
                let userIds = [];
                userIds.push(data.userId)
                deleteUser(userIds);
                layer.close(index);
                obj.del();
            });
        } else if (obj.event === 'edit') {
            layer.open({
                title: "添加用户",
                area: ['450px', '500px'],
                anim: 1,//弹出动画
                resize: false,//是否允许拉伸
                type: 2,
                content: '/SystemConfig/UserInfo',
                success: function (layero, index) {
                    //得到iframe页的窗口对象，执行iframe页的方法：iframeWin.method();
                    var iframe = window[layero.find('iframe')[0]['name']]; 
                    iframe.ParentSetValue(obj.data);
                    //对加载后的iframe进行宽高度自适应
                    layer.iframeAuto(index);
                }
            });
        }
    });

    //查询
    function getDataTable() {
       
        var msgDialogIndex = layer.msg('查询中，请稍等.....', { shade: 0.3,icon: 16 });
        tempDataTable.reload("currentTable", {
            parseData: function (res) { //res 即为原始返回的数据
                return {
                    "code": res.statusCode, //解析接口状态
                    "msg": res.msg, //解析提示文本
                    "count": res.resultData.totalCount, //解析数据长度
                    "data": res.resultData.userInfoList //解析数据列表
                };
            },
            url: "/SystemConfig/QueryUserList",
            method: "Post",
            where: {
                UserName: $('#UserName').val(),
                TimeEnable: $('#TimeEnable').prop("checked"),
                TimeStart: $('#TimeStart').val(),
                TimeEnd: $('#TimeEnd').val(),
            },
            done: function (res, curr, count) {
                //如果是异步请求数据方式，res即为你接口返回的信息。
                //如果是直接赋值的方式，res即为：{data: [], count: 99} data为当前页数据、count为数据总长度
                console.log(res);
                //得到当前页码
                console.log(curr);
                //得到数据总量
                console.log(count);
                layer.close(msgDialogIndex, function () {
                    layer.closeAll('dialog');
                });
            },
            error: function (errorObj, context) {
                //得到当前页码
                console.log(errorObj);
                //得到数据总量
                console.log(context);
                layer.close(msgDialogIndex, function () {
                    layer.closeAll('dialog');
                });
            }
        }, true);
    }

    function deleteUser(reqData)
    {
        $.ajax({
            type: "Post",
            url: "/SystemConfig/DeleteUser",
            data: { req: reqData },
            success: function (data) {
                if (data.status) {
                    layer.msg('删除成功', { icon: 6 });
                }
                else {
                    layer.msg(data.msg, { icon: 5 });
                }
            },
            error: function (jqXHR) {
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