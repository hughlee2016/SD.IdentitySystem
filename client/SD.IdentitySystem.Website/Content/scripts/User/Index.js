﻿//定义全局变量$tbGrid，用于刷新Grid
var $tbGrid = null;

//DOM初始化事件
$(function () {
    //加载列表
    searchUsers();
});

//加载用户列表
function getUsers(queryParams) {
    //指定url
    $.easyuiExt.datagrid.url = "/User/GetUsers";

    //定义列
    $.easyuiExt.datagrid.columns = [
        [
            { field: "ck", checkbox: true, halign: "center" },
            { field: "Id", title: "Id", halign: "center", hidden: true },
            { field: "Number", title: "用户名", halign: "center", width: 100 },
            { field: "Name", title: "真实姓名", halign: "center", width: 120 },
            {
                field: "AddedTime",
                title: "创建时间",
                align: "center",
                halign: "center",
                width: $.global.getRelativeWidth(15, $("#dvGrid")),
                formatter: function (value) {
                    return $.global.formatDate(value, "yyyy-MM-dd hh:mm:ss");
                }
            },
            {
                field: "Update",
                title: "编辑",
                width: 35,
                formatter: function (value, row) {
                    var start = '<a class="aLink" href="javascript: updateUser(\'';
                    var end = '\');" >编辑</a>';
                    var element = start + row.Number + end;

                    return element;
                }
            },
            {
                field: "Remove",
                title: "删除",
                width: 35,
                formatter: function (value, row) {
                    var start = '<a class="aLink" href="javascript: removeUser(\'';
                    var end = '\');" >删除</a>';
                    var element = start + row.Number + end;

                    return element;
                }
            }
        ]
    ];

    //初始化工具栏
    $.easyuiExt.datagrid.toolbarId = "#toolbar";

    //绑定参数模型
    $.easyuiExt.datagrid.queryParams = queryParams;

    //绑定$tbGrid
    $tbGrid = $("#tbGrid").datagrid($.easyuiExt.datagrid);
}

//创建用户
function createUser() {
    $.easyuiExt.showWindow("创建用户", "/User/Add", 360, 420);
}

//修改用户
function updateUser(loginId) {
    $.easyuiExt.showWindow("修改用户", "/User/Update/" + loginId, 360, 420);
}

//删除用户
function removeUser(loginId) {
    $.messager.confirm("Warning", "确定要删除吗？", function (confirm) {
        if (confirm) {
            $.ajax({
                type: "post",
                url: "/User/RemoveUser/" + loginId,
                success: function () {
                    $.messager.alert("OK", "删除成功！");
                    $.easyuiExt.updateGridInTab();
                },
                error: function (error) {
                    $.messager.alert("Error", error.responseText);
                }
            });
        }
    });
}

//删除选中用户
function removeUsers() {
    //获取所有的选中行
    var selectedRows = $("#tbGrid").datagrid("getSelections");

    //判断用户有没有选中
    if (selectedRows.length > 0) {
        $.messager.confirm("Warning", "确定要删除吗？", function (confirm) {
            if (confirm) {
                //填充用户登录名数组
                var selectedLoginIds = [];
                for (var i = 0; i < selectedRows.length; i++) {
                    selectedLoginIds.push(selectedRows[i].Number);
                }

                //JSON格式转换
                var params = $.global.appendArray(null, selectedLoginIds, "loginIds");

                $.ajax({
                    type: "POST",
                    url: "/User/RemoveUsers",
                    data: params,
                    success: function () {
                        $.messager.alert("OK", "删除成功！");
                        $.easyuiExt.updateGridInTab();
                    },
                    error: function (error) {
                        $.messager.alert("Error", error.responseText);
                    }
                });
            }
        });
    }
    else {
        $.messager.alert("警告", "请选中要删除的用户！");
    }
}

//搜索
function searchUsers() {
    var form = $("#frmQueries");
    var queryParams = $.global.formatForm(form);

    getUsers(queryParams);
}