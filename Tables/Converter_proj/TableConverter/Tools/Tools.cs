using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableConverter
{
    public class Tools
    {
        /// <summary>
        /// 读取器
        /// </summary>
        private static IExcelDataReader _reader = null;

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

        /// <summary>
        /// 读Excel
        /// </summary>
        public static bool LoadExcel (string tablePath)
        {
            if (!tablePath.EndsWith( ".xlsx" ))//非表文件跳过
            {
                Console.WriteLine(string.Format("非表文件！{0}",tablePath));
                return false;
            }

            using (var stream = File.Open( tablePath, FileMode.Open, FileAccess.Read ))
            {
                StringBuilder builder = new StringBuilder();
                var reader = ExcelReaderFactory.CreateReader( stream );
                while (reader.Read())
                {

                }
            }

            return true;
        }

        /// <summary>
        /// 将单表数据生成csv并进行一次flush至config目录下
        /// </summary>
        private static void FlushData ()
        {
            
        }



        /// <summary>
        ///  获取config目录路径
        /// </summary>
        public static string GetConfigPath ()
        {
            return string.Empty;
        }
    }
}
