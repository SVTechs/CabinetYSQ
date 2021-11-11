function ProcQuery() {
    App.direct.SaveInput({
        success: function (result) {
            App.IoGrid.store.loadPage(1); 
        }
    });
}

var saveChart = function (btn) {
    Ext.MessageBox.confirm('保存', '您是否要保存该图表?', function (choice) {
        if (choice === 'yes') {
            btn.up('panel').down('chart').download();
        }
    });
};
