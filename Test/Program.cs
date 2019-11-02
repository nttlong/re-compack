using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //ReCompack.Config.RootDir = "111";
            var mb = ReCompack.ProviderLoader.Get <RcApp.IMembership> ("config.xml", "providers/member");
            var token = mb.VerifyUser("Admin", "admin");
            Console.WriteLine(ReCompack.Config.RootDir);
        }
    }
}
