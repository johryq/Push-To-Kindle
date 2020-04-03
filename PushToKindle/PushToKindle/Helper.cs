using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushToKindle
{
    class Helper
    {
        public bool CmdHelper()
        {
            string kindleGenPath = Path.Combine(BookInfo.FilePath, $"../../KindleGen/KindleGen.exe");
            string infoPath = Path.Combine(BookInfo.FilePath, $"../../Kindle/{BookInfo.BookName}.opf");
            string cmdStr = $"\"{kindleGenPath}\"" + $" \"{infoPath}\"";
            try
            {
                Process p = new Process();
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.UseShellExecute = false; p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                p.Start();
                
                p.StandardInput.WriteLine(cmdStr + "&exit");
                p.StandardInput.AutoFlush = true;
                string strOuput = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
                p.Close();
                Console.WriteLine(strOuput);
                Console.ReadKey();
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e+"\nKindleGen 错误");
                Console.ReadKey();
                return false;
            }
        }
    }
}
