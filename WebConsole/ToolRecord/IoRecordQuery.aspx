<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IoRecordQuery.aspx.cs" Inherits="ToolRecord.ToolRecord_IoRecordQuery" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>工具借还记录</title>
    <script src="/resources/js/jquery-1.12.4.min.js"></script>
    <script src="/resources/js/IoRecord/IoRecordQuery.js"></script>

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
                                <ext:Label ID="lblUser" runat="server" FieldLabel="借取人" Visible="false"/>
                                <ext:DateField ID="dtTimeStart" runat="server" FieldLabel="起始日期" MaxWidth="240"/>
                                <ext:DateField ID="dtTimeEnd" runat="server" FieldLabel="结束日期" MaxWidth="240"/>
                                <ext:TextField ID="tbSearchUserName" runat="server" FieldLabel="工号" />
                                <ext:SelectBox ID="cbToolStatus" runat="server" DisplayField="state" ValueField="abbr" FieldLabel="工具状态" EmptyText="请选择...">
                                    <Items>
                                    </Items>
                                </ext:SelectBox>
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
                                <ext:Button runat="server" Text="返回" Visible="false" ID="btnReturn">
                                    <Listeners>
                                        <Click Handler="history.back(-1); " />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button runat="server" Text="借取频次" ID="btnIoRate" OnDirectClick="btnIoRate_OnDirectClick">
                                </ext:Button>
                                <ext:Button runat="server" Text="导出记录" ID="btnExport" OnDirectClick="btnExport_OnDirectClick">
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
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
                </ext:FormPanel>

                <ext:Window
                    ID="IoRateWindow"
                    runat="server"
                    Title="借取频次"
                    Icon="Information"
                    Width="1200"
                    Height="700"
                    BodyStyle="background-color: #fff;"
                    BodyPadding="5"
                    Hidden="true"
                    Modal="true">
                    <Items>
                        <ext:Panel
                            ID="IoRatePanel"
                            runat="server"
                            Width="1200"
                            Height="600"
                            Layout="Fit">
                            <TopBar>
                                <ext:Toolbar runat="server">
                                    <Items>
                                        <ext:RadioGroup runat="server" ID="rgDate"  OnDirectChange="rgDate_OnDirectChange">
                                            <Items>
                                                <ext:Radio runat="server" ID="rDay" BoxLabel="日" InputValue="Day" />
                                                <ext:Radio runat="server" ID="rMonth" BoxLabel="月" InputValue="Month" />
                                                <ext:Radio runat="server" ID="rYear" BoxLabel="年" InputValue="Year" />
                                            </Items>
                                        </ext:RadioGroup>
                                        <ext:ToolbarFill runat="server" />
                                        <ext:DateField ID="dfStartDate" runat="server" FieldLabel="起始日期" />
                                        <ext:DateField ID="dfEndDate" runat="server" FieldLabel="结束日期" />

                                        <ext:Button
                                            runat="server"
                                            Text="查询"
                                            ID="btnIoRateQuery"
                                            Icon="ArrowRefresh"
                                            OnDirectClick="btnIoRateQuery_OnDirectClick" />
<%--                                        <ext:Button
                                            runat="server"
                                            Text="保存"
                                            Icon="Disk"
                                            Handler="saveChart" />--%>
                                    </Items>
                                </ext:Toolbar>
                            </TopBar>
                            <Items>
                                <ext:CartesianChart
                                    ID="IoChart"
                                    runat="server">
                                    <Captions>
                                        <Title Text="借取频次" />
                                        <Credits Text="" Align="Right">
                                            <Style FontStyle="italic" />
                                        </Credits>
                                        <Items>
                                            <ext:ChartCaptionItem
                                                Name="customOne"
                                                Align="Left"
                                                AlignTo="Series"
                                                Docked="Bottom">
                                                <Style FillStyle="blue" FontFamily="Arial" FontSize="10" />
                                            </ext:ChartCaptionItem>
                                        </Items>
                                    </Captions>
                                    <Store>
                                        <ext:Store ID="IoRateStore" runat="server" AutoDataBind="true" OnReadData="IoRateStore_OnReadData">
                                            <Model>
                                                <ext:Model runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="ToolName" />
                                                        <ext:ModelField Name="Rate" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>

                                    <Axes>
                                        <ext:NumericAxis
                                            Position="Left"
                                            Fields="Rate"
                                            Grid="true"
                                            Title="借取次数"
                                            Minimum="0">
                                            <Renderer Handler="return Ext.util.Format.number(label, '0,0');" />
                                        </ext:NumericAxis>

                                        <ext:CategoryAxis Position="Bottom" Fields="ToolName" Title="工具种类">
                                            <Label RotationDegrees="-45" />
                                        </ext:CategoryAxis>
                                    </Axes>
                                    <Series>
                                        <ext:BarSeries
                                            Highlight="true"
                                            XField="ToolName"
                                            YField="Rate">
                                            <Tooltip runat="server" TrackMouse="true">
                                                <Renderer Handler="toolTip.setTitle(record.get('ToolName') + ': ' + record.get('Rate'));" />
                                            </Tooltip>
                                            <Label
                                                Display="InsideEnd"
                                                Field="Rate"
                                                Orientation="Horizontal"
                                                Color="#333"
                                                TextAlign="Center"
                                                RotationDegrees="45">
                                                <Renderer Handler="return Ext.util.Format.number(text, '0');" />
                                            </Label>
                                        </ext:BarSeries>
                                    </Series>
                                </ext:CartesianChart>
                            </Items>
                        </ext:Panel>
                    </Items>
                    <Listeners>
                        <AfterRender Handler="#{IoGrid}.setHeight(#{IoRateWindow}.getHeight() - 47 - 45);"></AfterRender>
                    </Listeners>
                    <Buttons>
                        <ext:Button runat="server" Text="导出记录" ID="Button1" OnDirectClick="btnExport_OnDirectClick" />
                    </Buttons>
                </ext:Window>
            </Items>
            <Listeners>
                <AfterLayout Handler="#{IoGrid}.setHeight(#{mainPanel}.getHeight() - 47 - 45);" />
            </Listeners>

        </ext:Viewport>
    </form>
</body>
</html>
