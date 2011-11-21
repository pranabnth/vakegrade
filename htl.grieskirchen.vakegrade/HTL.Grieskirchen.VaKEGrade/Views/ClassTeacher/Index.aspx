<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    
    
    

        <script type="text/javascript">


            $(document).ready(function () {


                $("#accordion").accordion();



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
    <h3><a href="#">Schülerdaten bearbeiten</a></h3>
    <div><table id="subjects" class="list"></table></div>

    <h3><a href="#">Schüler benoten</a></h3>
    <div><table id="students" class="list"></table></div>

    <h3><a href="#">Zeugnisse drucken</a></h3>
        
    </div>
   
    

    

</asp:Content>
