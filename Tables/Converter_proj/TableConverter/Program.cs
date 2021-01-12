using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableConverter
{
    class Program
    {


        static void Main (string[] args)
        {
            var local = System.Environment.CurrentDirectory;
            var rootPath = local.Substring(0, local.IndexOf( @"\Tables" ) );
            rootPath = string.Format( @"{0}\FFTA\Assets\Res\Config", rootPath );
            Console.WriteLine( rootPath );
            Console.ReadKey();


            return;
            //-----------------------TEST--------------------------
            
            //1、获取同目录下表路径配置文件（FFTA_Remake/Tables），获取不到则新建，路径指向Tables/Public文件夹

            var localPath = System.Environment.CurrentDirectory;
            var tableDirectory = localPath + @"\Public";
            var pathDirectory = localPath + @"\path.txt";
            //检查目录和创建
            if (!Directory.Exists(tableDirectory) || !File.Exists( pathDirectory ))
            {
                Console.WriteLine(string.Format("不存在目录 {0} ,手动创建...",tableDirectory));
                Tools.CreateDefaultPathFile( pathDirectory, tableDirectory );
                Console.WriteLine( "完成" );
            }

            //2、读Public下的所有Excel并生成tsv之Assets\Res\Config下，原文件直接覆盖
            DirectoryInfo dirInfo = new DirectoryInfo( tableDirectory );
            FileInfo[] files = dirInfo.GetFiles();

            foreach (var file in files)
                Tools.LoadExcel( tableDirectory + @"\" + file.Name );

            Console.ReadKey();
        }


    }
}
