using System;

namespace Game_V1._2
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            //using (Game1 game = new Game1())
            //{
            //    game.Run();
            //}
            Form1 form = new Form1();
            form.ShowDialog();
            //Test test1 = new Test();
            //test1.runTest();
        }
    }
#endif
}

