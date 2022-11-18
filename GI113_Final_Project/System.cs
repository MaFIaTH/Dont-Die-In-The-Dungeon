using System.Collections;
using static System.Console;
using static GI113_Final_Project.ConsoleUtilities;
using static GI113_Final_Project.DefinedColors;
using static GI113_Final_Project.MenuOptions;
using static GI113_Final_Project.MonsterInfo;
using static GI113_Final_Project.BossInfo;
using static GI113_Final_Project.PlayerInfo;
using static GI113_Final_Project.ItemInfo;
using static GI113_Final_Project.SkillInfo;
using static GI113_Final_Project.Inventory;

namespace GI113_Final_Project
{
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
        public static void WriteLineWithSpeed(string text, int milliSecond = 35, ConsoleColor[] colors = null)
        {
            if (colors != null)
            {
                BackgroundColor = colors[0];
                ForegroundColor = colors[1];
            }
            foreach (char c in text)
            {
                int milli = KeyAvailable ? 0 : milliSecond;
                Write(c);
                Thread.Sleep(milli);
            }
            if (KeyAvailable)
            {
                ReadKey(true);
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
        private static BossSkillInfo.SkillBaseStats[] bossSkill;
        private static int[] playerMaxHealth, monsterMaxHealth;
        private static int[] playerMaxAtk;
        private static int[] playerMaxDef, monsterMaxDef;
        private static int[] playerMaxMana;
        private static int[] bossMaxGauge;
        private static int[] defendDuration;
        private static int[] defPotionDuration, atkPotionDuration;
        private static int[] parryDuration, encoreDuration;
        private static int[] roarDuration, kingDuration;
        private static int currentPlayerTurn, currentMonsterTurn;
        private static bool[] parrying, underEncore, defending, usedDefPotion, usedAtkPotion;
        private static bool[] underRoar, underKing;
        private static bool finishedTurn;
        private static bool isBossFight;

        /*
        Index Guide
        0 = Name
        1 = HP
        2 = ATK
        3 = DEF
        4 = MP (only for players)
        */

        //Public void
        public static void InitializeCombat(MonsterBaseStats[] monsterPool, int maxMonsterAmount = 4, 
            BossBaseStats boss = null, BossSkillInfo.SkillBaseStats[] bossSkill = null,
            PlayerBaseStats[] playerPool = null)
        {
            isBossFight = boss != null;
            PlayerBaseStats[] temp = playerPool == null
                ? DefinedPlayerPool.defaultParty
                : playerPool;
            CombatSystem.bossSkill = bossSkill == null ? BossSkillInfo.SkillBaseStats.leviathan : bossSkill;
            currentPlayerPool = new ArrayList[temp.Length];
            randomizedMonsterPool = new ArrayList[isBossFight ? 1 : maxMonsterAmount];
            //Convert to player ArrayList 
            for (int i = 0; i < currentPlayerPool.Length; i++)
            {
                currentPlayerPool[i] = new ArrayList()
                    { temp[i].name, temp[i].hp, temp[i].atk, temp[i].def, temp[i].mp };
            }

            if (!isBossFight)
            {
                //Random and Convert to monster ArrayList 
                var rand = new Random();
                for (int i = 0; i < maxMonsterAmount; i++)
                {
                    int random = rand.Next(0, monsterPool.Length);
                    randomizedMonsterPool[i] = new ArrayList()
                    {
                        monsterPool[random].name, monsterPool[random].hp, monsterPool[random].atk,
                        monsterPool[random].def
                    };
                }
            }
            else
            {
                randomizedMonsterPool[0] = new ArrayList()
                {
                    boss.name, boss.hp, boss.atk, boss.def, boss.gauge
                };
            }

            //Set size for array
            //Player Stats
            playerMaxHealth = new int[currentPlayerPool.Length];
            playerMaxAtk = new int[currentPlayerPool.Length];
            playerMaxDef = new int[currentPlayerPool.Length];
            playerMaxMana = new int[currentPlayerPool.Length];
            
            //Monster Stats
            monsterMaxHealth = new int[randomizedMonsterPool.Length];
            monsterMaxDef = new int[randomizedMonsterPool.Length];
            bossMaxGauge = new int[randomizedMonsterPool.Length];
            
            //Player Skill Boolean
            defending = new bool[currentPlayerPool.Length];
            parrying = new bool[currentPlayerPool.Length];
            underEncore = new bool[currentPlayerPool.Length];
            
            //Player Item Boolean
            usedAtkPotion = new bool[currentPlayerPool.Length];
            usedDefPotion = new bool[currentPlayerPool.Length];

            //Player Skills/Items Duration
            defendDuration = new int[currentPlayerPool.Length];
            defPotionDuration = new int[currentPlayerPool.Length];
            atkPotionDuration = new int[currentPlayerPool.Length];
            parryDuration = new int[currentPlayerPool.Length];
            encoreDuration = new int[currentPlayerPool.Length];
            
            //Enemy Skill Boolean
            underRoar = new bool[currentPlayerPool.Length];
            underKing = new bool[currentPlayerPool.Length];
            
            //Enemy Debuff Duration
            roarDuration = new int[currentPlayerPool.Length];
            kingDuration = new int[currentPlayerPool.Length];
            
            string fightTitle = isBossFight ? BigText.bossFight : BigText.monsterFight;
            WriteLine(fightTitle);
            Thread.Sleep(2000);
            Clear();
            WriteLineWithSpeed("Player:");
            //scale player's stat
            /*
            PlayerScaleModifier.defaultScaleModifier = new[]
            {
                (3.75 * Program.level + 34.58),  (3.75 * Program.level + 34.58),  (3.75 * Program.level + 34.58),
                (3.75 * Program.level + 34.58)
            };
            */
            for (int i = 0; i < currentPlayerPool.Length; i++)
            {
                for (int j = 1; j < 5; j++)
                {
                    currentPlayerPool[i][j] = Convert.ToInt32(PlayerScaleModifier.defaultScaleModifier[j - 1]
                                                              * (int)currentPlayerPool[i][j]);
                }

                WriteLineWithSpeed(currentPlayerPool[i][0].ToString());
                playerMaxHealth[i] = (int)currentPlayerPool[i][1];
                playerMaxAtk[i] = (int)currentPlayerPool[i][2];
                playerMaxDef[i] = (int)currentPlayerPool[i][3];
                playerMaxMana[i] = (int)currentPlayerPool[i][4];
            }

            Thread.Sleep(1000);
            Clear();
            WriteLine(BigText.vs);
            Thread.Sleep(1000);
            Clear();
            string monsterOrBoss = isBossFight ? "Boss" : "Monster";
            WriteLineWithSpeed($"{monsterOrBoss}:");

            //scale monster's stat
            /*
            MonsterScaleModifier.defaultScaleModifier = new[]
            {
                (3.75 * Program.level + 34.58),  (3.75 * Program.level + 34.58),  (3.75 * Program.level + 34.58),
                (3.75 * Program.level + 34.58)
            };
            */
            if (!isBossFight)
            {
                for (int i = 0; i < maxMonsterAmount; i++)
                {
                    for (int j = 1; j < 4; j++)
                    {
                        randomizedMonsterPool[i][j] = Convert.ToInt32(MonsterScaleModifier.defaultScaleModifier[j - 1]
                                                                      * (int)randomizedMonsterPool[i][j]);
                    }

                    WriteLineWithSpeed(randomizedMonsterPool[i][0].ToString());
                    monsterMaxHealth[i] = (int)randomizedMonsterPool[i][1];
                    monsterMaxDef[i] = (int)randomizedMonsterPool[i][3];
                }
            }
            else
            {
                WriteLineWithSpeed(randomizedMonsterPool[0][0].ToString());
                monsterMaxHealth[0] = (int)randomizedMonsterPool[0][1];
                monsterMaxDef[0] = (int)randomizedMonsterPool[0][3];
                bossMaxGauge[0] = (int)randomizedMonsterPool[0][4];
                randomizedMonsterPool[0][4] = 0;
            }

            Thread.Sleep(1000);
            Clear();
        }

        public static bool StartCombat()
        {
            while (true)
            {
                if (!PlayerCheck())
                {

                    return false;
                }

                if (!MonsterCheck())
                {
                    Program.level++;
                    return true;
                }

                ResetStats();

                //Regen MP
                for (int i = 0; i < currentPlayerPool.Length; i++)
                {
                    if ((int)currentPlayerPool[i][1] > 0)
                    {
                        currentPlayerPool[i][4] = (int)currentPlayerPool[i][4] + 20;
                        currentPlayerPool[i][4] = (int)currentPlayerPool[i][4] > playerMaxMana[i]
                            ? playerMaxMana[i]
                            : (int)currentPlayerPool[i][4];
                    }
                }

                //Charge Gauge for boss
                if (isBossFight)
                {
                    randomizedMonsterPool[0][4] = (int)randomizedMonsterPool[0][4] + 1;
                }

                //Turns
                PlayerTurn();
                if (!MonsterCheck())
                {
                    return true;
                }

                if (!isBossFight)
                {
                    MonsterTurn();
                }
                else
                {
                    BossTurn();
                }
            }
        }

        //Status Check
        private static bool PlayerCheck()
        {
            //Check if player is alive
            for (int i = 0; i < currentPlayerPool.Length; i++)
            {
                if ((int)currentPlayerPool[i][1] > 0)
                {
                    return true;
                }
            }

            WriteLineWithSpeed("ALL PLAYERS ARE DEAD");
            Thread.Sleep(1000);
            return false;
        }

        private static bool MonsterCheck()
        {
            //check if monster is alive
            for (int i = 0; i < randomizedMonsterPool.Length; i++)
            {
                if ((int)randomizedMonsterPool[i][1] > 0)
                {
                    return true;
                }
            }

            Thread.Sleep(1000);
            WriteLineWithSpeed("ALL MONSTERS ARE DEAD");
            return false;
        }

        private static void ResetStats()
        {
            //Reset Stat
            for (int i = 0; i < currentPlayerPool.Length; i++)
            {
                //Reset Defense Stance
                if (defendDuration[i] <= 1 && defending[i])
                {
                    defendDuration[i] -= 1;
                    currentPlayerPool[i][3] = Convert.ToInt32((int)currentPlayerPool[i][3] / 1.5);
                    defending[i] = false;
                    WriteLineWithSpeed($"{currentPlayerPool[i][0]} dropped the defending stance...");
                }
                else if (defendDuration[i] > 1)
                {
                    defendDuration[i] -= 1;
                    WriteLineWithSpeed(
                        $"{currentPlayerPool[i][0]}'s defending stance remains {defendDuration[i]} turns...");
                }


                //Reset DEF potion effect
                if (defPotionDuration[i] <= 1 && usedDefPotion[i])
                {
                    defPotionDuration[i] -= 1;
                    currentPlayerPool[i][3] = (int)currentPlayerPool[i][3] - Convert.ToInt32(playerMaxDef[i]
                        * (ItemStats.defPotion.defValue / 100.0));
                    usedDefPotion[i] = false;
                    WriteLineWithSpeed($"{currentPlayerPool[i][0]}'s DEF potion effect ran out...");
                }
                else if (defPotionDuration[i] > 1)
                {
                    defPotionDuration[i] -= 1;
                    WriteLineWithSpeed(
                        $"{currentPlayerPool[i][0]}'s DEF potion effect remains {defPotionDuration[i]} turns...");
                }


                //Reset ATK potion effect
                if (atkPotionDuration[i] <= 1 && usedAtkPotion[i])
                {
                    atkPotionDuration[i] -= 1;
                    currentPlayerPool[i][2] = (int)currentPlayerPool[i][2] - Convert.ToInt32(playerMaxAtk[i]
                        * (ItemStats.defPotion.atkValue / 100.0));
                    usedAtkPotion[i] = false;
                    WriteLineWithSpeed($"{currentPlayerPool[i][0]}'s ATK potion effect ran out...");
                }
                else if (atkPotionDuration[i] > 1)
                {
                    atkPotionDuration[i] -= 1;
                    WriteLineWithSpeed(
                        $"{currentPlayerPool[i][0]}'s ATK potion effect remains {atkPotionDuration[i]} turns...");
                }


                //Reset Parry Skill
                if (parryDuration[i] <= 1 && parrying[i])
                {
                    parryDuration[i] -= 1;
                    parrying[i] = false;
                    WriteLineWithSpeed($"{currentPlayerPool[i][0]} dropped the parrying stance...");
                }
                else if (parryDuration[i] > 1)
                {
                    parryDuration[i] -= 1;
                    WriteLineWithSpeed(
                        $"{currentPlayerPool[i][0]}'s parrying stance remains {parryDuration[i]} turns...");
                }

                //Reset Encore Skill
                if (encoreDuration[i] <= 1 && underEncore[i])
                {
                    encoreDuration[i] -= 1;
                    currentPlayerPool[i][2] = (int)currentPlayerPool[i][2] - SkillBaseStats.encore.atkValue;
                    underEncore[i] = false;
                    WriteLineWithSpeed($"{currentPlayerPool[i][0]}'s Encore effect ran out...");
                }
                else if (encoreDuration[i] > 1)
                {
                    encoreDuration[i] -= 1;
                    WriteLineWithSpeed(
                        $"{currentPlayerPool[i][0]}'s Encore effect remains {encoreDuration[i]} turns...");
                }
                
                //Reset Roar Skill
                if (roarDuration[i] <= 1 && underRoar[i])
                {
                    roarDuration[i] -= 1;
                    currentPlayerPool[i][2] = (int)currentPlayerPool[i][2] + BossSkillInfo.SkillBaseStats.roar.atkValue;
                    underRoar[i] = false;
                    WriteLineWithSpeed($"{currentPlayerPool[i][0]}'s Roar of The Ocean effect ran out!");
                }
                else if (roarDuration[i] > 1)
                {
                    roarDuration[i] -= 1;
                    WriteLineWithSpeed(
                        $"{currentPlayerPool[i][0]}'s Roar of The Ocean effect remains {roarDuration[i]} turns...");
                }
                
                //Reset King Skill
                if (kingDuration[i] <= 1 && underKing[i])
                {
                    kingDuration[i] -= 1;
                    currentPlayerPool[i][3] = (int)currentPlayerPool[i][3] + BossSkillInfo.SkillBaseStats.king.defValue;
                    underKing[i] = false;
                    WriteLineWithSpeed($"{currentPlayerPool[i][0]}'s King of The Sea effect ran out!");
                }
                else if (kingDuration[i] > 1)
                {
                    kingDuration[i] -= 1;
                    WriteLineWithSpeed(
                        $"{currentPlayerPool[i][0]}'s King of The Sea effect remains {kingDuration[i]} turns...");
                }
            }

            NewEmptyLines(1);
        }

        //GUI
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
                if (isBossFight)
                {
                    WriteLine($"Skill Gauge: ");
                    CreateBar((int)randomizedMonsterPool[i][4], bossMaxGauge[i], manaBarColors);
                }
                WriteLine("═════════════════════════════════════");
            }

            if (isBossFight)
            {
                for (int i = 0; i < bossSkill.Length; i++)
                {
                    WriteLine($"{bossSkill[i].name}:");
                    WriteLine($"Description: {bossSkill[i].description}");
                    WriteLine($"Deal damage: {bossSkill[i].hpValue}");
                    if (bossSkill[i].debuff)
                    {
                        WriteLine($"Reduce ATK: {bossSkill[i].atkValue}");
                        WriteLine($"Reduce DEF: {bossSkill[i].defValue}");
                    }
                    string aoe = bossSkill[i].aoe
                        ? "Yes"
                        : "No";
                    WriteLine($"AOE: {aoe}");
                    WriteLine("═════════════════════════════════════");
                }
            }
        }

