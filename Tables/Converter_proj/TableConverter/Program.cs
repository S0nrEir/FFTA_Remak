using System;
using System.Collections.Generic;
using System.IO;

namespace TableConverter
{
    class Program
    {
        static void Main (string[] args)
        {
            var local = Environment.CurrentDirectory;
            var rootPath = local.Substring( 0, local.IndexOf( @"\Tables" ) );
            var excelPath = $@"{rootPath}\Tables\Public";
            var tableDirectory = excelPath;

            DirectoryInfo dirInfo = new DirectoryInfo( tableDirectory );
            var staticTableLst = new List<(string path,string name)>();
            var loadNormalSucc = true;
            //load normal table
            foreach (var file in dirInfo.GetFiles())
            {
                var fileName = file.Name;
                var tablePath = tableDirectory + @"\" + fileName;
                if (IsStaticTable( fileName ))
                {
                    staticTableLst.Add( (path : tablePath, name : fileName) );
                    continue;
                }

                if (!ExcelLoader.LoadExcel( tablePath, fileName ))
                {
                    loadNormalSucc = false;
                    break;
                }
            }

            //#todo load static table
            if (loadNormalSucc)
            {
                foreach (var tpl in staticTableLst)
                {
                    if (!ExcelLoader.LoadStaticTable( tpl.path, tpl.name ))
                        break;
                }
            }


            Console.ReadKey();
        }

        /// <summary>
        /// 检查是否为静态表，是返回true
        /// </summary>
        private static bool IsStaticTable (string fileName)
        {

            return false;
        }
    }
}
