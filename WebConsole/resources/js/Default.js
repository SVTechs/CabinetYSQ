function fillProfile() {
    App.direct.FillProfileInfo();
    return true;
}

function editProfile() {
    App.ProfileWindow.close();
    App.direct.OnEdtProfileClick({
        success: function (result) {
            if (result > 0) {
                Ext.Msg.alert('提示', '修改完成，请重新登录', function (btn, text) {
                    window.location.href = "Login.aspx";
                });
            } else {
                Ext.Msg.alert('提示', '修改失败');
            }
        }
    });
}