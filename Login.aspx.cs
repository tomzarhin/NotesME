using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        System.Diagnostics.Debug.WriteLine("LOGIN");
    }
    protected void login_Button(object sender, EventArgs e)
    {
        bool loggedIn = false;
        System.Diagnostics.Debug.WriteLine("LOGIN BUTTON CLICKED");

        // Parsing Data
        string email = Request.Form["InputEmail1"];
        string password = Request.Form["InputPassword1"];
        System.Diagnostics.Debug.WriteLine("Email: " + email);
        System.Diagnostics.Debug.WriteLine("Password: " + password);

        // Logged In check
        if (loggedIn == true)
        {
            Response.Redirect("/index.aspx");
        }
        else
        {
        }
    }
}