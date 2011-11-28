<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Noteneingabe
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
 <script type="text/javascript">


     $(document).ready(function () {

         $("#classes").selectable(
         {
             stop: function () {
                 var result = $("#subjects").empty();
                 $(".ui-selected", this).each(function () {
                     var index = $("#classes li").index(this);
                     retrieveSubjects(index);
                 });

             }
         }
         );

         $("#subjects").selectable();


         //         jQuery("#classes").jqGrid({
         //             url: '/Teacher/RetrieveClasses/',
         //             datatype: 'json',
         //             mtype: 'GET',
         //             colNames: ['Id', 'Votes'],
         //             colModel: [
         //                                        { name: 'Id', index: 'Id', width: 40, align: 'left', formatter: pointercursor},
         //                                        { name: 'Votes', index: 'Votes', width: 40, align: 'left' }],
         //             rowNum: 10,
         //             rowList: [5, 10, 20, 50],
         //             sortname: 'Id',
         //             sortorder: "desc",
         //             viewrecords: true,
         //             //imgpath: '/scripts/themes/coffee/images',
         //             caption: 'Klassenauswahl',
         //             autowidth: true
         //         });

         //         jQuery("#subjects").jqGrid({
         //             url: '/Teacher/RetrieveSubjects/',
         //             datatype: 'json',
         //             mtype: 'GET',
         //             colNames: ['Id', 'Votes', 'Title'],
         //             colModel: [
         //                                        { name: 'Id', index: 'Id', width: 40, align: 'left' },
         //                                        { name: 'Votes', index: 'Votes', width: 40, align: 'left' },
         //                                        { name: 'Title', index: 'Title', width: 200, align: 'left'}],
         //             rowNum: 10,
         //             rowList: [5, 10, 20, 50],
         //             sortname: 'Id',
         //             sortorder: "desc",
         //             viewrecords: true,
         //             //imgpath: '/scripts/themes/coffee/images',
         //             caption: 'Fachauswahl',
         //             autowidth: true
         //         });

         jQuery("#grades").jqGrid({
             url: '/Teacher/RetrieveGradeData/',
             datatype: 'json',
             mtype: 'GET',
             colNames: ['Id', 'Votes', 'Title'],
             colModel: [
                                        { name: 'Id', index: 'Id', width: 40, align: 'left' },
                                        { name: 'Votes', index: 'Votes', width: 40, align: 'left' },
                                        { name: 'Title', index: 'Title', width: 200, align: 'left'}],
             rowNum: 10,
             rowList: [5, 10, 20, 50],
             sortname: 'Id',
             sortorder: "desc",
             viewrecords: true,
             //imgpath: '/scripts/themes/coffee/images',
             caption: 'Noteneingabe',
             autowidth: true
         });



         function retrieveSubjects(classID) {
             
             $.ajax({
                 url: "/Teacher/RetrieveSubjectsOfClass?classID=" + classID,
                 cache: false,
                 success: function (eureMutter) {
                     alert("tschabam");
                 }
             });

         }

     });
           
 </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h3>Wählen sie die Klasse aus für die sie Noten eingeben möchten</h3>

    <table class="glist">
    <tr>
    <td >
    <h3>1. Wählen sie die Klasse aus <br/> für die sie Noten eingeben möchten</h3>
    
    <ol id="classes" class="selectable">

           <%foreach (HTL.Grieskirchen.VaKEGrade.Database.SchoolClass schoolClass in (List<HTL.Grieskirchen.VaKEGrade.Database.SchoolClass>)Model)
             { %>

             <li class="ui-widget-content"><%:schoolClass.Level.ToString() + schoolClass.Name %></li>

           
           <%} %>
    </ol>
    </td>
    <td >
    <h3>2. Wählen sie das Fach aus für <br/> das sie Noten eingeben möchten</h3>
    <ol id="subjects" class="selectable">

           <%foreach (HTL.Grieskirchen.VaKEGrade.Database.SchoolClass schoolClass in (List<HTL.Grieskirchen.VaKEGrade.Database.SchoolClass>)Model)
             { %>

             <li class="ui-widget-content"><%:schoolClass.Level.ToString() + schoolClass.Name %></li>

           
           <%} %>
    </ol>
    </td>
    </tr>
    <tr>
    <td >
    <h3>3. Geben sie Noten ein</h3>
    <table id="grades" ></table>
    </td>
    </tr>
    </table>
</asp:Content>
