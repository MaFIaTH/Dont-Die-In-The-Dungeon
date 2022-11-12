// See https://aka.ms/new-console-template for more information
using System;
using static System.Console;
using static GI113_Final_Project.ConsoleUtilities;
using static GI113_Final_Project.DefiniedColor;
using static GI113_Final_Project.MenuOptions;

namespace GI113_Final_Project
{
    class DefiniedColor
    {
        public static ConsoleColor[] defaultColors = { ConsoleColor.DarkRed, ConsoleColor.Yellow };
        public static ConsoleColor[] titleColors = { ConsoleColor.White, ConsoleColor.Red };
        public static ConsoleColor[] combatColors = { ConsoleColor.DarkRed, ConsoleColor.Yellow };
        public static ConsoleColor[] healthBarColors = { ConsoleColor.White, ConsoleColor.DarkRed };
        public static ConsoleColor[] startMenuColors = { ConsoleColor.DarkRed, ConsoleColor.Yellow };
    }

    class MenuOptions
    {
        public static string[] combatOptions = {"Attack", "Defense", "Skill", "Item"};
        public static string[] startMenuOptions = { "Start", "Exit" };
    }

    class ConsoleUtilities
    {
        public static int CreateMenu(string[] menuOptions, ConsoleColor[] colors)
        {
            int menuIndex = 0;
            BackgroundColor = colors[0];
            ForegroundColor = colors[1];
            int topPosition = CursorTop;
            WriteLine(menuOptions[0]);
            ResetColor();
            for (int i = 1; i < menuOptions.Length; i++)
            {
                WriteLine(menuOptions[i]);
            }
            SetCursorPosition(0, topPosition);
            CursorVisible = false;
            while (true)
            {
                ConsoleKey key = ReadKey(true).Key;
                if (key is ConsoleKey.S or ConsoleKey.DownArrow && menuIndex < menuOptions.Length - 1)
                {
                    ResetColor();
                    SetCursorPosition(0, topPosition + menuIndex);
                    Write(menuOptions[menuIndex]);
                    menuIndex++;
                    BackgroundColor = colors[0];
                    ForegroundColor = colors[1];
                    SetCursorPosition(0, topPosition + menuIndex);
                    Write(menuOptions[menuIndex]);
                    SetCursorPosition(0, topPosition + menuIndex);
                }
                if (key is ConsoleKey.W or ConsoleKey.UpArrow && menuIndex > 0)
                {
                    ResetColor();
                    SetCursorPosition(0, topPosition + menuIndex);
                    Write(menuOptions[menuIndex]);
                    menuIndex--;
                    BackgroundColor = colors[0];
                    ForegroundColor = colors[1];
                    SetCursorPosition(0, topPosition + menuIndex);
                    Write(menuOptions[menuIndex]);
                    SetCursorPosition(0, topPosition + menuIndex);
                }
                if (key is ConsoleKey.E or ConsoleKey.Enter)
                {
                    ResetColor();
                    return menuIndex;
                }
                
            }
        }
        public static void WriteLineWithSpeed(string text, int milliSecond = 50, ConsoleColor[]? colors = null)
        {
            if (colors != null)
            {
                BackgroundColor = colors[0];
                ForegroundColor = colors[1];
            }
            foreach (char c in text)
            {
                Thread.Sleep(milliSecond);
                Write(c);
            }
            Write("\n");
        }

        public static void NewEmptyLines(int amount, bool isCursorAtBottom = true)
        {
            if (!isCursorAtBottom)
            {
                SetCursorPosition(0, CursorTop + 1);
            }

            for (int i = 0; i < amount; i++)
            {
                WriteLine();
            }
        }
    }

    class HealthBar
    {
        public int maxHp { get; }
        public int currentHp { get; set; }
        public int allHealthSections { get; }
        public ConsoleColor[] colors { get; }

        public HealthBar(int maxHp, int allHealthSections, ConsoleColor[] colors)
        {
            this.maxHp = maxHp;
            currentHp = maxHp;
            this.allHealthSections = allHealthSections;
            this.colors = colors;
        }
        public void DecreaseHp(int damage)
        {
            currentHp -= damage;
            if (currentHp < 0)
            {
                currentHp = 0;
            }
        }
        public void IncreaseHp(int heal)
        {
            currentHp += heal;
            if (currentHp > maxHp)
            {
                currentHp = maxHp;
            }
        }
        public void DisplayHealthBar(bool displayPercent = false)
        {
            int percentHp = Convert.ToInt32(Math.Ceiling((double)currentHp / maxHp * 100));
            int healthUnitPerSection = Convert.ToInt32(maxHp / allHealthSections);
            int currentHealthSections = Convert.ToInt32(Math.Ceiling((double)currentHp / healthUnitPerSection));
            Write("║");
            BackgroundColor = colors[0];
            ForegroundColor = colors[1];
            for (int i = 0; i < currentHealthSections; i++)
            {
                Write("█");
            }
            for (int i = 0; i < allHealthSections - currentHealthSections; i++)
            {
                Write("▒");
            }
            ResetColor();
            string statistic = displayPercent ? $"({percentHp}%)" : $"({currentHp}/{maxHp})";
            WriteLine("║ " + statistic);
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            StartingMenu();
        }

        static void StartingMenu()
        {
            WriteLineWithSpeed("Don't Die In The Dungeon", colors: titleColors);
            NewEmptyLines(2);
            switch (CreateMenu(startMenuOptions, startMenuColors))
            {
                case 0:
                    Clear();
                    WriteLineWithSpeed("Starting the game");
                    break;
                case 1:
                    Clear();
                    WriteLineWithSpeed("Exiting the game");
                    break;
            }
        }
    }
}
