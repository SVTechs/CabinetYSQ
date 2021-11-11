<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WrenchInfo.aspx.cs" Inherits="TorqueWrenchCheck.TorqueWrenchCheck_WrenchInfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>扭力扳手信息</title>
    <script src="/resources/js/jquery-1.12.4.min.js"></script>
    <script src="/resources/js/TorqueWrenchCheck/WrenchInfo.js"></script>

    <link rel='stylesheet' href="/resources/css/basic.css?4.2.2" />
    
    <script>
        var timeFormat = function (value) {

        };
        var checkInterval = function (value) {
            switch (value) {
                case 'Day':
                    return '天';
                case 'Month':
                    return '月';
                case 'Year':
                    return '年';
            default:
            }
            
        };
    </script>
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
                    <ext:TextField ID="tbSearchCode" runat="server" FieldLabel="编号" />
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
                    <ext:ToolbarFill runat="server"/>
                    <ext:Button runat="server" Text="新增扳手" ID="btnAddWrenchInfo" OnDirectClick="btnAddWrenchInfo_OnDirectClick" ></ext:Button>

                </Items>
            </ext:Toolbar>
        </TopBar>
        <Items>
            <ext:GridPanel
                ID="WrenchGrid"
                runat="server"
                Border="True">
                <Store>
                    <ext:Store ID="WrenchGridData" OnReadData="WrenchGridData_OnReadData" OnDataBound="WrenchGridData_OnDataBound" PageSize="20" runat="server">
                        <Model>
                            <ext:Model runat="server">
                                <Fields>
                                    <ext:ModelField Name="Id" />
                                    <ext:ModelField Name="WrenchName" />
                                    <ext:ModelField Name="WrenchCode" />
                                    <ext:ModelField Name="WrenchSpec" />
                                    <ext:ModelField Name="StandardRange" />
                                    <ext:ModelField Name="WrenchPosition" />
                                    <ext:ModelField Name="DataOwner" />
                                    <ext:ModelField Name="CheckTime" />
                                    <ext:ModelField Name="NextCheckTime" />
                                    <ext:ModelField Name="CheckInterval" />
                                    <ext:ModelField Name="CheckIntervalType" />
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
                        <ext:Column runat="server" Text="扳手名称" DataIndex="WrenchName" Flex="1"></ext:Column>
                        <ext:Column runat="server" Text="扳手编号" DataIndex="WrenchCode" Flex="1"></ext:Column>
                        <ext:Column runat="server" Text="扳手规格" DataIndex="WrenchSpec" Flex="1"></ext:Column>
                        <ext:Column runat="server" Text="量程范围" DataIndex="StandardRange" Flex="1"></ext:Column>
                        <ext:Column runat="server" Text="扳手位置" DataIndex="WrenchPosition" Flex="1"></ext:Column>
                        <ext:Column runat="server" Text="扳手所在柜" DataIndex="DataOwner" Flex="1"></ext:Column>
                        <ext:DateColumn runat="server" Text="校验时间" DataIndex="CheckTime" Flex="1" Format="yyyy-MM-dd HH:mm:ss">
                        </ext:DateColumn>
                        <ext:DateColumn runat="server" Text="下次校验时间" DataIndex="NextCheckTime" Flex="1" Format="yyyy-MM-dd HH:mm:ss">
                        </ext:DateColumn>
                        <ext:Column runat="server" Text="校验间隔" DataIndex="CheckInterval" Flex="1"></ext:Column>
                        <ext:Column runat="server" Text="校验间隔类型" DataIndex="CheckIntervalType" Flex="1">
                            <Renderer Fn="checkInterval"></Renderer>
                        </ext:Column>
                        <ext:CommandColumn runat="server" Width="96">
                            <Commands>
                                <ext:GridCommand Icon="BulletDatabase" CommandName="Info" >
                                    <ToolTip Text="校验记录" />
                                </ext:GridCommand>
                                <ext:GridCommand Icon="BulletEdit" CommandName="Edit" >
                                    <ToolTip Text="修改扳手" />
                                </ext:GridCommand>
                                <ext:GridCommand Icon="BulletDelete" CommandName="Delete" >
                                    <ToolTip Text="删除扳手" />
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
    Title="编辑信息"
    Icon="Application"
    Width="350"
    BodyStyle="background-color: #fff;"
    BodyPadding="5"
    Hidden="true"
    Modal="True">
    <Items>
        <ext:TextField ID="tbWrenchId" runat="server" Hidden="True"/>
        <ext:TextField ID="tbWrenchName" runat="server" FieldLabel="扳手名称" />
        <ext:TextField ID="tbWrenchCode" runat="server" FieldLabel="扳手编号" />
        <ext:TextField ID="tbWrenchSpec" runat="server" FieldLabel="扳手规格" />
        <ext:TextField ID="tbStandardRange" runat="server" FieldLabel="量程范围" />
        <ext:NumberField ID="nbWrenchPosition" runat="server" FieldLabel="扳手位置" />
        <ext:SelectBox
            ID="cbEditWindowCabinet"
            runat="server"
            DisplayField="state"
            ValueField="abbr"
            FieldLabel="工具柜"
            EmptyText="请选择...">
            <Items>
            </Items>
        </ext:SelectBox>
        <ext:NumberField ID="nbCheckInterval" runat="server"  FieldLabel="校验间隔" />
        <ext:SelectBox
            ID="cbCheckIntervalType"
            runat="server"
            DisplayField="state"
            ValueField="abbr"
            FieldLabel="间隔类型"
            EmptyText="请选择...">
            <Items>
                <ext:ListItem  Text="日" Value="Day"/>
                <ext:ListItem  Text="月" Value="Month"/>
                <ext:ListItem  Text="年" Value="Year"/>
            </Items>
        </ext:SelectBox>
    </Items>
    <Buttons>
        <ext:Button runat="server" Text="提交">
            <Listeners>
                <Click Handler="submitWrench(); " />
            </Listeners>
        </ext:Button>
    </Buttons>
