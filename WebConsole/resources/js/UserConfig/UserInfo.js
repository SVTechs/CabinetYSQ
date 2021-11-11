function processGrid(command, dataId) {
    if (command === 'Delete') {
        if (!checkData(dataId)) return;
        deleteUser(dataId);
    }
    else if (command === 'Edit') {
        if (!checkDataAndFill(dataId)) return;
        App.UserWindow.setTitle('编辑用户'); 
        App.UserWindow.show();
    }
    else if (command === 'RoleEdit') {
        if (!checkDataAndFillRole(dataId)) return;
        App.RoleWindow.show();
    }
    else if (command === 'Perm') {
        if (!checkDataAndFillPerm(dataId)) return;
        App.PermWindow.show();
    }
}

function submitUser() {
    App.UserWindow.close();
    var msg = "", selChildren = App.TpDeptList.getChecked();
    Ext.each(selChildren, function (node) {
        if (msg.length > 0) {
            msg += ",";
        }
        msg += node.data.id;
    });
    if (App.UserWindow.title.indexOf('添加') === 0) {
        App.direct.OnAddUserClick(msg, {
            success: function (result) {
                App.UserGrid.store.reload();
            }
        });
    } else {
        App.direct.OnEdtUserClick(msg, {
            success: function (result) {
                App.UserGrid.store.reload();
            }
        });
    }
}

function deleteUser(dataId) {
    Ext.Msg.confirm('系统提示', '确定要删除该用户吗？',
        function (btn) {
            if (btn === 'yes') {
                App.direct.DeleteUserInfo(dataId, {
                    success: function (result) {
                        App.UserGrid.store.reload();
                    }
                });
            }
        }, this);
}

function clearInput() {
    App.tbUserId.setValue('');
    App.tbUserName.setValue('');
    App.tbUserPwd.setValue('');
    App.tbRealName.setValue('');
    App.tbUserTel.setValue('');
}

function checkData(dataId) {
    if (dataId.length === 0) {
        Ext.Msg.alert('提示', '请选择要操作的用户');
        return false;
    }
    return true;
}

function checkDataAndFill(dataId) {
    if (dataId.length === 0) {
        Ext.Msg.alert('提示', '请选择要操作的用户');
        return false;
    }
    App.direct.FillEdtInfo(dataId);
    App.direct.FillDeptInfo(dataId, {
        success: function (result) {
            var nodes = eval(result);
            if (nodes) {
                App.TpDeptList.setRootNode(nodes);
            }
            else {
                App.TpDeptList.getRootNode().removeAll();
            }
        }
    });
    return true;
}

function checkDataAndFillRole(dataId) {
    if (dataId.length === 0) {
        Ext.Msg.alert('提示', '请选择要操作的用户');
        return false;
    }
    App.direct.FillRoleInfo(dataId, {
        success: function (result) {
            var nodes = eval(result);
            if (nodes) {
                App.TpRoleList.setRootNode(nodes);
            }
            else {
                App.TpRoleList.getRootNode().removeAll();
            }
        }
    });
    return true;
}

function refreshPage() {
    window.location.href = document.URL;
}

var getRoles = function () {
    var msg = "",
        selChildren = App.TpRoleList.getChecked();

    Ext.each(selChildren, function (node) {
        if (msg.length > 0) {
            msg += ", ";
        }

        msg += node.data.id;
    });

    App.direct.SetUserRole(msg, {
        success: function (result) {
            App.RoleWindow.close();
        }
    });
};

function checkchange(node, checked) {
    //node.eachChild(function (n) {
    //    node.cascadeBy(function (n) {
    //        n.set('checked', checked);
    //    });
    //});

    ////check parent node if child node is check
    //p = node.parentNode;
    //var pChildCheckedCount = 0;
    //p.suspendEvents();
    //p.eachChild(function (c) {
    //    if (c.get('checked')) pChildCheckedCount++;
    //    if (p.parentNode) p.parentNode.set('checked', !!pChildCheckedCount);
    //    p.set('checked', !!pChildCheckedCount);
    //});
    //p.resumeEvents();
}

function checkDataAndFillPerm(dataId) {
    if (dataId.length === 0) {
        Ext.Msg.alert('提示', '请选择要操作的角色');
        return false;
    }
    App.direct.FillPermInfo(dataId, {
        success: function (result) {
            var nodes = eval(result);
            if (nodes) {
                App.TpPermList.setRootNode(nodes);
            }
            else {
                App.TpPermList.getRootNode().removeAll();
            }
        }
    });
    return true;
}

var getPerms = function () {
    var msg = "",
        selChildren = App.TpPermList.getChecked();

    Ext.each(selChildren, function (node) {
        if (msg.length > 0) {
            msg += ",";
        }

        msg += node.data.id;
    });

    App.direct.SetUserPerm(msg, {
        success: function (result) {
            App.PermWindow.close();
        }
    });
};

function ProcQuery() {
    App.direct.SaveQueryInput({
        success: function (result) {
            App.UserGrid.store.loadPage(1);
        }
    });
}