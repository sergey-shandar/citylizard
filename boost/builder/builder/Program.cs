using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace builder
{
    class Program
    {
        static void Main(string[] args)
        {
            var boostPath = @"..\..\..\..\..\Downloads\boost-1_54_0\";
            foreach(var directory in System.IO.Directory.GetDirectories(boostPath))
            {
            }
        }
    }
}
