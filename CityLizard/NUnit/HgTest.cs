namespace CityLizard.NUnit
{
    using N = global::NUnit.Framework;

    [N.TestFixture]
    class HgTest
    {
        [N.Test]
        public static void RootTest()
        {
            Hg.Hg.Root();
        }
    }
}
