using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Web.Mail;


namespace IPL.Constant.Email
{
    /// <summary>
    /// 
    /// </summary>    
    [Serializable()]
    public class EmailMessage
    {
        private string _to;

        public string To
        {
            get { return _to; }
            set { _to = value; }
        }

        private string _cc;

        public string CC
        {
            get { return _cc; }
            set { _cc = value; }
        }

        private string _bcc;

        public string BCC
        {
            get { return _bcc; }
            set { _bcc = value; }
        }


        //private StringBuilder _HTMLBody;

        //public StringBuilder HTMLBody
        //{
        //    get { return _HTMLBody; }
        //    set { _HTMLBody = value; }
        //}

        private string _from;

        public string From
        {
            get { return _from; }
            set { _from = value; }
        }
        private string _subject;

        public string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }
        private string _body;

        public string Body
        {
            get { return _body; }
            set { _body = value; }
        }

        private List<string> _attachments;

        public List<string> Attachments
        {
            get { return _attachments; }
            set { _attachments = value; }
        }

        //private List<string> _replyToEmail;

        //public List<string> ReplyToEmail
        //{
        //    get { return _replyToEmail; }
        //    set { _replyToEmail = value; }
        //}

        private bool _isBodyHtml = true;

        public bool IsBodyHtml
        {
            get { return _isBodyHtml; }
            set { _isBodyHtml = value; }
        }

        private string _TemplatePath;

        public string TemplatePath
        {
            get { return _TemplatePath; }
            set { _TemplatePath = value; }
        }

        private string _Template;

        public string Template
        {
            get { return _Template; }
            set { _Template = value; }
        }

        private ListDictionary _markerList = new ListDictionary();

        public ListDictionary MarkerList
        {
            get { return _markerList; }
            set
            {
                _markerList = value;
            }
        }

        private string _TemplateLogo;

        public string TemplateLogo
        {
            get { return _TemplateLogo; }
            set { _TemplateLogo = value; }
        }      
    }

    
}