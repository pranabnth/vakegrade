<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    
    
    

        <script type="text/javascript">
//            $(function () {
//                $("#accordion").accordion();
//            });
           
            $(document).ready(function () {


               



                jQuery("#students").jqGrid({
                    url: '/Admin/RetrieveAllStudents/',
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
                    caption: 'My first grid'
                });

                jQuery("#subjects").jqGrid({
                    url: '/Admin/RetrieveAllStudents/',
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
                    caption: 'My first grid'
                });

                jQuery("#classes").jqGrid({
                    url: '/Admin/RetrieveAllStudents/',
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
                    caption: 'My first grid'
                });

                jQuery("#branches").jqGrid({
                    url: '/Admin/RetrieveAllStudents/',
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
                    caption: 'My first grid'
                });

                jQuery("#subject_list").jqGrid({
                    url: '/Admin/RetrieveAllStudents/',
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
                    caption: 'My first grid'
                });

                jQuery("#branch_list").jqGrid({
                    url: '/Admin/RetrieveAllStudents/',
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
                    caption: 'My first grid'
                });

            });
   </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

     




    <h2>Aufgaben:</h2>
  

    

    <div id="accordion">
    <h3><a href="#">Neuen Schülerdaten importieren</a></h3>
    <div><% using (Html.BeginForm("RecieveStudentConfig", "Admin", FormMethod.Post, new { enctype = "multipart/form-data" }))
{%><br />
    <input class="file" type="file" name="files" id="file1" size="25" />

    <input class="button" type="submit" value="Upload file"/>      
<% } %>   </div>

    <h3><a href="#">Schülerdaten anzeigen</a></h3>
    <div><table id="students" class="list"></table></div>
    <h3><a href="#">Fächer verwalten</a></h3>
    <div><table id="subjects" class="list"></table></div>
    
    <h3><a href="#">Zweige verwalten</a></h3>
    <table class="list"><tr><td>
    <div><table id="branches" ></table></div>
    </td><td>
    <img src="../../Content/Arrow.png" alt="doppelpfeil"/>
    </td>
    <td>
    <div><table id="subject_list" ></table></div>
    </td>
    </tr>
    </table>
    <h3><a href="#">Klassen verwalten</a></h3>
    <table class="list"><tr><td>
    
    <div><table id="classes" ></table></div>
    </td><td>
    <img src="../../Content/Arrow.png" alt="doppelpfeil"/>
    </td>
    <td>
    <div><table id="branch_list" ></table></div>
    </td>
    </tr>
    </table>
    
    </div>
   

    

</asp:Content>
