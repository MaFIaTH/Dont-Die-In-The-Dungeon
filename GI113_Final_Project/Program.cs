// See https://aka.ms/new-console-template for more information

using System.Text;
using static System.Console;
using static GI113_Final_Project.ConsoleUtilities;
using static GI113_Final_Project.DefinedColors;
using static GI113_Final_Project.MenuOptions;
using static GI113_Final_Project.CombatSystem;
using static GI113_Final_Project.MonsterInfo;
using static GI113_Final_Project.Inventory;
using static GI113_Final_Project.ShopSystem;

namespace GI113_Final_Project
{
    public class Program
    {
        public static int level = 1;
        static void Main(string[] args)
        { 
            OutputEncoding = Encoding.UTF8;
            StartingMenu();
        }

        static void StartingMenu()
        {
            SetWindowSize(LargestWindowWidth, LargestWindowHeight);
            CursorVisible = false;
            WriteLine(BigText.gameTitle);
            NewEmptyLines(1);
            WriteLine("Basic Controls:");
            WriteLine("W / Up Arrow = Move Up The Menu\n" +
                      "S / Down Arrow = Move Down The Menu\n" +
                      "E / Enter = Confirm Selection\n" +
                      "Tap or hold down any key to speed up text animation.\n" +
                      "(Holding down E or Enter is not advised as it may interferes with menu selection)");
            NewEmptyLines(1);
            switch (CreateMenu(startMenuOptions, startMenuColors))
            {
                case 0:
                    Clear();
                    WriteLineWithSpeed("Starting the game...");
                    Thread.Sleep(1000);
                    Clear();
                    StoryIntro();
                    CharacterIntro();
                    Clear();
                    StartingCombat();
                    break;
                case 1:
                    Clear();
                    WriteLineWithSpeed("Exiting the game...");
                    Environment.Exit(0);
                    break;
            }
        }

        static void StoryIntro()
        {
            WriteLine(BigText.intro);
            NewEmptyLines(1);
            WriteLineWithSpeed("Once upon the time, the mysterious dungeon was discovered between the crack of the ocean.\n" +
                               "Many travellers and adventurers tried to reach the deepest end but alas, nobody has gotten out of it alive to tell the tales.\n" +
                               "Your party is one of the challengers of this dungeon.\n" +
                               "Will we see a tragic tale or a legend is up to you...\n" +
                               "\n" +
                               @"My final wish to you...");
            WriteLineWithSpeed(@"""Don't Die in The Dungeon""", 150);
            NewEmptyLines(1);
            WriteLineWithSpeed("Press any key to continue...");
            ReadKey(true);
            Clear();
        }
        static void GoodEnding()
        {
            WriteLine(BigText.goodEnding);
            NewEmptyLines(1);
            WriteLineWithSpeed("The Leviathan has been defeated thanks to your party's valor and perseverance.\n" +
                               "Something has fallen out of the Leviathan mouth. Your party picked it up with curiosity...\n" +
                               "Suddenly, they are teleported to a vast landscape with nothing but a beautiful sea of stars...\n" +
                               "What happened to them after that is still unknown, however, what we know for sure is that...\n");
            WriteLineWithSpeed(@"""You Survived The Dungeon""", 150);
            NewEmptyLines(1);
            WriteLineWithSpeed("Press any key to continue...");
            ReadKey(true);
            Clear();
        }
        static void BadEnding()
        {
            WriteLine(BigText.badEnding);
            NewEmptyLines(1);
            WriteLineWithSpeed("After many exhausting fights, your party lost every glimpses of hope and started falling one by one...\n" +
                               "May your efforts and spirits guide the next party to the victory.\n" +
                               "But but for now, rest in peace...\n");
            WriteLineWithSpeed(@"""You Died in The Dungeon...""", 150);
            NewEmptyLines(1);
            WriteLineWithSpeed("Press any key to continue...");
            ReadKey(true);
            Clear();
        }


