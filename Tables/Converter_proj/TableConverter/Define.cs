using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableConverter
{
    public class Define
    {

        /// <summary>
        /// UnityConfig目录
        /// </summary>
        private static string _configPath = string.Empty;

        /// <summary>
        /// UnityConfig目录
        /// </summary>
        public static string ConfigPath
        {
            get
            {
                if (string.IsNullOrEmpty( ConfigPath ))
                {
                    //todo 这里写死
                    var env = System.Environment.CurrentDirectory;
                    var rootPath = env.Substring( env.Length - env.IndexOf( "Tables" ) );
                    using (var fs = new FileStream( "" ,FileMode.Open,FileAccess.Read))
                    {
                        
                    }

                }

                return _configPath;
            }
        }
    }
}
