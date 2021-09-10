using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Diagnostics; // This class allows me to debug my code.


//Dieuci djate Nsibu 220169136

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btn_Submit_Click(object sender, EventArgs e)
    {

        try
        {
            var from = "forexnewshouse@gmail.com"; // here i enter the sender email 
            var to = "forexnewshouse@gmail.com";
            const string password = "Forex2021";
           

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.Credentials = new System.Net.NetworkCredential(from, password); // here i enter the actual password of the sender email.
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;
            MailMessage mail = new MailMessage();



            //Setting From , 
            mail.From = new MailAddress(from, "forexnewshouse");
            mail.To.Add(new MailAddress(to));


            //here i set message 
            mail.Subject = subject.Text.ToString();
            mail.Body = ("Name :  "+name.Text+ "\n" + "Email :  "+email.Text+ "\n"+ "Phone :  "+phone.Text+ "\n" + "\n" + message.Text);

            smtpClient.Send(mail);
            confirm.Text = "Your message has been sent, Thank you for contacting us";
            name.Text = "";
            email.Text = "";
            phone.Text = "";
            message.Text = "";
        }
        catch (Exception)
        {
            confirm.Text = "Sorry Something went wrong!!";
        }
    }
}
