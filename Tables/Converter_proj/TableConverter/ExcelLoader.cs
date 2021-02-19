using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableConverter
{
    class ExcelLoader
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
                Console.WriteLine( string.Format( "非表文件！{0}", tablePath ) );
                return false;
            }

            //检查config目录是否存在，不存在创建，存在覆盖
            if (!File.Exists( Define.ConfigPath ))
            {
                File.Create( Define.ConfigPath );
                //Console.WriteLine( string.Format( "未检查到路径 {0}" ) );
                //return false;
            }

            
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
                        switch (currRow)
                        {
                            //#TODO处理字段名重复、值和对应类型不匹配的问题
                            case 0://第一行，字段名
                                {
                                    var fieldName = rowCursor[currCol] as string;
                                    if (string.IsNullOrEmpty( fieldName ))
                                        return false;

                                    //强制要求首行必须是ID
                                    if (currCol == 0 && !string.Equals( "ID", fieldName ))
                                    {
                                        Console.WriteLine( $"表{tablePath}，首行必须是ID" );
                                        return false;
                                    }

                                    builder.Append( fieldName );
                                    AppendComma( builder, currCol, colCount );
                                    break;
                                }
                            case 1://第二行类型
                                {
                                    var typ = GetFieldType( rowCursor[currCol] );
                                    if (string.IsNullOrEmpty( typ ))
                                    {
                                        Console.WriteLine( $"表{tablePath}，行{currRow},列{currCol}类型错误" );
                                        return false;
                                    }

                                    builder.Append( typ );
                                    AppendComma( builder, currCol, colCount );
                                }
                                break;
                            case 3://第三行注释，跳过
                                continue;
                                //break;
                            default://默认生成行
                                {
                                    var value = rowCursor[currCol] as string;
                                    if (string.IsNullOrEmpty( value ))
                                    {
                                        Console.WriteLine($"读取表{tablePath}数据错误，行：{currRow},列:{currCol}");
                                        return false;
                                    }
                                    builder.Append( value );
                                }
                                break;
                        }
                        builder.Append( "\n" );
                    }
                }//end for
                //File.WriteAllText( Define.ConfigPath + @"\" + _reader.Name, builder.ToString(), Encoding.UTF8 );
                var bytes = Encoding.UTF8.GetBytes( builder.ToString() );
                bf.Write( bytes, 0, bytes.Length );
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
            if (string.IsNullOrEmpty( type ))
                return null;

            switch (type)
            {
                case "INT":
                    return "INT";
                case "STRING":
                    return "STRING";
                case "FLOAT":
                    return "FLOAT";
                case "INT[]":
                    return "INT[]";
                case "STRING[]":
                    return "STRING[]";
                case "FLOAT[]":
                    return "FLOAT[]";
                default:
                    return null;
            }
        }

        /// <summary>
        /// tsv文件逗号连接
        /// </summary>
        private static void AppendComma (StringBuilder builder, int pos, int len)
        {
            if (pos != len - 1)//行末尾
                builder.Append( "," );
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
            else//如果已存在，就清掉原来的文本内容
                File.WriteAllText( configTablePath, string.Empty, Encoding.UTF8 );

            stream = new FileStream( configTablePath, FileMode.Truncate, FileAccess.ReadWrite );
            BufferedStream bs = new BufferedStream( stream );
            return bs;
        }
    }
}
