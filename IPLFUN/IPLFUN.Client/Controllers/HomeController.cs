using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using Data.IPLFUN;
using IPL.Constant.Email;
using IPL.Entity;

namespace IPLFUN.Client.Controllers
{
    public class HomeController : Controller
    {
        IPLSchedule iplSchedule = new IPLSchedule();
        DBUser userDB = new DBUser();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult History()
        {
            if (getUser() != null)
            {
                bool isBidActive = false;
                IPLUser user = getUser();
                ViewBag.user = user;
                List<BidHistory> history = new List<BidHistory>();
                history = iplSchedule.getBidderHistory(user.userId,out isBidActive,0);
                return View(history);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        public ActionResult Bid()
        {
            if (getUser() != null)
            {
                IPLUser user = getUser();
                ViewBag.user = user;
                List<BidQuestion> bidQuestions = new List<BidQuestion>();
                bidQuestions = iplSchedule.getBidQuestionForBidder(user.userId);
                return View(bidQuestions);
            }
            else
                return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            IPLUser user = userDB.authenticateUser(model.Username, model.Password);
            if (user != null)
            {
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                  1,                                     // ticket version
                  model.Username,                              // authenticated username
                  DateTime.Now,                          // issueDate
                  DateTime.Now.AddMonths(2),           // expiryDate
                  true,                          // true to persist across browser sessions
                  user.roleId.ToString() + ":" + user.userId + ":" + user.Name,                              // can be used to store additional user data
                  FormsAuthentication.FormsCookiePath);  // the path for the cookie

                // Encrypt the ticket using the machine key
                string encryptedTicket = FormsAuthentication.Encrypt(ticket);

                // Add the cookie to the request to save it
                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                cookie.HttpOnly = true;
                Response.Cookies.Add(cookie);
                if (user.roleId == 3)
                    return Json(new { data = new Result { Status = ResultStatus.Success, Message = "Successfully authenticated." } }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { data = new Result { Status = ResultStatus.Error, Message = "Username or password is incorrect." } }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { data = new Result { Status = ResultStatus.Error, Message = "Username or password is incorrect." } }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SubmitBidResultByBidder(bool val, int masterId)
        {
            BidResult success = iplSchedule.submitBidResultByBidder(val, masterId, getUser().userId);
            if (success.IsSucceed == 1)
                return Json(new { data = new Result { Status = ResultStatus.Success, Message = "Thank you for submitting your bid." }, favour = success.InFavour, against = success.Against, history = success.BidHistory }, JsonRequestBehavior.AllowGet);
            else if (success.IsSucceed == -2)
                return Json(new { data = new Result { Status = ResultStatus.Warning, Message = "Sorry bid is not available." } }, JsonRequestBehavior.AllowGet);
            else if (success.IsSucceed == -3)
                return Json(new { data = new Result { Status = ResultStatus.Error, Message = "Sorry insufficient balance to bid." } }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { data = new Result { Status = ResultStatus.Error, Message = "Error occurred while submitting pole." } }, JsonRequestBehavior.AllowGet);
        }

        private IPLUser getUser()
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            string username = string.Empty;
            IPLUser user = null;
            if (authCookie != null)
            {
                user = new IPLUser();
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                string name = ticket.Name;
                string userData = ticket.UserData;
                user.userId = Convert.ToInt32(userData.Split(':')[1]);
                user.Name = Convert.ToString(userData.Split(':')[2]);
            }
            return user;
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();

            // clear authentication cookie
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);

            // clear session cookie (not necessary for your current problem but i would recommend you do it anyway)
            SessionStateSection sessionStateSection = (SessionStateSection)WebConfigurationManager.GetSection("system.web/sessionState");
            HttpCookie cookie2 = new HttpCookie(sessionStateSection.CookieName, "");
            cookie2.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie2);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult LoadBidHistory(int MasterId)
        {
            if (getUser() != null)
            {
                bool isBidActive = false;
                IPLUser user = getUser();
                ViewBag.user = user;
                List<BidHistory> history = new List<BidHistory>();
                history = iplSchedule.getBidderHistory(user.userId, out isBidActive, MasterId);
                ViewBag.isBidActive = isBidActive;
                return PartialView("_BidHistoryPartial", history);
            }
            else
                return RedirectToAction("Index", "Home");
        }
        public JsonResult getUserPoints()
        {
            IPLUser currentUser = getUser();
            if (currentUser != null)
            {
                DBUser dbuser = new DBUser();
                var points = dbuser.getUserBalance(currentUser.userId);
                return Json(new { data = new Result { Status = ResultStatus.Success, Message = "Points retrived successfully." }, points = points }, JsonRequestBehavior.AllowGet);                
            }
            else {
                return Json(new { data = new Result { Status = ResultStatus.Error, Message = "User object is null"} }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> ForgotPassword(string Username)
        {
            IPLBidder user = userDB.getUserByUserName(Username);
            if (user == null)
                return Json(new { data = new Result { Status = ResultStatus.Error, Message = "User not found" } }, JsonRequestBehavior.AllowGet);
            else
            {
                Task.Run(async () => SendEmail(user));
                return Json(new { data = new Result { Status = ResultStatus.Success, Message = "Password is sent on you email id, please check you inbox." }}, JsonRequestBehavior.AllowGet);
            }
        }

        public void SendEmail(IPLBidder iplBidder)
        {
            MailHelper mailHelper = new MailHelper();
            EmailMessage mailMessage = new EmailMessage();
            mailMessage.From = Constants.FromAddress;
            mailMessage.TemplatePath = Constants.TemplatePath;
            mailMessage.TemplateLogo = Constants.TemplateLogo;
            MailHelper helper = new MailHelper();
            mailMessage.Subject = "Forgot password on cricmoolah.com";
            mailMessage.To = iplBidder.email;
            mailMessage.Body = "";
            mailMessage.MarkerList.Add("{$loginId}", iplBidder.userName);
            mailMessage.MarkerList.Add("{$password}", iplBidder.password);
            mailMessage.MarkerList.Add("{$firstname}", iplBidder.firstName + " " + iplBidder.lastName);
            mailMessage.IsBodyHtml = true;
            mailMessage.Template = "ForgotPassword";
            string message = helper.DraftMail(mailMessage);
        }

    }
}