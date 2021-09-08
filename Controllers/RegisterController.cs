using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using RegisterAndLogin.Models;

namespace RegisterAndLogin.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register

        [HttpGet]
        public ActionResult Registration()
        {

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Registration([Bind(Exclude = "IsEmailVerified,ActivationCode")] User user)
        {

            bool Status = false;
            string message = "";

            //Model validation
            if (ModelState.IsValid)
            {

                #region //Email is already exist
                var isExist = IsEmailExist(user.EmailID);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already exist");
                    return View(user);
                }
                #endregion

                #region //Generate Activation Code
                user.ActivationCode = Guid.NewGuid();

                #endregion

                #region // Password hashing
                user.Password = Crypto.Hash(user.Password);
                user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword); //
                #endregion

                user.IsEmailVerified = false;

                #region //Save data to database
                using (UserDatabaseEntities2 dc = new UserDatabaseEntities2())
                {
                    dc.Users.Add(user);

                    dc.SaveChanges();

                    //Send Email to the user
                    SendVerificationLinkEmail(user.EmailID, user.ActivationCode.ToString());
                    message = "Registration successfully done, Account activation link " +
                        "has been sent to your email id:" + user.EmailID;

                    Status = true;
                }

                #endregion
            }
            else
            {

                message = "Invalid Request";
            }

            ViewBag.Message = message;
            ViewBag.Status = Status;
            return View(user);

        }


        //Verify Account
        [HttpGet]
        public ActionResult VerifyAccount(string Id)
        {
            bool Status = false;
            using (UserDatabaseEntities2 dc = new UserDatabaseEntities2())
            {
                dc.Configuration.ValidateOnSaveEnabled = false; //This line i added here is to avoid confirm password
                                                                   // does not axist match issue on save change.

                var v = dc.Users.Where(a => a.ActivationCode == new Guid(Id)).FirstOrDefault();

                if (v != null)
                {
                    v.IsEmailVerified = true;
                    dc.SaveChanges();
                    Status = true;
                }
                else
                {
                    ViewBag.Nessage = "Invalid account";

                }
            }

                ViewBag.Status = Status;

                return View();
            





        }

        //Login
        [HttpGet]
        public ActionResult Login()
        {

            return View();
        }


        //Login POST
        [HttpPatch]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login, string ReturnUrl = "")
        {

            string message = "";

            using (UserDatabaseEntities2 dc = new UserDatabaseEntities2())
            {
                var v = dc.Users.Where(a => a.EmailID == login.EmailID).FirstOrDefault();

                if (v != null)
                {
                    if (string.Compare(Crypto.Hash(login.Password), v.Password) == 0)
                    {
                        int timeout = login.RememberMe ? 525600 : 20; //525600 = 1year
                        var ticket = new FormsAuthenticationTicket(login.EmailID, login.RememberMe, timeout);
                        string encrytion = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrytion);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);

                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                    }
                    else
                    {
                        return RedirectToAction("Register", "Login");
                      
                    }
                }
                else
                {
                    message = "Invalid credentials provided, please try again";


                }
            }

            ViewBag.Message = message;
            return View();

        }

        //Logout
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }

        [NonAction]
        public bool IsEmailExist(string emailID)
        {
            using (UserDatabaseEntities2 dc = new UserDatabaseEntities2())
            {
                var v = dc.Users.Where(a => a.EmailID == emailID).FirstOrDefault();
                return v != null;
            }
        }



        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode)
        {
            var verifyUrl = "Register/VerifyAccount" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("chumanxazonke1569@gmail.com", "Chuma Nxazonke");
            var toMail = new MailAddress(emailID);
            var fromEmailPassword = "Chustar_196@"; //Replace the password with actual password
            string subject = "Your account is successfully created";


            string body = "<br/><br/>We are excited to tell you that your account is" +
                " successfully created. Please click on the below link to verify your account" +
                " <br/><br/><a href='" + link + "'>" + link + "</a> ";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)

            };

            using (var message = new MailMessage(fromEmail, toMail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            })
                smtp.Send(message);

        }




    } 
            
    
}