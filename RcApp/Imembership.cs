using System;
using System.Collections.Generic;
using System.Text;

namespace RcApp
{
    public interface IMembership
    {
        bool VerifyUser(string username, string password);
    }
}
