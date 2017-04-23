using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Net.Mail;

namespace IPL.Constant.Email
{
    public class MailSettings
    {
        private static MailSettings mailSettings;
        public static MailSettings Instance
        {

            get
            {
                if (mailSettings == null)
                    mailSettings = new MailSettings();
                return mailSettings;
            }

        }


        static Configuration config = null;
        static MailSettingsSectionGroup settings = null;
        private MailSettings()
        {
            config = WebConfigurationManager.OpenWebConfiguration("/");// (HttpContext.Current.Request.ApplicationPath);
            settings = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");
            _host = settings.Smtp.Network.Host;
            _Port = settings.Smtp.Network.Port;
            _Username = settings.Smtp.Network.UserName;
            _Password = settings.Smtp.Network.Password;
            _EnableSsl = settings.Smtp.Network.EnableSsl;
        }

        private string _host;

        public string Host
        {
            get { return _host; }
            set { _host = value; }
        }

        private int _Port;

        public int Port
        {
            get { return _Port; }
            set { _Port = value; }
        }

        private string _Username;

        public string Username
        {
            get { return _Username; }
            set { _Username = value; }
        }

        private string _Password;

        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }

        //private MailMessage _message;

        //public MailMessage Message
        //{
        //    get { return _message; }
        //    set { _message = value; }
        //}

        private List<MailMessage> _messageList = new List<MailMessage>();

        public List<MailMessage> MessageList
        {
            get { return _messageList; }
            set { _messageList = value; }
        }

        private bool _EnableSsl;

        public bool EnableSsl
        {
            get { return _EnableSsl; }
            set { _EnableSsl = value; }
        }


        public void mailDefaultSettings()
        {

        }
    }
}
