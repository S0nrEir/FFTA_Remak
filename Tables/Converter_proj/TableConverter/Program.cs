using System;
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

            //load normal table
            foreach (var file in dirInfo.GetFiles())
            {
                if (!ExcelLoader.LoadExcel( tableDirectory + @"\" + file.Name ,file.Name))
                    break;
            }

            //#todo load static table

            Console.ReadKey();
        }


    }
}
