<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DepartInfo.aspx.cs" Inherits="UserConfig.UserConfig_DepartInfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>部门信息管理</title>
    <script src="/resources/js/jquery-1.12.4.min.js"></script>
    <script src="/resources/js/UserConfig/DepartInfo.js"></script>

    <link rel='stylesheet' href="/resources/css/basic.css?4.2.2" />
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
                    BodyPadding="5"
                    Width="550">
                    <Items>
                        <ext:TreePanel
                            ID="menuTreeEdt"
                            Title="部门信息管理"
                            runat="server"
                            Scrollable="Vertical"
                            UseArrows="true"
                            CollapseFirst="false"
                            RootVisible="true"
                            Animate="false">
                            <Fields>
                                <ext:ModelField Name="departName" />
                                <ext:ModelField Name="departLevel" />
                                <ext:ModelField Name="departOrder" />
                                <ext:ModelField Name="departDesp" />
                            </Fields>
                            <ColumnModel>
                                <Columns>
                                    <ext:TreeColumn
                                        runat="server"
                                        Text="部门名称"
                                        Flex="3"
                                        Sortable="true"
                                        DataIndex="departName" />
                                    <ext:Column
                                        runat="server"
                                        Text="部门层级"
                                        Flex="1"
                                        Sortable="true"
                                        DataIndex="departLevel" />
                                    <ext:Column
                                        runat="server"
                                        Text="部门排序"
                                        Flex="1"
                                        Sortable="true"
                                        DataIndex="departOrder" />
                                    <ext:Column
                                        runat="server"
                                        Text="部门说明"
                                        Flex="1"
                                        Sortable="true"
                                        DataIndex="departDesp" />
                                </Columns>
                            </ColumnModel>
                            <Listeners>
                                <ItemClick Handler="onTreeItemClick(record, e);" />
                            </Listeners>
                        </ext:TreePanel>
                    </Items>
                    <Buttons>
                        <ext:Button ID="btnMenuAdd" runat="server" Text="添加子部门">
                            <Listeners>
                                <Click Handler="clearInput(); #{DeptWindow}.setTitle('添加部门'); #{DeptWindow}.show();" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button ID="btnMenuEdit" runat="server" Text="编辑部门">
                            <Listeners>
                                <Click Handler="if (!checkNodeAndFill()) return; #{DeptWindow}.setTitle('编辑角色');  #{DeptWindow}.show();" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button ID="btnMenuDel" runat="server" Text="删除部门">
                            <Listeners>
                                <Click Handler="if (!checkNode()) return; deleteDepart();" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>
            </Items>
        </ext:Container>

        <ext:Window
            ID="DeptWindow"
            runat="server"
            Title="编辑部门"
            Icon="Application"
            Width="350"
            BodyStyle="background-color: #fff;"
            BodyPadding="5"
            Hidden="true"
            Modal="true">
            <Items>
                <ext:TextField ID="tbDepartId" runat="server" Hidden="True" FieldLabel="部门Id" />
                <ext:TextField ID="tbDepartName" runat="server" FieldLabel="部门名称" />
                <ext:TextField ID="tbDepartOrder" runat="server" FieldLabel="部门排序" />
                <ext:TextField ID="tbDepartDesp" runat="server" FieldLabel="部门描述" />
            </Items>
            <Buttons>
                <ext:Button runat="server" Text="提交">
                    <Listeners>
                        <Click Handler="submitDepart(); " />
                    </Listeners>
                </ext:Button>
            </Buttons>
        </ext:Window>

        <input id="optNodeId" style="display: none" runat="server" />
    </form>
</body>
</html>
