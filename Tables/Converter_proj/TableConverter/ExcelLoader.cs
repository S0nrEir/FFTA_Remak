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
        //private static BufferedStream _bs = null;

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
        public static bool LoadExcel (string tablePath,string fileName)
        {
            Console.WriteLine( $"读取文件:{tablePath}..." );
            if (!tablePath.EndsWith( ".xlsx" ))//非表文件跳过
            {
                Console.WriteLine( string.Format( "非表文件！{0}", tablePath ) );
                return false;
            }

            //检查config目录是否存在，不存在创建，存在覆盖
            if (!Directory.Exists( Define.ConfigPath ))
                Directory.CreateDirectory( Define.ConfigPath );

            using (var stream = File.Open( tablePath, FileMode.Open, FileAccess.Read ))
            {
                _reader = ExcelReaderFactory.CreateReader( stream );
                StringBuilder builder = new StringBuilder();
                //var bf = GetBuffer( _reader.Name );
                //var bf = GetBuffer( fileName.Replace(".xlsx",".csv") );
                var sw = GetStreamWriter( fileName.Replace( ".xlsx", ".csv" ) );
                
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
                                    if (currCol == 0 && !string.Equals( "id", fieldName ))
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
                            case 2://第三行注释，跳过
                                {
                                    var annoatation = rowCursor[currCol] as string;
                                    builder.Append( annoatation );
                                    AppendComma( builder, currCol, colCount );
                                }
                                break;
                            default://默认生成行
                                {
                                    var value = rowCursor[currCol].ToString();
                                    if (string.IsNullOrEmpty( value ))
                                    {
                                        Console.WriteLine($"读取表{tablePath}数据错误，行：{currRow},列:{currCol}");
                                        return false;
                                    }
                                    builder.Append( value );
                                    AppendComma( builder, currCol, colCount );
                                }
                                break;
                        }
                    }
                    builder.Append( "\n" );
                }//end for
                //File.WriteAllText( Define.ConfigPath + @"\" + _reader.Name, builder.ToString(), Encoding.UTF8 );
                //var encoding = new UTF8Encoding( true );
                //var bytes = encoding.( builder.ToString() );
                //var bytes = Encoding.UTF8.GetBytes( builder.ToString() );
                //bf.Write( bytes, 0, bytes.Length );
                //bf.Flush();
                //bf.Close();
                sw.Write( builder.ToString() );
                sw.Flush();
                sw.Close();
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
        /// 获取tsv格式的buffer
        /// </summary>
        private static BufferedStream GetBuffer (string fileName)
        {
            var configTablePath = Define.ConfigPath + @"\" + fileName;
            FileStream stream = null;

            if (!File.Exists( configTablePath ))
            {
                stream = File.Create( configTablePath );
                stream.Close();
            }
            else//如果已存在，就清掉原来的文本内容
                File.WriteAllText( configTablePath, string.Empty, Encoding.UTF8 );

            stream = new FileStream( configTablePath, FileMode.Open, FileAccess.Write );
            BufferedStream bs = new BufferedStream( stream );
            return bs;
        }

        /// <summary>
        /// 获取streamWriter
        /// </summary>
        private static StreamWriter GetStreamWriter (string fileName)
        {
            var configTablePath = Define.ConfigPath + @"\" + fileName;
            FileStream stream = null;

            if (!File.Exists( configTablePath ))
            {
                stream = File.Create( configTablePath);
                stream.Close();
            }
            else//如果已存在，就清掉原来的文本内容
                File.WriteAllText( configTablePath, string.Empty, Encoding.UTF8 );

            stream ??= new FileStream( configTablePath, FileMode.Open, FileAccess.Write );
            //BufferedStream bs = new BufferedStream( stream );
            //excel在读取csv文件时是通过文件头的byte order mark来识别编码的，这导致生成的csv文件如果没有BOM则excel无法识别csv文件的编码，中文可能导致乱码，这里手动处理
            var encoding = new UTF8Encoding( true );
            var sw = new StreamWriter( stream, encoding );
            return sw;
        }

    }
}