        private static void UpdateSkillGUI()
        {
            WriteLine("═════════════════════════════════════");
            for (int i = 0; i < DefinedSkillPool.defaultSkillPool[currentPlayerTurn].Length; i++)
            {
                SkillBaseStats skill = DefinedSkillPool.defaultSkillPool[currentPlayerTurn][i];
                WriteLine($"{skill.name}:");
                WriteLine($"Description: {skill.description}");
                WriteLine($"MP Cost: {skill.mpCost}");
                if (skill.percentageKill > 0)
                {
                    WriteLine($"Instant kill if HP under: <{skill.percentageKill}%");
                }

                string healOrHurt = skill.offensive
                    ? "Deal damage"
                    : "Heal";
                WriteLine($"{healOrHurt}: {skill.hpValue}");
                string harmOrBoost = skill.offensive
                    ? "Reduce enemy's"
                    : "Boost player's";
                WriteLine($"{harmOrBoost} ATK: {skill.atkValue}");
                WriteLine($"{harmOrBoost} DEF: {skill.defValue}");
                string aoe = skill.aoe
                    ? "Yes"
                    : "No";
                WriteLine($"AOE: {aoe}");
                WriteLine("═════════════════════════════════════");
            }
        }

        private static void UpdateItemGUI()
        {
            WriteLine("═════════════════════════════════════");
            for (int i = 0; i < ItemInventory.currentInventory.Length; i++)
            {
                WriteLine(
                    $"{ItemInventory.currentInventory[i].item.name} (x{ItemInventory.currentInventory[i].amount}):");
                WriteLine($"Description: {ItemInventory.currentInventory[i].item.description}");
                WriteLine($"HP: +{ItemInventory.currentInventory[i].item.hpValue}%");
                WriteLine($"ATK: +{ItemInventory.currentInventory[i].item.atkValue}%");
                WriteLine($"DEF: +{ItemInventory.currentInventory[i].item.defValue}%");
                WriteLine($"MP: +{ItemInventory.currentInventory[i].item.mpValue}%");
                string revive = ItemInventory.currentInventory[i].item.revive ? "Yes" : "No";
                WriteLine($"Revive: {revive}");
                WriteLine("═════════════════════════════════════");
            }
        }

