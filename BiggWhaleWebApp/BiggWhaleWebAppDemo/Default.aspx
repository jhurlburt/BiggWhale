<%@ Page Language="C#" Title="BiggWhale" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="BiggWhaleWebAppDemo.Default" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

</asp:Content>
<!DOCTYPE html>
<html>
<head>
    <title><%: Title %> - Bigg Whale, LLC</title>
	<meta charset="utf-8" />
    <link rel="stylesheet" type="text/css" href="Content/Site.css" />
</head>
<body>
    <div class="content-wrapper">
        <div class="float-left">
            <p class="site-title">
                <img src="Images/logo_jonas_128_notext.png" />
            </p>
        </div>
        <div class="float-right">
            <ul id="menu">
                <li><a runat="server" href="~/Dashboard.aspx">Login</a></li>
            </ul>
        </div>
    </div>
    <div id="first-image">
        <img alt="financial report" src="Images/marketing-w-reports-web.jpg" /><br />
    </div>
    <div class="paragraph">
        Bigg Whale strives to create innovative visual and communication tools, as well as predictive analytical software for the financial investing industry.
    </div>
    <div id="second-image">
        <img alt="annual report" src="Images/annual-report-web-crop.jpg" /><br />
    </div>
    <div class="paragraph">
        In today's competitive marketplace, being different and unique is vital to survive. That idea has fueled Bigg Whale. We work to deliver user-friendly apps and software to professionals within the investment industry.
    </div>
</body>
</html>

        