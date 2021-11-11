<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ToolInfo.aspx.cs" Inherits="ToolInfo.ToolInfo_ToolInfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>工具基础信息</title>
    <script src="/resources/js/jquery-1.12.4.min.js"></script>
    <script src="/resources/js/ToolInfo/ToolInfo.js"></script>

    <link rel='stylesheet' href="/resources/css/basic.css?4.2.2" />
</head>
<body>
<ext:ResourceManager runat="server" />
<form id="form1" runat="server">
<ext:Viewport runat="server" ID="vwpLayout" Layout="fit">
<Items>
    <ext:FormPanel
        ID="mainPanel"
        runat="server"
        Title="请输入查询条件"
        BodyPadding="5">
        <FieldDefaults LabelAlign="Left" MsgTarget="Side" />
        <TopBar>
            <ext:Toolbar runat="server">
                <Items>
                    <%--<ext:TextField ID="tbSearchBarCode" runat="server" FieldLabel="条形码" />--%>
                    <ext:TextField ID="tbSearchName" runat="server" FieldLabel="名称"  Visible ="false"/>
                     <ext:SelectBox
                        ID="cbSearchName"
                        runat="server"
                        DisplayField="state"
                        ValueField="abbr"
                        FieldLabel="名称"
                        EmptyText="请选择...">
                        <Items>
                        </Items>
                    </ext:SelectBox>
                    <ext:TextField ID="tbSearchSpec" runat="server" FieldLabel="规格" />
                    <ext:SelectBox
                        ID="cbCabinet"
                        runat="server"
                        DisplayField="state"
                        ValueField="abbr"
                        FieldLabel="工具柜"
                        EmptyText="请选择...">
                        <Items>
                        </Items>
                    </ext:SelectBox>
                    <ext:Button runat="server" Text="查询">
                        <Listeners>
                            <Click Handler="ProcQuery(); " />
                        </Listeners>
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </TopBar>
        <Items>
            <ext:GridPanel
                ID="ToolGrid"
                runat="server"
                Border="True">
                <Store>
                    <ext:Store ID="ToolGridData" OnReadData="ToolGridData_OnReadData_Refresh" PageSize="20" runat="server">
                        <Model>
                            <ext:Model runat="server">
                                <Fields>
                                    <ext:ModelField Name="Id" />
                                    <ext:ModelField Name="ToolName" />
                                    <ext:ModelField Name="ToolSpec" />
                                    <ext:ModelField Name="ToolPosition" />
                                    <ext:ModelField Name="ToolType" />
                                    <ext:ModelField Name="DataOwner" />
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
                        <ext:Column runat="server" Text="Id" DataIndex="Id" Hidden="True" Hideable="False" Flex="1"></ext:Column>
                        <ext:Column runat="server" Text="工具名称" DataIndex="ToolName" Flex="1"></ext:Column>
                        <ext:Column runat="server" Text="规格" DataIndex="ToolSpec" Flex="1"></ext:Column>
                        <ext:Column runat="server" Text="工具编号" DataIndex="ToolPosition" Flex="1"></ext:Column>
                        <ext:Column runat="server" Text="工具类型" DataIndex="ToolType" Flex="1"></ext:Column>
                        <ext:Column runat="server" Text="位置" DataIndex="DataOwner" Flex="1"></ext:Column>
                        <ext:CommandColumn runat="server" Width="64">
                            <Commands>
                                <ext:GridCommand Icon="BulletEdit" CommandName="Edit">
                                    <ToolTip Text="编辑工具" />
                                </ext:GridCommand>
                                <ext:CommandSeparator />
                                <ext:GridCommand Icon="BulletDatabase" CommandName="Info">
                                    <ToolTip Text="借取记录" />
                                </ext:GridCommand>
                            </Commands>
                            <Listeners>
                                <Command Handler="processGrid(command, record.data.Id);" />
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
                        </Items>
                        <Plugins>
                            <ext:ProgressBarPager runat="server" />
                        </Plugins>
                    </ext:PagingToolbar>
                </BottomBar>

            </ext:GridPanel>
        </Items>
    </ext:FormPanel>
    
    <ext:Window
        ID="EditWindow"
        runat="server"
        Title="编辑工具"
        Icon="Application"
        Width="350"
        BodyStyle="background-color: #fff;"
        BodyPadding="5"
        Hidden="true"
        Modal="true">
        <Items>
            <ext:TextField ID="tbToolId" runat="server" Hidden="True" FieldLabel="工具Id" />
            <ext:TextField ID="tbToolName" runat="server" FieldLabel="工具名称" />
            <ext:TextField ID="tbToolSpec" runat="server" FieldLabel="规格" />
            <ext:SelectBox
                ID="cbToolType"
                runat="server"
                DisplayField="state"
                ValueField="abbr"
                FieldLabel="分类"
                EmptyText="请选择...">
                <Items>
                </Items>
            </ext:SelectBox>
            <%--<ext:TextField ID="tbToolBrand" runat="server" FieldLabel="品牌" />--%>
