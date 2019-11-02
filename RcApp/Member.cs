using System;
using System.Collections.Generic;
using System.Text;

namespace RcApp
{
    public class Member
    {
        private IMembership _ins;

        public IMembership Instance
        {
            [System.Diagnostics.DebuggerStepThrough]
            get
            {
                if (_ins == null)
                {
                    _ins = ReCompack.ProviderLoader.Get<IMembership>("config.xml", "providers/sqlMembership");
                }
                return _ins;
            }
        }

        public bool VerifyUser(string Username, string Password)
        {

            return Instance.VerifyUser(Username, Password);
        }
    }
}
