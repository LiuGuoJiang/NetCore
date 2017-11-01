using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBackend.Api.Service
{
    public interface IMailService
    {
        void Send(string subject, string msg);
    }
}
