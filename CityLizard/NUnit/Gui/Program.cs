namespace CityLizard.NUnit.Gui
{
    using S = System;
    using N = global::NUnit.Gui;

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [S.STAThread]
        static void Main(string[] args)
        {
            N.AppEntry.Main(args);
        }
    }
}
