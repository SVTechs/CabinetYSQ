function processGrid(command, dataId) {
    //if (command === 'Delete') {
    //    if (!checkData(dataId)) return;
    //    deleteTool(dataId);
    //}
    if (command === 'Info') {
        if (!checkData(dataId)) return;
        App.InfoWindow.show();
    }
    else if (command === 'Edit') {
        if (!checkDataAndFill(dataId)) return;
        App.EditWindow.setTitle('编辑工具'); 
        App.EditWindow.show();
    }
}

function ProcQuery() {
    App.direct.SaveInput({
        success: function (result) {
            App.ToolGrid.store.loadPage(1);
        }
    });
}

function submitTool() {
    App.EditWindow.close();
    if (App.EditWindow.title.indexOf('添加') === 0) {
        App.direct.OnAddToolClick({
            success: function (result) {
                App.ToolGrid.store.reload();
            }
        });
    } else {
        App.direct.OnEdtToolClick({
            success: function (result) {
                App.ToolGrid.store.reload();
            }
        });
    }
}

function clearInput() {
    App.tbToolId.setValue('');
    App.tbToolName.setValue('');
    App.tbToolSpec.setValue('');
    App.cbToolCate.setValue('');
    //App.cbCabinetName.setValue('');
}

function deleteTool(dataId) {
    Ext.Msg.confirm('系统提示', '确定要删除该工具吗？',
        function (btn) {
            if (btn === 'yes') {
                App.direct.DeleteToolInfo(dataId, {
                    success: function (result) {
                        App.ToolGrid.store.reload();
                    }
                });
            }
        }, this);
}

function checkData(dataId) {
    if (dataId.length === 0) {
        Ext.Msg.alert('提示', '请选择要查看的工具');
        return false;
    }
    App.direct.ToolInfo(dataId);
    return true;
}

function checkDataAndFill(dataId) {
    if (dataId.length === 0) {
        Ext.Msg.alert('提示', '请选择要编辑的工具');
        return false;
    }
    App.direct.FillEdtInfo(dataId);
    return true;
}
