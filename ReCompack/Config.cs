using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace ReCompack
{
    public class Config
    {
        private static readonly object objLockRootDir=new object();
        private static string _rootDir;
        
        public static string RootDir
        {
            [System.Diagnostics.DebuggerStepThrough]
            get
            {
                if (_rootDir == null)
                {
                    lock (objLockRootDir)
                    {
                        if (_rootDir == null)
                        {
                            _rootDir = AppDomain.CurrentDomain.BaseDirectory;
                            var index = 0;
                            if ((index=_rootDir.IndexOf(Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar)) > -1)
                            {
                                _rootDir = _rootDir.Substring(0,index);
                            }
                        }
                    }
                }
                return _rootDir;
            }
        }

        public static XDocument LoadXml(string xmlFilePath)
        {
            var xml = XDocument.Load(GetFullPath(xmlFilePath));
            return xml;
        }

        public static string GetFullPath(string strPath)
        {
            return string.Join(Path.DirectorySeparatorChar, Config.RootDir, strPath);
        }
    }
}
