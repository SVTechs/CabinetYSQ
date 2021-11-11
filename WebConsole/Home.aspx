<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Home" %>

<%@ Import Namespace="WebConsole.Config" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="resources/css/main.css" />
</head>

<body class="welcome">
    <ext:ResourceManager runat="server" />

    <ext:Container runat="server">
        <Content>
            <div class="container">
                <div class="extnet-notification">
                    <div class="notification-container">
                        <div class="notification-img">
                            <%--<img src="resources/images/crch.png" style="width: auto; height: 80px; margin-top: 20px; margin-left: 20px;">--%>
                        </div>
                        <div class="notification-text">
                            欢迎使用 <strong>迎水桥机务段</strong> <%=Env.AppName %><br/>
                            当前用户:<%=RealName %>, 上次登录:<%=LastLogin %>
                        </div>
                    </div>
                </div>

                <div class="welcome-cards">

                    <div class="welcome-card">
                        <div class="welcome-card-icon">
                            <img src="resources/icons/docs.svg"/>
                        </div>
                        <div class="welcome-card-title">程序说明</div>
                        <div class="welcome-card-body">
                            <p>
                                <%=Env.AppName %>具备工具管理、状态检测、作业数据的采集管理功能，
                                实现信息系统数据的互联互通，便于工具的管理与状态显示。
                            </p>
                        </div>
                    </div>

                    <div class="welcome-card">
                        <div class="welcome-card-icon">
                            <img src="resources/icons/support.svg" />
                        </div>
                        <div class="welcome-card-title">支持</div>
                        <div class="welcome-card-body">
                            <p>
                                <%=Env.AppName %>由神州高铁、新联铁设计开发，如遇到使用问题，
                                请联系我们的技术部门获取支持。
                            </p>
                        </div>
                    </div>

                    <div class="welcome-card">
                        <div class="welcome-card-icon">
                            <img src="resources/icons/download.svg" />
                        </div>
                        <div class="welcome-card-title">兼容性</div>
                        <div class="welcome-card-body">
                            <p>
                                请使用基于Google Chrome或Mozilla Firefox核心的浏览器访问本网站
                            </p>
                        </div>
                    </div>

                </div>

                <ul class="popular-links">
                    <li><a href="javascript:void(0);">版本: <%= Env.AppVer %></a></li>
                    <li><a href="javascript:void(0);">神州高铁</a></li>
                    <li><a href="javascript:void(0);">新联铁</a></li>
                </ul>
            </div>
        </Content>
    </ext:Container>

</body>
</html>
