using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;

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

        private static void AppendFile(XElement files, string src, string target)
        {
            files.Append(nuspecNs.Element(
                "file",
                noNs.Attribute("src", src),
                noNs.Attribute("target", target)));
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
            var files = nuspecNs.Element("files");
            var itemGroup = msbuildNs.Element("ItemGroup");
            Console.WriteLine(libraryName);
            var libraryNamePrefix = libraryName + "_";
            var cpp = new StringBuilder();
            foreach (var file in Directory.GetFiles(srcDirectory))
            {
                var fileName = Path.GetFileName(file);
                Console.WriteLine("    " + libraryNamePrefix + fileName);
                var newFileName = libraryNamePrefix + fileName;
                File.Copy(file, newFileName, true);
                AppendFile(files, newFileName, @"lib\native\src\");
                if (Path.GetExtension(fileName) == ".cpp")
                {
                    itemGroup.Append(msbuildNs.Element(
                        "ClCompile",
                        noNs.Attribute(
                            "Include",
                            @"$(MSBuildThisFileDirectory)..\..\lib\native\src\" +
                                newFileName)));
                    cpp.AppendLine("#include \"" + fileName + "\"");
                }
            }
            foreach (var directory in Directory.GetDirectories(srcDirectory))
            {
                SrcDirectory(
                    directory, libraryNamePrefix + Path.GetFileName(directory));
            }
            var id = libraryName + "_src";
            var targetsFile = id + ".targets";
            AppendFile(files, targetsFile, @"build\native\");
            var nuspec = nuspecNs.Element("package").Append(
                nuspecNs.Element("metadata").Append(
                    nuspecNs.Element("id").Append(id),
                    nuspecNs.Element("version").Append(version.ToString())),
                    nuspecNs.Element("authors").Append(author),
                    nuspecNs.Element("owner").Append(author), 
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
                            noNs.Attribute("version", versionRange))),
                files);
            nuspec.CreateDocument().Save(id + ".nuspec");
            //
            var pp =
                libraryName.ToUpper() + "_NO_LIB;%(PreprocessorDefinitions)";
            var targets = msbuildNs.Element(
                "Project", noNs.Attribute("ToolVersion", "4.0")).Append(
                msbuildNs.Element("ItemDefinitionGroup").Append(
                    msbuildNs.Element("ClCompile").Append(
                        msbuildNs.Element("PreprocessorDefinitions").Append(
                            pp))),
                itemGroup);
            targets.CreateDocument().Save(targetsFile);
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
