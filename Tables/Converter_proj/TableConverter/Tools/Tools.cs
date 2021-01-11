using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableConverter
{
    public class Tools
    {
        /// <summary>
        /// 创建默认的路径文件
        /// </summary>
        public static void CreateDefaultPathFile (string path, string dicPath)
        {
            //目录不存在，先创建目录
            if (!Directory.Exists( dicPath ))
                Directory.CreateDirectory( dicPath );

            if (!File.Exists( path ))
            {
                FileStream fs = new FileStream( path, FileMode.OpenOrCreate, FileAccess.ReadWrite );
                StreamWriter sw = new StreamWriter( fs );
                sw.WriteLine( dicPath );
                sw.Close();
                fs.Close();
            }
        }
    }
}
