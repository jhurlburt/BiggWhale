<%@ Page Title="Funds" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Search.aspx.cs" Inherits="BiggWhaleWebAppDemo.Search"%>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

    <script type="text/javascript" src="~/Content/jquery-latest.js"></script> 
    <script type="text/javascript" src="~/Content/jquery.tablesorter.js"></script>
    <link href="https://netdna.bootstrapcdn.com/bootstrap/3.0.3/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdn.datatables.net/plug-ins/1.10.7/integration/bootstrap/3/dataTables.bootstrap.css" rel="stylesheet" />
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js" ></script>
    <script type="text/javascript" src="https://cdn.datatables.net/1.10.7/js/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/plug-ins/1.10.7/integration/bootstrap/3/dataTables.bootstrap.js"></script>
<%--        <asp:ScriptManager ID="ScriptManagerMain"
            runat="server"
            EnablePageMethods="true" 
            ScriptMode="Release" 
            LoadScriptsBeforeUI="true">
        </asp:ScriptManager>--%>
            <table class="table-responsive" style="table-layout: auto; empty-cells: show">
                <tr>
                    <td class="col-md-1">
                        <asp:Label ID="Label3" runat="server">Select a minimum of Nav Return Percentage</asp:Label>
                        <br />
                        <asp:TextBox runat="server" ID="navReturnPercent" OnTextChanged="navReturnPercent_TextChanged" TextMode="Number">
                        </asp:TextBox>
                    </td>
                    <td class="col-md-1">
                        <asp:Label ID="Label4" runat="server">Select time period&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:Label>
                        <br />
                        <asp:DropDownList runat="server" ID="timePeriodSelect" OnSelectedIndexChanged="timePeriodSelect_SelectedIndexChanged">
                            <asp:ListItem Text="YTD" Value="0" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Weekly" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Monthly" Value="4"></asp:ListItem>
                            <asp:ListItem Text="1 Year" Value="5"></asp:ListItem>
                            <asp:ListItem Text="5 Years" Value="1"></asp:ListItem>
                            <asp:ListItem Text="10 Years" Value="2"></asp:ListItem>
                        </asp:DropDownList>

                    </td>
                    <td class="col-md-1">
                        <asp:Label ID="Label1" runat="server">Asset Class</asp:Label>
                        <br />
                        <asp:DropDownList runat="server" ID="classSelect" OnSelectedIndexChanged="classSelect_SelectedIndexChanged" CssClass="col-xs-offset-0">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="col-md-1">
                        <asp:Label ID="Label2" runat="server">Show Top Holdings for</asp:Label>
                        <br />
                        <asp:DropDownList runat="server" ID="topFundsSelect" onChange="updateDataTable();" OnSelectedIndexChanged="topFundsSelect_SelectedIndexChanged" ></asp:DropDownList>
                    </td>
                    <td class="col-md-1">&nbsp;</td>
                    <td class="col-md-1">&nbsp;</td>
                </tr>
                <tr>
                    <td class="col-md-1">
                        &nbsp;</td>
                    <td class="col-md-1">            
                        &nbsp;</td>
                    <td class="col-md-1">&nbsp;</td>
                </tr>
    </table>
            <%--<asp:DropDownList runat="server" ID="navReturnPercent" OnSelectedIndexChanged="navReturnPercent_SelectedIndexChanged">
                <asp:ListItem Text="Less than -20%" Value="0"></asp:ListItem>
                <asp:ListItem Text="-20% to -10%" Value="1"></asp:ListItem>
                <asp:ListItem Text="-10% to 0%" Value="2"></asp:ListItem>
                <asp:ListItem Text="0% to 10%" Value="3"></asp:ListItem>
                <asp:ListItem Text="10% to 20%" Value="4"></asp:ListItem>
                <asp:ListItem Text="Greater than 20%" Value="5"></asp:ListItem>
                <asp:ListItem Text="No filter." Value="6" Selected="True"></asp:ListItem>
            </asp:DropDownList>--%>
    <br />
    <br />
            <asp:PlaceHolder ID = "PlaceHolder1" runat="server" />

            
    <br />
    <br />
                        <asp:RegularExpressionValidator id="RegularExpressionValidator1"
                   ControlToValidate="navReturnPercent"
                   ValidationExpression="\d+"
                   Display="Static"
                   EnableClientScript="true"
                   ErrorMessage="Please enter numbers only"
                   runat="server"/>
    <br />

<script type="text/javascript">
    function updateDataTable() {
        //PageMethods.updateDataTable(onSuccess, onFailure);
    }

    function onSuccess(result) {
        alert(result);
    }

    function onFailure(error) {
        alert(error);
    }

    var topHoldingsTable, asc1 = 1,
            asc2 = 1,
            asc3 = 1;
    window.onload = function () {
        topHoldingsTable = document.getElementById("topHoldingsTable");
    }

    function sort_table(tbody, col, asc) {
        var rows = tbody.rows,
            rlen = rows.length,
            arr = new Array(),
            i, j, cells, clen;
        // fill the array with values from the table
        for (i = 1; i < rlen; i++) {
            cells = rows[i].cells;
            clen = cells.length;
            arr[i] = new Array();
            for (j = 0; j < clen; j++) {
                arr[i][j] = cells[j].innerHTML;
            }
        }
        // sort the array by the specified column number (col) and order (asc)
        arr.sort(function (a, b) {
            return (a[col] == b[col]) ? 0 : ((a[col] > b[col]) ? asc : -1 * asc);
        });
        // replace existing rows with new rows created from the sorted array
        for (i = 1; i < rlen; i++) {
            try {
                rows[i].innerHTML = "<td>" + arr[i-1] + "</td><td>" + "</td>";
            }
            catch (e) {
                var ex = e;
            }
        }
    }
</script>
    <asp:PlaceHolder ID = "PlaceHolder2" runat="server" />
</asp:Content>
<asp:Content ID="Content1" runat="server" contentplaceholderid="HeadContent">
    <style type="text/css">
        .auto-style3 {
            position: relative;
            min-height: 1px;
            float: left;
            width: 40%;
            left: 646px;
            top: 56px;
            padding-left: 15px;
            padding-right: 15px;
        }
    </style>
</asp:Content>
