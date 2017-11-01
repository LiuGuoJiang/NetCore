using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBackend.Api.Service
{
    public class LocalMailService: IMailService
    {
        private readonly string _mailTo = Startup.Configuration["mailSettings:mailFromAddress"];
        private readonly string _mailFrom = Startup.Configuration["mailSettings:mailFromAddress"];
        public void Send(string subject,string msg)
        {
            Debug.WriteLine($"从{_mailFrom}给{_mailTo}通过{nameof(LocalMailService)}发送了邮件");
        }
    }
}