<%--            <ext:SelectBox
                ID="cbCabinetName"
                runat="server"
                DisplayField="state"
                ValueField="abbr"
                FieldLabel="工具柜"
                EmptyText="请选择...">
                <Items>
                </Items>
            </ext:SelectBox>--%>
        </Items>
        <Buttons>
            <ext:Button runat="server" Text="提交">
                <Listeners>
                    <Click Handler="submitTool(); " />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    
<ext:Window
    ID="InfoWindow"
    runat="server"
    Title="借取记录"
    Icon="Information"
    Width="1200"
    Height="700"
    BodyStyle="background-color: #fff;"
    BodyPadding="5"
    Hidden="true"
    Modal="true">
    <Items>
        <ext:GridPanel
            ID="IoGrid"
            runat="server"
            Border="True">
            <Store>
                <ext:Store ID="IoGridData" OnReadData="IoGridData_OnReadData_Refresh" PageSize="20" runat="server">
                    <Model>
                        <ext:Model runat="server">
                            <Fields>
                                <ext:ModelField Name="srvId" />
                                <ext:ModelField Name="EventTime" />
                                <ext:ModelField Name="ToolPosition" />
                                <ext:ModelField Name="ToolName" />
                                <ext:ModelField Name="WorkerName" />
                                <ext:ModelField Name="DataOwner" />
                                <ext:ModelField Name="Status" />
                                <ext:ModelField Name="ReturnTime" />
                                <ext:ModelField Name="ReturnUser" />
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
                    <ext:Column runat="server" Text="借取时间" DataIndex="EventTime" Flex="1"></ext:Column>
                    <ext:Column runat="server" Text="工具位置" DataIndex="ToolPosition" Flex="1"></ext:Column>
                    <ext:Column runat="server" Text="工具名称" DataIndex="ToolName" Flex="1"></ext:Column>
                    <ext:Column runat="server" Text="借取人" DataIndex="WorkerName" Width="120" Flex="1" />
                    <ext:Column runat="server" Text="工具柜" DataIndex="DataOwner" Flex="1"></ext:Column>
                    <ext:Column runat="server" Text="状态" DataIndex="Status" Flex="1"></ext:Column>
                    <ext:Column runat="server" Text="归还时间" DataIndex="ReturnTime" Flex="1"></ext:Column>
                    <ext:Column runat="server" Text="归还人" DataIndex="ReturnUser" Flex="1"></ext:Column>
                </Columns>
            </ColumnModel>
            <SelectionModel>
                <ext:RowSelectionModel runat="server" />
            </SelectionModel>
            <BottomBar>
                <ext:PagingToolbar runat="server">
                    <Items>
                    </Items>
                    <Plugins>
                        <ext:ProgressBarPager runat="server" />
                    </Plugins>
                </ext:PagingToolbar>
            </BottomBar>
        </ext:GridPanel>
    </Items>
    <Listeners>
        <AfterRender Handler="#{IoGrid}.setHeight(#{InfoWindow}.getHeight() - 47 - 45);"></AfterRender>
    </Listeners>
    <Buttons>
        <ext:Button runat="server" Text="导出记录" ID="btnExport" OnDirectClick="btnExport_OnDirectClick"/>
    </Buttons>
</ext:Window>
</Items>
<Listeners>
    <AfterLayout Handler="#{ToolGrid}.setHeight(#{mainPanel}.getHeight() - 47 - 45);" />
</Listeners>
</ext:Viewport>
</form>
</body>
</html>
