<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    
    
        <script type="text/javascript" src="../../Scripts/jquery.nestedAccordion.js"></script>

        <script type="text/javascript">
            

            $(document).ready(function () {

                $("html").addClass("js");
                $.fn.accordion.defaults.container = false;

                $("#accordion").accordion({
                    obj: "div",
                    wrapper: "div",
                    el: ".h",
                    head: "h4, h5",
                    next: "div",
                    showMethod: "slideFadeDown",
                    hideMethod: "slideFadeUp",
                    initShow: "div.shown"
                });
               



                jQuery("#students").jqGrid({
                    url: '/Admin/RetrieveAllStudents/',
                    datatype: 'json',
                    mtype: 'GET',
                    colNames: ['Nachname', 'Vorname', 'Title'],
                    colModel: [
                                        { name: 'Nachname', index: 'Nachname', width: 40, align: 'left' },
                                        { name: 'Vorname', index: 'Vorname', width: 40, align: 'left' },
                                        { name: 'Title', index: 'Title', width: 200, align: 'left'}],
                    rowNum: 10,
                    rowList: [5, 10, 20, 50],
                    sortname: 'Id',
                    sortorder: "desc",
                    viewrecords: true,
                    //imgpath: '/scripts/themes/coffee/images',
                    caption: 'Schüler'
                });

                jQuery("#classes").jqGrid({
                    url: '/Admin/RetrieveClassList/',
                    datatype: 'json',
                    width: 400,
                    mtype: 'GET',
                    colNames: ['Stufe', 'Bezeichnung', 'Lehrer'],
                    colModel: [
                                        { name: 'Stufe', index: 'level', width: 50, align: 'center', editable: 'true' },
                                        { name: 'Bezeichnung', index: 'name', width: 50, align: 'center', editable: 'true' },
                                        { name: 'Teacher', index: 'teacher', width: 50, align: 'center', editable: 'true' }
                                        ],
                    rowNum: 10,
                    rowList: [5, 10, 20, 50],
                    sortname: 'Id',
                    sortorder: "desc",
                    viewrecords: true,
                    caption: 'Klassen',
                    pager: '#classpager',
                    editurl: '/Admin/EditClass',
                    viewrecords: true
                });

                jQuery("#branches").jqGrid({
                    url: '/Admin/RetrieveBranchList/',
                    datatype: 'json',
                    width: 400,
                    mtype: 'GET',
                    colNames: ['Stufe', 'Bezeichnung', 'Lehrer'],
                    colModel: [
                                        { name: 'Stufe', index: 'level', width: 50, align: 'center', editable: 'true' },
                                        { name: 'Bezeichnung', index: 'name', width: 50, align: 'center', editable: 'true' },
                                        { name: 'Teacher', index: 'teacher', width: 50, align: 'center', editable: 'true' }
                                        ],
                    rowNum: 10,
                    rowList: [5, 10, 20, 50],
                    sortname: 'Id',
                    sortorder: "desc",
                    viewrecords: true,
                    caption: 'Zweige',

                    viewrecords: true
                });

                jQuery("#subjects").jqGrid({
                    url: '/Admin/RetrieveSubjects/',
                    datatype: 'json',
                    width: 400,
                    mtype: 'GET',
                    colNames: ['Bezeichnung', 'Bezeichnung', 'Lehrer'],
                    colModel: [
                                        { name: 'Stufe', index: 'level', width: 50, align: 'center', editable: 'true' },
                                        { name: 'Bezeichnung', index: 'name', width: 50, align: 'center', editable: 'true' },
                                        { name: 'Teacher', index: 'teacher', width: 50, align: 'center', editable: 'true' }
                                        ],
                    rowNum: 10,
                    rowList: [5, 10, 20, 50],
                    sortname: 'Id',
                    sortorder: "desc",
                    viewrecords: true,
                    caption: 'Zweige',

                    viewrecords: true
                });
                
                loadBranches();

                $("html").removeClass("js");
            });

            function loadBranchSubjects() {
                $.ajax({
                    url: "/Admin/RetrieveBranchList/",
                    cache: false,
                    success: function (data) {
                        if (data != null) {
                            for (var i = 0, len = data.length; i < len; ++i) {

                                $('#branchAccordion').append('<h5>' + data[i][0] + '</h5><div id="' + data[i][1] + '" class="inner"></div>');
                            }
                        }
                    }
                });
            }


            function loadBranches() {
                $.ajax({
                    url: "/Admin/RetrieveBranchList/",
                    cache: false,
                    success: function (data) {
                        if (data != null) {
                            for (var i = 0, len = data.length; i < len; ++i) {

                                $('#branchAccordion').append('<h5>' + data[i][0] + '</h5><div id="' + data[i][1] + '" class="inner">Fächer:<br/>Fach hinzufügen: <br/><input type="text"/><button value="Hinzufügen" onclick="addSubj('+data[i][1]+')"></div>');
                            }
                            $("#branchAccordion").accordion({
                                obj: "div",
                                wrapper: "div",
                                el: ".h",
                                head: "h4, h5",
                                next: "div",
                                showMethod: "slideFadeDown",
                                hideMethod: "slideFadeUp",
                                initShow: "div.shown"
                            });
                        }
                    }
                });

            }

            function addSubject(subjID) {
                alert(subjID);
            }


            function addData(){
                var level  = jQuery("#levelInp").val();
                var label = jQuery("#labelInp").val();
                var teacher = jQuery("#teacherInp").val();
                var branch = jQuery("#branchInp").val();

                $.ajax({
                url: "/Admin/AddEditClass?level="+level+"&label="+label+"&teacher="+teacher+"&branch="+branch,
                cache: false,
                    success: function (data) {
                      if(data != null){
                        alert(data);
                      }
                      

//                    classData = data;

//                    $('#classes').empty();
//                    for (var i = 0, len = data.length; i < len; ++i) {
//                        $('#classes').append('<li class="ui-widget-content">' + data[i][0] + '</li>');
//                    }

                }
            });
            }
   </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Aufgaben:</h2>
    
    <div id="accordion" style="width:1000px" >
    <h4>Neuen Schülerdaten importieren</h4>

    <div><% using (Html.BeginForm("RecieveStudentConfig", "Admin", FormMethod.Post, new { enctype = "multipart/form-data" }))
{%><br />
    <input class="file" type="file" name="files" id="file1" size="25" />

    <input class="button" type="submit" value="Upload file"/>      
<% } %>   </div>

    <h4><a href="#">Schülerdaten anzeigen</a></h4>

    <div><table id="students" class="list"></table></div>

    <h4><a href="#">Fächer verwalten</a></h4>
    <div><table id="subjects" class="list"></table></div>
    
    <h4><a href="#">Zweige verwalten</a></h4>
    <div id="branchAccordion" class="inner">
    </div>
    
    
    <h4><a href="#">Klassen verwalten</a></h4>
    
    
    <div><table id="classes" ></table>
    
        <table>
        <tr>
        <td>Schulstufe
        </td>
        <td>Bezeichnung
        </td>
        <td>Klassenlehrer
        </td>
        <td>Zweig
        </td>
        
        </tr>
        <tr>
        <td>
        <select id="levelInp">
            <option label="1">1</option>
            <option label="2">2</option>
            <option label="3">3</option>
            <option label="4">4</option>
        </select>
        </td>
        <td>
        <input type="text" width="10" id="labelInp"/>
        </td>
        <td>
        <input type="text" id="teacherInp"/>
        </td>
        <td>
        <input type="text" id="branchInp" />
        </td>
        <td><input class="button" type="button" value="Hinzufügen" onclick="addData()"/>
        </td>
        </tr>
    </table>
    </div>
   </div>    

</asp:Content>
