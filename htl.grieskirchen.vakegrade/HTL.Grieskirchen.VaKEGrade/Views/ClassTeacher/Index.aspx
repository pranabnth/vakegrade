<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.List<HTL.Grieskirchen.VaKEGrade.Database.SchoolClass>>"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    
    
    
	

        <script type="text/javascript">
            $(document).ready(function () {
                jQuery("#students").jqGrid({
                    url: '/ClassTeacher/RetrieveAllStudents/',
                    datatype: 'json',
                    mtype: 'GET',
                    colNames: ['Nachname', 'Vorname', 'Religion', 'Geburtsdatum', 'Geschlecht'],
                    colModel: [
                                        { name: 'lastName', index: 'lastName', width: 200, align: 'left', editable: true },
                                        { name: 'firstName', index: 'firstName', width: 200, align: 'left', editable: true },
                                        { name: 'religion', index: 'religion', width: 200, align: 'left', editable: true },
                                        { name: 'birthDate', index: 'birthDate', width: 200, align: 'left', editable: true },
                                        { name: 'gender', index: 'gender', width: 40, align: 'left', editable: true }
                                        
                                        ],
                    rowNum: 10,
                    rowList: [5, 10, 20, 50],
                    pager: '#pager',
                    editurl: '/ClassTeacher/SaveStudent/1',
                    sortname: 'Id',
                    sortorder: "desc",
                    viewrecords: true,
                    caption: 'My first grid'
                });
                jQuery("#list").navGrid('#pager',
                   {},
                   { height: 280, reloadAfterSubmit: false, closeAfterEdit: true, closeOnEscape: true }, // edit options
                   {height: 280, reloadAfterSubmit: false, closeOnEscape: true }, // add options
                   {reloadAfterSubmit: false, closeOnEscape: true }, // del options 
                   {} // search options
                   );
            });

   </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

     




    <h2>Aufgaben:</h2>
  

    

    <div id="accordion">
    <h3><a href="#">Schülerdaten bearbeiten</a></h3>
        
    <div>
        <table id="students" class="list"></table>
        <div id="pager" class="scroll" style="text-align:center;"></div>
    </div>

    <h3><a href="#">Schüler benoten</a></h3>
    <div id="classes">
    </div>

    <h3><a href="#">Zeugnisse drucken</a></h3>
    <div></div>
    </div>
   
    

    

</asp:Content>
