﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.List<HTL.Grieskirchen.VaKEGrade.Database.SchoolClass>>"  %>
<%@ Import Namespace="Trirand.Web.Mvc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    
    
    
	

   <script type="text/javascript">
       var lastSelection;

       function editRow(id) {
           if (id && id !== lastSelection) {
               var grid = $("#PupilGrid");
               grid.restoreRow(lastSelection);
               grid.editRow(id, true);
               lastSelection = id;
           }
       }          

   </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

     




    <h2>Aufgaben:</h2>
  

    

    <div id="accordion">
    <h3><a href="#">Schülerdaten bearbeiten</a></h3>
        
    <div>
        <%= Html.Trirand().JQGrid((JQGrid)Session["PupilGModel"], "PupilGrid") %>
    </div>

    <h3><a href="#">Schüler benoten</a></h3>
    <div id="classes">
    </div>

    <h3><a href="#">Zeugnisse drucken</a></h3>
    <div></div>
    </div>
   
    

    

</asp:Content>
