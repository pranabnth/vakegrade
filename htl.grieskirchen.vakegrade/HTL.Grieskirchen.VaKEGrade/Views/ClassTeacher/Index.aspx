<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.List<HTL.Grieskirchen.VaKEGrade.Database.SchoolClass>>"  %>
<%@ Import Namespace="Trirand.Web.Mvc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    
    
    
	

   <script type="text/javascript">
       var lastPupilSelection;
       var lastSPFSelection;

       $(document).ready(function () {
           jQuery("#accordion").accordion({ autoHeight: false });
           jQuery("#pupils").jqGrid({
               url: '/ClassTeacher/RetrieveAllStudents',
               datatype: 'json',
               mtype: 'POST',
               colNames: ['Nachname', 'Vorname', 'Religion', 'Geburtsdatum', 'Geschlecht'],
               colModel: [
               // { name: 'ID', index: 'ID', width: 40, align: 'left', visible: false, editable: false },
                     {name: 'LastName', index: 'LastName', width: 150, align: 'left', editable: true },
                     { name: 'FirstName', index: 'FirstName', width: 150, align: 'left', editable: true },
                     { name: 'Religion', index: 'Religion', width: 150, align: 'left', editable: true },
                     { name: 'Birthdate', index: 'Birthdate', width: 150, align: 'left', editable: true, editoptions: {
                         size: 10, maxlengh: 10,
                         dataInit: function (element) {
                             $(element).datepicker({ dateFormat: 'dd.mm.yy', constrainInput: false, showOn: 'focus', buttonText: '...' });
                         }
                     }
                     },
                     { name: 'Gender', index: 'Gender', width: 100, align: 'left', editable: true, edittype: 'select', editoptions: { value: "m:männlich;w:weiblich"} }
                     ],
               rowNum: 10,
               rowList: [5, 10, 20, 50],
               sortname: 'Id',
               sortorder: 'desc',
               viewrecords: true,
               editurl: '/ClassTeacher/EditStudent',
               pager: '#pupilsPager',
               caption: 'Schülerliste',
               height: "auto",
               width: "700px",
               onSelectRow: editPupilRow
           }).navGrid('#pupilsPager', { add: true, edit: false, del: true, refresh: true, view: false, search: false, closeAfterEdit: true, closeAfterAdd: true, closeOnEscape: true });
           jQuery("#spfs").jqGrid({
               url: '/ClassTeacher/RetrieveSPFs?pupilID=0',
               datatype: 'json',
               mtype: 'POST',
               colNames: ['Gegenstand', 'Schulstufe'],
               colModel: [
               // { name: 'ID', index: 'ID', width: 40, align: 'left', visible: false, editable: false },
                     {name: 'SubjectID', index: 'SubjectID', width: 100, align: 'left', editable: true, edittype: "select", editrules: { required: true }, editoptions: { dataUrl: '/ClassTeacher/RetrieveAllSubjects'} },
                     { name: 'Level', index: 'Level', width: 100, align: 'left', editable: true },
                     ],
               rowNum: 10,
               rowList: [5, 10, 20, 50],
               sortname: 'Id',
               sortorder: 'desc',
               viewrecords: true,
               editurl: '/ClassTeacher/EditSPF',
               //imgpath: '/scripts/themes/coffee/images',
               caption: 'SPFs',
               height: "auto",
               pager: '#spfsPager',
               onSelectRow: editSPFRow
           }).navGrid('#spfsPager', { add: true, edit: false, del: true, refresh: true, view: false, search: false, closeAfterEdit: true, closeAfterAdd: true, closeOnEscape: true });
           $("#pupilsPager_center").remove();
           $("#pupilsPager_right").remove();
           $("#spfsPager_center").remove();
           $("#spfsPager_right").remove();
          
       });

             

       function updateSPFGrid(pupilID) {
           jQuery("#spfs").setGridParam({ url: '/ClassTeacher/RetrieveSPFs?pupilID=' + pupilID }).trigger("reloadGrid");
       }

       function editPupilRow(id) {
           if (id) {
               var grid = $("#pupils");
               grid.restoreRow(lastPupilSelection);
               grid.editRow(id, true);
               lastPupilSelection = id;
           }
           updateSPFGrid(id);
       }

       function editSPFRow(id) {
           if (id) {
               var grid = $("#spfs");
            
               grid.setGridParam({ url: '/ClassTeacher/EditSPF'});
               grid.restoreRow(lastSPFSelection);
               grid.editRow(id, true);
               lastSPFSelection = id;
           }
       }

       function receivePupils() {
           $.ajax({
               url: '/ClassTeacher/RetrieveAllStudentsHTML',
               cache: false,
               type: 'POST',
               success: function (data) {
                   jQuery("#printCandidates").append(data);
               }
           });
       }

       function requestCertificates() {
           var checked = $('#printall').attr('checked');
           if (checked) {
               window.open("/ClassTeacher/GenerateCertificates");
           } else {
               var studentIds = "";
               $("#printCandidates :checked").each(function () {

                   studentIds = studentIds+$(this).val() + ",";
               });

               if (studentIds != "") {
                   window.open("/ClassTeacher/GenerateSpecificCertificates?studentIds=" + studentIds);
               }
                
           }
       }
//       function showSPFSubGrid(subgrid_id, row_id) {
//           // the "showSubGrid_OrdersGrid" function is autogenerated and available globally on the page by the second child grid. 
//           // Calling it will place the child grid below the parent expanded row and will call the action specified by the DaraUrl property
//           // of the child grid, with ID equal to the ID of the parent expanded row                
//           showSubGrid_SPFGrid(subgrid_id, row_id);
//       }             

   </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

     




    <h2>Aufgaben:</h2>
  

    
    
    <div id="accordion" >    
    <h3><a href="#">Schülerdaten bearbeiten</a></h3>    
    <div>
        <table>
            <tr>
                <td>
                    <table id="pupils"></table>
                    <div id="pupilsPager"></div>
                </td>
                <td valign="top">
                    <table id="spfs"></table>
                    <div id="spfsPager"></div>
                </td>
            </tr>
        </table>
    </div>
    <h3><a href="#">Schüler benoten</a></h3>
    <div>
        <form action="/Teacher/">
            <input type="submit" value="Noten eingeben"/>
        </form>
    </div>

    <h3 onclick="receivePupils()"><a href="#">Zeugnisse drucken</a></h3>
    <div>
        <label for="printall">
        <input type="checkbox" id="printall"/>
        Alle Zeugnisse drucken
        </label>
        <p>Zeugnisse für folgende Schüler drucken:</p>
        <div id="printc">
        <table id="printCandidates" border="1px">            
        </table>
        </div>
        <table>
            <tr>
                <td><input type="button" id="actualizePrintCandidates" onclick="receivePupils()" value="Aktualisieren"/></td>
                <td>
        <input type="button" id="printCertificates" onclick="requestCertificates()" value="Zeugnisse drucken"/></td>
            </tr>
        </table>
    </div>
    
    </div>
   
    

    

</asp:Content>
