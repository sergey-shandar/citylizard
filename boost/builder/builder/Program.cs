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
        private static readonly Version version = new Version(1, 54, 0, 0);

        private const string author = "Sergey Shandar";

        private static readonly XNamespace noNs = XNamespace.Get("");

        private static readonly XNamespace nuspecNs = XNamespace.Get(
            "http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd");

        private static readonly XNamespace msbuildNs = XNamespace.Get(
            "http://schemas.microsoft.com/developer/msbuild/2003");

        private static readonly string srcPath = @"lib\native\src\";

        private static void AppendFile(XElement files, string src, string target)
        {
            files.Append(nuspecNs.Element(
                "file",
                noNs.Attribute("src", src),
                noNs.Attribute("target", target)));
        }

        private static void SrcFiles(
            XElement nuspecFiles,
            ICollection<String> cpp,
            string directory,
            string subDirectory = "")
        {
            var fullDirectory = Path.Combine(directory, subDirectory);
            var nuspecDirecotry = Path.Combine(srcPath, subDirectory);
            foreach (var file in Directory.GetFiles(fullDirectory))
            {
                var fileName = Path.GetFileName(file);
                AppendFile(nuspecFiles, file, nuspecDirecotry);
                Console.WriteLine("    :" + fileName);
                if (Path.GetExtension(fileName) == ".cpp")
                {
                    cpp.Add("#include \"" + Path.Combine(subDirectory, fileName) + "\"");
                }
            }
            foreach (var d in Directory.GetDirectories(fullDirectory))
            {
                SrcFiles(
                    nuspecFiles,
                    cpp,
                    directory,
                    Path.Combine(subDirectory, Path.GetFileName(d)));
            }
        }

        private static void SrcDirectory(
            string srcDirectory, string libraryName)
        {
            var versionRange = 
                "[" +
                version +
                "," +
                new Version(version.Major, version.Minor + 1) +
                ")";
            Console.WriteLine(libraryName);
            var libraryNamePrefix = libraryName + "_";
            var cpp = new LinkedList<String>();
            var files = nuspecNs.Element("files");
            SrcFiles(files, cpp, srcDirectory); 
            var id = libraryName + "_src";
            var targetsFile = id + ".targets";
            var libraryCpp = libraryName + ".cpp";
            AppendFile(files, targetsFile, @"build\native\");
            AppendFile(files, libraryCpp, srcPath);
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
                files);
            var nuspecFile = id + ".nuspec";
            nuspec.CreateDocument().Save(nuspecFile);
            //
            var pp =
                libraryName.ToUpper() + "_NO_LIB;%(PreprocessorDefinitions)";
            var targets = msbuildNs.Element("Project",
                noNs.Attribute("ToolVersion", "4.0")).Append(
                msbuildNs.Element("ItemDefinitionGroup").Append(
                    msbuildNs.Element("ClCompile").Append(
                        msbuildNs.Element("PreprocessorDefinitions").Append(
                            pp))),                            
                msbuildNs.Element("ItemGroup").Append(
                    msbuildNs.Element(
                        "ClCompile",
                        noNs.Attribute(
                            "Include",
                            Path.Combine(
                                @"$(MSBuildThisFileDirectory)..\..\",
                                srcPath,
                                libraryCpp)))));
            targets.CreateDocument().Save(targetsFile);
            //
            File.WriteAllLines(libraryCpp, cpp);
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
                    SrcDirectory(src, "boost_" + Path.GetFileName(directory));
                }
            }
        }
    }
}
