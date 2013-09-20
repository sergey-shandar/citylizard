using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace builder
{
    class Program
    {
        static void Main(string[] args)
        {
            var boostPath = @"..\..\..\..\..\..\..\Downloads\boost_1_54_0\libs\";
            // TODO: include hpp/cpp/asm files from src folder.
            foreach(var directory in Directory.GetDirectories(boostPath))
            {
                var src = Path.Combine(directory, "src");
                if (Directory.Exists(src))
                {
                    Console.WriteLine(Path.GetFileName(directory));
                    foreach (var file in Directory.GetFiles(src))
                    {
                        Console.WriteLine("    " + Path.GetFileName(file));
                    }
                }
            }
        }
    }
}
