﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="BiggWhaleWebAppDemo.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h2>LogIn</h2>
        <asp:Label ID="Label1" runat="server" Text="Please log in below to access Funds list."></asp:Label>
        <br />
        <br />
        <asp:Login ID="LoginControl" runat="server" 
            onauthenticate="LoginControl_Authenticate">
        </asp:Login>
    </div>
    </form>
</body>
</html>