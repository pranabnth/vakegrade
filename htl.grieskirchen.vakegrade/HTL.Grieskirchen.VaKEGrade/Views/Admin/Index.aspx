<%@ Page Title="Admin Panel" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Admin Panel
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    
    
        <script type="text/javascript" src="../../Scripts/jquery.nestedAccordion.js"></script>

        <script type="text/javascript">


            $(document).ready(function () {

                $("html").addClass("js");
                $.fn.accordion.defaults.container = false;

                //                $(".accordion").accordion({
                //                    obj: "div",
                //                    wrapper: "div",
                //                    el: ".h",
                //                    head: "h4, h5",
                //                    next: "div",
                //                    showMethod: "slideFadeDown",
                //                    hideMethod: "slideFadeUp",
                //                    initShow: "div.shown"
                //                });
                $("#options").accordion({obj: "div", wrapper: "div", el:".h", next: 'div.outer', head:'h4, h5' });




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
                    width: 600,
                    mtype: 'GET',
                    colNames: ['Bezeichnung', 'Verpflichtend', 'Klassen'],
                    colModel: [
                                        { name: 'Bezeichnung', index: 'label', width: 50, align: 'center', editable: 'true' },
                                        { name: 'Verpflichtend', index: 'obligatory', width: 50, align: 'center', editable: 'true' },
                                        { name: 'Klassen', index: 'classes', width: 50, align: 'center', editable: 'true' }
                                        ],
                    rowNum: 200,
                    rowList: [5, 10, 20, 50],
                    sortname: 'Id',
                    sortorder: "desc",
                    viewrecords: true,
                    caption: 'Fächer',

                    viewrecords: true
                });

                loadBranches();
                loadInfo();

                $("html").removeClass("js");
            });

            function loadBranchSubjects(branchID) {
                $.ajax({
                    url: "/Admin/RetrieveBranchSubjects/?branchID=" + branchID,
                    cache: false,
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    success: function (data) {
                        if (data != null) {
                            var curID;
                            for (var i = 0, len = data.length; i < len; ++i) {
                                curID = '#sub' + branchID;
                                jQuery(curID).append(data[i] + '<input type="button" onclick="deleteSubjectRef()" value="Löschen"/><br/>');
//                                var data[i].split(';');
                            }
                        }
                    }
                });
            }

            function loadInfo() {
                $.ajax({
                    url: "/Admin/RetrieveGeneralInfo/",
                    cache: false,
                    type: 'POST',
                    success: function (data) {
                        jQuery('#headmaster').append(data);
                    }
                });
            }


            function loadBranches() {
                $.ajax({
                    url: "/Admin/RetrieveBranchList/",
                    type: "POST",
                    cache: false,
                    success: function (data) {
                        if (data != null) {
                            for (var i = 0, len = data.length; i < len; ++i) {

                                $('#branchAccordion').append('<h5>' + data[i][0] + '</h5><div id="' + data[i][1] + '" ><br/><br/><div id="sub' + data[i][1] + '"/><br/><br/> <br/>Bezeichnung: <input class="textbox" type="text" id="newSubName' + data[i][1] + '"/> Klasse: <input type="text" class="textbox" id="level'+data[i][1]+'"><input type="button" class="button" value="Hinzufügen" onclick="addSubj('+ data[i][1] +')"/></div>');
                                loadBranchSubjects(data[i][1]);
                            }

                            $("#options").accordion({ obj: "div", wrapper: "div", el: ".h", next: 'div.outer', head: 'h4, h5' });

//                            $("#branchAccordion").accordion({
//                                obj: "div",
//                                wrapper: "div",
//                                el: ".h",
//                                head: "h4, h5",
//                                next: "div",
//                                showMethod: "slideFadeDown",
//                                hideMethod: "slideFadeUp",
//                                initShow: "div.shown"
//                            });
                        }
                    }
                });

            }

            function addSubj(branchID) {
                $.ajax({
                    url: "/Admin/NewSubjectAssignment/?branchID=" + branchID + "&name=" + jQuery('#newSubName' + branchID).val() + "&level=" + jQuery('#level' + branchID).val(),
                    cache: false,
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    success: function (data) {
                        if (data != null) {

                            jQuery("#sub" + branchID).append(data + "<br/>");

                        } else {
                            alert("Das von ihnen eingegebene Fach konnte nicht in die Datenbank gespeichert werden, überprüfen sie bitte in ihrer Eingabe ob dieses Fach existiert.");
                        }
                    }
                });
            }

            function addNewSubject() {


                var name = jQuery("#newSubjectGeneralName").val();

                var voluntairy = jQuery("#newSubjectVoluntairy").val();
                var binding;
                if (jQuery("#newSubjectBinding").val() == "Ja") {
                    binding = false;
                } else {
                    binding = true;
                }


                var voluntairy;
                if (jQuery("#newSubjectVoluntairy").val() == "Ja") {
                    voluntairy = false;
                } else {
                    voluntairy = true;
                }


                $.ajax({
                    url: "/Admin/AddSubject?subName=" + name + "&voluntairy=" + voluntairy + "&binding=" + binding,
                    cache: false,
                    type: "POST",
                    success: function (data) {
                        if (data != "succ") {
                            alert(data);
                        } else {
                            jQuery("#subjects").trigger("reloadGrid");

                        }
                    }
                });
            }


            function deleteSubject() {


                var name = jQuery("#delSubjectName").val();


                $.ajax({
                    url: "/Admin/EditSubject?subName=" + name ,
                    cache: false,
                    type: "POST",
                    success: function (data) {
                        if (data != "succ") {
                            alert(data);
                        } else {
                            jQuery("#subjects").trigger("reloadGrid");

                        }


                        //                    classData = data;

                        //                    $('#classes').empty();
                        //                    for (var i = 0, len = data.length; i < len; ++i) {
                        //                        $('#classes').append('<li class="ui-widget-content">' + data[i][0] + '</li>');
                        //                    }

                    }
                });
            }

            function addData(){

                var level = jQuery("#newSubjName").val();
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
    
    <div id="options" class="accordion" style="width:1000px" >
    <h4>
    Schülerdaten einlesen
    </h4>
    <div ><% using (Html.BeginForm("RecieveStudentConfig", "Admin", FormMethod.Post, new { enctype = "multipart/form-data" }))
{%><br />
    <input class="file" type="file" name="files" id="file1" size="25" />

    <input class="button" type="submit" value="Upload file"/>      
<% } %>   </div>

    

    <h4>Schülerdaten anzeigen</h4>
    <div><table id="students" class="list"></table></div>
    <h4>Fächer verwalten</h4>
    <div class="new"><table id="subjects" class="list"></table>
    <br />
    <br />
    Name:
    <input class="textbox" type="text" id="delSubjectName"/>
    <input type="button" class="button" value="Löschen" onclick="deleteSubject()"/>
    <br />
    <br />
    Name:
    <input class="textbox" type="text" id="newSubjectGeneralName"/>
    Verpflichtend:
    <select id="newSubjectVoluntairy">
            <option label="false">Ja</option>
            <option label="true">Nein</option>
    </select>
    Wird benotet:
    <select id="newSubjectBinding">
            <option label="false">Ja</option>
            <option label="true">Nein</option>
    </select>
    <input type="button" class="button" value="Hinzufügen" onclick="addNewSubject()"/>
    </div>
    <h4>Zweige verwalten</h4>
    <div id="branchAccordion" >
    </div>
    
    
    <h4>Klassen verwalten</h4>
    <div ><table id="classes" ></table>
    <br/>
    <br/>
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
        <input class="textbox" type="text" width="10" id="labelInp"/>
        </td>
        <td>
        <input class="textbox" type="text" id="teacherInp"/>
        </td>
        <td>
        <input class="textbox" type="text" id="branchInp" />
        </td>
        <td><input type="button" class="button" value="Hinzufügen" onclick="addData()"/>
        </td>
        </tr>
    </table>
    </div>
    <h4>Allgemeine Informationen</h4>
    <div>
    Schulleiter: 
    <input type="text" class="textbox" value="" id="headmaster"/>
    <input type="button" class="button" value="Speichern" />
    </div>
   </div>    

</asp:Content>
