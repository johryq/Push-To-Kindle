using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PushToKindleWPF.Kindle
{
    class EmailHelper
    {
        //定义默认的 邮件服务器、帐户、密码、发邮件地址
        private static readonly string mailSvr = "smtp.qq.com"; //域名也是OK的mail.163.com
        //private static readonly string mailTo = "@qq.com";

        /// <summary>
        /// 调用SMTP发送邮件
        /// </summary>
        /// <param name="ReciveAddrList">收件人列表</param>
        /// <param name="Subject">主题</param>
        /// <param name="Content">正文</param>
        /// <param name="AttachFile">附件，Dictionary<格件名，详细地址></param>
        /// <returns></returns>
        public bool SendMail(string sendEmail, string pwd, List<string> ReciveAddrList, string Subject, string Content, Dictionary<string, string> AttachFile,ref string errorinfo)
        {
            using (SmtpClient smtp = new SmtpClient())
            {
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;

                smtp.Host = mailSvr;
                smtp.Credentials = new NetworkCredential(sendEmail, pwd);//身份认证

                MailMessage mail = new MailMessage();//建立邮件
                mail.SubjectEncoding = Encoding.GetEncoding("utf-8");//主题编码
                mail.BodyEncoding = Encoding.GetEncoding("utf-8");//正文编码
                mail.Priority = MailPriority.Normal;//邮件的优先级为中等
                mail.IsBodyHtml = false;//正文为纯文本，如果需要用HTML则为true
                mail.From = new MailAddress(sendEmail);//发件人


                foreach (string toAddr in ReciveAddrList)//逐一添加收件人
                {
                    mail.To.Add(toAddr);
                }

                mail.Subject = Subject;//主题
                mail.Body = Content;//正文

                if (AttachFile != null)
                {
                    foreach (string key in AttachFile.Keys)
                    {
                        Attachment file = new Attachment(AttachFile[key]);
                        file.Name = key;
                        mail.Attachments.Add(file);
                    }
                }
                try
                {
                    smtp.Send(mail);//正式发邮件
                    mail.Dispose();
                    smtp.Dispose();
                }
                catch (Exception ex)
                {
                    errorinfo = ex.ToString();
                    Console.WriteLine(ex.ToString());
                }
            }


            return true;
        }
    }
}
