<%@Page Title="VakeGrade Login" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    VaKEGrade Login
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    
    
    <%if(ViewData["error"]!=null){%>
        
          <p><%: ViewData["error"].ToString()%></p>
        <%} %>
    
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
                <input class="textbox" type="text" name="tbUsername" />
                </td>
                </tr>
                <tr>
                <td>
                Passwort:
                </td>
                <td>
                <input class="textbox" type="password" name="tbPassword"/>
                
                </td>
                </tr>
                <tr>
                    <td colspan="2" width="100%">
                        <input class="button" type="submit" value="Login" />
                    </td>
                </tr>
            </table>
         <% } %>  
     </div>
    
    
</asp:Content>
