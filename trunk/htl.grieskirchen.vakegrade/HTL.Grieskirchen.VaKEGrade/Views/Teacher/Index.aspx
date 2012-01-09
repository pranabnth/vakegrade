<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="Trirand.Web.Mvc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Noteneingabe
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
 <script type="text/javascript">

     var subjectData;
     var classData;
     var g_classID;

     $(document).ready(function () {
         //$("#gradeData").hide();

         $("#classes").selectable(
         {
             stop: function () {
                 var result = $("#subjects").empty();
                 $(".ui-selected", this).each(function () {
                     var index = $("#classes li").index(this);
                     var classID = classData[index][1];

                     retrieveSubjects(classID);
                 });

             }
         }
         );

         $("#subjects").selectable(
                     {
                         stop: function () {
                             //var result = $("#subjects").empty();
                             $(".ui-selected", this).each(function () {
                                 var index = $("#classes li").index(this);
                                 var subjectID = subjectData[index][1];
                                 
                                 retrieveGradeData(subjectID);

                             });

                         }
                     }
                    );


         $.ajax({
             url: "/Teacher/RetrieveClasses/",
             cache: false,
             success: function (data) {

                 classData = data;

                 $('#classes').empty();
                 for (var i = 0, len = data.length; i < len; ++i) {
                     $('#classes').append('<li class="ui-widget-content">' + data[i][0] + '</li>');
                 }

             }
         });




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

         //         jQuery("#grades").jqGrid({
         //             url: '/Teacher/RetrieveGradeData/',
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
         //             caption: 'Noteneingabe',
         //             autowidth: true
         //         });

     });

         function retrieveSubjects(classID) {

             g_classID = classID;
             $.ajax({
                 url: "/Teacher/RetrieveSubjectsOfClass?classID=" + classID,
                 cache: false,
                 success: function (data) {

                     subjectData = data;

                     $('#subjects').empty();
                     for (var i = 0, len = data.length; i < len; ++i) {
                         $('#subjects').append('<li class="ui-widget-content">' + data[i][0] + '</li>');
                     }


                 }
             });

         }
     

     function retrieveGradeData(subjectID) {
         $.ajax({
             url: "/Teacher/GenerateGradeGrid?classID=" + g_classID + "&subjectID=" + subjectID,
             cache: false,
             type: 'POST',
             contentType: 'application/json; charset=utf-8',
             dataType: "json",
             success: function (result) {

                 $("#GradeGrid").GridUnload();

                 var colNames = result.ColNames;
                 var colModel = result.ColModel;


                 
                 $("#GradeGrid").jqGrid({
                     url: '/Teacher/RetrieveGradeData/',
                     datatype: 'json',
                     mtype: 'GET',
                     colNames: colNames,
                     colModel: colModel,
                     rowNum: 10,
                     rowList: [5, 10, 20, 50],
                     sortname: 'Id',
                     sortorder: "desc",
                     viewrecords: true,
                     //imgpath: '/scripts/themes/coffee/images',
                     caption: 'Klassenauswahl',
                     autowidth: true
                 });


             }
         });
     }
           
 </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h3>Wählen sie die Klasse aus für die sie Noten eingeben möchten</h3>

    <table class="glist">
    <tr>
    <td >
    <h3>1. Wählen sie die Klasse aus <br/> für die sie Noten eingeben möchten</h3>
    
    <ol id="classes" class="selectable">

    </ol>
    </td>
    <td >
    <h3>2. Wählen sie das Fach aus für <br/> das sie Noten eingeben möchten</h3>
    <ol id="subjects" class="selectable">

           
    </ol>
    </td>
    </tr>
    <tr>
    <td >
    <h3>3. Geben sie Noten ein</h3>
    <div id="gradeData">
    <table id="GradeGrid"></table>
    </div>
    </td>
    </tr>
    </table>
</asp:Content>
