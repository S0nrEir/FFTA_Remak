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
        /// FFTA_Remak\FFTA\Assets\Res\Config
        /// </summary>
        public static string ConfigPath => GetConfigPath();

        /// <summary>
        /// 获取config路径
        /// </summary>
        private static string GetConfigPath ()
        {
            //#todo这里写死
            var env = System.Environment.CurrentDirectory;
            //FFTA\FFTA_Remake\FFTA_Remak
            var rootPath = env.Substring( 0, Environment.CurrentDirectory.IndexOf( @"\Tables" ) );
            rootPath = string.Format( @"{0}\FFTA\Assets\Res\Config", rootPath );

            return rootPath;
        }

    }
}
