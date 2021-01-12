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
        private static string _configPath = string.Empty;

        /// <summary>
        /// FFTA_Remak\FFTA\Assets\Res\Config
        /// </summary>
        public static string ConfigPath
        {
            get
            {
                if (string.IsNullOrEmpty( ConfigPath ))
                {
                    //#todo这里写死
                    var env = System.Environment.CurrentDirectory;
                    //FFTA\FFTA_Remake\FFTA_Remak
                    var rootPath = env.Substring( 0, System.Environment.CurrentDirectory.IndexOf( @"\Tables" ) );
                    rootPath = string.Format( @"{0}\FFTA\Assets\Res\Config",rootPath );
                    _configPath = rootPath;
                }

                return _configPath;
            }
        }
    }
}