        //Fetching
        private static List<string> FetchMonster(bool fetchHp, bool fetchDead = true)
        {
            List<string> monsterOptions = new List<string>();
            //Fetch alive monster's name
            for (int i = 0; i < randomizedMonsterPool.Length; i++)
            {
                if ((int)randomizedMonsterPool[i][1] > 0 || fetchDead)
                {
                    if (fetchHp)
                    {
                        monsterOptions.Add(randomizedMonsterPool[i][1].ToString());
                    }
                    else
                    {
                        monsterOptions.Add(randomizedMonsterPool[i][0].ToString());
                    }

                }
            }

            return monsterOptions;
        }

        private static List<string> FetchPlayer(bool fetchHp, bool fetchDead = true)
        {
            List<string> playerOptions = new List<string>();

            //Fetch alive player's name
            for (int i = 0; i < currentPlayerPool.Length; i++)
            {
                if ((int)currentPlayerPool[i][1] > 0 || fetchDead)
                {
                    if (fetchHp)
                    {
                        playerOptions.Add(currentPlayerPool[i][1].ToString());
                    }
                    else
                    {
                        playerOptions.Add(currentPlayerPool[i][0].ToString());
                    }
                }
            }

            return playerOptions;
        }

        private static List<string> FetchItem(bool fetchAmount, bool fetchEmpty = true)
        {
            List<string> itemOptions = new List<string>();

            //Fetch Items
            for (int i = 0; i < ItemInventory.currentInventory.Length; i++)
            {
                if (ItemInventory.currentInventory[i].amount > 0 || fetchEmpty)
                {
                    if (fetchAmount)
                    {
                        itemOptions.Add(ItemInventory.currentInventory[i].amount.ToString());
                    }
                    else
                    {
                        itemOptions.Add(ItemInventory.currentInventory[i].item.name);
                    }
                }
            }

            return itemOptions;
        }

