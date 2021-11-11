var onTreeItemClick = function (record, e) {
    var recordId = record.getId();
    if (recordId.substr(0, 2) === 'F-') {
        //FuncNode
        //HideMenuBtn
        App.btnMenuAdd.setVisible(false);
        App.btnMenuEdit.setVisible(false);
        App.btnMenuDel.setVisible(false);
        App.btnFuncCode.setVisible(true);
        App.btnFuncAdd.setVisible(false);
        App.btnFuncEdit.setVisible(true);
        App.btnFuncDel.setVisible(true);
    } 
    else if (recordId.substr(0, 2) === 'W-' || recordId.substr(0, 2) === 'ro') {
        //FolderNode
        //ShowMenuBtn
        App.btnMenuAdd.setVisible(true);
        App.btnMenuEdit.setVisible(true);
        App.btnMenuDel.setVisible(true);
        App.btnFuncCode.setVisible(false);
        App.btnFuncAdd.setVisible(false);
        App.btnFuncEdit.setVisible(false);
        App.btnFuncDel.setVisible(false);
    }
    else if (recordId.substr(0, 2) === 'P-') {
        //PageNode
        //ShowMenuBtn
        App.btnMenuAdd.setVisible(false);
        App.btnMenuEdit.setVisible(true);
        App.btnMenuDel.setVisible(true);
        App.btnFuncCode.setVisible(false);
        App.btnFuncAdd.setVisible(true);
        App.btnFuncEdit.setVisible(false);
        App.btnFuncDel.setVisible(false);
    }
    $('#optNodeId').val(recordId.substr(2));
};

function submitMenu() {
    App.MenuWindow.close();
    if (App.MenuWindow.title.indexOf('添加') === 0) {
        App.direct.OnAddMenuClick({
            success: function (result) {
                refreshTree(App.menuTreeEdt);
            }
        });
    } else {
        App.direct.OnEdtMenuClick($('#optNodeId').val(), {
            success: function (result) {
                refreshTree(App.menuTreeEdt);
            }
        });
    }
}

function deleteMenu() {
    Ext.Msg.confirm('系统提示', '确定要删除该项菜单吗？',
        function (btn) {
            if (btn === 'yes') {
                App.direct.DeletePageMenu($('#optNodeId').val(), {
                    success: function (result) {
                        refreshTree(App.menuTreeEdt);
                    }
                });
            }
        }, this);
}

function clearMenuInput() {
    App.tbMenuName.setValue('');
    App.tbMenuIcon.setValue('');
    App.tbMenuOrder.setValue('');
    App.tbMenuUrl.setValue('');
    App.tbMenuDesp.setValue('');
    App.cbMenuType.setValue(0);
    App.cbMenuVisible.setValue(1);
}

function submitFunc() {
    App.FuncWindow.close();
    if (App.FuncWindow.title.indexOf('添加') === 0) {
        App.direct.OnAddFuncClick({
            success: function (result) {
                refreshTree(App.menuTreeEdt);
            }
        });
    } else {
        App.direct.OnEdtFuncClick($('#optNodeId').val(), {
            success: function (result) {
                refreshTree(App.menuTreeEdt);
            }
        });
    }
}

function deleteFunc() {
    Ext.Msg.confirm('系统提示', '确定要删除该项功能吗？',
        function (btn) {
            if (btn === 'yes') {
                App.direct.DeleteFunc($('#optNodeId').val(), {
                    success: function (result) {
                        refreshTree(App.menuTreeEdt);
                    }
                });
            }
        }, this);
}

function clearFuncInput() {
    App.tbFuncName.setValue('');
    App.tbFuncOrder.setValue('');
    App.tbFuncDesp.setValue('');
}

function showFuncCode() {
    var funcId = $('#optNodeId').val();

    $('#funcIdArea').attr('data-clipboard-text', funcId);
    $('#funcIdArea').text(funcId);
    var clipCtrl = $('#funcIdArea')[0];
    var clipboard = new Clipboard(clipCtrl);
    App.FuncCodeWindow.show();
}

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
        Ext.Msg.alert('提示', '请选择要操作的菜单');
        return false;
    }
    return true;
}

function checkMenuAndFill() {
    if ($('#optNodeId').val().length === 0) {
        Ext.Msg.alert('提示', '请选择要操作的菜单');
        return false;
    }
    App.direct.FillEdtInfo($('#optNodeId').val());
    return true;
}

function checkFuncNode() {
    if ($('#optNodeId').val().length === 0) {
        Ext.Msg.alert('提示', '请选择要操作的功能');
        return false;
    }
    return true;
}

function checkFuncAndFill() {
    if ($('#optNodeId').val().length === 0) {
        Ext.Msg.alert('提示', '请选择要操作的功能');
        return false;
    }
    App.direct.FillEdtFuncInfo($('#optNodeId').val());
    return true;
}


function refreshPage() {
    window.location.href = document.URL;
}