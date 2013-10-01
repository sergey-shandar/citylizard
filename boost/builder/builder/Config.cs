using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace builder
{
    static class Config
    {
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
                name: "serialization",
                compilationUnitList: new[]
                {
                    new CompilationUnit(
                        name: "w",
                        fileList: new []
                        {
                            new CppFile("basic_text_wiprimitive.cpp")
                        }),
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
    }
}
