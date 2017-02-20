<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TopHoldings.aspx.cs" Inherits="BiggWhaleWebAppDemo.TopHoldings" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>CEF Funds</title>
    <style type="text/css">
        body
        {
            font-family: Arial;
            font-size: 10pt;
        }
        table
        {
            height:50%;
            border:1px solid #ccc;
            border-collapse:collapse;
            overflow:auto;  
        }
        table th
        {
            background-color: #F7F7F7;
            color: #333;
            font-weight: bold;
            overflow:auto;  
        }
        table th, table td
        {
            padding: 5px;
            border-color: #ccc;
            overflow:auto;  
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
            <asp:PlaceHolder ID = "PlaceHolder1" runat="server" />
            <asp:PlaceHolder ID = "PlaceHolder2" runat="server" />
    </form>
</body>
</html>
