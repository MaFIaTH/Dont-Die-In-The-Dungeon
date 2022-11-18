// See https://aka.ms/new-console-template for more information

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
            StartingMenu();
        }

        static void StartingMenu()
        {
            SetWindowSize(LargestWindowWidth, LargestWindowHeight);
            CursorVisible = false;
            WriteLine(BigText.gameTitle);
            //WriteLineWithSpeed("Don't Die In The Dungeon", colors: titleColors);
            NewEmptyLines(1);
            switch (CreateMenu(startMenuOptions, startMenuColors))
            {
                case 0:
                    Clear();
                    WriteLineWithSpeed("Starting the game");
                    Thread.Sleep(1000);
                    Clear();
                    StoryIntro();
                    //CharacterIntro();
                    Clear();
                    StartingCombat(DefinedMonsterPool.oneToFive);
                    break;
                case 1:
                    Clear();
                    WriteLineWithSpeed("Exiting the game");
                    break;
            }
        }

        static void StoryIntro()
        {
            WriteLineWithSpeed("Text test01");
            Thread.Sleep(1000);
            Clear();
        }
        static void GoodEnding()
        {
            WriteLineWithSpeed("Good ending <3");
            Thread.Sleep(1000);
            Clear();
        }
        static void BadEnding()
        {
            WriteLineWithSpeed("Bad ending T-T");
            Thread.Sleep(1000);
            Clear();
        }


        static void CharacterIntro()
        {
            string charClass = String.Empty;
            string description = String.Empty;
            for (int i = 0; i < PlayerInfo.DefinedPlayerPool.defaultParty.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        charClass = "Knight";
                        description =
                            "Knight is a strongest fighter in the party with overall high HP, ATK and DEF but average MP." +
                            "\nTheir skills prioritize offensive attack and parrying";
                        break;
                    case 1:
                        charClass = "Mage";
                        description =
                            "Mage is an excellent fighter in the party with overall moderate HP, ATK and DEF but high MP." +
                            "\nTheir skills prioritize offensive single attack and AOE";
                        break;
                    case 2:
                        charClass = "Archer";
                        description =
                            "Archer is an average fighter, with overall moderate HP, ATK, DEF and MP." +
                            "\nTheir skills prioritize offensive AOE attack and creative kill";
                        break;
                    case 3:
                        charClass = "Bard";
                        description =
                            "Bard is a bad fighter but an excellent support, with overall low HP, ATK, DEF and MP" +
                            "\nTheir skills prioritize party healing and ATK boost";
                        break;
                }
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

        static void StartingCombat(MonsterBaseStats[] monsterPool)
        {
            /*
            WriteLineWithSpeed($"Entering level {level}...");
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
            var random = new Random();
            Wallet.AddMoney(random.Next(level * 100, level * 200 + 1));
            ShopMainMenu();
            if (level is 5 or 10)
            {
                
            }
            else
            {
                InitializeCombat(monsterPool, boss: BossInfo.BossBaseStats.leviathan, bossSkill: BossSkillInfo.SkillBaseStats.leviathan);
            }
            if (!StartCombat())
            {
                WriteLineWithSpeed($"You died at level {level}...");
                WriteLineWithSpeed($"Hope you're lucky next time...");
                WriteLineWithSpeed($"Exiting the game...");
                return;
            }
            else
            {
                Clear();
                var rand = new Random();
                WriteLineWithSpeed($"You passed level {level}!");
                Wallet.AddMoney(rand.Next(level * 100, level * 200 + 1));
                level++;
                WriteLineWithSpeed($"Entering Shop...");
            }
        }
    }
}
