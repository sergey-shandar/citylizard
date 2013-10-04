using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace builder
{
    class Package
    {
        public readonly string Name;

        public readonly IEnumerable<string> FileList;

        public readonly IEnumerable<CompilationUnit> CompilationUnitList;

        public Package(
            string name,
            IEnumerable<string> fileList = null,
            IEnumerable<CompilationUnit> compilationUnitList = null)
        {
            Name = name;
            FileList = fileList;
            CompilationUnitList = compilationUnitList.EmptyIfNull();
        }

        public Package(): this(null)
        {
        }

        public string PackageId(string libraryName)
        {
            return "boost_" + libraryName + (Name == null ? "": "_" + Name);
        }        
    }
}
