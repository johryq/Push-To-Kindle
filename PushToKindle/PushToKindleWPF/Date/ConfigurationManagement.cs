using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushToKindleWPF.Date
{
    class ConfigurationManagement
    {

        Configuration configuration1 = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        public bool HaveKey()
        {

            foreach (string key in configuration1.AppSettings.Settings.AllKeys)
            {
                if (key == "sendEmail")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="emailObj"></param>
        public void AppendKey(Object.EmailObj emailObj)
        {
            configuration1.AppSettings.Settings.Add("sendEmail", emailObj.SendEmail);
            configuration1.AppSettings.Settings.Add("pwd", emailObj.PassWorld);
            configuration1.AppSettings.Settings.Add("receiveEmail", emailObj.ReceiveEmail);
            configuration1.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");//重新加载新的配置文件   
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="emailObj"></param>
        public void UpdataValue(Object.EmailObj emailObj)
        {
            configuration1.AppSettings.Settings["sendEmail"].Value = emailObj.SendEmail;
            configuration1.AppSettings.Settings["pwd"].Value = emailObj.PassWorld;
            configuration1.AppSettings.Settings["receiveEmail"].Value = emailObj.ReceiveEmail;
            configuration1.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");//重新加载新的配置文件  
        }

        public Object.EmailObj GetVal()
        {
            Object.EmailObj emailObj = new Object.EmailObj();
            emailObj.SendEmail = ConfigurationManager.AppSettings["sendEmail"];
            emailObj.PassWorld = ConfigurationManager.AppSettings["pwd"];
            emailObj.ReceiveEmail = ConfigurationManager.AppSettings["receiveEmail"];
            return emailObj;
        }

        public void DelInfo()
        {
            configuration1.AppSettings.Settings["sendEmail"].Value = "";
            configuration1.AppSettings.Settings["pwd"].Value = "";
            configuration1.AppSettings.Settings["receiveEmail"].Value = "";
            configuration1.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");//重新加载新的配置文件  
        }
    }
}
