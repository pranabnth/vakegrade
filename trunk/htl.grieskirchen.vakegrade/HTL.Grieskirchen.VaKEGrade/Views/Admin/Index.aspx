<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script src="../../Scripts/jquery-1.6.4.js" type="text/javascript" />

    <script language="JavaScript" type="text/javascript">
        var id = 1;
        jQuery.ready(InitGrid);

        function InitGrid() {
            alert("starting jquery init");
            jQuery("#list").jqGrid({
                url: '/Admin/RetrieveAllStudents/',
                datatype: 'json',
                mtype: 'GET',
                colNames: ['BID', 'Flugnummer', 'Datum', 'PersonenAnzahl'],
                colModel:
                     [
                       { name: 'BID', index: 'BID', editable: true, editoptions: { readonly: true, size: 20 }, width: 55, sortable: false, hidden: true },
                       { name: 'Flugnummer', index: 'Flugnummer', editable: true, editoptions: { readonly: false, size: 20 }, width: 80, align: 'right' },
                       { name: 'Datum', index: 'Datum', editable: true, editoptions: { readonly: false, size: 20 }, width: 80, align: 'right' },
                       { name: 'PersonenAnzahl', index: 'PersonenAnzahl', editable: true, editoptions: { readonly: false, size: 20 }, width: 120, align: 'right' }
                     ],
                pager: jQuery('#pager'),
                rowNum: 5,
                //cellEdit: true,
                rowList: [5, 10, 20],
                sortname: 'id',
                sortorder: "desc",
                height: '100%',
                width: '100%',
                viewrecords: true,
                //onSelectRow: SetOrderID,
                // ondblClickRow: ShowOrderDetails,
                caption: 'FlightDetails'
                //editurl: '/Home/Edit/'

                //   });
            });
            alert("gridinit center");
            jQuery("#list").navGrid('#pager',
                   {},
                   { height: 280, reloadAfterSubmit: false, closeAfterEdit: true, closeOnEscape: true }, // edit options
                   {height: 280, reloadAfterSubmit: false, closeOnEscape: true }, // add options
                   {reloadAfterSubmit: false, closeOnEscape: true }, // del options 
                   {} // search options
                   );
            alert("gridinit complete");
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    

    <h2>Schülerdaten eingeben:</h2>


    <% using (Html.BeginForm("RecieveStudentConfig", "Admin", FormMethod.Post, new { enctype = "multipart/form-data" }))
{%><br />
    <input class="textbox" type="file" name="files" id="file1" size="25" />

    <input class="button" type="submit" value="Upload file" />      
<% } %>   

    <table id="list" class="scroll"></table>

    

</asp:Content>
