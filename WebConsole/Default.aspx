<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Import Namespace="WebConsole.Config" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title><%=Env.AppName %></title>

    <script src="resources/js/frame.js"></script>
    <script src="resources/js/Default.js"></script>
    <link rel='stylesheet' href='resources/css/main.css?4.2.2' />
    <style>
        .profile-edit {
            display: inline;
            margin-left: 15px;
        }

            .profile-edit a {
                color: black;
            }

        .log-out {
            display: inline;
            margin-left: 3px;
        }

            .log-out a {
                color: black;
            }
    </style>
</head>
<body>
    <ext:ResourceManager ID="ResourceManager1" runat="server" />

    <ext:Viewport runat="server" Layout="BorderLayout">
        <Items>
            <ext:Panel
                runat="server"
                Header="false"
                Region="North"
                Height="100"
                Border="false">
                <Content>
                    <header class="site-header" role="banner">
                        <nav class="top-navigation">
                            <div class="logo-container">
                                <img src="resources/images/header.jpg?0.3" style="height: 100px; width: 100%;" />
                            </div>
                            <div style="position: absolute; left: 81.5%; top: 78px; color: black;">
                                <div style="display: inline;">
                                    <ext:Label ID="lbUserName" runat="server" Text="当前用户: "></ext:Label>
                                    <ext:HyperlinkButton ID="btnProfileEdit" runat="server" Text="设置" Icon="ApplicationEdit">
                                        <Listeners>
                                            <Click Handler="fillProfile(); #{ProfileWindow}.show();" />
                                        </Listeners>
                                    </ext:HyperlinkButton>
                                    <ext:HyperlinkButton ID="btnLogOut" runat="server" Text="退出" Icon="ApplicationStop"
                                        OnDirectClick="btnLogOut_OnDirectClick">
                                    </ext:HyperlinkButton>
                                </div>
                            </div>
                        </nav>
                    </header>
                </Content>
            </ext:Panel>

            <ext:Panel
                runat="server"
                Region="West"
                Layout="Fit"
                Width="270"
                Header="false"
                MarginSpec="0"
                Border="False">
                <TopBar>
                    <ext:Toolbar runat="server" Cls="left-header">
                        <Items>
                            <ext:Label ID="Label1" runat="server" Height="32" Text="">
                            </ext:Label>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Items>
                    <ext:TreePanel
                        ID="menuTree"
                        runat="server"
                        Header="false"
                        Scrollable="Vertical"
                        UseArrows="true"
                        CollapseFirst="false"
                        RootVisible="false"
                        Animate="false"
                        HideHeaders="true">
                        <Listeners>
                            <ItemClick Handler="onTreeItemClick(record, e);" />
                        </Listeners>
                    </ext:TreePanel>
                </Items>
            </ext:Panel>

            <ext:TabPanel
                ID="MainTabs"
                runat="server"
                Region="Center"
                MarginSpec="0"
                Cls="tabs"
                Border="False"
                MinTabWidth="115">
                <Items>
                    <ext:Panel
                        ID="tabHome"
                        runat="server"
                        Title="首页"
                        HideMode="Offsets"
                        IconCls="fa fa-home">
                        <Loader runat="server" Mode="Frame" Url="Home.aspx">
                            <LoadMask ShowMask="true" />
                        </Loader>
                    </ext:Panel>
                </Items>
                <Plugins>
                    <ext:TabCloseMenu runat="server" />
                </Plugins>
            </ext:TabPanel>
        </Items>
    </ext:Viewport>

    <form id="form1" runat="server">
        <ext:Window
            ID="ProfileWindow"
            runat="server"
            Title="用户设置"
            Icon="Application"
            Width="350"
            BodyStyle="background-color: #fff;"
            BodyPadding="5"
            Hidden="true"
            Modal="true">
            <Items>
                <ext:TextField ID="tbEdtProfilePwd" InputType="Password" EmptyText="不修改请留空" runat="server" FieldLabel="新密码" />
                <ext:TextField ID="tbEdtProfileNewPwd" InputType="Password" EmptyText="不修改请留空" runat="server" FieldLabel="密码确认" />
                <ext:TextField ID="tbEdtProfileTel" runat="server" FieldLabel="电话号码" />
            </Items>
            <Buttons>
                <ext:Button runat="server" Text="提交">
                    <Listeners>
                        <Click Handler="editProfile(); " />
                    </Listeners>
                </ext:Button>
            </Buttons>
        </ext:Window>
    </form>
</body>
</html>
