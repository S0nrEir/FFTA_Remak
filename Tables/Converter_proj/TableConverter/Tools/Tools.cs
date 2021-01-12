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
        /// 缓冲流
        /// </summary>
        private static BufferedStream _bs = null;

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

            if (!File.Exists( Define.ConfigPath ))
            {
                Console.WriteLine(string.Format("未检查到路径 {0}"));
                return false;
            }

            //检查config目录是否存在，不存在创建，存在覆盖

            using (var stream = File.Open( tablePath, FileMode.Open, FileAccess.Read ))
            {
                _reader = ExcelReaderFactory.CreateReader( stream );
                StringBuilder builder = new StringBuilder();
                var bf = GetBuffer( _reader.Name );

                var set = _reader.AsDataSet();
                var table = set.Tables[0];//sheet1
                var rowCount = table.Rows.Count;
                var colCount = table.Columns.Count;
                //遍历表
                for (int currRow = 0; currRow < rowCount; currRow++)
                {
                    var rowCursor = table.Rows[currRow];//当前行
                    for (int currCol = 0; currCol < colCount; currCol++)
                    {
                        if (currRow == 0)
                        {
                            var typ = GetFieldType( rowCursor[currCol] );
                            if (string.IsNullOrEmpty( typ ))
                            {
                                Console.WriteLine(string.Format("表{0}，行{1}列{2}类型错误"));
                                return false;
                            }

                            if (currCol != colCount - 1)
                                typ += ",";

                            builder.Append( typ );
                        }
                        else if (currRow == 1)
                        {

                        }
                        else if (currRow == 2)
                        {

                        }
                        else
                        {
                            
                        }

                        #region
                        //switch (currRow)
                        //{
                        //    case 0://第一行字段列
                        //        {
                        //            var typ = GetFieldType( rowCursor[currCol] );
                        //            if (string.IsNullOrEmpty( typ ))
                        //            {

                        //            }
                        //            //builder.Append( GetFieldType(rowCursor[currCol]) );
                        //            break;
                        //        }
                        //    case 1://第二行类型
                        //        break;
                        //    case 3://第三行注释
                        //        break;
                        //    default://默认生成行
                        //        break;
                        //}
                        #endregion
                    }

                    builder.Append( "\n" );
                }
                bf.Flush();
                bf.Close();
                builder.Clear();
                builder = null;
            }

            return true;
        }

        /// <summary>
        /// 获取表字段类型
        /// </summary>
        private static string GetFieldType (object obj)
        {
            var type = obj as string;
        }


        /// <summary>
        /// buffer
        /// </summary>
        private static BufferedStream GetBuffer (string fileName)
        {
            var configTablePath = Define.ConfigPath + @"\" + fileName;
            FileStream stream = null;

            if (!File.Exists( configTablePath ))
                stream = File.Create( configTablePath );
            else
            {
                File.WriteAllText( configTablePath, string.Empty, Encoding.UTF8 );
                stream = new FileStream( configTablePath, FileMode.Truncate, FileAccess.ReadWrite );
            }

            BufferedStream bs = new BufferedStream( stream );
            return bs;
        }
    }
}
