using System.Collections;
using static System.Console;
using static GI113_Final_Project.ConsoleUtilities;
using static GI113_Final_Project.DefinedColors;
using static GI113_Final_Project.MenuOptions;

namespace GI113_Final_Project;

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
                    //CursorVisible = true;
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

        public static void CreateBar(int currentValue, int maxValue, ConsoleColor[] colors, int barSections = 20)
        {
            double unitPerSection = (double)maxValue / barSections;
            int currentSections = Convert.ToInt32(Math.Ceiling(currentValue / unitPerSection));
            Write("║");
            BackgroundColor = colors[0];
            ForegroundColor = colors[1];
            for (int i = 0; i < currentSections; i++)
            {
                Write("█");
            }
            for (int i = 0; i < barSections - currentSections; i++)
            {
                Write("▒");
            }
            ResetColor();
            WriteLine("║ " + $"({currentValue}/{maxValue})");
        }
    }

class CombatSystem
    {
        private static ArrayList[] randomizedMonsterPool;
        private static ArrayList[] currentPlayerPool;
        private static int[] playerMaxHealth, monsterMaxHealth;
        private static int[] playerMaxMana;
        private static int currentPlayerTurn, currentMonsterTurn = 0;
        private static bool finishedTurn = false;
        public static void InitializeCombat(MonsterInfo.MonsterBaseStats[] monsterPool, PlayerInfo.PlayerBaseStats[] playerPool = null,
            int maxMonsterAmount = 4)
        {
            PlayerInfo.PlayerBaseStats[] temp = playerPool == null
                ? PlayerInfo.DefinedPlayerPool.defaultParty
                : playerPool;
            currentPlayerPool = new ArrayList[temp.Length];
            randomizedMonsterPool = new ArrayList[maxMonsterAmount];
            //Convert to player ArrayList 
            for (int i = 0; i < currentPlayerPool.Length; i++)
            {
                currentPlayerPool[i] = new ArrayList()
                    { temp[i].name, temp[i].hp, temp[i].atk, temp[i].def, temp[i].mp };
            }
            //Random and Convert to monster ArrayList 
            var rand = new Random();
            for (int i = 0; i < maxMonsterAmount; i++)
            {
                int random = rand.Next(0, monsterPool.Length);
                randomizedMonsterPool[i] = new ArrayList()
                {
                    monsterPool[random].name, monsterPool[random].hp, monsterPool[random].atk, monsterPool[random].def
                };
            }
            
            playerMaxHealth = new int[currentPlayerPool.Length];
            playerMaxMana = new int[currentPlayerPool.Length];
            monsterMaxHealth = new int[randomizedMonsterPool.Length];
            WriteLineWithSpeed("Player:");
            //scale player's stat
            for (int i = 0; i < currentPlayerPool.Length; i++)
            {
                for (int j = 1; j < 5; j++)
                {
                    currentPlayerPool[i][j] = Convert.ToInt32(PlayerInfo.PlayerScaleModifier.defaultScaleModifier[j - 1] 
                                                              * (int)currentPlayerPool[i][j]);
                }
                WriteLineWithSpeed(currentPlayerPool[i][0].ToString());
                playerMaxHealth[i] = (int)currentPlayerPool[i][1];
                playerMaxMana[i] = (int)currentPlayerPool[i][4];
            }
            Thread.Sleep(1000);
            Clear();
            WriteLineWithSpeed("VS");
            Thread.Sleep(1000);
            Clear();
            WriteLineWithSpeed("Enemy:");
            //scale monster's stat
            for (int i = 0; i < maxMonsterAmount; i++)
            {
                for (int j = 1; j < 4; j++)
                {
                    randomizedMonsterPool[i][j] = Convert.ToInt32(MonsterInfo.MonsterScaleModifier.defaultScaleModifier[j - 1] *
                                                                  (int)randomizedMonsterPool[i][j]);
                }
                WriteLineWithSpeed(randomizedMonsterPool[i][0].ToString());
                monsterMaxHealth[i] = (int)randomizedMonsterPool[i][1];
            }
            Thread.Sleep(1000);
            Clear();
        }

        public static bool StartCombat()
        {
            //Check if player is alive
            for (int i = 0; i < currentPlayerPool.Length; i++)
            {
                if ((int)currentPlayerPool[i][1] > 0)
                {
                    break;
                }
                WriteLineWithSpeed("ALL PLAYERS ARE DEAD");
                return false;
            }
            
            //check if monster is alive
            for (int i = 0; i < randomizedMonsterPool.Length; i++)
            {
                if ((int)randomizedMonsterPool[i][1] > 0)
                {
                    break;
                }
                WriteLineWithSpeed("ALL MONSTERS ARE DEAD");
                return true;
            }
            
            //Player's turn
            if ((int)currentPlayerPool[currentPlayerTurn][1] > 0)
            {
                finishedTurn = false;
                while (!finishedTurn)
                {
                    UpdatePlayerGUI();
                    NewEmptyLines(1);
                    WriteLineWithSpeed($"It's {currentPlayerPool[currentPlayerTurn][0]} turn!");
                    NewEmptyLines(1);
                    switch (CreateMenu(combatOptions, combatColors))
                    {
                        case 0:
                            //WriteLineWithSpeed("Preparing to attack");
                            PlayerAttack();
                            break;
                        case 1:
                            break;
                        case 2:
                            break;
                        case 3:
                            break;
                    }
                }
            }
            else
            {
                currentPlayerTurn = currentMonsterTurn == 3 ? 0 : currentPlayerTurn + 1;
            }

            return false;
        }

        private static void UpdatePlayerGUI()
        {
            WriteLine("═════════════════════════════════════");
            for (int i = 0; i < currentPlayerPool.Length; i++)
            {
                WriteLine($"{currentPlayerPool[i][0]}:");
                Write("HP: ");
                CreateBar((int)currentPlayerPool[i][1], playerMaxHealth[i], healthBarColors);
                WriteLine($"ATK: {currentPlayerPool[i][2]}");
                WriteLine($"DEF: {currentPlayerPool[i][3]}");
                Write("MP: ");
                CreateBar((int)currentPlayerPool[i][4], playerMaxMana[i], manaBarColors);
                WriteLine("═════════════════════════════════════");
            }
        }

        private static void UpdateMonsterGUI()
        {
            WriteLine("═════════════════════════════════════");
            for (int i = 0; i < randomizedMonsterPool.Length; i++)
            {
                WriteLine($"{randomizedMonsterPool[i][0]}:");
                Write("HP: ");
                CreateBar((int)randomizedMonsterPool[i][1], monsterMaxHealth[i], healthBarColors);
                WriteLine($"ATK: {randomizedMonsterPool[i][2]}");
                WriteLine($"DEF: {randomizedMonsterPool[i][3]}");
                WriteLine("═════════════════════════════════════");
            }
        }
        private static void PlayerAttack()
        {
            List<string> monsterOptions = new List<string>();
            //Fetch alive monster's name
            for (int i = 0; i < randomizedMonsterPool.Length; i++)
            {
                if ((int)randomizedMonsterPool[i][1] > 0)
                {
                    monsterOptions.Add(randomizedMonsterPool[i][0].ToString());
                }
            }
            monsterOptions.Add("Cancel");
            Clear();
            UpdateMonsterGUI();
            NewEmptyLines(1);
            WriteLineWithSpeed("Select Target to Attack");
            NewEmptyLines(1);
            int attackIndex = CreateMenu(monsterOptions.ToArray(), combatColors);
            if (attackIndex == monsterOptions.Count - 1)
            {
                Clear();
                return;
            }
            int damage = (int)currentPlayerPool[currentPlayerTurn][2] - (int)randomizedMonsterPool[attackIndex][3];
            randomizedMonsterPool[attackIndex][1] = (int)randomizedMonsterPool[attackIndex][1] - damage;
            Clear();
            WriteLineWithSpeed($"{currentPlayerPool[currentPlayerTurn][0]} Attack!");
            WriteLineWithSpeed($"Dealing {damage} damage!");
            Thread.Sleep(1000);
            Clear();
            UpdateMonsterGUI();
            Thread.Sleep(1000);
            finishedTurn = true;
        }
    }
    