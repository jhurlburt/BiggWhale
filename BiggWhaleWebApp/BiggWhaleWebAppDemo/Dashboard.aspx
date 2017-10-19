<%@ Page Language="C#" Title="Dashboard" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="BiggWhaleWebAppDemo.Dashboard" EnableEventValidation="false" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <hgroup class="title">
        <h1><%: Title %></h1>
        <h2>
            <br />
            Fund Data Queries<br />
        </h2>
    </hgroup>
    <div>
        <asp:Label ID="lblError" runat="server" Visible="False" CssClass="error-message">No records to display for the selected Nav Return percentage, time period and asset class.</asp:Label>
        <table class="table-responsive" style="table-layout: auto; empty-cells: show">
            <tr>
                <td class="col-md-1">
                    <asp:Label ID="lblSelectMinNavReturn" runat="server">Minimum Nav Return %</asp:Label>
                    <br />
                    <asp:TextBox runat="server" ID="txtNavReturnPct" OnTextChanged="txtNavReturnPct_TextChanged" TextMode="Number" AutoPostBack="True">10</asp:TextBox>
                </td>
                <td class="col-md-1">
                    <asp:Label ID="lblTimePdSelect" runat="server">Time Period</asp:Label>
                    <br />
                    <asp:DropDownList runat="server" ID="cboTimePeriod" OnSelectedIndexChanged="cboTimePeriod_SelectedIndexChanged" AutoPostBack="True">
                        <asp:ListItem Text="YTD" Value="0" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Weekly" Value="3"></asp:ListItem>
                        <asp:ListItem Text="Monthly" Value="4"></asp:ListItem>
                        <asp:ListItem Text="1 Year" Value="5"></asp:ListItem>
                        <asp:ListItem Text="5 Years" Value="1"></asp:ListItem>
                        <asp:ListItem Text="10 Years" Value="2"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="col-md-1">
                    <asp:Label ID="lblAssetClassSelect" runat="server" Visible="False">Asset Class</asp:Label>
                    <br />
                    <asp:DropDownList runat="server" ID="cboClassSelect" OnSelectedIndexChanged="cboClassSelect_SelectedIndexChanged" CssClass="col-xs-offset-0" AutoPostBack="True" Visible="False">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="col-md-1">
                    <asp:Button ID="btnAllClasses" runat="server" OnClick="btnAllClasses_Click" Text="All Classes" />
                    <br />
                </td>
                <td class="col-md-1">&nbsp;</td>
                <td class="col-md-1">&nbsp;</td>
            </tr>
        </table>
        <asp:Panel ID="pnlChart" runat="server" Visible="true">
            <div id="asset-class-chart" style="float: left;">
                <asp:Chart ID="chrAssetClass" runat="server" DataMember="DefaultView" Height="431px" Palette="Bright" Width="600px" OnClick="chrAssetClass_Click">
                    <Series>
                        <asp:Series ChartType="Pie" Legend="Legend1" Name="Series1" XValueMember="Asset_Class" YValueMembers="Count" PostBackValue="#VALX" ToolTip="#AXISLABEL : #PERCENT" CustomProperties="PieLabelStyle=Disabled">
                        </asp:Series>
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1">
                        </asp:ChartArea>
                    </ChartAreas>
                    <Legends>
                        <asp:Legend Name="Legend1">
                        </asp:Legend>
                    </Legends>
                    <Titles>
                        <asp:Title Name="Asset Classes" Text="Asset Classes">
                        </asp:Title>
                    </Titles>
                </asp:Chart>
            </div>
            <div id="asset-class-key" style="float: left; margin-left: 50px;">
                <asp:Label ID="lblAssetKey" runat="server" Text="Count of funds" CssClass="grid-title"></asp:Label>
                <asp:GridView ID="grdAssetKey" runat="server" AllowSorting="False" AutoGenerateColumns="False" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical">
                    <AlternatingRowStyle BackColor="#DCDCDC" />
                    <Columns>
                        <asp:BoundField DataField="Asset_Class" HeaderText="Asset Class" SortExpression="Asset_Class" />
                        <asp:BoundField DataField="Count" HeaderText="Count" ReadOnly="True" SortExpression="Count" />
                    </Columns>
                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                    <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                    <SortedAscendingHeaderStyle BackColor="#0000A9" />
                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                    <SortedDescendingHeaderStyle BackColor="#000065" />
                </asp:GridView>
            </div>
            <asp:SqlDataSource ID="dsAssetClasses" runat="server" ConnectionString="<%$ ConnectionStrings:CEF_dbConnectionString %>"
                SelectCommand="SELECT [Asset Class] AS Asset_Class, Count([Asset Class]) AS Count 
                FROM [Funds] a, [FundDetails] b 
                WHERE a.Id = b.Fund_Id 
                AND b.[Crawl Date] = (select Max([Crawl Date]) from FundDetails where fund_id = a.id)
                AND b.YTDNAVReturn &gt;= @ReturnPercent 
                GROUP BY [Asset Class] 
                ORDER BY Count">
                <SelectParameters>
                    <asp:SessionParameter Name="ReturnPercent" SessionField="ReturnPercent" />
                </SelectParameters>
            </asp:SqlDataSource>
            <div id="fund-returns" style="float:left;">
                <asp:Label ID="lblAssetClassFundReturns" runat="server" Text="Asset Class Fund Returns for All Classes"></asp:Label>
                <asp:GridView ID="grdFundSummaryDetail" runat="server" AllowSorting="False" AutoGenerateColumns="False" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" OnDataBound="grdFundSummaryDetail_DataBound">
                    <AlternatingRowStyle BackColor="#DCDCDC" />
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                        <asp:BoundField DataField="Asset Class" HeaderText="Asset Class" SortExpression="Asset Class" />
                        <asp:BoundField DataField="Ticker Symbol" HeaderText="Ticker Symbol" SortExpression="Ticker Symbol" />
                        <asp:BoundField DataField="YTD NAV Return" HeaderText="YTD NAV Return" SortExpression="YTD NAV Return" />
                        <asp:BoundField DataField="One Year NAV" HeaderText="One Year NAV" SortExpression="One Year NAV" />
                        <asp:BoundField DataField="YTD Market Return" HeaderText="YTD Market Return" SortExpression="YTD Market Return" />
                        <asp:BoundField DataField="One Year Market Return" HeaderText="One Year Market Return" SortExpression="One Year Market Return" />
                        <asp:BoundField DataField="Discount" HeaderText="Discount" SortExpression="Discount" />
                        <asp:BoundField DataField="One Year Discount" HeaderText="One Year Discount" SortExpression="One Year Discount" />
                        <asp:BoundField DataField="NAV Trend" HeaderText="NAV Trend" ReadOnly="True" SortExpression="NAV Trend" ShowHeader="False" Visible="False" />
                        <asp:BoundField DataField="Market Trend" HeaderText="Market Trend" ReadOnly="True" SortExpression="Market Trend" />
                        <asp:BoundField DataField="Discount Trend" HeaderText="Discount Trend" ReadOnly="True" SortExpression="Discount Trend" />
                    </Columns>
                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                    <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                    <SortedAscendingHeaderStyle BackColor="#0000A9" />
                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                    <SortedDescendingHeaderStyle BackColor="#000065" />
                </asp:GridView>
                <asp:SqlDataSource ID="dsFundSummaryDetail" runat="server" ConnectionString="<%$ ConnectionStrings:CEF_dbConnectionString %>"
                    SelectCommand="select distinct f.Name, f.[Asset Class], f.[Ticker Symbol], YTDNAVReturn as 'YTD NAV Return', OneYearNAVReturnAct  as 'One Year NAV',YTDMarketReturn as 'YTD Market Return', OneYearMarketReturn as 'One Year Market Return', PremiumDiscount as 'Discount',YTDPremiumDiscountAvg as 'One Year Discount', case WHEN YTDNAVReturn-OneYearNAVReturnAct &gt; 0 THEN 'Rising' WHEN YTDNAVReturn-OneYearNAVReturnAct = 0 THEN 'Steady' ELSE 'Falling' END as 'NAV Trend',case WHEN YTDMarketReturn-OneYearMarketReturn &gt; 0 THEN 'Rising' WHEN YTDMarketReturn-OneYearMarketReturn = 0 THEN 'Steady' ELSE 'Falling' END as 'Market Trend', case WHEN PremiumDiscount-YTDPremiumDiscountAvg &gt; 0 THEN 'Rising' WHEN PremiumDiscount-YTDPremiumDiscountAvg = 0 THEN 'Steady' ELSE 'Falling' END as 'Discount Trend'
                        from Funds f, FundDetails fd  
                         where f.id = fd.fund_id  
                         and f.[Asset Class] like @AssetClass 
                        and fd.[Crawl Date] = (select Max([Crawl Date]) from FundDetails where fund_id = f.id) 
                         and fd.YTDNAVReturn  &gt;= @ReturnPercent
                        order by  f.[Asset Class]">
                    <SelectParameters>
                        <asp:SessionParameter Name="AssetClass" SessionField="AssetClass" DbType="String" />
                        <asp:SessionParameter Name="ReturnPercent" SessionField="ReturnPercent" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
