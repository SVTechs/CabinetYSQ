<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserInfo.aspx.cs" Inherits="UserConfig.UserConfig_UserInfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>人员信息管理</title>
    <script src="/resources/js/jquery-1.12.4.min.js"></script>
    <script src="/resources/js/UserConfig/UserInfo.js"></script>

    <link rel='stylesheet' href="/resources/css/basic.css?4.2.2" />
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager runat="server" />
        <ext:Viewport runat="server" ID="vwpLayout" Layout="fit">
            <Items>
                <ext:FormPanel
                    ID="mainPanel"
                    runat="server"
                    BodyPadding="5">
                    <Items>
                        <ext:GridPanel
                            ID="UserGrid"
                            runat="server"
                            Title="人员信息">
                            <TopBar>
                                <ext:Toolbar runat="server">
                                    <Items>
                                        <ext:TextField ID="tbSearchUserName" runat="server" FieldLabel="工号" />
                                        <ext:TextField ID="tbSearchRealName" runat="server" FieldLabel="姓名" />
                                        <ext:Button runat="server" Text="查询">
                                            <Listeners>
                                                <Click Handler="ProcQuery(); " />
                                            </Listeners>
                                        </ext:Button>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <Store>
                                <ext:Store ID="UserGridData" OnReadData="UserGridData_OnReadData_Refresh" PageSize="20" runat="server">
                                    <Model>
                                        <ext:Model runat="server">
                                            <Fields>
                                                <ext:ModelField Name="srvId" />
                                                <ext:ModelField Name="userName" />
                                                <ext:ModelField Name="realName" />
                                                <ext:ModelField Name="userTel" />
                                                <ext:ModelField Name="lastChanged" />
                                            </Fields>
                                        </ext:Model>
                                    </Model>
                                    <Proxy>
                                        <ext:PageProxy />
                                    </Proxy>
                                </ext:Store>
                            </Store>
                            <ColumnModel>
                                <Columns>
                                    <ext:Column runat="server" Text="Id" DataIndex="srvId" Hidden="True" Hideable="False" Flex="1"></ext:Column>
                                    <ext:Column runat="server" Text="用户名" DataIndex="userName" Flex="1"></ext:Column>
                                    <ext:Column runat="server" Text="姓名" DataIndex="realName" Flex="1"></ext:Column>
                                    <ext:Column runat="server" Text="电话" DataIndex="userTel" Flex="1"></ext:Column>
                                    <ext:Column runat="server" Text="最后更改" DataIndex="lastChanged" Width="120" Flex="1" />
                                    <ext:CommandColumn runat="server" Width="64">
                                        <Commands>
                                            <ext:GridCommand Icon="UserEdit" CommandName="RoleEdit">
                                                <ToolTip Text="角色分配" />
                                            </ext:GridCommand>
                                            <ext:CommandSeparator />
                                            <ext:GridCommand Icon="ApplicationKey" CommandName="Perm">
                                                <ToolTip Text="权限附加" />
                                            </ext:GridCommand>
                                        </Commands>
                                        <Listeners>
                                            <Command Handler="processGrid(command, record.data.srvId);" />
                                        </Listeners>
                                    </ext:CommandColumn>
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel runat="server" />
                            </SelectionModel>
                            <BottomBar>
                                <ext:PagingToolbar runat="server">
                                    <Items>
                                        <%--                        <ext:Label runat="server" Text="每页条数:" />
                        <ext:ToolbarSpacer runat="server" Width="10" />
                        <ext:ComboBox runat="server" Width="80">
                            <Items>
                                <ext:ListItem Text="1" />
                                <ext:ListItem Text="2" />
                                <ext:ListItem Text="10" />
                                <ext:ListItem Text="20" />
                            </Items>
                            <SelectedItems>
                                <ext:ListItem Value="10" />
                            </SelectedItems>
                            <Listeners>
                                <Select Handler="#{UserGrid}.store.pageSize = parseInt(this.getValue(), 10); #{UserGrid}.store.reload();" />
                            </Listeners>
                        </ext:ComboBox>--%>
                                    </Items>
                                    <Plugins>
                                        <ext:ProgressBarPager runat="server" />
                                    </Plugins>
                                </ext:PagingToolbar>
                            </BottomBar>
                            <%--            <Buttons>
                <ext:Button runat="server" Text="新增用户" IconCls="fa fa-user-plus" Handler="">
                    <Listeners>
                        <Click Handler="clearInput(); #{UserWindow}.setTitle('添加用户'); #{UserWindow}.show();" />
                    </Listeners>
                </ext:Button>
            </Buttons>--%>
                        </ext:GridPanel>
                    </Items>
                </ext:FormPanel>

                <ext:Window
                    ID="UserWindow"
                    runat="server"
                    Title="编辑用户"
                    Icon="Application"
                    Width="650"
                    Height="450"
                    Scrollable="Vertical"
                    BodyStyle="background-color: #fff;"
                    BodyPadding="5"
                    Hidden="true"
                    Modal="true">
                    <Items>
                        <ext:Container runat="server" Layout="Column">
                            <Items>
                                <ext:Container runat="server" Layout="FormLayout" ColumnWidth=".5" Padding="5">
                                    <Items>
                                        <ext:TextField ID="tbUserId" runat="server" Hidden="True" FieldLabel="用户Id" />
                                        <ext:TextField ID="tbUserName" runat="server" FieldLabel="用户名" />
                                        <ext:TextField ID="tbUserPwd" runat="server" InputType="Password" EmptyText="不更改密码无需填写" FieldLabel="密码" />
                                        <ext:TextField ID="tbRealName" runat="server" FieldLabel="姓名" />
                                        <ext:TextField ID="tbUserTel" runat="server" FieldLabel="电话" />
                                    </Items>
                                </ext:Container>
                                <ext:Container runat="server" Layout="FormLayout" ColumnWidth=".5" Padding="5">
                                    <Items>
                                        <ext:TextField ID="TextField1" runat="server" FieldLabel="职务" />
                                    </Items>
                                </ext:Container>
                                <ext:Container runat="server" Layout="FormLayout" ColumnWidth="1.0" Padding="5">
                                    <Items>
                                        <ext:TreePanel
                                            ID="TpDeptList"
                                            runat="server"
                                            Title="所属部门"
                                            Icon="Accept"
                                            UseArrows="true"
                                            Scrollable="Vertical"
                                            Animate="true"
                                            RootVisible="false">
                                            <Listeners>
                                                <Render Handler="this.getRootNode().expand(true);" Delay="50" />
                                                <CheckChange Fn="checkchange" />
                                            </Listeners>
                                        </ext:TreePanel>
                                    </Items>
                                </ext:Container>
                            </Items>
                        </ext:Container>
                    </Items>
                    <Buttons>
                        <ext:Button runat="server" Text="提交">
                            <Listeners>
                                <Click Handler="submitUser(); " />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:Window>

                <ext:Window
                    ID="RoleWindow"
                    runat="server"
                    Title="编辑角色"
                    Icon="Application"
                    Width="350"
                    Height="450"
                    Scrollable="Vertical"
                    BodyStyle="background-color: #fff;"
                    BodyPadding="5"
                    Hidden="true"
                    Modal="true">
                    <Items>
                        <ext:TextField ID="tbRoleUserId" runat="server" Hidden="True" FieldLabel="用户Id" />
                        <ext:TreePanel
                            ID="TpRoleList"
                            runat="server"
                            Title="角色列表"
                            Icon="Accept"
                            UseArrows="true"
                            Scrollable="Vertical"
                            Animate="true"
                            RootVisible="false">
                            <Listeners>
                                <Render Handler="this.getRootNode().expand(true);" Delay="50" />
                            </Listeners>
                        </ext:TreePanel>
                    </Items>
                    <Buttons>
                        <ext:Button runat="server" Text="提交">
                            <Listeners>
                                <Click Fn="getRoles" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:Window>

                <ext:Window
                    ID="PermWindow"
                    runat="server"
                    Title="编辑权限"
                    Icon="Application"
                    Width="350"
                    Height="450"
                    Scrollable="Vertical"
                    BodyStyle="background-color: #fff;"
                    BodyPadding="5"
                    Hidden="true"
                    Modal="true">
                    <Items>
                        <ext:TextField ID="tbPermUserId" runat="server" Hidden="True" FieldLabel="用户Id" />
                        <ext:TreePanel
                            ID="TpPermList"
                            runat="server"
                            Title="权限附加(增加角色外权限)"
                            Icon="Accept"
                            UseArrows="true"
                            Scrollable="Vertical"
                            Animate="true"
                            RootVisible="false">
                            <Listeners>
                                <Render Handler="this.getRootNode().expand(true);" Delay="50" />
                                <CheckChange Fn="checkchange" />
                            </Listeners>
                        </ext:TreePanel>
                    </Items>
                    <Buttons>
                        <ext:Button runat="server" Text="提交">
                            <Listeners>
                                <Click Fn="getPerms" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:Window>
            </Items>
            <Listeners>
                <AfterLayout Handler="#{UserGrid}.setHeight(#{mainPanel}.getHeight());" />
            </Listeners>
        </ext:Viewport>
    </form>
</body>
</html>
