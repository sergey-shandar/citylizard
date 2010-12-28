using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityLizard.NUnit
{
    using N = global::NUnit.Framework;

    [N.TestFixture]
    public static class BuildTest
    {
        [N.Test]
        public static void SilverlightBuildTest()
        {
            Build.Build.BuildSolution("../../../TypedDom/SL.CityLizard.TypedDom.sln");
        }
    }
}
