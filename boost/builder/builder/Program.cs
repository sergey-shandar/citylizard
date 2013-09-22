using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using System.Diagnostics;

namespace builder
{
    class Program
    {
        private static readonly Version version = new Version(1, 54, 0, 20);

        private const string author = "Sergey Shandar";

        private static readonly XNamespace noNs = XNamespace.Get("");

        private static readonly XNamespace nuspecNs = XNamespace.Get(
            "http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd");

        private static readonly XNamespace msbuildNs = XNamespace.Get(
            "http://schemas.microsoft.com/developer/msbuild/2003");

        private static readonly string srcPath = @"lib\native\src\";

        private sealed class Library
        {
            public readonly string Name;

            public readonly bool Exclude;

            public readonly string[] ExcludeList;

            public Library(
                string name, bool exclude = false, string[] excludeList = null)
            {
                Name = name;
                Exclude = exclude;
                ExcludeList = excludeList == null ? new string[0]: excludeList;
            }
        }

        private static readonly Library[] libraryList = new[]
        {
            // chrono depends on system
            // context
            new Library(
                name: "context", 
                exclude: true),
            // coroutine depends on context
            new Library(
                name: "coroutine",
                excludeList: new[]
                {
                    @"detail\segmented_stack_allocator.cpp",
                    @"detail\standard_stack_allocator_posix.cpp"
                })
        };

        private sealed class Files
        {
            public readonly Library Lib;
            public readonly XElement NuspecFiles = nuspecNs.Element("files");
            public readonly ICollection<string> Cpp = new LinkedList<string>();
            public readonly XElement ImportGroup = 
                msbuildNs.Element("ItemGroup");
            private string Dir;

            public Files(string name, string dir)
            {
                Lib = libraryList.FirstOrDefault(library => library.Name == name);
                if (Lib == null)
                {
                    Lib = new Library(name);
                }
                Dir = dir;
            }

            public void Add(string file)
            {
                ImportGroup.Append(msbuildNs.Element(
                    "ClCompile",
                    noNs.Attribute(
                        "Include",
                        Path.Combine(
                            @"$(MSBuildThisFileDirectory)..\..\",
                            srcPath,
                            file))).Append(
                    msbuildNs.Element("PrecompiledHeader").Append("NotUsing")));
            }

            public void AppendFile(string src, string target)
            {
                NuspecFiles.Append(nuspecNs.Element(
                    "file",
                    noNs.Attribute("src", src),
                    noNs.Attribute("target", target)));
            }

            public void Run(string subDir = "")
            {
                var fullDirectory = Path.Combine(Dir, subDir);
                var nuspecDirecotry = Path.Combine(srcPath, subDir);
                var filePrefix = 
                    "boost_" +
                    Lib.Name + 
                    "_" + 
                    (subDir == "" ? "" : subDir.Replace('\\', '_') + '_');
                foreach (var file in Directory.GetFiles(fullDirectory))
                {
                    var fileName = Path.GetFileName(file);
                    var relativePath = Path.Combine(subDir, fileName);
                    //
                    if (Lib.ExcludeList.FirstOrDefault(
                            f => f == relativePath) ==
                        null)
                    {
                        if (Path.GetExtension(fileName) == ".cpp")
                        {
                            var newFile = filePrefix + fileName;
                            File.Copy(file, newFile, true);
                            AppendFile(newFile, nuspecDirecotry);
                            Add(Path.Combine(subDir, newFile));
                            /*
                            Cpp.Add(
                                "#include \"" +
                                Path.Combine(subDir, newFile) +
                                "\"");
                             * */
                        }
                        else
                        {
                            AppendFile(file, nuspecDirecotry);
                        }
                    }
                }
                foreach (var d in Directory.GetDirectories(fullDirectory))
                {
                    this.Run(Path.Combine(subDir, Path.GetFileName(d)));
                }
            }
        }

        private static void SrcDirectory(
            string srcDirectory, string name)
        {
            var files = new Files(name, srcDirectory);
            if (files.Lib.Exclude)
            {
                return;
            }
            var libraryName = "boost_" + name;
            var versionRange = 
                "[" +
                new Version(version.Major, version.Minor) +
                "," +
                new Version(version.Major, version.Minor + 1) +
                ")";
            Console.WriteLine(libraryName);
            var libraryNamePrefix = libraryName + "_";
            files.Run();
            var id = libraryName + "_src";
            var targetsFile = id + ".targets";
            var libraryCpp = libraryName + ".cpp";
            files.AppendFile(targetsFile, @"build\native\");
            files.AppendFile(libraryCpp, srcPath);
            var nuspec = nuspecNs.Element("package").Append(
                nuspecNs.Element("metadata").Append(
                    nuspecNs.Element("id").Append(id),
                    nuspecNs.Element("version").Append(version.ToString()),
                    nuspecNs.Element("authors").Append(author),
                    nuspecNs.Element("owners").Append(author), 
                    nuspecNs.Element("licenseUrl").Append(
                        "http://www.boost.org/LICENSE_1_0.txt"),
                    nuspecNs.Element("projectUrl").Append("http://boost.org/"),
                    nuspecNs.Element("requireLicenseAcceptance").Append(
                        "false"),
                    nuspecNs.Element("description").Append(
                        libraryName + ", source files."),
                    nuspecNs.Element("dependencies").Append(
                        nuspecNs.Element(
                            "dependency",
                            noNs.Attribute("id", "boost"),
                            noNs.Attribute("version", versionRange)))),
                files.NuspecFiles);
            var nuspecFile = id + ".nuspec";
            nuspec.CreateDocument().Save(nuspecFile);
            //
            var pp =
                libraryName.ToUpper() + "_NO_LIB;%(PreprocessorDefinitions)";
            files.Add(libraryCpp);
            var targets = msbuildNs.Element("Project",
                noNs.Attribute("ToolVersion", "4.0")).Append(
                msbuildNs.Element("ItemDefinitionGroup").Append(
                    msbuildNs.Element("ClCompile").Append(
                        msbuildNs.Element("PreprocessorDefinitions").Append(
                            pp))),            
                files.ImportGroup);
            targets.CreateDocument().Save(targetsFile);
            //
            File.WriteAllLines(libraryCpp, files.Cpp);
            //
            Process.Start(
                new ProcessStartInfo(
                    @"C:\programs\nuget.exe", "pack " + nuspecFile)
                {
                     UseShellExecute = false,
                }).WaitForExit();
        }

        static void Main(string[] args)
        {
            var boostPath =
                @"..\..\..\..\..\..\..\Downloads\boost_1_54_0\libs\";
            // TODO: include hpp/cpp/asm files from src folder.
            foreach(var directory in Directory.GetDirectories(boostPath))
            {
                var src = Path.Combine(directory, "src");
                if (Directory.Exists(src))
                {
                    SrcDirectory(src, Path.GetFileName(directory));
                }
            }
        }
    }
}
