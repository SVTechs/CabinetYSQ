var onTreeItemClick = function(record, e) {
    var recordId = record.getId();
    $('#optNodeId').val(recordId);
};

function submitRole() {
    App.RoleWindow.close();
    if (App.RoleWindow.title.indexOf('添加') === 0) {
        App.direct.OnAddRoleClick({
            success: function(result) {
                refreshTree(App.roleTreeEdt);
            }
        });
    } else {
        App.direct.OnEdtRoleClick({
            success: function (result) {
                refreshTree(App.roleTreeEdt);
            }
        });
    }
}

function deleteRole() {
    Ext.Msg.confirm('系统提示', '确定要删除该角色吗？',
        function (btn) {
            if (btn === 'yes') {
                App.direct.DeleteRoleInfo($('#optNodeId').val(), {
                    success: function (result) {
                        refreshTree(App.roleTreeEdt);
                    }
                });
            }
        }, this);
}

function refreshTree(tree) {
    App.direct.GetRoleNode({
        success: function (result) {
            var nodes = eval(result);
            if (nodes) {
                tree.setRootNode(nodes);
            }
            else {
                tree.getRootNode().removeAll();
            }
        }
    });
}

function checkData() {
    if ($('#optNodeId').val().length === 0) {
        Ext.Msg.alert('提示', '请选择要操作的角色');
        return false;
    }
    return true;
}

function clearInput() {
    App.tbRoleId.setValue('');
    App.tbRoleName.setValue('');
    App.tbRoleOrder.setValue('');
    App.tbRoleDesp.setValue('');
}

function checkDataAndFill() {
    if ($('#optNodeId').val().length === 0) {
        Ext.Msg.alert('提示', '请选择要操作的角色');
        return false;
    }
    App.direct.FillEdtInfo($('#optNodeId').val());
    return true;
}

function refreshPage() {
    window.location.href = document.URL;
}

function checkchange(node, checked) {
    //node.eachChild(function (n) {
    //    node.cascadeBy(function (n) {
    //        n.set('checked', checked);
    //    });
    //});

    //check parent node if child node is check
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

function checkDataAndFillPerm() {
    if ($('#optNodeId').val().length === 0) {
        Ext.Msg.alert('提示', '请选择要操作的角色');
        return false;
    }
    App.direct.FillPermInfo($('#optNodeId').val(), {
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

    App.direct.SetRolePerm(msg, {
        success: function (result) {
            App.PermWindow.close();
        }
    });
};