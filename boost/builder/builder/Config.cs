using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace builder
{
    static class Config
    {
        public class SubLibrary
        {
            public readonly string ParentLibrary;
            public readonly string Name;

            public readonly IEnumerable<string> FileList;

            public SubLibrary(
                string parentLibrary,
                string name,
                IEnumerable<string> fileList)
            {
                ParentLibrary = parentLibrary;
                Name = name;
                FileList = fileList;
            }
        }

        public static readonly SubLibrary[] SubLibraryList = new[]
        {
            new SubLibrary(
                "iostreams", "zlib", Collections.New("zlib.cpp", "gzip.cpp")),
            new SubLibrary(
                "iostreams", "bzip2", Collections.New("bzip2.cpp")),
            new SubLibrary(
                "locale", "icu", Collections.New("icu.cpp")),
            new SubLibrary(
                "mpi", "python", Collections.New("python")),
        };

        public static readonly Library[] LibraryList = new[]
        {
            // chrono depends on system
            // context needs ASM.
            // coroutine
            new Library(
                name: "coroutine",
                compilationUnitList: new[]
                {
                    new CompilationUnit(
                        name: null,
                        // 3 options:
                        //   1. segmented stacks,
                        //   2. posix standard stack,
                        //   3. windows standard stack.
                        fileList: new[] 
                        {
                            new CppFile(
                                @"detail\segmented_stack_allocator.cpp",
                                "defined BOOST_USE_SEGMENTED_STACKS"),
                            new CppFile(
                                @"detail\standard_stack_allocator_posix.cpp",
                                "!defined BOOST_USE_SEGMENTED_STACKS && !defined BOOST_WINDOWS"),
                            new CppFile(
                                @"detail\standard_stack_allocator_windows.cpp",
                                "!defined BOOST_USE_SEGMENTED_STACKS && defined BOOST_WINDOWS"),
                        }),
                }),
            // filesystem depends on system.
            new Library(
                name: "filesystem",
                compilationUnitList: new[]
                {
                    new CompilationUnit(
                        name: "path",
                        fileList: new[]
                        {
                            new CppFile("path.cpp")
                        }),
                }),
            // graph depens on regex. WIP
            // graph_parallel depends on mpi.
            // iostreams.
            // mpi depends on serialization and MPI (3rd party).
            new Library(
                name: "mpi",
                compilationUnitList: new[]
                {
                    // archive_serializer_map.ipp
                    // 1. content_oarchive.cpp
                    // 2. ...
                    CompilationUnit.Cpp("mpi_datatype_oarchive"),
                    CompilationUnit.Cpp("packed_iarchive"),
                    CompilationUnit.Cpp("packed_oarchive"),
                    CompilationUnit.Cpp("packed_skeleton_iarchive"),
                    CompilationUnit.Cpp("packed_skeleton_oarchive"),
                    CompilationUnit.Cpp("text_skeleton_oarchive"),
                }),
            // mpi python.
            new Library(name: "mpi_python"),
            // regex.
            new Library(
                name: "regex",
                compilationUnitList: new[]
                {
                    CompilationUnit.Cpp("usinstances"),
                    CompilationUnit.Cpp("instances"),
                    CompilationUnit.Cpp("winstances"),
                }),
            // serialization.
            new Library(
                name: "serialization",
                compilationUnitList: new[]
                {
                    // basic_text_iprimitive.ipp
                    // 1. basic_text_iprimitive.cpp
                    // basic_text_oprimitive.ipp
                    // 1. basic_text_oprimitive.cpp
                    // archive_serializer_map.ipp
                    // 1. polymorphic_iarchive.cpp
                    // basic_xml_grammar.ipp
                    // 1. xml_grammar
                    new CompilationUnit(
                        name: "2",
                        fileList: new[]
                        {
                            // basic_text_iprimitive.ipp
                            // 2.
                            new CppFile("basic_text_wiprimitive.cpp"),
                            // basic_text_oprimitive.ipp
                            // 2.
                            new CppFile("basic_text_woprimitive.cpp"),
                            // archive_serializer_map.ipp
                            // 2.
                            new CppFile("polymorphic_oarchive.cpp"),
                            // basic_xml_grammar.ipp
                            // 2.
                            new CppFile("xml_wgrammar.cpp"),
                        }),
                    // archive_serializer_map.ipp
                    // 3. ...
                    CompilationUnit.Cpp("binary_iarchive"),
                    CompilationUnit.Cpp("binary_oarchive"),
                    CompilationUnit.Cpp("binary_wiarchive"),
                    CompilationUnit.Cpp("binary_woarchive"),
                    CompilationUnit.Cpp("text_iarchive"),
                    CompilationUnit.Cpp("text_oarchive"),
                    CompilationUnit.Cpp("text_wiarchive"),
                    CompilationUnit.Cpp("text_woarchive"),
                    CompilationUnit.Cpp("xml_iarchive"),
                    CompilationUnit.Cpp("xml_oarchive"),
                    CompilationUnit.Cpp("xml_wiarchive"),
                    CompilationUnit.Cpp("xml_woarchive"),
                }),
        };

        /*
        // TODO: make sublibraries.
        public static readonly Library[] LibraryList = new[]
        {
            new Library(
                name: "coroutine", 
                compilationUnitList: new[]
                {
                    new CompilationUnit(
                        name: null,
                        fileList: new[] 
                        {
                            new CppFile(
                                @"detail\segmented_stack_allocator.cpp",
                                "defined BOOST_USE_SEGMENTED_STACKS"),
                            new CppFile(
                                @"detail\standard_stack_allocator_posix.cpp",
                                "!defined BOOST_WINDOWS"),
                            new CppFile(
                                @"detail\standard_stack_allocator_windows.cpp",
                                "defined BOOST_WINDOWS"),
                        }), 
                }),
            new Library(
                name: "filesystem",
                compilationUnitList: new[]
                {
                    new CompilationUnit(
                        name: "path",
                        fileList: new[]
                        {
                            new CppFile("path.cpp")
                        }),
                }),
            new Library(
                name: "iostreams",
                compilationUnitList: new[]
                {
                    new CompilationUnit(
                        name: null, 
                        fileList: new[]
                        {
                            // hack
                            new CppFile(
                                "bzip2.cpp",
                                "!defined BOOST_IOSTREAMS_NO_BZIP2"),
                            // hack
                            new CppFile(
                                "zlib.cpp",
                                "!defined BOOST_IOSTREAMS_NO_ZLIB"),
                            new CppFile(
                                "gzip.cpp",
                                "!defined BOOST_IOSTREAMS_NO_ZLIB"),
                        }),
                }),
            new Library(
                name: "icu",
                compilationUnitList: new[]
                {
                    new CompilationUnit(
                        name: null,
                        fileList: new[]
                        {
                            new CppFile("icu", "defined BOOST_LOCALE_ICU"),
                        }),
                }),
            new Library(
                name: "mpi_python"),
            new Library(
                name: "serialization",
                compilationUnitList: new[]
                {
                    CompilationUnit.Cpp("basic_text_iprimitive"),
                    CompilationUnit.Cpp("basic_text_wiprimitive"),
                    //
                    CompilationUnit.Cpp("binary_iarchive"),
                    CompilationUnit.Cpp("binary_oarchive"),
                    CompilationUnit.Cpp("binary_wiarchive"),
                    CompilationUnit.Cpp("binary_woarchive"),
                    CompilationUnit.Cpp("polymorphic_iarchive"),
                    CompilationUnit.Cpp("polymorphic_oarchive"),
                    CompilationUnit.Cpp("text_iarchive"),
                    CompilationUnit.Cpp("text_oarchive"),
                    CompilationUnit.Cpp("text_wiarchive"),
                    CompilationUnit.Cpp("text_woarchive"),
                    CompilationUnit.Cpp("xml_iarchive"),
                    CompilationUnit.Cpp("xml_oarchive"),
                    CompilationUnit.Cpp("xml_wiarchive"),
                    CompilationUnit.Cpp("xml_woarchive"),
                }),
            new Library(
                name: "thread",
                compilationUnitList: new[]
                {
                    new CompilationUnit(
                        name: null,
                        fileList: new[]
                        {
                            new CppFile(
                                @"pthread",
                                "!defined BOOST_WINDOWS"),
                            new CppFile(
                                @"win32",
                                "defined BOOST_WINDOWS"),
                        }),
                }),
        };
         * */
    }
}
