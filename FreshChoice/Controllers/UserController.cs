using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net;
using System.Web.Security;
using FreshChoice.Models;
using CustomAuthorizationFilter.Infrastructure;
using FreshChoice.Utilities;

namespace FreshChoice.Controllers
{
    public class UserController : Controller
    {
        [CustomAuthorize("User", "Admin")]
        public ActionResult MyOrders()
        {
            int userId = int.Parse(Convert.ToString(Session["UserId"]));
            return View(CartHelper.GetInstance(userId).GetAllOrderItems());
        }
        // GET: User
        public ActionResult Registration()
        {
            return View();
        }
        //[CustomAuthorize("Admin")]
        public ActionResult AdminRegistration()
        {
            return View();
        }
        //[CustomAuthorize("Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminRegistration([Bind(Exclude = "IsEmailVerified,ActivationCode")] User user)
        {
            bool Status = false;
            string message = "";
                #region //Email is already Exist 
                var isExist = IsEmailExist(user.UserEmail);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already exist");
                    return View(user);
                }
                #endregion

                #region Generate Activation Code 
                user.ActivationCode = Guid.NewGuid();
                #endregion

                #region  Password Hashing 
                user.UserName = user.FirstName + " " + user.LastName;
                user.UserContact = user.UserContact;
                user.RoleId = user.RoleId;
                user.UserPassword = Crypto.Hash(user.UserPassword);
                user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword); //
                #endregion
                user.IsEmailVerified = true;

                #region Save to Database
                using (FreshChoiceEntities dc = new FreshChoiceEntities())
                {
                    dc.Users.Add(user);
                    dc.SaveChanges();

                    //Send Email to User
                    SendVerificationLinkEmail(user.UserEmail, user.ActivationCode.ToString());
                    message = "Registration successfully done. Account activation link " +
                        " has been sent to your email id:" + user.UserEmail;
                    Status = true;
                }
                #endregion
            
            ViewBag.Message = message;
            ViewBag.Status = Status;
            return Redirect("/user/adminlogin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude = "IsEmailVerified,ActivationCode")] User user)
        {
            bool Status = false;
            string message = "";
            //
            // Model Validation 
            if (ModelState.IsValid)
            {

                #region //Email is already Exist 
                var isExist = IsEmailExist(user.UserEmail);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already exist");
                    return View(user);
                }
                #endregion

                #region Generate Activation Code 
                user.ActivationCode = Guid.NewGuid();
                #endregion

                #region  Password Hashing 
                user.UserName = user.FirstName + " " + user.LastName;
                user.UserContact = user.UserContact;
                user.RoleId = 1;
                user.UserPassword = Crypto.Hash(user.UserPassword);
                user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword); //
                #endregion
                user.IsEmailVerified = false;

                #region Save to Database
                using (FreshChoiceEntities dc = new FreshChoiceEntities())
                {
                    dc.Users.Add(user);
                    dc.SaveChanges();
                   
                    //Send Email to User
                    SendVerificationLinkEmail(user.UserEmail, user.ActivationCode.ToString());
                    message = "Registration successfully done. Account activation link " +
                        " has been sent to your email id:" + user.UserEmail;
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
            return Redirect("/");
        }
        [NonAction]
        public bool IsEmailExist(string emailID)
        {
            using (FreshChoiceEntities dc = new FreshChoiceEntities())
            {
                var v = dc.Users.Where(a => a.UserEmail == emailID).FirstOrDefault();
                return v != null;
            }
        }

        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode)
        {
            var verifyUrl = "/User/VerifyAccount/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("hmhaseembsc@gmail.com", "Dotnet Awesome");
            var toEmail = new MailAddress(emailID);
          //  var fromEmailPassword = "b20cf7d6ad81e9"; // Replace with actual password
            string subject = "Your account is successfully created!";

            string body = "<br/><br/>We are excited to tell you that your Fresh Choice account is" +
                " successfully created. Please click on the below link to verify your account" +
                " <br/><br/><a href='" + link + "'>" + link + "</a> ";
            var client = new SmtpClient("smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("382b7c699ff95f", "b20cf7d6ad81e9"),
                EnableSsl = true
            };
            

          using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })

            client.Send(message); 


        }

        //Verify Account  

        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            using (FreshChoiceEntities dc = new FreshChoiceEntities())
            {
                dc.Configuration.ValidateOnSaveEnabled = false; // This line I have added here to avoid 
                                                                // Confirm password does not match issue on save changes
                var v = dc.Users.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    v.IsEmailVerified = true;
                    dc.SaveChanges();
                    Status = true;
                }
                else
                {
                    ViewBag.Message = "Invalid Request";
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
        public ActionResult AdminLogin()
        {

            return View();
        }
        //Login POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminLogin(UserLogin login, string ReturnUrl = "/Admin")
        {
            string message = "";
            using (FreshChoiceEntities dc = new FreshChoiceEntities())
            {
                var v = dc.Users.Where(a => a.UserEmail == login.UserEmail).FirstOrDefault();
                if (v != null)
                {
                    if (!v.IsEmailVerified)
                    {
                        ViewBag.Message = "Please verify your email first";
                        return View();
                    }

                    if (string.Compare(Crypto.Hash(login.UserPassword), v.UserPassword) == 0)
                    {
                        int timeout = login.RememberMe ? 525600 : 20; // 525600 min = 1 year
                        var ticket = new FormsAuthenticationTicket(login.UserEmail, login.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);

                        Session["UserId"] = v.UserId;
                        Session["UserName"] = v.UserName;
                        Session["UserRole"] = v.Role.RoleName;

                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                    }
                    else
                    {
                        message = "Invalid credential provided";
                    }
                }
                else
                {
                    message = "Invalid credential provided";
                }
            }
            ViewBag.Message = message;
            return View();
        }

        //Login POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login, string ReturnUrl = "/")
        {
            string message = "";
            using (FreshChoiceEntities dc = new FreshChoiceEntities())
            {
                var v = dc.Users.Where(a => a.UserEmail == login.UserEmail).FirstOrDefault();
                if (v != null)
                {
                    if (!v.IsEmailVerified)
                    {
                        ViewBag.Message = "Please verify your email first";
                        return View();
                    }

                    if (string.Compare(Crypto.Hash(login.UserPassword), v.UserPassword) == 0)
                    {
                        int timeout = login.RememberMe ? 525600 : 20; // 525600 min = 1 year
                        var ticket = new FormsAuthenticationTicket(login.UserEmail, login.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);

                        Session["UserId"] = v.UserId;
                        Session["UserName"] = v.UserName;
                        Session["UserRole"] = v.Role.RoleName;

                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        message = "Invalid credential provided";
                    }
                }
                else
                {
                    message = "Invalid credential provided";
                }
            }
            ViewBag.Message = message;
            return View();
        }

        //Logout
        public ActionResult Logout()
        {
            Session["UserName"] = string.Empty;
            Session["UserId"] = string.Empty;
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }

        public ActionResult Address()
        {
            return View();
        }
        //Login POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Address(Address address)
        {
            int userId = int.Parse(Convert.ToString(Session["UserId"]));
            using (FreshChoiceEntities db = new FreshChoiceEntities())
            {
                Address newAddress = new Address();
                newAddress.UserId = userId;
                newAddress.AddressName = address.AddressName;
                newAddress.AddressDescription = address.AddressDescription;
                newAddress.AddressLatitude = address.AddressLatitude;
                newAddress.AddressLongitude = address.AddressLongitude;
                db.Addresses.Add(newAddress);
                db.SaveChanges();
            }
            return Redirect("/Cart");
        }

    }
}