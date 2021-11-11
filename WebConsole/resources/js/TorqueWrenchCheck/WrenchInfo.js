function processGrid(command, dataId) {
    if (command === 'Delete') {
        //if (!checkData(dataId)) return;
        deleteWrench(dataId);
    }
    if (command === 'Info') {
        //if (!checkData(dataId)) return;
        App.direct.ShowWrenchCheckRecord(dataId);
        App.InfoWindow.show();
    }
    if (command === 'Edit') {
        //if (!checkDataAndFill(dataId)) return;
        App.direct.FillEdtInfo(dataId);
    }
    if (command === 'ShowPdf') {

        App.direct.ShowWrenchCheckPdf(dataId, {
            success: function (result) {
                window.open(result, "_blank");   
            }
        });
        
    }
    else if (command === 'DeletePdf') {
        App.direct.DeleteWrenchCheckPdf(dataId);
    }
}

function ProcQuery() {
    App.direct.SaveInput({
        success: function (result) {
            App.WrenchGrid.store.loadPage(1);
        }
    });
}

function submitWrench() {
    //App.EditWindow.close();
    if (App.EditWindow.title.indexOf('添加') === 0) {
        App.direct.OnAddWrenchClick({
            success: function (result) {
                App.WrenchGrid.store.reload();
            }
        });
    } else {
        App.direct.OnEdtWrenchClick({
            success: function (result) {
                App.WrenchGrid.store.reload();
            }
        });
    }
}

function deleteWrench(dataId) {
    Ext.Msg.confirm('系统提示', '确定要删除该扳手吗？',
        function (btn) {
            if (btn === 'yes') {
                App.direct.DeleteWrenchInfo(dataId, {
                    success: function (result) {
                        App.WrenchGrid.store.reload();
                    }
                });
            }
        }, this);
}

function checkData(dataId) {
    if (dataId.length === 0) {
        Ext.Msg.alert('提示', '请选择扳手');
        return false;
    }
    App.direct.ToolInfo(dataId);
    return true;
}

function checkDataAndFill(dataId) {
    if (dataId.length === 0) {
        Ext.Msg.alert('提示', '请选择要编辑的扳手');
        return false;
    }
    App.direct.FillEdtInfo(dataId);
    return true;
}

function fileUpLoad() {
    App.direct.FileUpLoad();
    return true;
}


