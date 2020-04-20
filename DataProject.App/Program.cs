using DataProject.App.Infrastructure;
using System;

namespace DataProject.App
{
    class Program
    {
        public SpListManager SpListManager { get; set; }

        public Program()
        {
            SpListManager = new SpListManager();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Sp ye aktarım başladı...");

            SpListManager.SpSaveAylikIseGirenler();
            SpListManager.SpSaveHaftalikIseGirenler();

            // yeni test

            Console.WriteLine("Sp ye aktarım tamamlandı");
        }
    }
}