        private static List<string> FetchSkill(bool fetchMp)
        {
            List<string> skillOptions = new List<string>();
            for (int i = 0; i < DefinedSkillPool.defaultSkillPool[currentPlayerTurn].Length; i++)
            {
                if (fetchMp)
                {
                    skillOptions.Add(DefinedSkillPool.defaultSkillPool[currentPlayerTurn][i].mpCost.ToString());
                }
                else
                {
                    skillOptions.Add(DefinedSkillPool.defaultSkillPool[currentPlayerTurn][i].name);
                }
            }

            return skillOptions;
        }

        //Turn
        private static void PlayerTurn()
        {
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
                            PlayerAttack();
                            break;
                        case 1:
                            PlayerDefend();
                            break;
                        case 2:
                            PlayerSkill();
                            break;
                        case 3:
                            PlayerItem();
                            break;
                    }
                }
            }
            else
            {
                currentPlayerTurn = currentPlayerTurn == currentPlayerPool.Length - 1 ? 0 : currentPlayerTurn + 1;
                PlayerTurn();
            }
        }
        private static void MonsterTurn()
        {
            //Monster's turn
            if ((int)randomizedMonsterPool[currentMonsterTurn][1] > 0)
            {
                Clear();
                UpdateMonsterGUI();
                NewEmptyLines(1);
                WriteLineWithSpeed($"It's {randomizedMonsterPool[currentMonsterTurn][0]} turn!");
                NewEmptyLines(1);
                Thread.Sleep(1000);
                MonsterAttack();
            }
            else
            {
                currentMonsterTurn = currentMonsterTurn == randomizedMonsterPool.Length - 1
                    ? 0
                    : currentMonsterTurn + 1;
                MonsterTurn();
            }
        }
        private static void BossTurn()
        {
            Clear();
            UpdateMonsterGUI();
            NewEmptyLines(1);
            WriteLineWithSpeed($"It's {randomizedMonsterPool[currentMonsterTurn][0]} turn!");
            NewEmptyLines(1);
            Thread.Sleep(1000);
            if ((int)randomizedMonsterPool[0][4] >= bossMaxGauge[0])
            {
                BossSkill();
            }
            else
            {
                MonsterAttack();
            }
        }
    

        //Player Actions
        private static void PlayerAttack()
        {
            var rand = new Random();
            int crit = rand.Next(1, 101) < 15 ? 1 : 2;
            string critName = crit == 1 ? "Normal" : "Critical";
            //Fetch all monsters' hp
            List<string> monsterHp = FetchMonster(true);
            List<int> availableIndex = new List<int>();
            
            //Select only the ones that are alive
            for (int i = 0; i < monsterHp.Count; i++)
            {
                if (Convert.ToInt32(monsterHp[i]) > 0)
                {
                    availableIndex.Add(i);
                }
            }
            List<string> monsterOptions = new List<string>();
            
            //Add available monster to the menu list
            foreach (int i in availableIndex)
            {
                monsterOptions.Add(randomizedMonsterPool[i][0].ToString());
            }
            monsterOptions.Add("Cancel"); //Add cancel option.
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
            
            //Calculate damage
            int damage = (Convert.ToInt32((int)currentPlayerPool[currentPlayerTurn][2] * 
                                         WeaponInventory.weaponSets[currentPlayerTurn].baseModifier * crit)) - 
                         (int)randomizedMonsterPool[availableIndex[attackIndex]][3];
            damage = damage <= 0 ? 0 : damage; //set to 0 if less than 0
            
            //Apply damage
            randomizedMonsterPool[availableIndex[attackIndex]][1] = 
                (int)randomizedMonsterPool[availableIndex[attackIndex]][1] - damage;
            
            //Set to 0 if less than 0
            randomizedMonsterPool[availableIndex[attackIndex]][1] = 
                (int)randomizedMonsterPool[availableIndex[attackIndex]][1] < 0
                ? 0
                : (int)randomizedMonsterPool[availableIndex[attackIndex]][1];
            
            Clear();
            WriteLineWithSpeed($"{currentPlayerPool[currentPlayerTurn][0]} Attack!");
            WriteLineWithSpeed($"Dealing {critName} {damage} damage!");
            Thread.Sleep(1000);
            Clear();
            UpdateMonsterGUI();
            Thread.Sleep(1000);
            currentPlayerTurn = currentPlayerTurn == currentPlayerPool.Length - 1 ? 0 : currentPlayerTurn + 1;
            finishedTurn = true;
        }
        private static void PlayerDefend()
        {
            string[] options = new[] { "Confirm", "Cancel" };
            Clear();
            UpdatePlayerGUI();
            NewEmptyLines(1);
            WriteLineWithSpeed("Would you like to continue?");
            NewEmptyLines(1);
            int attackIndex = CreateMenu(options, combatColors);
            if (attackIndex == options.Length - 1)
            {
                Clear();
                return;
            }
            
            //Calculate Defense
            int addDef = Convert.ToInt32((int)currentPlayerPool[currentPlayerTurn][3] * 1.5);
            
            //Apply Defense
            currentPlayerPool[currentPlayerTurn][3] = addDef;
            
            //Set Duration
            defendDuration[currentPlayerTurn] = 1;
            
            Clear();
            WriteLineWithSpeed($"{currentPlayerPool[currentPlayerTurn][0]} defend themselves!");
            WriteLineWithSpeed($"Defense rose to {currentPlayerPool[currentPlayerTurn][3]}!");
            Thread.Sleep(1000);
            Clear();
            UpdatePlayerGUI();
            Thread.Sleep(2000);
            defending[currentPlayerTurn] = true;
            currentPlayerTurn = currentPlayerTurn == currentPlayerPool.Length - 1 ? 0 : currentPlayerTurn + 1;
            finishedTurn = true;
        }
        private static void PlayerSkill()
        {
            bool finishedSelecting = false;
            List<string> skillAmount = FetchSkill(true);
            List<int> availableIndex = new List<int>();
            
            //Select only the ones that remains
            for (int i = 0; i < skillAmount.Count; i++)
            {
                if ((int)currentPlayerPool[currentPlayerTurn][4] >= Convert.ToInt32(skillAmount[i]))
                {
                    availableIndex.Add(i);
                }
            }
            
            //Add available items to the menu list
            List<string> skillOptions = new List<string>();
            foreach (int i in availableIndex)
            {
                skillOptions.Add(DefinedSkillPool.defaultSkillPool[currentPlayerTurn][i].name);
            }
            skillOptions.Add("Cancel"); //Add cancel option.
            while (!finishedSelecting)
            {
                Clear();
                UpdateSkillGUI();
                NewEmptyLines(1);
                WriteLineWithSpeed("Select Skill to Use");
                NewEmptyLines(1);
                int skillIndex = CreateMenu(skillOptions.ToArray(), combatColors);
                if (skillIndex == skillOptions.Count - 1)
                {
                    Clear();
                    return;
                }
                finishedSelecting = UseSkill(availableIndex[skillIndex]);
            }
        }
        private static bool UseSkill(int skillIndex)
        {
            SkillBaseStats selectedSkill = DefinedSkillPool.defaultSkillPool[currentPlayerTurn][skillIndex];
            List<string> options = new List<string>();
            
            if (selectedSkill.offensive)
            {
                //Parry
                if (selectedSkill.parry)
                {
                    options = new List<string>() { "Confirm", "Cancel" };
                    Clear();
                    WriteLineWithSpeed("Would you like to continue?");
                    NewEmptyLines(1);
                    int index = CreateMenu(options.ToArray(), combatColors);
                    if (index == options.Count - 1)
                    {
                        Clear();
                        return false;
                    }
                    
                    int turn = 1;
                    Clear();
                    WriteLineWithSpeed($"{currentPlayerPool[currentPlayerTurn][0]} used {selectedSkill.name}!");
                    WriteLineWithSpeed($"Parry stance will last for {turn} turn...");
                    parryDuration[currentPlayerTurn] += turn;
                    parrying[currentPlayerTurn] = true;
                    Thread.Sleep(1000);
                    Clear();
                    currentPlayerPool[currentPlayerTurn][4] =
                        (int)currentPlayerPool[currentPlayerTurn][4] - selectedSkill.mpCost;
                    currentPlayerTurn = currentPlayerTurn == currentPlayerPool.Length - 1 ? 0 : currentPlayerTurn + 1;
                    finishedTurn = true;
                    return true;
                }
                
                //Fetch all monsters' HP
                List<string> monsterHp = FetchMonster(true);
                List<int> availableIndex = new List<int>();
            
                //Select only the ones that are alive
                for (int i = 0; i < monsterHp.Count; i++)
                {
                    if (Convert.ToInt32(monsterHp[i]) > 0)
                    {
                        availableIndex.Add(i);
                    }
                }
                
                //Offensive AOE
                if (selectedSkill.aoe)
                {
                    options = new List<string>() { "Confirm", "Cancel" };
                    Clear();
                    WriteLineWithSpeed("Would you like to continue?");
                    NewEmptyLines(1);
                    int index = CreateMenu(options.ToArray(), combatColors);
                    if (index == options.Count - 1)
                    {
                        Clear();
                        return false;
                    }
                    int damage = 0;
                    for (int i = 0; i < availableIndex.Count; i++)
                    {
                        damage = selectedSkill.hpValue / availableIndex.Count;
                        randomizedMonsterPool[availableIndex[i]][1] =
                            (int)randomizedMonsterPool[availableIndex[i]][1] - damage;
                        randomizedMonsterPool[availableIndex[i]][1] =
                            (int)randomizedMonsterPool[availableIndex[i]][1] <= 0
                                ? 0
                                : (int)randomizedMonsterPool[availableIndex[i]][1];
                    }
                    Clear();
                    WriteLineWithSpeed($"{currentPlayerPool[currentPlayerTurn][0]} used {selectedSkill.name}!");
                    WriteLineWithSpeed($"Dealt {damage} damage across all enemies!");
                    Thread.Sleep(1000);
                    Clear();
                    UpdateMonsterGUI();
                    Thread.Sleep(1000);
                    Clear();
                    currentPlayerPool[currentPlayerTurn][4] =
                        (int)currentPlayerPool[currentPlayerTurn][4] - selectedSkill.mpCost;
                    currentPlayerTurn = currentPlayerTurn == currentPlayerPool.Length - 1 ? 0 : currentPlayerTurn + 1;
                    finishedTurn = true;
                    return true;
                }
                
                //Add available monster to the menu list
                foreach (int i in availableIndex)
                {
                    options.Add(randomizedMonsterPool[i][0].ToString());
                }

                options.Add("Cancel");
                Clear();
                UpdateMonsterGUI();
                NewEmptyLines(1);
                WriteLineWithSpeed("Select Target");
                NewEmptyLines(1);
                int monsterIndex = CreateMenu(options.ToArray(), combatColors);
                if (monsterIndex == options.Count - 1)
                {
                    Clear();
                    return false;
                }
                Clear();
                
                //Deal Damage
                if (selectedSkill.hpValue > 0)
                {
                    if (selectedSkill.percentageKill > 0 &&
                        Convert.ToDouble((int)randomizedMonsterPool[availableIndex[monsterIndex]][1] 
                                         / monsterMaxHealth[availableIndex[monsterIndex]]) < 0.25)
                    {
                        randomizedMonsterPool[availableIndex[monsterIndex]][1] = 0;
                        Clear();
                        WriteLineWithSpeed($"{currentPlayerPool[currentPlayerTurn][0]} used {selectedSkill.name}!");
                        WriteLineWithSpeed($"Clean Kill activated, monster is dead!");
                    }
                    else
                    {
                        int damage;
                        damage = selectedSkill.hpValue;
                        randomizedMonsterPool[availableIndex[monsterIndex]][1] =
                            (int)randomizedMonsterPool[availableIndex[monsterIndex]][1] - damage;
                        randomizedMonsterPool[availableIndex[monsterIndex]][1] =
                            (int)randomizedMonsterPool[availableIndex[monsterIndex]][1] <= 0
                                ? 0
                                : (int)randomizedMonsterPool[availableIndex[monsterIndex]][1];
                        Clear();
                        WriteLineWithSpeed($"{currentPlayerPool[currentPlayerTurn][0]} used {selectedSkill.name}!");
                        WriteLineWithSpeed($"Dealt {damage} damage!");
                    }
                    Thread.Sleep(1000);
                    Clear();
                    UpdateMonsterGUI();
                    Thread.Sleep(1000);
                    Clear();
                    currentPlayerPool[currentPlayerTurn][4] =
                        (int)currentPlayerPool[currentPlayerTurn][4] - selectedSkill.mpCost;
                    currentPlayerTurn = currentPlayerTurn == currentPlayerPool.Length - 1 ? 0 : currentPlayerTurn + 1;
                    finishedTurn = true;
                    return true;
                }
            }
            else
            {
                //Fetch all players' hp
                List<string> playerHp = FetchPlayer(true);
                List<int> availableIndex = new List<int>();
                
                //Select only the ones that are alive
                for (int i = 0; i < playerHp.Count; i++)
                {
                    if (Convert.ToInt32(playerHp[i]) > 0)
                    {
                        availableIndex.Add(i);
                    }
                }

                //Inoffensive AOE
                if (selectedSkill.aoe)
                {
                    options = new List<string>() { "Confirm", "Cancel" };
                    Clear();
                    WriteLineWithSpeed("Would you like to continue?");
                    NewEmptyLines(1);
                    int index = CreateMenu(options.ToArray(), combatColors);
                    if (index == options.Count - 1)
                    {
                        Clear();
                        return false;
                    }
                    
                    //Heal
                    if (selectedSkill.hpValue > 0)
                    {
                        int heal = 0;
                        for (int i = 0; i < currentPlayerPool.Length; i++)
                        {
                            if ((int)currentPlayerPool[i][1] > 0)
                            {
                                heal = selectedSkill.hpValue;
                                currentPlayerPool[i][1] = (int)currentPlayerPool[i][1] + heal;
                                currentPlayerPool[i][1] = (int)currentPlayerPool[i][1] > playerMaxHealth[i]
                                    ? playerMaxHealth[i]
                                    : (int)currentPlayerPool[i][1];
                            }
                        }
                        Clear();
                        WriteLineWithSpeed($"{currentPlayerPool[currentPlayerTurn][0]} used {selectedSkill.name}!");
                        WriteLineWithSpeed($"Healed {heal} HP across the party!");
                    }
                    
                    //Boost ATK (Encore)
                    if (selectedSkill.atkValue > 0)
                    {
                        int boost = 0;
                        for (int i = 0; i < currentPlayerPool.Length; i++)
                        {
                            if ((int)currentPlayerPool[i][2] > 0)
                            {
                                boost = selectedSkill.atkValue;
                                currentPlayerPool[i][2] = (int)currentPlayerPool[i][2] + boost;
                                underEncore[i] = true;
                                encoreDuration[i] += 10;
                            }
                        }
                        Clear();
                        WriteLineWithSpeed($"{currentPlayerPool[currentPlayerTurn][0]} used {selectedSkill.name}!");
                        WriteLineWithSpeed($"Boosted ATK by {boost} points across the party!");
                    }
                    Thread.Sleep(1000);
                    Clear();
                    UpdatePlayerGUI();
                    Thread.Sleep(1000);
                    Clear();
                    currentPlayerPool[currentPlayerTurn][4] =
                        (int)currentPlayerPool[currentPlayerTurn][4] - selectedSkill.mpCost;
                    currentPlayerTurn = currentPlayerTurn == currentPlayerPool.Length - 1 ? 0 : currentPlayerTurn + 1;
                    finishedTurn = true;
                    return true;
                }
                //Add available player to the menu list
                foreach (int i in availableIndex)
                {
                    options.Add(currentPlayerPool[i][0].ToString());
                }
            }

            return false;
        }
        private static void PlayerItem()
        {
            bool finishedSelecting = false;
            //Fetch all items' amount
            List<string> itemAmount = FetchItem(true);
            List<int> availableIndex = new List<int>();
            
            //Select only the ones that remains
            for (int i = 0; i < itemAmount.Count; i++)
            {
                if (Convert.ToInt32(itemAmount[i]) > 0)
                {
                    availableIndex.Add(i);
                }
            }
            
            //Add available items to the menu list
            List<string> itemOptions = new List<string>();
            foreach (int i in availableIndex)
            {
                itemOptions.Add(ItemInventory.currentInventory[i].item.name);
            }
            itemOptions.Add("Cancel"); //Add cancel option.
            while (!finishedSelecting)
            {
                Clear();
                UpdateItemGUI();
                NewEmptyLines(1);
                WriteLineWithSpeed("Select Item to Use");
                NewEmptyLines(1);
                int itemIndex = CreateMenu(itemOptions.ToArray(), combatColors);
                if (itemIndex == itemOptions.Count - 1)
                {
                    Clear();
                    return;
                }
                finishedSelecting = UseItem(availableIndex[itemIndex]);
            }
        }
        private static bool UseItem(int itemIndex)
        {
            //bool finishedUsing = false;
            List<string> playerOptions = FetchPlayer(false);
            playerOptions.Add("Cancel");
            Clear();
            UpdatePlayerGUI();
            NewEmptyLines(1);
            WriteLineWithSpeed("Select Player to Use Item");
            NewEmptyLines(1);
            int playerIndex = CreateMenu(playerOptions.ToArray(), combatColors);
            if (playerIndex == playerOptions.Count - 1)
            {
                Clear();
                return false;
            }
            
            Clear();
            
            //Check item condition
            if ((int)currentPlayerPool[playerIndex][1] > 0 && ItemInventory.currentInventory[itemIndex].item.revive)
            {
                WriteLineWithSpeed("Cannot use revive item with alive member");
                Thread.Sleep(1000);
                return false;
            }
            if ((int)currentPlayerPool[playerIndex][1] <= 0 && !ItemInventory.currentInventory[itemIndex].item.revive)
            {
                WriteLineWithSpeed("Cannot use this item with fallen member");
                Thread.Sleep(1000);
                return false;
            }
            if (usedAtkPotion[playerIndex] || usedDefPotion[playerIndex])
            {
                WriteLineWithSpeed("You are already under effect of potion");
                Thread.Sleep(1000);
                return false;
            }

            //Revive
            if (ItemInventory.currentInventory[itemIndex].item.revive)
            {
                WriteLineWithSpeed($"{currentPlayerPool[playerIndex][0]} has been revived!");
            }
            
            //Heal
            if (ItemInventory.currentInventory[itemIndex].item.hpValue > 0)
            {
                int heal = Convert.ToInt32(playerMaxHealth[playerIndex]
                    * (ItemInventory.currentInventory[itemIndex].item.hpValue / 100.0));
                currentPlayerPool[playerIndex][1] = (int)currentPlayerPool[playerIndex][1] + heal;
                currentPlayerPool[playerIndex][1] =
                    (int)currentPlayerPool[playerIndex][1] > playerMaxHealth[playerIndex]
                        ? playerMaxHealth[playerIndex]
                        : (int)currentPlayerPool[playerIndex][1];
                WriteLineWithSpeed($"Healed {heal} HP!");
            }
            
            //Boost ATK
            if (ItemInventory.currentInventory[itemIndex].item.atkValue > 0)
            {
                int atkBoost = Convert.ToInt32(playerMaxAtk[playerIndex]
                                               * (ItemInventory.currentInventory[itemIndex].item.atkValue / 100.0));
                currentPlayerPool[playerIndex][2] = (int)currentPlayerPool[playerIndex][2] + atkBoost;
                WriteLineWithSpeed($"Boosted ATK {atkBoost} points, now {currentPlayerPool[playerIndex][2]}!");
                atkPotionDuration[playerIndex] += 10;
                usedAtkPotion[playerIndex] = true;
            }
            
            //Boost DEF
            if (ItemInventory.currentInventory[itemIndex].item.defValue > 0)
            {
                int defBoost = Convert.ToInt32(playerMaxDef[playerIndex]
                                               * (ItemInventory.currentInventory[itemIndex].item.defValue / 100.0));
                currentPlayerPool[playerIndex][3] = (int)currentPlayerPool[playerIndex][3] + defBoost;
                WriteLineWithSpeed($"Boosted DEF {defBoost} points, now {currentPlayerPool[playerIndex][3]}!");
                defPotionDuration[playerIndex] += 10;
                usedDefPotion[playerIndex] = true;
            }
            
            //Recover MP
            if (ItemInventory.currentInventory[itemIndex].item.mpValue > 0)
            {
                int recover = Convert.ToInt32(playerMaxMana[playerIndex]
                                              * (ItemInventory.currentInventory[itemIndex].item.mpValue / 100.0));
                currentPlayerPool[playerIndex][4] = (int)currentPlayerPool[playerIndex][4] + recover;
                currentPlayerPool[playerIndex][4] =
                    (int)currentPlayerPool[playerIndex][4] > playerMaxMana[playerIndex]
                        ? playerMaxMana[playerIndex]
                        : (int)currentPlayerPool[playerIndex][4];
                WriteLineWithSpeed($"Recovered {recover} MP!");
            }
            ItemInventory.RemoveItem(ItemInventory.currentInventory[itemIndex], 1);
            Thread.Sleep(1000);
            Clear();
            UpdatePlayerGUI();
            Thread.Sleep(2000);
            Clear();
            currentPlayerTurn = currentPlayerTurn == currentPlayerPool.Length - 1 ? 0 : currentPlayerTurn + 1;
            finishedTurn = true;
            return true;
        }

        //Monster Action
        private static void MonsterAttack()
        {
            var rand = new Random();
            int crit = rand.Next(1, 101) < 15 ? 1 : 2;
            string critName = crit == 1 ? "Normal" : "Critical";
            //Fetch all players' hp
            List<string> playerHp = FetchPlayer(true);
            List<int> availableIndex = new List<int>();
            //Select only the ones that are alive
            for (int i = 0; i < playerHp.Count; i++)
            {
                if (Convert.ToInt32(playerHp[i]) > 0)
                {
                    availableIndex.Add(i);
                }
            }
            int randomizedIndex = rand.Next(0, availableIndex.Count);
            
            //Attack Parrying Member
            if (parrying[randomizedIndex])
            {
                int parryDamage = (int)randomizedMonsterPool[currentMonsterTurn][2] * crit;
                randomizedMonsterPool[currentMonsterTurn][1] =
                    (int)randomizedMonsterPool[currentMonsterTurn][1] - parryDamage;
                randomizedMonsterPool[currentMonsterTurn][1] =
                    (int)randomizedMonsterPool[currentMonsterTurn][1] <= 0
                        ? 0
                        : (int)randomizedMonsterPool[currentMonsterTurn][1];
                Clear();
                WriteLineWithSpeed($"{randomizedMonsterPool[currentMonsterTurn][0]} attacks " +
                                   $"{currentPlayerPool[availableIndex[randomizedIndex]][0]}!");
                WriteLineWithSpeed($"But {currentPlayerPool[availableIndex[randomizedIndex]][0]} is parrying!");
                WriteLineWithSpeed($"{randomizedMonsterPool[currentMonsterTurn][0]} was dealt back {parryDamage} damage!");
                Thread.Sleep(1000);
                Clear();
                UpdateMonsterGUI();
                Thread.Sleep(1000);
                Clear();
                if (!isBossFight)
                {
                    currentMonsterTurn = currentMonsterTurn == randomizedMonsterPool.Length - 1
                        ? 0
                        : currentMonsterTurn + 1;
                }
                return;
            }
            
            //Calculate damage
            int damage = (int)randomizedMonsterPool[currentMonsterTurn][2] * crit -
                         (int)currentPlayerPool[availableIndex[randomizedIndex]][3];
            
            damage = damage <= 0 ? 0 : damage; //set to 0 if less than 0
            
            //Apply damage
            currentPlayerPool[availableIndex[randomizedIndex]][1] =
                (int)currentPlayerPool[availableIndex[randomizedIndex]][1] - damage;
            
            //set to 0 if less than 0
            currentPlayerPool[availableIndex[randomizedIndex]][1] =
                (int)currentPlayerPool[availableIndex[randomizedIndex]][1] < 0
                    ? 0
                    : (int)currentPlayerPool[availableIndex[randomizedIndex]][1];
            
            Clear();
            WriteLineWithSpeed($"{randomizedMonsterPool[currentMonsterTurn][0]} attacks " +
                               $"{currentPlayerPool[availableIndex[randomizedIndex]][0]}!");
            WriteLineWithSpeed($"Deals {critName} {damage} damage!");
            Thread.Sleep(1000);
            Clear();
            UpdatePlayerGUI();
            Thread.Sleep(1000);
            Clear();
            if (!isBossFight)
            {
                currentMonsterTurn = currentMonsterTurn == randomizedMonsterPool.Length - 1
                    ? 0
                    : currentMonsterTurn + 1;
            }
        }

        private static void BossSkill()
        {
            var rand = new Random();
            //Fetch all players' hp
            List<string> playerHp = FetchPlayer(true);
            List<int> availableIndex = new List<int>();
            //Select only the ones that are alive
            for (int i = 0; i < playerHp.Count; i++)
            {
                if (Convert.ToInt32(playerHp[i]) > 0)
                {
                    availableIndex.Add(i);
                }
            }
            int randomizedIndex = rand.Next(0, availableIndex.Count);
            int randomSkill = rand.Next(0, bossSkill.Length);
            
            if (bossSkill[randomSkill].aoe)
            {
                int damage = bossSkill[randomSkill].hpValue;
                for (int i = 0; i < currentPlayerPool.Length; i++)
                {
                    if ((int)currentPlayerPool[i][1] > 0)
                    {
                        //Apply damage
                        currentPlayerPool[i][1] =
                            (int)currentPlayerPool[i][1] - damage;
                        currentPlayerPool[i][1] =
                            (int)currentPlayerPool[i][1] < 0
                                ? 0
                                : (int)currentPlayerPool[i][1];
                    }
                }
                Clear();
                WriteLineWithSpeed(
                    $"{randomizedMonsterPool[currentMonsterTurn][0]} used {bossSkill[randomSkill].name}!");
                WriteLineWithSpeed($"Deals {damage} damage across the party!");
            }
            else
            {
                int damage = bossSkill[randomSkill].hpValue;
                //Apply damage
                currentPlayerPool[availableIndex[randomizedIndex]][1] =
                    (int)currentPlayerPool[availableIndex[randomizedIndex]][1] - damage;
            
                //set to 0 if less than 0
                currentPlayerPool[availableIndex[randomizedIndex]][1] =
                    (int)currentPlayerPool[availableIndex[randomizedIndex]][1] < 0
                        ? 0
                        : (int)currentPlayerPool[availableIndex[randomizedIndex]][1];
                Clear();
                WriteLineWithSpeed($"{randomizedMonsterPool[currentMonsterTurn][0]} used {bossSkill[randomSkill].name} on" +
                                   $" {currentPlayerPool[availableIndex[randomizedIndex]][0]}!");
                WriteLineWithSpeed($"Deals {damage} damage!");
            }
            
            //Debuff ATK
            if (bossSkill[randomSkill].atkValue > 0)
            {
                int debuff = 0;
                for (int i = 0; i < currentPlayerPool.Length; i++)
                {
                    if ((int)currentPlayerPool[i][1] > 0)
                    {
                        debuff = bossSkill[randomSkill].atkValue;
                        currentPlayerPool[i][2] = (int)currentPlayerPool[i][2] - debuff;
                        currentPlayerPool[i][2] =
                            (int)currentPlayerPool[i][2] < 0 ? 0 : (int)currentPlayerPool[i][2];
                        underRoar[i] = true;
                        roarDuration[i] += 10;
                    }
                }
                Clear();
                WriteLineWithSpeed($"{randomizedMonsterPool[currentMonsterTurn][0]} used {bossSkill[randomSkill].name}!");
                WriteLineWithSpeed($"Reduced ATK by {debuff} points across the party...");
            }
                
            //Debuff DEF
            if (bossSkill[randomSkill].defValue > 0)
            {
                int debuff = 0;
                for (int i = 0; i < currentPlayerPool.Length; i++)
                {
                    if ((int)currentPlayerPool[i][1] > 0)
                    {
                        debuff = bossSkill[randomSkill].defValue;
                        currentPlayerPool[i][3] = (int)currentPlayerPool[i][3] - debuff;
                        currentPlayerPool[i][3] =
                            (int)currentPlayerPool[i][3] < 0 ? 0 : (int)currentPlayerPool[i][3];
                        underKing[i] = true;
                        kingDuration[i] += 10;
                    }
                }
                Clear();
                WriteLineWithSpeed($"{randomizedMonsterPool[currentMonsterTurn][0]} used {bossSkill[randomSkill].name}!");
                WriteLineWithSpeed($"Reduced DEF by {debuff} points across the party...");
            }
            
            randomizedMonsterPool[currentMonsterTurn][4] = 0;
            Thread.Sleep(1000);
            Clear();
            UpdatePlayerGUI();
            Thread.Sleep(1000);
            Clear();
        }
    }

    class ShopSystem
    {
        public class ItemShop
        {
            private static void UpdateItemGUI()
            {
                WriteLine("═════════════════════════════════════");
                for (int i = 0; i < ItemInventory.currentInventory.Length; i++)
                {
                    WriteLine($"{ItemInventory.currentInventory[i].item.name}:");
                    WriteLine($"Currently have: x{ItemInventory.currentInventory[i].amount}");
                    WriteLine($"Price: {ItemInventory.currentInventory[i].item.price}");
                    WriteLine($"Description: {ItemInventory.currentInventory[i].item.description}");
                    WriteLine($"HP: {ItemInventory.currentInventory[i].item.hpValue}");
                    WriteLine($"ATK: {ItemInventory.currentInventory[i].item.atkValue}");
                    WriteLine($"DEF: {ItemInventory.currentInventory[i].item.defValue}");
                    WriteLine($"MP: {ItemInventory.currentInventory[i].item.mpValue}");
                    string revive = ItemInventory.currentInventory[i].item.revive ? "Yes" : "No";
                    WriteLine($"Revive: {revive}");
                    WriteLine("═════════════════════════════════════");
                }
            }
            private static List<string> FetchItems()
            {
                List<string> itemOptions = new List<string>();
            
                //Fetch Items
                for (int i = 0; i < ItemInventory.currentInventory.Length; i++)
                {
                    itemOptions.Add(ItemInventory.currentInventory[i].item.name);
                }

                return itemOptions;
            }
        }

        public class Blacksmith
        {
            private static void UpdateWeaponGUI()
            {
                WriteLine("═════════════════════════════════════");
                for (int i = 0; i < WeaponInventory.weaponSets.Length; i++)
                {
                    WriteLine($"{WeaponInventory.weaponSets[i].name}:");
                    WriteLine($"Current Level: {WeaponInventory.weaponSets[i].lvl}");
                    WriteLine($"Current Modifier: {WeaponInventory.weaponSets[i].baseModifier}");
                    if (WeaponInventory.weaponSets[i].lvl < 5)
                    {
                        double newStat = WeaponInventory.weaponSets[i].baseModifier + (Program.level - 1) / 10.0;
                        WriteLine($"Upgrade From: {WeaponInventory.weaponSets[i].baseModifier} -> {newStat}");
                        WriteLine($"Upgrade Price: {WeaponInventory.weaponSets[i].lvl * 100}");
                    }
                    else
                    {
                        WriteLine($"Weapon is at max level, cannot upgrade any further...");
                    }
                    WriteLine("═════════════════════════════════════");
                }
            }
            private static List<string> FetchWeapons()
            {
                List<string> weaponOptions = new List<string>();
            
                //Fetch Items
                for (int i = 0; i < WeaponInventory.weaponSets.Length; i++)
                {
                    if (WeaponInventory.weaponSets[i].lvl < 5)
                    {
                        weaponOptions.Add(WeaponInventory.weaponSets[i].name);
                    }
                }

                return weaponOptions;
            }
        }
    }
}

