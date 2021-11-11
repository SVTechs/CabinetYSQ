﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CabinetMonitor.aspx.cs" Inherits="ToolMonitor.ToolMonitor_CabinetMonitor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>工具柜选择</title>
    <link href="/resources/css/examples.css" rel="stylesheet" />

    <style>
        div.item-wrap {
            float: left;
            border: 1px solid transparent;
            margin: 5px 25px 5px 25px;
            width: 100px;
            cursor: pointer;
            height: 120px;
            text-align: center;
        }

            div.item-wrap img {
                margin: 5px 0px 0px 5px;
                width: 61px;
                height: 77px;
            }

            div.item-wrap h6 {
                font-size: 14px;
                color: #3A4B5B;
                font-family: tahoma,arial,san-serif;
                margin: 0px;
            }

        div.x-view-over {
            border: solid 1px silver;
        }

        #items-ct {
            padding: 0px 30px 24px 30px;
        }

            #items-ct h2 {
                border-bottom: 2px solid #3A4B5B;
                cursor: pointer;
            }

                #items-ct h2 div {
                    background: transparent url(resources/images/group-expand-sprite.gif) no-repeat 3px -47px;
                    padding: 4px 4px 4px 17px;
                    font-family: tahoma,arial,san-serif;
                    font-size: 12px;
                    color: #3A4B5B;
                }

            #items-ct .collapsed h2 div {
                background-position: 3px 3px;
            }

            #items-ct .group-body {
                margin-left: 2px;
            }

            #items-ct .collapsed .group-body {
                display: none;
            }
    </style>

    <script>
        var itemClick = function (view, record, item, index, e) {
            var group = e.getTarget("h2", 3, true),
                subItem;

            if (group) {
                group.up("div").toggleCls("collapsed");
                return false;
            }

            subItem = e.getTarget(".item-wrap");

            if (subItem) {
                //Ext.Msg.alert("Click", "The node with id='" + subItem.id + "' has been clicked");
            }
        };

        function refreshGrid() {
             App.CabinetGrid.store.reload();
        }
        var t = window.setInterval(refreshGrid, 3000);
    </script>
</head>
<body>
    <form runat="server">
        <ext:ResourceManager runat="server" />

        <h2 style="margin-top: 0;"><asp:LinkButton ID="btnCabinetChooser" runat="server" style="color: black;" OnClick="btnCabinetChooser_Click">所有工具柜</asp:LinkButton> - <%=CabinetName %></h2>

        <ext:Panel
            runat="server"
            Cls="items-view"
            Border="false">
            <Items>
                <ext:DataView
                    ID="CabinetGrid"
                    runat="server"
                    SingleSelect="true"
                    ItemSelector="div.group-header"
                    EmptyText="没有找到工具信息">
                    <Store>
                        <ext:Store ID="GroupStore" runat="server">
                            <Model>
                                <ext:Model runat="server">
                                    <Fields>
                                        <ext:ModelField Name="Title" />
                                        <ext:ModelField Name="Items" Type="Object" />
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <Tpl runat="server">
                        <Html>
                            <div id="items-ct">
                        <tpl for=".">
                            <div class="group-header">
                                <h2><div>{Title}</div></h2>
                                <div class="group-body">
                                    <tpl for="Items">
                                        <div id="{Id}" class="item-wrap" style="background-color: {BackColor}; height:150px;">
                                            <img src="{Icon}"/>
                                            <div>
                                                <h6 style="color: white;">{ToolCode}.{Title}</h6>
                                                <h6 style="color: white;">{ToolStatus}</h6>
                                            </div>
                                        </div>
                                    </tpl>
                                    <div style="clear:left"></div>
                                </div>
                            </div>
                        </tpl>
                    </div>
                        </Html>
                    </Tpl>
                    <Listeners>
                        <ItemClick Fn="itemClick" />
                        <Refresh Handler="this.el.select('.item-wrap', true).addClsOnOver('x-view-over');" Delay="100" />
                    </Listeners>
                </ext:DataView>
            </Items>
        </ext:Panel>
    </form>
</body>
</html>
