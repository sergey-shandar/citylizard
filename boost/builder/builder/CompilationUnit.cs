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

        public readonly string CppFile;

        public CompilationUnit(
            string name, string cppFile)
        {
            Name = name;
            CppFile = cppFile;
        }

        public CompilationUnit(): this(null, null)
        {
        }

        public string LocalPath
        {
            get { return Path.GetDirectoryName(CppFile); }
        }

        public string FileName(string packageId)
        {
            return
                packageId + 
                (Name == null ? "" : "_" + Name) +
                ".cpp";
        }

        public void Make(string packageId, Package package)
        {
            File.WriteAllLines(
                FileName(packageId),
                new[]
                    {
                        "#define _SCL_SECURE_NO_WARNINGS",
                        "#define _CRT_SECURE_NO_WARNINGS",
                        "#pragma warning(disable: 4503 4752 4800)"
                    }.
                    Concat(package.LineList).
                    Concat(
                        new[]
                        { 
                            "#include \"" + Path.GetFileName(CppFile) + "\""
                        })
                );
        }
    }
}
