<%@ Page Title="Advanced Search" Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="BiggWhaleWebAppDemo.Search"%>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <hgroup class="title">
        <h1><%: Title %></h1>
        <h2>
            <br />
            Fund Data Search<br />
        </h2>
    </hgroup>
    <div id="advanced-search">
        <table class="table-responsive" style="table-layout: auto; empty-cells: show; width: 500px;">
            <tr>
                <td class="col-md-1">
                    <asp:Label ID="Label3" runat="server">Minimum Nav Return %</asp:Label>
                    <br />
                    <asp:TextBox runat="server" ID="txtNavReturnPct" OnTextChanged="navReturnPercent_TextChanged" TextMode="Number" />
                </td>
                <td class="col-md-1">
                    <asp:Label ID="lblTimePeriod" runat="server">Time Period</asp:Label>
                    <br />
                    <asp:DropDownList runat="server" ID="cboTimePeriod" OnSelectedIndexChanged="timePeriodSelect_SelectedIndexChanged">
                        <asp:ListItem Text="YTD" Value="0" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Weekly" Value="3"></asp:ListItem>
                        <asp:ListItem Text="Monthly" Value="4"></asp:ListItem>
                        <asp:ListItem Text="1 Year" Value="5"></asp:ListItem>
                        <asp:ListItem Text="5 Years" Value="1"></asp:ListItem>
                        <asp:ListItem Text="10 Years" Value="2"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="col-md-1">
                    <asp:Label ID="lblAssetClass" runat="server">Asset Class</asp:Label>
                    <br />
                    <asp:DropDownList runat="server" ID="cboAssetClass" OnSelectedIndexChanged="classSelect_SelectedIndexChanged" CssClass="col-xs-offset-0" />
                </td>
            </tr>
        </table>
        <asp:PlaceHolder ID = "phSearchResults" runat="server" />
        <asp:RegularExpressionValidator id="RegularExpressionValidator1"
                ControlToValidate="txtNavReturnPct"
                ValidationExpression="\d+"
                Display="Static"
                EnableClientScript="true"
                ErrorMessage="Please enter numbers only"
                runat="server"/>
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

    </div>
</asp:Content>
