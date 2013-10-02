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
        static IEnumerable<IEnumerable<string>> GetFileListList(
            string name, string directory)
        {
            foreach(var d in Directory.GetDirectories(directory))
            {
                // short name of directory.
                var fn = Path.GetFileName(d);
                var libName = name + "_" + fn;
                var library =
                    Config.LibraryList.
                    FirstOrDefault(lib => lib.Name == libName);
                if (library == null)
                {
                    // add this short name to all its files and return them.
                    yield return
                        GetFiles(libName, d).
                        Select(f => Path.Combine(fn, f));
                }
                else
                {
                    MakeLibrary(library, d);
                }
            }
            yield return 
                Directory.
                GetFiles(directory).
                Select(f => Path.GetFileName(f));
        }

        static IEnumerable<string> GetFiles(string name, string directory)
        {
            return GetFileListList(name, directory).SelectMany(f => f);
        }

        static void MakeLibrary(Library libraryConfig, string src)
        {
            var files = GetFiles(libraryConfig.Name, src).ToList();

            var compilationUnitConfigList =
                libraryConfig.CompilationUnitList;

            var defaultFileListConfig =
                compilationUnitConfigList.
                FirstOrDefault(u => u.Name == null).
                NewIfNull().
                FileList;

            var additionalCompilationUnitList =
                compilationUnitConfigList.
                Where(u => u.Name != null).
                ToList();

            var additionalCppFileList =
                additionalCompilationUnitList.
                SelectMany(u => u.FileList).
                Select(f => f.Name).
                ToHashSet();

            var cppFiles =
                files.
                Where(f =>
                    !additionalCppFileList.Contains(f) &&
                    Path.GetExtension(f) == ".cpp").
                Select(f =>
                {
                    var config =
                        defaultFileListConfig.
                        FirstOrDefault(c =>
                            c.Name == f ||
                            c.Name == Path.GetDirectoryName(f));
                    var condition = config == null ? null : config.Condition;
                    return new CppFile(f, condition);
                });

            var compilationUnitList =
                additionalCompilationUnitList.
                Concat(Collections.New(
                    new CompilationUnit(null, cppFiles)));

            var library = new Library(
                libraryConfig.Name, src, files, compilationUnitList);

            library.Create();
        }

        static void Main(string[] args)
        {
            var boostLibs = 
                @"..\..\..\..\..\..\..\Downloads\boost_1_54_0\libs\";
            // TODO: include hpp/cpp/asm files from src folder.
            foreach (var directory in Directory.GetDirectories(boostLibs))
            {
                var src = Path.Combine(directory, "src");
                if (Directory.Exists(src))
                {
                    var name = Path.GetFileName(directory);

                    var libraryConfig =
                        Config.LibraryList.
                        Where(l => l.Name == name).
                        FirstOrDefault() ??
                        new Library(name);

                    MakeLibrary(libraryConfig, src);
                }
            }
        }

        /*
        private static readonly Version version = new Version(1, 54, 0, 46);

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

            public readonly string Preprocessor;

            public readonly string Part;

            public Library(
                string name,
                bool exclude = false,
                string[] excludeList = null,
                string preprocessor = "",
                string part = null)
            {
                Name = name;
                Exclude = exclude;
                ExcludeList = excludeList == null ? new string[0]: excludeList;
                Preprocessor = preprocessor;
                Part = part;
            }
        }

        private static readonly Library[] libraryList = new[]
        {
            // boost.chrono depends on boost.system
            // boost.context
            new Library(
                name: "context", 
                exclude: true),
            // boost.coroutine depends on boost.context
            new Library(
                name: "coroutine",
                excludeList: new[]
                {
                    @"detail\segmented_stack_allocator.cpp",
                    @"detail\standard_stack_allocator_posix.cpp"
                }),
            // boost.filesystem depeneds on boost.system
            // boost.graph depends on boost.regex
            // boost.graph_parallel depends on boost.mpi, boost.serialization.
            // boost.iostreams_bzip2 depends on bzip.
            new Library(
                name: "iostreams_bzip2"),
            // boost.iostreams_zlib depends on zlib
            new Library(
                name: "iostreams_zlib"),
            // boost.iostreams_gzip depends on boost.iostreams_zlib
            new Library(
                name: "iostreams_gzip"),
            // boost.locale depends on
            //     boost.thread,
            //     boost.system, 
            //     boost.date_time
            //     boost.chron
            new Library(
                name: "locale",
                // TODO: build with ICU.
                excludeList: new[] { "posix", "icu" },
                preprocessor: "BOOST_LOCALE_NO_POSIX_BACKEND;"),
            // boost.log depends on
            //     boost.system,
            //     boost.filesystem,
            //     boost.date_time,
            //     boost.thread,
            //     boost.chrono.
            new Library(
                name: "log",
                preprocessor:
                    "BOOST_SPIRIT_USE_PHOENIX_V3;" + 
                    "BOOST_LOG_WITHOUT_EVENT_LOG;" + 
                    "BOOST_LOG_SETUP_NO_LIB;"),
            // boost.mpi_python
            new Library(
                name: "mpi_python"),
            // boost.thread
            new Library(
                name: "thread",
                excludeList: new[] { "pthread" },
                preprocessor: "BOOST_THREAD_BUILD_LIB;",
                part: "path.cpp"),
        };

        private sealed class Unit
        {
            public readonly string Name;

            public readonly ICollection<string> Cpp = new LinkedList<string>();
        }

        private sealed class Files
        {
            public readonly Library Lib;
            public readonly XElement NuspecFiles = nuspecNs.Element("files");
            public readonly XElement ImportGroup = 
                msbuildNs.Element("ItemGroup");
            private string Dir;

            public readonly ICollection<string> Cpp = new LinkedList<string>();

            public Files(string name, string dir)
            {
                Lib = libraryList.FirstOrDefault(library => library.Name == name);
                if (Lib == null)
                {
                    Lib = new Library(name);
                }
                Dir = dir;
            }

            public void AddTargetCpp(string file)
            {
                var fullPath = Path.Combine(
                    @"$(MSBuildThisFileDirectory)..\..\", srcPath, file);
                ImportGroup.Append(msbuildNs.Element(
                    "ClCompile",
                    noNs.Attribute("Include", fullPath)).
                    Append(msbuildNs.Element("PrecompiledHeader").Append("NotUsing")).
                    Append(msbuildNs.Element("SDLCheck").Append("false")));
            }

            public void IncludeCpp(string file)
            {
                Cpp.Add("#include \"" + file + "\"");
            }

            public void AddSource(string src, string target)
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
                    if (Lib.ExcludeList.FirstOrDefault(
                            f => f == relativePath) == null)
                    {
                        var newFile = filePrefix + fileName;
                        var libraryFile = Path.GetFileNameWithoutExtension(
                            newFile);
                        var lib = libraryList.FirstOrDefault(
                            f => "boost_" + f.Name == libraryFile);
                        if (Path.GetExtension(fileName) != ".cpp")
                        {
                            AddSource(file, nuspecDirecotry);
                        }
                        else if (lib == null)
                        {
                            // make strong name.
                            // File.Copy(file, newFile, true);
                            // AppendFile(newFile, nuspecDirecotry);
                            // Add(Path.Combine(subDir, newFile));
                            AddSource(file, nuspecDirecotry);
                            IncludeCpp(relativePath);
                        }
                        else
                        {
                            // TODO: make a library.
                        }
                    }
                }
                foreach (var d in Directory.GetDirectories(fullDirectory))
                {
                    var newSubDir = Path.Combine(subDir, Path.GetFileName(d));
                    var newName = Lib.Name + "_" + newSubDir.Replace('\\', '_');
                    var newLib = libraryList.FirstOrDefault(
                            lib => lib.Name == newName);
                    if (newLib != null)
                    {
                        SrcDirectory(
                            Path.Combine(fullDirectory, subDir), newName);
                    }
                    else if (Lib.ExcludeList.FirstOrDefault(
                            f => f == newSubDir) == null)
                    {
                        this.Run(newSubDir);
                    }
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
            //
            files.AddSource(targetsFile, @"build\native\");
            //
            {
                var libraryCpp = libraryName + ".cpp";
                File.WriteAllLines(libraryName + ".cpp", files.Cpp);
                files.AddSource(libraryCpp, @"lib\native\src\");
                files.AddTargetCpp(libraryCpp);
            }
            //
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
            var upperName = libraryName.ToUpper();
            var pp =
                files.Lib.Preprocessor +
                upperName +
                "_NO_LIB;%(PreprocessorDefinitions)";
            var targets = msbuildNs.Element("Project",
                noNs.Attribute("ToolVersion", "4.0")).Append(
                msbuildNs.Element("ItemDefinitionGroup").Append(
                    msbuildNs.Element("ClCompile").Append(
                        msbuildNs.Element("PreprocessorDefinitions").Append(
                            pp))),
                files.ImportGroup);
            targets.CreateDocument().Save(targetsFile);
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
         * */
    }
}