</ext:Window>
    
<ext:Window
    ID="InfoWindow"
    runat="server"
    Title="校验记录"
    Icon="Information"
    Width="1200"
    Height="700"
    BodyStyle="background-color: #fff;"
    BodyPadding="5"
    Hidden="true"
    Modal="true">
    <Items>
        <ext:GridPanel
            ID="CheckGrid"
            runat="server"
            Border="True">
            <TopBar>
                <ext:Toolbar runat="server">
                    <Items>
                        <ext:FileUploadField runat="server" ID="fuFile" ButtonText="报告上传" ButtonOnly="True">
                            <Listeners>
                                <Change Fn="fileUpLoad"></Change>
                            </Listeners>
                        </ext:FileUploadField>
                        <ext:Button runat="server" Text="导出记录" ID="btnExport" OnDirectClick="btnExport_OnDirectClick">
                        </ext:Button>
                    </Items>
                </ext:Toolbar>
            </TopBar>
            <Store>
                <ext:Store ID="CheckGridData" OnReadData="CheckGridData_OnReadData" PageSize="20" runat="server">
                    <Model>
                        <ext:Model runat="server">
                            <Fields>
                                <ext:ModelField Name="Id" />
                                <ext:ModelField Name="WrenchId" />
                                <ext:ModelField Name="WrenchName" />
                                <ext:ModelField Name="WrenchPosition" />
                                <ext:ModelField Name="DataOwner" />
                                <ext:ModelField Name="WorkerId" />
                                <ext:ModelField Name="WorkerName" />
                                <ext:ModelField Name="EventTime" />
                                <ext:ModelField Name="Status" />
                                <ext:ModelField Name="PdfFile" />
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
                    <ext:Column runat="server" Text="WrenchId" DataIndex="WrenchId" Hidden="True" Hideable="False" Flex="1"></ext:Column>
                    <ext:Column runat="server" Text="扳手名称" DataIndex="WrenchName" Flex="1" Hidden="True"></ext:Column>
                    <ext:Column runat="server" Text="扳手位置" DataIndex="WrenchPosition" Flex="1"></ext:Column>
                    <ext:Column runat="server" Text="扳手所在柜" DataIndex="DataOwner" Flex="1"></ext:Column>
                    <ext:Column runat="server" DataIndex="WorkerId" Flex="1" Hidden="True" Hideable="False"></ext:Column>
                    <ext:Column runat="server" Text="上传人" DataIndex="WorkerName" Flex="1"></ext:Column>
                    <ext:DateColumn runat="server" Text="上传时间" DataIndex="EventTime" Flex="1" Format="yyyy-MM-dd HH:mm:ss">
                    </ext:DateColumn>
                    <ext:Column runat="server" Text="校验结果" DataIndex="Status" Flex="1"></ext:Column>
                    <ext:CommandColumn runat="server" Width="64">
                        <Commands>
                            <ext:GridCommand Icon="BulletEarth" CommandName="ShowPdf" >
                                <ToolTip Text="查看报告" />
                            </ext:GridCommand>
                            <ext:GridCommand Icon="BulletDelete" CommandName="DeletePdf" >
                                <ToolTip Text="删除报告" />
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
    <Listeners>
        <AfterRender Handler="#{CheckGrid}.setHeight(#{InfoWindow}.getHeight() - 47 - 45);"></AfterRender>
    </Listeners>
</ext:Window>
</Items>
<Listeners>
    <AfterLayout Handler="#{WrenchGrid}.setHeight(#{mainPanel}.getHeight() - 47 - 45);" />
</Listeners>
</ext:Viewport>
</form>
</body>
</html>
