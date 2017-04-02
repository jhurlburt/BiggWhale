<%@ Page Title="About Bigg Whale" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="About.aspx.cs" Inherits="BiggWhaleWebAppDemo.About" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <hgroup class="title">
        <h1><%: Title %>.</h1>
        <h2><br />Bigg Whale LLC, is dedicated to providing accurate data for maximum returns.</h2>
    </hgroup>

    <article>
        <p>        
            Bond data sourced from.
        </p>

        <p>        
            http://cefa.com
        </p>

        <p>        
            http://cefconnect.com
        </p>
    </article>

    <aside>
        <h3>Site Navigation</h3>
        <p>        
            Use these links to access other pages
        </p>
        <ul>
            <li><a runat="server" href="~/Search.aspx">Advanced Search</a></li>
            <li><a runat="server" href="~/Dashboard.aspx">Dashboard</a></li>
            <li><a runat="server" href="~/Contact.aspx">Contact</a></li>
        </ul>
    </aside>
</asp:Content>