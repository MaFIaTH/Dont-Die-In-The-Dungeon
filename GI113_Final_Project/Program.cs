// See https://aka.ms/new-console-template for more information
using static System.Console;
using static GI113_Final_Project.ConsoleUtilities;
using static GI113_Final_Project.DefinedColors;
using static GI113_Final_Project.MenuOptions;
using static GI113_Final_Project.CombatSystem;
using static GI113_Final_Project.MonsterInfo;

namespace GI113_Final_Project
{
    class Program
    {
        static void Main(string[] args)
        {
            StartingMenu();
        }

        static void StartingMenu()
        {
            CursorVisible = false;
            WriteLineWithSpeed("Don't Die In The Dungeon", colors: titleColors);
            NewEmptyLines(1);
            switch (CreateMenu(startMenuOptions, startMenuColors))
            {
                case 0:
                    Clear();
                    WriteLineWithSpeed("Starting the game");
                    Thread.Sleep(1000);
                    Clear();
                    StartingCombat();
                    break;
                case 1:
                    Clear();
                    WriteLineWithSpeed("Exiting the game");
                    break;
            }
        }

        static void StartingCombat()
        {
            //WriteLine("Start COMBAT!!!");
            InitializeCombat(DefinedMonsterPool.oneToFive);
            if (!StartCombat())
            {
                return;
            }
        }
    }
}
