using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace builder
{
    sealed class CompilationUnit
    {
        public readonly string Name;

        public readonly IEnumerable<CppFile> FileList;

        public CompilationUnit(
            string name, IEnumerable<CppFile> fileList = null)
        {
            Name = name;
            FileList = fileList.EmptyIfNull();
        }

        public CompilationUnit(): this(null)
        {
        }

        public static CompilationUnit Cpp(string name)
        {
            return
                new CompilationUnit(
                    name, Collections.New(new CppFile(name + ".cpp")));
        }

        public string FileName(string packageId)
        {
            return
                packageId + 
                (Name == null ? "" : "_" + Name) +
                ".cpp";
        }

        public void Make(string packageId)
        {
            File.WriteAllLines(
                FileName(packageId),
                Collections.
                    New(
                        "#define _SCL_SECURE_NO_WARNINGS",
                        "#define _CRT_SECURE_NO_WARNINGS").
                    Concat(
                        Collections.
                            New(new CppFile(@"boost\config.hpp")).
                            Concat(FileList).
                            SelectMany(f => f.Code)
                    )
                );
        }
    }
}
