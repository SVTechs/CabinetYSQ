<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MenuConfig.aspx.cs" Inherits="SysConfig.SysConfig_MenuConfig" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>系统菜单管理</title>
    <script src="/resources/js/jquery-1.12.4.min.js"></script>
    <script src="/resources/js/SysConfig/MenuConfig.js"></script>
    <script src="/resources/js/clipboard.min.js"></script>

    <link href="/resources/css/examples.css" rel="stylesheet" />

    <script>
        var formatHours = function (v) {
            if (v < 1) {
                return Math.round(v * 60) + " mins";
            } else if (Math.floor(v) !== v) {
                var min = v - Math.floor(v);
                return Math.floor(v) + "h " + Math.round(min * 60) + "m";
            } else {
                return v + " hour" + (v === 1 ? "" : "s");
            }
        };

        var handler = function (grid, rowIndex, colIndex, actionItem, event, record, row) {
            Ext.Msg.alert('Editing' + (record.get('done') ? ' completed task' : ''), record.get('task'));
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager runat="server" />

        <%--        <ext:Container
            runat="server"
            Layout="VBoxLayout">
            <Items>

            </Items>
        </ext:Container>--%>
        <ext:Viewport runat="server" ID="vwpLayout" Layout="fit">
            <Items>
                <ext:FormPanel
                    ID="mainPanel"
                    runat="server"
                    BodyPadding="5"
                    Scrollable="Both">
                    <Items>
                        <ext:TreePanel
                            ID="menuTreeEdt"
                            Title="系统菜单管理(项目双击展开)"
                            runat="server"
                            Scrollable="Vertical"
                            UseArrows="true"
                            Collapsible="true"
                            RootVisible="true"
                            Animate="false"
                            FolderSort="true">
                            <Fields>
                                <ext:ModelField Name="menuName" />
                                <ext:ModelField Name="menuLevel" />
                                <ext:ModelField Name="menuOrder" />
                                <ext:ModelField Name="menuUrl" />
                                <ext:ModelField Name="IsVisible" />
                            </Fields>
                            <ColumnModel>
                                <Columns>
                                    <ext:TreeColumn
                                        runat="server"
                                        Text="项目名称"
                                        Flex="3"
                                        Sortable="true"
                                        DataIndex="menuName" />
                                    <ext:Column
                                        runat="server"
                                        Text="项目层级"
                                        Flex="1"
                                        Sortable="true"
                                        DataIndex="menuLevel" />
                                    <ext:Column
                                        runat="server"
                                        Text="项目排序"
                                        Flex="1"
                                        Sortable="true"
                                        DataIndex="menuOrder" />
                                    <ext:Column
                                        runat="server"
                                        Text="链接地址"
                                        Flex="1"
                                        Sortable="true"
                                        DataIndex="menuUrl" />
                                    <ext:Column
                                        runat="server"
                                        Text="是否可见"
                                        Flex="1"
                                        Sortable="true"
                                        DataIndex="isVisible" />
                                </Columns>
                            </ColumnModel>
                            <Listeners>
                                <ItemClick Handler="onTreeItemClick(record, e);" />
                            </Listeners>
                        </ext:TreePanel>

                    </Items>
                    <Buttons>
                        <ext:Button ID="btnMenuAdd" runat="server" Text="添加子菜单">
                            <Listeners>
                                <Click Handler="clearMenuInput(); #{MenuWindow}.setTitle('添加菜单'); #{MenuWindow}.show();" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button ID="btnFuncAdd" runat="server" Hidden="True" Text="添加功能">
                            <Listeners>
                                <Click Handler="clearFuncInput(); #{FuncWindow}.setTitle('添加功能'); #{FuncWindow}.show();" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button ID="btnFuncCode" runat="server" Hidden="True" Text="功能代码">
                            <Listeners>
                                <Click Handler="showFuncCode();" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button ID="btnMenuEdit" runat="server" Text="编辑菜单">
                            <Listeners>
                                <Click Handler="if (!checkMenuAndFill()) return; #{MenuWindow}.setTitle('编辑菜单'); #{MenuWindow}.show();" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button ID="btnMenuDel" runat="server" Text="删除菜单">
                            <Listeners>
                                <Click Handler="if (!checkNode()) return; deleteMenu();" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button ID="btnFuncEdit" runat="server" Hidden="True" Text="编辑功能">
                            <Listeners>
                                <Click Handler="if (!checkFuncAndFill()) return; #{FuncWindow}.setTitle('编辑功能'); #{FuncWindow}.show();" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button ID="btnFuncDel" runat="server" Hidden="True" Text="删除功能">
                            <Listeners>
                                <Click Handler="if (!checkFuncNode()) return; deleteFunc();" />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:FormPanel>

                <ext:Window
                    ID="MenuWindow"
                    runat="server"
                    Title="修改菜单"
                    Icon="Application"
                    Width="350"
                    BodyStyle="background-color: #fff;"
                    BodyPadding="5"
                    Hidden="true"
                    Modal="true">
                    <Items>
                        <ext:TextField ID="tbMenuName" runat="server" FieldLabel="菜单名称" />
                        <ext:TextField ID="tbMenuIcon" runat="server" FieldLabel="菜单图标" />
                        <ext:TextField ID="tbMenuOrder" runat="server" Text="0" FieldLabel="菜单排序" />
                        <ext:SelectBox
                            ID="cbMenuType"
                            runat="server"
                            DisplayField="state"
                            ValueField="abbr"
                            FieldLabel="菜单类型"
                            EmptyText="请选择...">
                            <Items>
                                <ext:ListItem Text="分类" Value="0" Index="0"></ext:ListItem>
                                <ext:ListItem Text="链接-标签页" Value="1" Index="1"></ext:ListItem>
                                <ext:ListItem Text="链接-新窗口" Value="2" Index="2"></ext:ListItem>
                            </Items>
                        </ext:SelectBox>
                        <ext:TextField ID="tbMenuUrl" runat="server" FieldLabel="链接地址" />
                        <ext:SelectBox
                            ID="cbMenuVisible"
                            runat="server"
                            DisplayField="state"
                            ValueField="abbr"
                            FieldLabel="是否可见"
                            EmptyText="请选择...">
                            <Items>
                                <ext:ListItem Text="否" Value="0" Index="0"></ext:ListItem>
                                <ext:ListItem Text="是" Value="1" Index="1"></ext:ListItem>
                            </Items>
                        </ext:SelectBox>
                        <ext:TextField ID="tbMenuDesp" runat="server" FieldLabel="菜单说明" />
                    </Items>
                    <Buttons>
                        <ext:Button runat="server" Text="提交">
                            <Listeners>
                                <Click Handler="submitMenu(); " />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:Window>

                <ext:Window
                    ID="FuncWindow"
                    runat="server"
                    Title="编辑功能"
                    Icon="Application"
                    Width="350"
                    BodyStyle="background-color: #fff;"
                    BodyPadding="5"
                    Hidden="true"
                    Modal="true">
                    <Items>
                        <ext:TextField ID="tbFuncName" runat="server" FieldLabel="功能名称" />
                        <ext:TextField ID="tbFuncOrder" runat="server" FieldLabel="功能排序" />
                        <ext:TextField ID="tbFuncDesp" runat="server" FieldLabel="功能描述" />
                    </Items>
                    <Buttons>
                        <ext:Button ID="btnEditFunc" runat="server" Text="提交">
                            <Listeners>
                                <Click Handler="submitFunc(); " />
                            </Listeners>
                        </ext:Button>
                    </Buttons>
                </ext:Window>

                <ext:Window
                    ID="FuncCodeWindow"
                    runat="server"
                    Title="功能代码 (点击即可复制)"
                    Icon="Application"
                    Width="350"
                    BodyStyle="background-color: #fff;"
                    BodyPadding="5"
                    Hidden="true"
                    Modal="true">
                    <Content>
                        <div id="funcIdArea" data-clipboard-text=""></div>
                    </Content>
                </ext:Window>

                
            </Items>
            <Listeners>
                <AfterLayout Handler="#{menuTreeEdt}.setHeight(#{mainPanel}.getHeight() - 47 - 45);" />
            </Listeners>
        </ext:Viewport>
        <input id="optNodeId" style="display: none" runat="server" />
    </form>
</body>
</html>
