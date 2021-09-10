<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Contact Form</title>
    <link rel="stylesheet" href="Style.css">
</head>
<body>
    <form id="form" runat="server">
        <div>
            <div class="conatct-form">
                <div class="input">
                    <h1>Contact Us</h1>

                    <h3>
                        <asp:Label ID="confirm" runat="server" Text=""></asp:Label>
                   </h3>
                      


                    <p>Name*</p>

                    <asp:TextBox ID="name" runat="server" placeholder="Enter name" Required></asp:TextBox>



                    <p>Email*</p>
                    <asp:TextBox ID="email" runat="server" placeholder="Enter  email" Required></asp:TextBox>



                    <p>Phone (optional)</p>
                    <asp:TextBox ID="phone" runat="server" placeholder="Enter  phone" ></asp:TextBox>



                     <p>Subject*</p>
                    <asp:TextBox ID="subject" runat="server" placeholder="Enter  your subject" Required></asp:TextBox>




                     <p>Message*</p>
                    <asp:TextBox ID="message" runat="server" placeholder="type  your message..." Required TextMode="MultiLine" Rows="4"></asp:TextBox>

                   


                    <asp:Button ID="Submit" runat="server" Text="submit" OnClick="btn_Submit_Click" />
                

                </div>
            </div>
          

    </form>
</body>
</html>
