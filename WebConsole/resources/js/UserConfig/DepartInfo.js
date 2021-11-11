var onTreeItemClick = function (record, e) {
    var recordId = record.getId();
    $('#optNodeId').val(recordId);
    //record[record.isExpanded() ? 'collapse' : 'expand']();
};

function refreshTree(tree) {
    App.direct.GetTreeNode({
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

function checkNode() {
    if ($('#optNodeId').val().length === 0) {
        Ext.Msg.alert('提示', '请选择要操作的部门');
        return false;
    }
    return true;
}

function checkNodeAndFill() {
    if ($('#optNodeId').val().length === 0) {
        Ext.Msg.alert('提示', '请选择要操作的部门');
        return false;
    }
    App.direct.FillEdtInfo($('#optNodeId').val());
    return true;
}

function submitDepart() {
    App.DeptWindow.close();
    if (App.DeptWindow.title.indexOf('添加') === 0) {
        App.direct.OnAddDepartClick({
            success: function (result) {
                refreshTree(App.menuTreeEdt);
            }
        });
    } else {
        App.direct.OnEdtDepartClick({
            success: function (result) {
                refreshTree(App.menuTreeEdt);
            }
        });
    }
}

function deleteDepart() {
    Ext.Msg.confirm('系统提示', '确定要删除该部门吗？',
        function (btn) {
            if (btn === 'yes') {
                App.direct.DeleteDepartInfo($('#optNodeId').val(), {
                    success: function (result) {
                        refreshTree(App.menuTreeEdt);
                    }
                });
            }
        }, this);
}

function clearInput() {
    App.tbDepartId.setValue('');
    App.tbDepartName.setValue('');
    App.tbDepartOrder.setValue('');
    App.tbDepartDesp.setValue('');
}