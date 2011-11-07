<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    VaKEGrade Login
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    
    
    
    
    <div>
    <h2>Melden sie sich bitte mit ihrem Benutzernamen und ihrem Passwort an:</h2>
        <%using (Html.BeginForm("Login", "Authentification", FormMethod.Post))
          { %>
            <table id="login">
                <tr>
                <td>
                Benutzername:
                    </td>
                <td>
                <input type="text" name="tbUsername" />
                </td>
                </tr>
                <tr>
                <td>
                Passwort:
                </td>
                <td>
                <input type="password" name="tbPassword"/>
                
                </td>
                </tr>
                <tr>
                    <td colspan="2" width="100%">
                        <input type="submit" value="Login" />
                    </td>
                </tr>
            </table>
         <% } %>  
     </div>
    
    
</asp:Content>
