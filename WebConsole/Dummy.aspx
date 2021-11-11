<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Dummy.aspx.cs" Inherits="Dummy" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="/resources/css/examples.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager runat="server" />

        <ext:Container
            runat="server"
            Layout="VBoxLayout"
            Height="650">
            <Items>
                <ext:FormPanel
                    runat="server"
                    Title="开发中"
                    BodyPadding="5"
                    DefaultButton="0">
                    <Items>
                        <ext:TextField runat="server" FieldLabel="-" />
                    </Items>
                    <Buttons>
                        <ext:Button runat="server" Text="开发中" OnClientClick="Ext.Msg.alert('提示', '功能开发中');" />
                    </Buttons>
                </ext:FormPanel>
            </Items>
        </ext:Container>
    </form>
</body>
</html>
