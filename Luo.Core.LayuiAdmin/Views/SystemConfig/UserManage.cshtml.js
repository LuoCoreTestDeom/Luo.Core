

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
                getDataTable(obj);
                break;
            case 'btnAdd':
                var data = checkStatus.data;
                const layerIndex = layer.open({
                    area: ['450px', '500px'],
                    type: 2,
                    maxmin: true,
                    content: '/SystemConfig/AddUser'
                });
                //layer.full(layerIndex);
                break;
            case 'btnDelete':
                layer.msg(checkStatus.isAll ? '全选' : '未全选');
                break;
        };
    });


    function getDataTable(obj) {
        debugger;
        var msgDialogIndex = layer.msg('查询中，请稍等.....', { icon: 16, time: false, shade: 0 });
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
                layer.close(msgDialogIndex);
            },
            error: function (errorObj, context) {

                //得到当前页码
                console.log(errorObj);

                //得到数据总量
                console.log(context);
            }
        }, true);
    }




});