using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;
using System.Diagnostics;

namespace builder
{
    sealed class Library
    {
        public readonly string Name;

        public readonly string Directory;

        public readonly IEnumerable<string> FileList;

        public readonly IEnumerable<CompilationUnit> CompilationUnitList;

        public readonly IEnumerable<Library> LibraryList;

        public Library(
            string name,
            string directory = null,
            IEnumerable<string> fileList = null,
            IEnumerable<CompilationUnit> compilationUnitList = null,
            IEnumerable<Library> libraryList = null)
        {
            Name = name;
            Directory = directory;
            FileList = fileList;
            CompilationUnitList = compilationUnitList.EmptyIfNull();
            LibraryList = libraryList.EmptyIfNull();
        }

        public Library(): this(null)
        {
        }

        public string LibraryId
        {
            get { return "boost_" + Name; }
        }

        public string NuspecId
        {
            get { return LibraryId + "_src"; } 
        }

        public void Create()
        {
            var versionRange = 
                "[" +
                new Version(version.Major, version.Minor) +
                "," +
                new Version(version.Major, version.Minor + 1) +
                ")";
            var description = "Boost." + Name;
            var srcFiles = 
                FileList.Select(f => File(
                    Path.Combine(Directory, f),
                    Path.Combine(targetSrcPath, f)));
            var unitFiles = CompilationUnitList.
                Select(u => File(u.FileName(this), targetSrcPath));
            var targetsFile = NuspecId + ".targets";
            var nuspec =
                N("package").Append(
                    N("metadata").Append(
                        N("id", NuspecId),
                        N("version", version.ToString()),
                        N("authors", authors),
                        N("owners", authors),
                        N("licenseUrl", "http://www.boost.org/LICENSE_1_0.txt"),
                        N("projectUrl", "http://boost.org/"),
                        N("requireLicenseAcceptance", "false"),
                        N("description", description),
                        N("dependencies").Append(
                            N(
                                "dependency",
                                A("id", "boost"),
                                A("version", versionRange)
                            )
                        )
                    ),
                    N("files").
                        Append(srcFiles).
                        Append(unitFiles).
                        Append(File(targetsFile, targetBuildPath))
                );
            var nuspecFile = NuspecId + ".nuspec";
            nuspec.CreateDocument().Save(nuspecFile);
            //
            foreach (var u in CompilationUnitList)
            {
                u.Make(this);
            }
            //
            var pd = LibraryId.ToUpper() + "_NO_LIB;%(PreprocessorDefinitions)";
            var srcPath = Path.Combine(
                    @"$(MSBuildThisFileDirectory)..\..\", targetSrcPath);
            var unitList = CompilationUnitList.Select(u => 
                M("ClCompile", 
                    A("Include", Path.Combine(srcPath, u.FileName(this)))
                ).Append(
                    M("PrecompiledHeader", "NotUsing")
                ));
            var targets = 
                M("Project", A("ToolVersion", "4.0")).Append(
                    M("ItemDefinitionGroup").Append(
                        M("ClCompile").Append(
                            M("PreprocessorDefinitions", pd)
                        )
                    ),
                    M("ItemGroup").Append(unitList)
                );
            targets.CreateDocument().Save(targetsFile);
            Process.Start(
                new ProcessStartInfo(
                    @"C:\programs\nuget.exe", "pack " + nuspecFile)
                {
                    UseShellExecute = false,
                }).WaitForExit();
        }

        private static XElement N(
            string elementName, params XAttribute[] attributeList)
        {
            return n.Element(elementName, attributeList);
        }

        private static XElement M(
            string elementName, params XAttribute[] attributeList)
        {
            return m.Element(elementName, attributeList);
        }

        private static XElement N(string elementName, string content)
        {
            return N(elementName).Append(content);
        }

        private static XElement M(string elementName, string content)
        {
            return M(elementName).Append(content);
        }

        private static XAttribute A(string attributeName, string value)
        {
            return x.Attribute(attributeName, value);
        }

        private static XElement File(string src, string target)
        {
            return N("file", A("src", src), A("target", target));
        }

        private static readonly Version version = new Version(1, 54, 0, 83);

        private const string authors = "Sergey Shandar, Boost";

        private static readonly XNamespace n = XNamespace.Get(
            "http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd");

        private static readonly XNamespace m = XNamespace.Get(
            "http://schemas.microsoft.com/developer/msbuild/2003");

        private static readonly XNamespace x = XNamespace.Get("");

        private const string targetSrcPath = @"lib\native\src\";

        private const string targetBuildPath = @"build\native\";
    }
}
