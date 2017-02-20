<%@ Page Language="C#" Title="Dashboard" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="BiggWhaleWebAppDemo.Dashboard" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <hgroup class="title">
        <h1><%: Title %>.</h1>
        <h2>
            <br />
            Fund Data Queries<br />
        </h2>
    </hgroup>
    <div>
        <table class="table-responsive" style="table-layout: auto; empty-cells: show">
            <tr>
                <td class="col-md-1">
                    <asp:Label ID="Label1" runat="server">Select a minimum of Nav Return Percentage</asp:Label>
                    <br />
                    <asp:TextBox runat="server" ID="navReturnPercentFilter2" OnTextChanged="navReturnPercentFilter2_TextChanged" TextMode="Number" AutoPostBack="True">10</asp:TextBox>
                </td>
                <td class="col-md-1">
                    <asp:Label ID="Label2" runat="server">Select time period&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:Label>
                    <br />
                    <asp:DropDownList runat="server" ID="timePeriodSelect" OnSelectedIndexChanged="timePeriodSelect_SelectedIndexChanged" AutoPostBack="True">
                        <asp:ListItem Text="YTD" Value="0" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Weekly" Value="3"></asp:ListItem>
                        <asp:ListItem Text="Monthly" Value="4"></asp:ListItem>
                        <asp:ListItem Text="1 Year" Value="5"></asp:ListItem>
                        <asp:ListItem Text="5 Years" Value="1"></asp:ListItem>
                        <asp:ListItem Text="10 Years" Value="2"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="col-md-1">
                    <asp:Label ID="Label8" runat="server" Visible="False">Asset Class</asp:Label>
                    <br />
                    <asp:DropDownList runat="server" ID="classSelect" OnSelectedIndexChanged="classSelect_SelectedIndexChanged" CssClass="col-xs-offset-0" AutoPostBack="True" Visible="False">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="col-md-1">
                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click1" Text="All Classes" />
                    <br />
                </td>
                <td class="col-md-1">&nbsp;</td>
                <td class="col-md-1">&nbsp;</td>
            </tr>
            <tr>
                <td class="col-md-1">&nbsp;</td>
                <td class="col-md-1">&nbsp;</td>
                <td class="col-md-1">&nbsp;</td>
            </tr>
        </table>
        <asp:Chart ID="Chart1" runat="server" DataMember="DefaultView" Height="431px" Palette="Bright" Width="600px" OnClick="Chart1_Click" OnLoad="Chart1_Load">
            <Series>
                <asp:Series ChartType="Pie" Legend="Legend1" Name="Series1" XValueMember="Asset_Class" YValueMembers="Count" PostBackValue="#VALX" ToolTip="#AXISLABEL : #PERCENT">
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
        <br />
        <br />
        <asp:Label ID="Label7" runat="server" Text="Count of funds"></asp:Label>

        <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" DataSourceID="FundAssetClasses" GridLines="Vertical">
            <AlternatingRowStyle BackColor="#DCDCDC" />
            <Columns>
                <asp:BoundField DataField="Asset_Class" HeaderText="Asset_Class" SortExpression="Asset_Class" />
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
        <br />

        <asp:SqlDataSource ID="FundAssetClasses" runat="server" ConnectionString="<%$ ConnectionStrings:CEF_dbConnectionString %>"
            SelectCommand="SELECT [Asset Class] AS Asset_Class, Count([Asset Class]) AS Count 
            FROM [Funds] a, [FundDetails] b 
            WHERE a.Id = b.Fund_Id 
            AND b.[Crawl Date] = (select Max([Crawl Date]) from FundDetails where fund_id = a.id)
            AND b.YTDNAVReturn &gt;= @Chart1ReturnPercent 
            GROUP BY [Asset Class] 
            ORDER BY Count">
            <SelectParameters>
                <asp:SessionParameter Name="Chart1ReturnPercent" SessionField="ReturnPercent" />
            </SelectParameters>
        </asp:SqlDataSource>

        <asp:Label ID="Label9" runat="server" Text="Asset Class Fund Returns for All Classes"></asp:Label>

        <asp:GridView ID="GridView2" runat="server" AllowSorting="True" AutoGenerateColumns="False" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" DataSourceID="FundSummaryDetails" GridLines="Vertical" OnDataBound="GridView2_DataBound">
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
        <asp:SqlDataSource ID="FundSummaryDetails" runat="server" ConnectionString="<%$ ConnectionStrings:CEF_dbConnectionString %>"
            SelectCommand="select distinct f.Name, f.[Asset Class], f.[Ticker Symbol], YTDNAVReturn as 'YTD NAV Return', OneYearNAVReturnAct  as 'One Year NAV',YTDMarketReturn as 'YTD Market Return', OneYearMarketReturn as 'One Year Market Return', PremiumDiscount as 'Discount',YTDPremiumDiscountAvg as 'One Year Discount', case WHEN YTDNAVReturn-OneYearNAVReturnAct &gt; 0 THEN 'Rising' WHEN YTDNAVReturn-OneYearNAVReturnAct = 0 THEN 'Steady' ELSE 'Falling' END as 'NAV Trend',case WHEN YTDMarketReturn-OneYearMarketReturn &gt; 0 THEN 'Rising' WHEN YTDMarketReturn-OneYearMarketReturn = 0 THEN 'Steady' ELSE 'Falling' END as 'Market Trend', case WHEN PremiumDiscount-YTDPremiumDiscountAvg &gt; 0 THEN 'Rising' WHEN PremiumDiscount-YTDPremiumDiscountAvg = 0 THEN 'Steady' ELSE 'Falling' END as 'Discount Trend'
                from Funds f, FundDetails fd  
                 where f.id = fd.fund_id  
                 and f.[Asset Class] like @SessionChart1AssetClass 
                and fd.[Crawl Date] = (select Max([Crawl Date]) from FundDetails where fund_id = f.id) 
                 and fd.YTDNAVReturn  &gt;= @Chart1ReturnPercent
                order by  f.[Asset Class]">
            <SelectParameters>
                <asp:SessionParameter Name="SessionChart1AssetClass" SessionField="Chart1AssetClass" DbType="String" />
                <asp:SessionParameter Name="Chart1ReturnPercent" SessionField="ReturnPercent" />
            </SelectParameters>
        </asp:SqlDataSource>
        <br />
        <br />

        <table style="border-collapse: collapse; border-spacing: 0px; table-layout: auto;" border="1" class="table">
            <tr>
                <td class="col-md-1">
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:CEF_dbConnectionString %>"
                        SelectCommand="select a.[Asset Class], a.TotalFunds as 'Total Funds', b.FilteredFunds as 'Filtered Funds'
                            From ( 
                             select distinct fb.[Asset Class], count(fb.Name) as TotalFunds
                             from (select distinct f.id, f.Name, f.[Asset Class], YTDNAVReturn from Funds f, FundDetails fd  
                             where f.id = fd.fund_id  
                             and f.[Asset Class] like '%'
                             and fd.[Crawl Date] = (select Max([Crawl Date]) from FundDetails where fund_id = f.id) 
                             and fd.YTDNAVReturn &gt;= -1000 and fd.YTDNAVReturn &lt;= 1000) fb 
                             group by fb.[Asset Class] 
                             ) a 
                            Inner Join (
                             select distinct fc.[Asset Class], count(fc.Name) as FilteredFunds
                              from ( select * from (select distinct f.id, f.Name, f.[Asset Class], YTDNAVReturn from Funds f, FundDetails fd 
                             where f.id = fd.fund_id  
                             and fd.[Crawl Date] = (select Max([Crawl Date]) from FundDetails where fund_id = f.id) 
                             and fd.YTDNAVReturn &gt;= @Chart1ReturnPercent ) a 
                            ) fc 
                            group by fc.[Asset Class] 
                            ) b 
                             On a.[Asset Class] = b.[Asset Class] 
                             order by b.[FilteredFunds] desc">
                        <SelectParameters>
                            <asp:SessionParameter Name="Chart1ReturnPercent" SessionField="ReturnPercent" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <asp:SqlDataSource ID="DashboardTable1" runat="server" ConnectionString="<%$ ConnectionStrings:CEF_dbConnectionString %>" SelectCommand="select [Asset Class], count([Name]) as  'Fund Count' from (select distinct f.id, f.[Asset Class], f.Name, f.[Ticker Symbol], fd.YTDNAVReturn, fd.YTDMarketReturn, f.[Percent Leveraged Assets], fd.DistributionYield, f.[Total Net Assets], fd.YTDPremiumDiscountAvg, fd.[Crawl Date]  from Funds f, FundDetails fd  where f.id = fd.fund_id  and fd.[Crawl Date] = (select Max([Crawl Date]) from FundDetails where fund_id = f.id) and fd.YTDNAVReturn &gt;= 10 ) a  
group By [Asset Class] order by [Fund Count] desc, a.[Asset Class] asc"></asp:SqlDataSource>
                </td>
            </tr>
            <tr>
                <td class="col-md-1"></td>
            </tr>
            <tr>
                <td class="col-md-1">&nbsp;</td>
            </tr>
            <tr>
                <td class="col-md-1">&nbsp;</td>
            </tr>
        </table>
    </div>
</asp:Content>
