<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.5/jquery.min.js"></script>
        <script type="text/javascript">
            $(document).ready(function () {




            });
   </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

     




    <h2>Schülerdaten eingeben:</h2>


    <% using (Html.BeginForm("RecieveStudentConfig", "Admin", FormMethod.Post, new { enctype = "multipart/form-data" }))
{%><br />
    <input class="file" type="file" name="files" id="file1" size="25" />

    <input class="button" type="submit" value="Upload file" />      
<% } %>   

    <table id="list" class="scroll"></table>

    

</asp:Content>
