<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="_Login" %>

<%@ Import Namespace="WebConsole.Config" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title><%=Env.AppName %></title>

    <style>
        .input-none-border {
            border-style: none;
        }
        .x-16 .x-btn-default-small .x-btn-inner {
            font-size: 16px !important;
        }
    </style>
    
    <script type="text/javascript">
        function inputKeyDown(field, e) {
            if (e.getKey() === Ext.EventObject.ENTER) {
                checkInput();
            } 
        }

        function checkInput() {
            App.direct.BtnLoginClick();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager runat="server" />
        <div style="background: url('./resources/images/login-bg.jpg?0.3'); width: 1440px; height: 899px; margin: auto">
            <div id="main-content" style="padding-top: 350px; padding-left: 460px; width: 500px; float: left;">
                <div style="line-height: 50px; padding-top: 45px; padding-left: 30px; height: 400px; width: 100%; color: #003366; font-size: 16px; float: left">
                    <div style="width: 370px; float: left; padding-left: 135px;">
                        <div style="width: 90%; float: left;">
                            <ext:TextField ID="tbUserName" Text="" StyleSpec="font-size: 20px;height: 41px;line-height:41px;width:265px;"
                                runat="server">
                                <Listeners>
                                    <SpecialKey Fn="inputKeyDown"></SpecialKey>
                                </Listeners>
                            </ext:TextField>
                        </div>
                        <div style="width: 90%; float: left; padding-top: 21px; margin-top: 5px;">
                            <ext:TextField ID="tbUserPassword" Text="" StyleSpec="font-size: 20px;height: 41px;line-height:41px;width:265px;"
                                InputType="Password" runat="server">
                                <Listeners>
                                    <SpecialKey Fn="inputKeyDown"></SpecialKey>
                                </Listeners>
                            </ext:TextField>
                        </div>
                    </div>
                    <div style="width: 358px; padding-left: 51px; padding-top: 22px; float: left;">
                        <div style="width: 80px; float: left; text-align: center">
                            <ext:Button ID="btnLogin" runat="server" CtCls="x-16" Text="登录" Width="358" Height="47" OnClientClick="checkInput" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
