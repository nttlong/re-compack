using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Linq;
using System.Reflection;

namespace ReCompack
{
    public class ProviderLoader
    {
        static Hashtable CongfigCache = new Hashtable();
        private static object objCongfigCache=new object();

        public static T Get<T>(string XmlConfigPath, string ProviderTag)
        {
            try
            {
                if (CongfigCache[XmlConfigPath] == null)
                {
                    lock (objCongfigCache)
                    {
                        if (CongfigCache[XmlConfigPath] == null)
                        {
                            string[] items = ProviderTag.Split('/');
                            XDocument xml = Config.LoadXml(XmlConfigPath);
                            XElement ele = xml.Root;
                            foreach (var item in items)
                            {
                                ele = ele.Elements().FirstOrDefault(p => p.Name.LocalName.ToLower() == item.ToLower());
                                if (ele == null)
                                {
                                    throw new Exception($"{ProviderTag} was not found in {Config.GetFullPath(XmlConfigPath)}");
                                }

                            }

                            if (ele.Attribute("value") == null)
                            {
                                throw new Exception($"the value of '{ProviderTag}' was not found in {Config.GetFullPath(XmlConfigPath)}");
                            }
                            string value = ele.Attribute("value").Value.Trim(' ').TrimStart(' ').TrimEnd(' ');
                            if (value.IndexOf(",") == -1)
                            {
                                throw new Exception($"'{value}' at '{ProviderTag}' tag in '{Config.GetFullPath(XmlConfigPath)}' is incorrect format\r\n" +
                                    $"The correct format must be <Class name>,<assembly name>\r\n" +
                                    $"Example:\r\n" +
                                    $"<{ele.Name.LocalName} value=\"MyProvider,MyAssembly\">");
                            }
                            var asmName = value.Split(',')[1];
                            var typeName = value.Split(',')[0];
                            var myAssembly = Assembly.Load(asmName);
                            var myType = myAssembly.ExportedTypes.FirstOrDefault(p => p.Name.ToLower() == typeName.ToLower());
                            if (myType == null)
                            {
                                throw new Exception($"{typeName} can not found in assembly {asmName} when load '{ProviderTag}' in {Config.GetFullPath(XmlConfigPath)} ");
                            }
                            var myInstance = Activator.CreateInstance(myType);
                            if (myInstance == null)
                            {
                                throw new Exception($"{typeName} can not found in assembly {asmName} when load '{ProviderTag}' in {Config.GetFullPath(XmlConfigPath)} ");
                            }
                            if (!(myInstance is T))
                            {
                                throw new Exception($"{typeName} in assembly {asmName} is not combipality with {typeof(T).FullName} when load '{ProviderTag}' in {Config.GetFullPath(XmlConfigPath)} ");
                            }
                            CongfigCache[XmlConfigPath] = myInstance;

                        }
                    }
                }
                return (T)CongfigCache[XmlConfigPath];
            }
            catch (FileLoadException ex)
            {

                throw new Exception($"load {ex.FileName} error:{ex.Message}");
            }
            catch(FileNotFoundException ex)
            {
                throw new Exception($"load {ex.FileName} was not found error:{ex.Message}");
            }
        }
    }
}
