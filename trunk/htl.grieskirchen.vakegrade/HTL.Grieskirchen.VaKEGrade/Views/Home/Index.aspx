<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    VaKEGrade Login
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    
    <form id="form1" runat="server">
    
    
    <div>
    <h2>Melden sie sich bitte mit ihrem Benutzernamen und ihrem Passwort an:</h2>
        <table id="login">
            <tr>
            <td>
            Benutzername:
                </td>
            <td>
            <input type="text" />
            </td>
            </tr>
            <tr>
            <td>
            Passwort:
            </td>
            <td>
            <input type="password"/>
            </td>
            </tr>
        </table>
    </div>
    
    </form>
    
</asp:Content>
