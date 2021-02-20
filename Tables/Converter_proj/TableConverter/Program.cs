using System;
using System.IO;

namespace TableConverter
{
    class Program
    {
        static void Main (string[] args)
        {
            //var local = System.Environment.CurrentDirectory;
            //var rootPath = local.Substring(0, local.IndexOf( @"\Tables" ) );
            //rootPath = string.Format( @"{0}\FFTA\Assets\Res\Config", rootPath );
            //Console.WriteLine( rootPath );
            //Console.ReadKey();

            //return;
            //-----------------------TEST--------------------------

            //1、获取同目录下表路径配置文件（FFTA_Remake/Tables），获取不到则新建，路径指向Tables/Public文件夹
            //var localPath = System.Environment.CurrentDirectory;

            var local = Environment.CurrentDirectory;
            var rootPath = local.Substring( 0, local.IndexOf( @"\Tables" ) );
            var excelPath = $@"{rootPath}\Tables\Public";
            //var localPath = string.Format( @"{0}\FFTA\Assets\Res\Config", rootPath );
            var localPath = $@"{rootPath}\FFTA\Assets\Res\Config";
            var tableDirectory = excelPath;
            //var tableDirectory = localPath + @"\Public";
            //var pathDirectory = localPath + @"\path.txt";
            ////检查目录和创建
            //if (!Directory.Exists(tableDirectory) || !File.Exists( pathDirectory ))
            //{
            //    Console.WriteLine(string.Format($"不存在目录 {tableDirectory} ,创建..."));
            //    ExcelLoader.CreateDefaultPathFile( pathDirectory, tableDirectory );
            //    Console.WriteLine( "完成" );
            //}

            //2、读Public下的所有Excel并生成tsv至Assets\Res\Config下，原文件直接覆盖
            DirectoryInfo dirInfo = new DirectoryInfo( tableDirectory );
            FileInfo[] files = dirInfo.GetFiles();

            foreach (var file in files)
            {
                if (!ExcelLoader.LoadExcel( tableDirectory + @"\" + file.Name ,file.Name))
                    break;
            }

            Console.ReadKey();
        }


    }
}
