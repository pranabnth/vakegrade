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
     var subjectID;

     

     $(document).ready(function () {
         //$("#gradeData").hide();
         var lastPupilSelection;

         

//         jQuery("#edit").editable("/Teacher/EditGrades");

//         var keys = new KeyTable({
//             "table": document.getElementById('students')
//         });

//         $('#students tbody td').each(function () {
//             keys.event.action(this, function (nCell) {
//                 /* Block KeyTable from performing any events while jEditable is in edit mode */
//                 keys.block = true;

//                 /* Initialise the Editable instance for this table */
//                 $(nCell).editable(function (sVal) {
//                     /* Submit function (local only) - unblock KeyTable */
//                     keys.block = false;
//                     return sVal;
//                 }, {
//                     "onblur": 'submit',
//                     "onreset": function () {
//                         /* Unblock KeyTable, but only after this 'esc' key event has finished. Otherwise
//                         * it will 'esc' KeyTable as well
//                         */
//                         setTimeout(function () { keys.block = false; }, 0);
//                     }
//                 });

//                 /* Dispatch click event to go into edit mode - Saf 4 needs a timeout... */
//                 setTimeout(function () { $(nCell).click(); }, 0);
//             });
         //         });

         


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

                                 var index = $("#subjects li").index(this);

                                 subjectID = subjectData[index][1];
                                 

                                 loadTable(subjectID);

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


     });

     function loadTable(subjectID) {
         jQuery("#gradeTable").empty();
         jQuery("#gradeTable").append('<thead><tr id="head"></tr></thead>');

         $.ajax({
             url: "/Teacher/RetrieveSubArea?classID=" + g_classID + "&subjectID=" + subjectID,
             cache: false,
             type: 'POST',
             contentType: 'application/json; charset=utf-8',
             success: function (data) {

                 

                 for (var i = 0, len = data.length; i < len; ++i) {

                     jQuery("#head").append("<td>" + data[i] + "</td>");
                 }
             }
         });

         $.ajax({
             url: "/Teacher/RetrieveGrades?classID=" + g_classID + "&subjectID=" + subjectID,
             cache: false,
             type: 'POST',
             contentType: 'application/json; charset=utf-8', 
             success: function (data) {



                 for (var i = 0, len = data.length; i < len; ++i) {
                     jQuery("#gradeTable").append('<tr id="tr' + i + '"/>');
                     jQuery("#tr" + i).append('<td>' + data[i][0][0] + '</td>');
                     for (var j = 1, len2 = data[i].length; j < len2; ++j) {

                         jQuery("#tr" + i).append('<td ><input class="textbox" maxlength="1" id="tableData' + data[i][j][1] + ';' + data[i][j][2] + '" type="text" value="' + data[i][j][0] + '" onkeydown="validate(' + data[i][j][1] + ',' + data[i][j][2] + ')" )"/></td>');
                     }

                 }

             }
         });
     }

     function validate(s, a) {
         $("#status").css('background-color', 'red');
         var val = document.getElementById('tableData' + s + ';' + a).value;
         if (val != "") {
             update(s, a);
         }
     }

     function update(s, a) {



         var val = document.getElementById('tableData' + s + ';' + a).value;

         document.getElementById('tableData' + s + ';' + a).style.background = "#F39814";
         document.getElementById('tableData' + s + ';' + a).focus();

         $.ajax({
             url: "/Teacher/EditGrades?value=" + val + "&studId=" + s + "&arId=" + a,
             cache: false,
             type: 'POST',
             contentType: 'application/json; charset=utf-8',
             success: function (data) {
                 if (data == "success") {

                     $("#status").css('background-color', '#F39814');
                     document.getElementById('tableData' + s + ';' + a).style.background = "white";
                 }
             }
         });
     }

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
                     url: "/Teacher/RetrieveGradeData?classID=" + g_classID + "&subjectID=" + subjectID,
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
                     autowidth: true,
                     editurl: '/Teacher/EditGrades?subjectID='+subjectID,
                     onSelectRow: editGradeRow
//                     loadComplete: function (data) {
//                         $.each(data.rows, function (i, item) {
//                             grid.editRow(id, true);
//                         });
//                     }
                     
                 });

             }
         });


         function editGradeRow(id) {
           if (id) {
               var grid = $(this);

               grid.editRow(id, true);
               lastGradeSelection = id;
           }
       }
     }
           
 </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class"content">
    <h3>Wählen sie die Klasse aus für die sie Noten eingeben möchten</h3>

    <table>
    <tr>
    <td >
    <h3>1. Wählen sie die Klasse aus <br/> für die sie Noten eingeben möchten</h3>
    </td>
    <td>
    <h3>2. Wählen sie das Fach aus für <br/> das sie Noten eingeben möchten</h3>
    </td>
    </tr>
    <tr class="glist">
    <td>
    <ol id="classes" class="selectable">
    </ol>
    </td>
    <td>
    <ol id="subjects" class="selectable">
    </ol>
    </td>
    
    </tr>
    
    
    
       
    </table>
    <h3>3. Geben sie Noten ein</h3>
    <table id="gradeTable" rules="all" cellpadding="5" class="gradeTable">
        <thead>
        <tr id="head">
            
        </tr>
            
        </thead>
        
    </table>
    
    </div>

</asp:Content>
