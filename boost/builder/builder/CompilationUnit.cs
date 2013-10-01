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
            FileList = fileList.NewIfNull();
        }

        public CompilationUnit(): this(null)
        {
        }

        public string FileName(Library library)
        {
            return
                library.LibraryId + 
                (Name == null ? "" : "_" + Name) +
                ".cpp";
        }

        public void Make(Library library)
        {
            File.WriteAllLines(
                FileName(library),
                Collections.
                    New(new CppFile(@"boost\config.hpp")).
                    Concat(FileList).
                    SelectMany(f => f.Code));
        }
    }
}