        static void CharacterIntro()
        {
            string charClass = String.Empty;
            string description = String.Empty;
            string bigText = String.Empty;
            for (int i = 0; i < PlayerInfo.DefinedPlayerPool.defaultParty.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        bigText = BigText.knight;
                        charClass = "Knight";
                        description =
                            "Knight is a strongest fighter in the party with overall high HP, ATK and DEF but average MP." +
                            "\nTheir skills prioritize offensive attack and parrying";
                        break;
                    case 1:
                        bigText = BigText.mage;
                        charClass = "Mage";
                        description =
                            "Mage is an excellent fighter in the party with overall moderate HP, ATK and DEF but high MP." +
                            "\nTheir skills prioritize offensive single attack and AOE";
                        break;
                    case 2:
                        bigText = BigText.archer;
                        charClass = "Archer";
                        description =
                            "Archer is an average fighter, with overall moderate HP, ATK, DEF and MP." +
                            "\nTheir skills prioritize offensive AOE attack and creative kill";
                        break;
                    case 3:
                        bigText = BigText.bard;
                        charClass = "Bard";
                        description =
                            "Bard is a bad fighter but an excellent support, with overall low HP, ATK, DEF and MP" +
                            "\nTheir skills prioritize party healing and ATK boost";
                        break;
                }
                WriteLine(bigText);
                NewEmptyLines(1);
                WriteLineWithSpeed($"Class: {charClass}");
                WriteLineWithSpeed($"Description: {description}");
                EnterNames(i);
                Clear();
            }
        }
        static void EnterNames(int index)
        {
            string name;
            WriteLineWithSpeed("Please name this character");
            NewEmptyLines(1);
            while (true)
            {
                Write("Enter Name: ");
                name = ReadLine();
                if (string.IsNullOrEmpty(name))
                {
                    WriteLine("Name cannot be empty, please include at least 1 alphabet");
                }
                else if (int.TryParse(name, out _) || double.TryParse(name, out _))
                {
                    WriteLine("Name cannot be only number, please include at least 1 alphabet");
                }
                else if (!string.IsNullOrEmpty(name))
                {
                    break;
                }

            }
            PlayerInfo.DefinedPlayerPool.defaultParty[index].name = name;

        }

        public static void StartingCombat()
        {
            /*
            WriteLineWithSpeed($"Entering level {level}...");4
            //For testing
            var random = new Random();
            Wallet.AddMoney(random.Next(level * 100, level * 200 + 1));
            ItemInventory.AddItem(ItemInventory.reviverInventory, 2);
            ItemInventory.AddItem(ItemInventory.atkPotionInventory, 2);
            ItemInventory.AddItem(ItemInventory.defPotionInventory, 2);
            ItemInventory.AddItem(ItemInventory.hpPotionInventory, 2);
            ItemInventory.AddItem(ItemInventory.mpPotionInventory, 2);
            WeaponInventory.UpLevel(WeaponInfo.WeaponBaseStats.sword);
            Clear();
            //
            */
            /*
            var random = new Random();
            Wallet.AddMoney(random.Next(level * 100, level * 200 + 1));
            */
            WriteLineWithSpeed($"Entering level {level}...");
            Thread.Sleep(1000);
            Clear();
            if (level % 5 == 0)
            {
                if (level / 5 == 1)
                {
                    InitializeCombat(DefinedMonsterPool.oneToFive, boss: BossInfo.BossBaseStats.megalodon, 
                        bossSkill: BossSkillInfo.SkillBaseStats.megalodon);
                }
                else if (level / 5 == 2)
                {
                    InitializeCombat(DefinedMonsterPool.oneToFive, boss: BossInfo.BossBaseStats.leviathan, 
                        bossSkill: BossSkillInfo.SkillBaseStats.leviathan);
                }
            }
            else
            {
                if (level < 5)
                {
                    InitializeCombat(DefinedMonsterPool.oneToFive);
                }
                else
                {
                    InitializeCombat(DefinedMonsterPool.fiveToTen);
                }
                
            }
            if (!StartCombat())
            {
                Clear();
                BadEnding();
                WriteLineWithSpeed($"You died at level {level}...");
                WriteLineWithSpeed($"Hope you're lucky next time...");
                WriteLineWithSpeed($"Exiting the game...");
                Thread.Sleep(1000);
                Environment.Exit(0);
                return;
            }
            if (level == 10)
            {
                Clear();
                GoodEnding();
                WriteLineWithSpeed($"Exiting the game...");
                Thread.Sleep(1000);
                Environment.Exit(0);
                return;
            }
            else
            {
                Clear();
                var rand = new Random();
                WriteLineWithSpeed($"You passed level {level}!");
                Wallet.AddMoney(rand.Next(Convert.ToInt32((37.5 * level + 12.5)), Convert.ToInt32((95 * level * 1.5 + 25))));
                level++;
                WriteLineWithSpeed($"Entering Shop...");
                Thread.Sleep(1000);
                ShopMainMenu();
            }
        }
    }
}
