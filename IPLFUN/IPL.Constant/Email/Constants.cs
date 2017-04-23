using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace IPL.Constant.Email
{
    public static class Constants
    {
        #region Path
        public static string currentDomainPath = HttpRuntime.AppDomainAppPath;
        public static string TemplatePath = HttpRuntime.AppDomainAppPath + @"EmailTemplates\"; /*EmailTemplate: folder with all email templates*/
        public static string TemplateLogo = HttpRuntime.AppDomainAppPath + @"Images\logo.png"; /*for header image if any*/       
        public static string BasePath = "";
        public static string FromAddress = "admin@cricmoolah.com";
        public static string BCCAddress = "";
        #endregion
    }
}
