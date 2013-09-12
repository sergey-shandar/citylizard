using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using System.Diagnostics;

using CityLizard.Xml;

namespace Modules
{
    using B = CityLizard.Xml.Untyped;

    class Program
    {
        [Flags]
        enum Type
        {
            None = 0,
            Static = 1,
            Shared = 2,
            StaticShared = Static|Shared,
        }

        class Output
        {
            public readonly string Name;
            public readonly Type Type;

            public Output(string name, Type type = Program.Type.StaticShared)
            {
                Name = name;
                Type = type;
            }
        };

        class Library
        {
            public readonly string Name;
            public readonly Output[] LibNameList;
            public readonly string[] HeaderList;
            public readonly bool Built;

            public Library(
                string name,
                Output[] libNameList,
                string[] headerList,
                bool built = false)
            {
                Name = name;
                LibNameList = libNameList;
                HeaderList = headerList;
                Built = built;
            }
        }

        // atomic
        // chrono
        // context
        // date_time
        // iostream
        //  archive => zip,bzip2
        // random

        static void Main(string[] args)
        {
            var LibaryList = new[]
            {
                // not in a list
                new Library(
                    "atomic",
                    new[] { new Output("atomic") },
                    new[] { "atomic", "atomic.hpp" }),
                new Library(
                    "chrono",
                    new[] { new Output("chrono") },
                    new[] { "chrono", "chrono.hpp" }),
                new Library(
                    "context",
                    new[] { new Output("context") },
                    new[] { "context" }),
                // not in a list, lib only
                new Library(
                    "coroutine",
                    new[] { new Output("coroutine", Type.Static) },
                    new[] { "coroutine" }),
                new Library(
                    "date_time",
                    new[] { new Output("date_time") },
                    new[] { "date_time" }),
                // optional, lib only
                new Library(
                    "exception",
                    new[] { new Output("exception", Type.Static) },
                    new[]
                    {
                        "exception", "exception.hpp", "throw_exception.hpp"
                    }),
                new Library(
                    "filesystem",
                    new[] { new Output("filesystem") },
                    new[] { "filesystem", "filesystem.hpp" }),
                // graph parallel?
                new Library(
                    "graph",
                    new[] { new Output("graph") },
                    new[] { "graph" }),
                new Library(
                    "iostreams",
                    new[] { new Output("iostreams") },
                    new[] { "iostream" }),
                new Library(
                    "locale",
                    new[] { new Output("locale") },
                    new[] { "locale", "locale.hpp" }),
                // not in a list
                new Library(
                    "log",
                    new[] { new Output("log"), new Output("log_setup"), },
                    new[] { "log" }),
                // optional
                new Library(
                    "math",
                    new[]
                    {   
                        new Output("math_c99"),
                        new Output("math_c99f"),
                        new Output("math_c99l"),
                        new Output("math_tr1"),
                        new Output("math_tr1f"),
                        new Output("math_tr1l"),
                    },
                    new[] { "math", "math_fwd.hpp" }),
                // not built.
                new Library(
                    "mpi",
                    new Output[0],
                    new[] { "mpi", "mpi.hpp" },
                    false),
                new Library(
                    "program_options",
                    new[] { new Output("program_options") },
                    new[] { "program_options", "program_options.hpp" }),
                // not built.
                new Library(
                    "python",
                    new Output[0],
                    new[] { "python", "python.hpp" },
                    false),
                // optional
                new Library(
                    "random",
                    new[] { new Output("random") },
                    new[] { "random", "random.hpp" }),
                new Library(
                    "regex",
                    new[] { new Output("regex") },
                    new[] { "regex", "regex.h", "regex.hpp", "regex_fwd.hpp" }),
                // optional: boost_zip, boost_bzip2.
                new Library(
                    "serialization",
                    new[]
                    {
                        new Output("serialization"),
                        new Output("wserialization"),
                    },
                    new[] { "archive", "serialization" }),
                new Library(
                    "signals",
                    new[] { new Output("signals") },
                    new[] { "signals", "signal.hpp", "signals.hpp" }),
                new Library(
                    "system",
                    new[] { new Output("system") },
                    new[] { "system" }),
                // optional
                new Library(
                    "test",
                    new[] 
                    {
                        new Output("prg_exec_monitor"),
                        new Output("test_exec_monitor", Type.Static),
                        new Output("unit_test_framework"),
                    },
                    new[] { "test" }),
                new Library(
                    "thread",
                    new[] { new Output("thread") },
                    new[] { "thread", "thread.hpp" }),
                new Library(
                    "timer",
                    new[] { new Output("timer") },
                    new[] { "timer", "timer.hpp" }),
                new Library(
                    "wave",
                    new[] { new Output("wave") },
                    new[] { "wave", "wave.hpp" }),
            };

            /*
            var boost = @"..\..\..\..\..\..\..\Downloads\boost_1_54_0\boost";
            // directories.
            //var directories = Directory.GetDirectories(boost).
            //    Select(
            //        directory => Path.GetFileNameWithoutExtension(directory));
            var files = Directory.GetFiles(boost, "*.hpp").
                Select(file => Path.GetFileNameWithoutExtension(file));
            // core files.
            var coreFiles = files.
                Except(directories).
                Except(new string[] { "concept_check" }).ToArray();
            //
            var x = new Untyped(
                "http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd");
            var document = new XDocument();
            var authors = "Sergey Shandar";
            var id = "boost.core";
            var xmlFiles = x.X("files");
            foreach (var coreFile in coreFiles)
            {
                var file = coreFile + ".hpp";
                var source = Path.Combine(boost, file);
                var target = Path.Combine(
                    @"lib\native\include\boost", file);
                xmlFiles.Add(x.E(
                    "file", B.A("src", source), B.A("target", target)));
            }
            xmlFiles.
            document.Add(
                x.X("package")
                    [x.X("metadata")
                        [x.X("id")[id]]
                        [x.X("version")["1.54.0.31"]]
                        [x.X("authors")[authors]]
                        [x.X("owners")[authors]]
                        [x.X("licenseUrl")
                            ["http://www.boost.org/LICENSE_1_0.txt"]
                        ]
                        [x.X("projectUrl")["http://boost.org/"]]
                        [x.X("requireLicenseAcceptance")["false"]]
                        [x.X("description")[id]]
                        [x.X("dependencies")]
                    ]
                    [xmlFiles]);
            document.Save("boost.nuspec");
            
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = @"c:\programs\nuget.exe",
                    // WorkingDirectory = @"c:\programs",
                    // RedirectStandardOutput = true,
                    // UseShellExecute = false,
                    // RedirectStandardInput = true,
                    // RedirectStandardError = true,
                    // CreateNoWindow = true,
                    UseShellExecute = false,
                    Arguments = "pack boost.nuspec",
                }
            };
            process.Start();
            process.WaitForExit();
            /*
            var libraries = files.Union(directories).ToArray();
            foreach (var library in libraries)
            {
                var libraryFiles = new List<string>();
                if (Directory.Exists(library))
                {
                    libraryFiles.AddRange(Directory.GetFiles(library));
                }
                var libraryFile = library + ".hpp";
                if (File.Exists(libraryFile))
                {
                    libraryFiles.Add(libraryFile);
                }
            }
            */
        }
    }
}
