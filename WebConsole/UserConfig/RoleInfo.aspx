<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RoleInfo.aspx.cs" Inherits="UserConfig.UserConfig_RoleInfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>角色信息管理</title>
    <script src="/resources/js/jquery-1.12.4.min.js"></script>
    <script src="/resources/js/UserConfig/RoleInfo.js"></script>

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
                        <ext:TreePanel
                            ID="roleTreeEdt"
                            Title="角色信息管理"
                            runat="server"
                            Scrollable="Vertical"
                            UseArrows="true"
                            CollapseFirst="false"
                            RootVisible="true">
                            <Fields>
                                <ext:ModelField Name="roleName" />
                                <ext:ModelField Name="roleLevel" />
                                <ext:ModelField Name="roleOrder" />
                                <ext:ModelField Name="roleDesp" />
                            </Fields>
                            <ColumnModel>
                                <Columns>
                                    <ext:TreeColumn
                                        runat="server"
                                        Text="角色名称"
                                        Flex="3"
                                        Sortable="true"
                                        DataIndex="roleName" />
                                    <ext:Column
                                        runat="server"
                                        Text="角色层级"
                                        Flex="1"
                                        Sortable="true"
                                        DataIndex="roleLevel" />
                                    <ext:Column
                                        runat="server"
                                        Text="角色排序"
                                        Flex="1"
                                        Sortable="true"
                                        DataIndex="roleOrder" />
                                    <ext:Column
                                        runat="server"
                                        Text="角色说明"
                                        Flex="1"
                                        Sortable="true"
                                        DataIndex="roleDesp" />
                                </Columns>
                            </ColumnModel>
                            <Listeners>
                                <ItemClick Handler="onTreeItemClick(record, e);" />
                            </Listeners>
                        </ext:TreePanel>
                    </Items>
                    <Buttons>
                        <ext:Button ID="btnMenuAdd" runat="server" Text="添加下级角色">
                            <Listeners>
                                <Click Handler="clearInput(); #{RoleWindow}.setTitle('添加角色'); #{RoleWindow}.show();" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button ID="btnMenuEdit" runat="server" Text="编辑角色">
                            <Listeners>
                                <Click Handler="if (!checkDataAndFill()) return; #{RoleWindow}.setTitle('编辑角色'); #{RoleWindow}.show();" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button ID="btnMenuPerm" runat="server" Text="权限分配">
                            <Listeners>
                                <Click Handler="if (!checkDataAndFillPerm()) return; #{PermWindow}.show();" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button ID="btnMenuDel" runat="server" Text="删除角色">
                            <Listeners>
                                <Click Handler="if (!checkData()) return; deleteRole();" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>

                <ext:Window
                    ID="RoleWindow"
                    runat="server"
                    Title="编辑角色"
                    Icon="Application"
                    Width="350"
                    BodyStyle="background-color: #fff;"
                    BodyPadding="5"
                    Hidden="true"
                    Modal="true">
                    <Items>
                        <ext:TextField ID="tbRoleId" runat="server" Hidden="True" FieldLabel="角色Id" />
                        <ext:TextField ID="tbRoleName" runat="server" FieldLabel="角色名称" />
                        <ext:TextField ID="tbRoleOrder" runat="server" FieldLabel="角色排序" />
                        <ext:TextField ID="tbRoleDesp" runat="server" FieldLabel="角色描述" />
                    </Items>
                    <Buttons>
                        <ext:Button runat="server" Text="提交">
                            <Listeners>
                                <Click Handler="submitRole(); " />
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
                        <ext:TextField ID="tbPermRoleId" runat="server" Hidden="True" FieldLabel="角色Id" />
                        <ext:TreePanel
                            ID="TpPermList"
                            runat="server"
                            Title="权限列表"
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
                <AfterLayout Handler="#{roleTreeEdt}.setHeight(#{mainPanel}.getHeight() - 47 - 45);" />
            </Listeners>
        </ext:Viewport>

        <input id="optNodeId" style="display: none" runat="server" />
    </form>
</body>
</html>
