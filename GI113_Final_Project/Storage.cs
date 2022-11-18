using static GI113_Final_Project.ConsoleUtilities;

namespace GI113_Final_Project
{
    //Console Related Class
    class DefinedColors
{
    // name = { BackgroundColor, ForegroundColor };
    public static ConsoleColor[] defaultColors = { ConsoleColor.DarkRed, ConsoleColor.Yellow };
    public static ConsoleColor[] titleColors = { ConsoleColor.White, ConsoleColor.Red };
    public static ConsoleColor[] combatColors = { ConsoleColor.DarkRed, ConsoleColor.Yellow };
    public static ConsoleColor[] healthBarColors = { ConsoleColor.White, ConsoleColor.DarkRed };
    public static ConsoleColor[] manaBarColors = { ConsoleColor.White, ConsoleColor.Blue };
    public static ConsoleColor[] startMenuColors = { ConsoleColor.DarkRed, ConsoleColor.Yellow };
}
    class MenuOptions
{
    // name = { "A", "B", "C", .... };
    public static string[] combatOptions = {"Attack", "Defend", "Skill", "Item"};
    public static string[] startMenuOptions = { "Start", "Exit" };
}
    class BigText
{
    public static string gameTitle = @"
                ██████╗  ██████╗ ███╗   ██╗████████╗    ██████╗ ██╗███████╗    ██╗███╗   ██╗    ████████╗██╗  ██╗███████╗    ██████╗ ██╗   ██╗███╗   ██╗ ██████╗ ███████╗ ██████╗ ███╗   ██╗
                ██╔══██╗██╔═══██╗████╗  ██║╚══██╔══╝    ██╔══██╗██║██╔════╝    ██║████╗  ██║    ╚══██╔══╝██║  ██║██╔════╝    ██╔══██╗██║   ██║████╗  ██║██╔════╝ ██╔════╝██╔═══██╗████╗  ██║
                ██║  ██║██║   ██║██╔██╗ ██║   ██║       ██║  ██║██║█████╗      ██║██╔██╗ ██║       ██║   ███████║█████╗      ██║  ██║██║   ██║██╔██╗ ██║██║  ███╗█████╗  ██║   ██║██╔██╗ ██║
                ██║  ██║██║   ██║██║╚██╗██║   ██║       ██║  ██║██║██╔══╝      ██║██║╚██╗██║       ██║   ██╔══██║██╔══╝      ██║  ██║██║   ██║██║╚██╗██║██║   ██║██╔══╝  ██║   ██║██║╚██╗██║
                ██████╔╝╚██████╔╝██║ ╚████║   ██║       ██████╔╝██║███████╗    ██║██║ ╚████║       ██║   ██║  ██║███████╗    ██████╔╝╚██████╔╝██║ ╚████║╚██████╔╝███████╗╚██████╔╝██║ ╚████║
                ╚═════╝  ╚═════╝ ╚═╝  ╚═══╝   ╚═╝       ╚═════╝ ╚═╝╚══════╝    ╚═╝╚═╝  ╚═══╝       ╚═╝   ╚═╝  ╚═╝╚══════╝    ╚═════╝  ╚═════╝ ╚═╝  ╚═══╝ ╚═════╝ ╚══════╝ ╚═════╝ ╚═╝  ╚═══╝
                                                                                                                                                                            
            ";

    public static string vs = @"
 ██▒   █▓  ██████ 
▓██░   █▒▒██    ▒ 
 ▓██  █▒░░ ▓██▄   
  ▒██ █░░  ▒   ██▒
   ▒▀█░  ▒██████▒▒
   ░ ▐░  ▒ ▒▓▒ ▒ ░
   ░ ░░  ░ ░▒  ░ ░
     ░░  ░  ░  ░  
      ░        ░  
     ░            
";

    public static string monsterFight = @"
███    ███  ██████  ███    ██ ███████ ████████ ███████ ██████      ███████ ██  ██████  ██   ██ ████████ 
████  ████ ██    ██ ████   ██ ██         ██    ██      ██   ██     ██      ██ ██       ██   ██    ██    
██ ████ ██ ██    ██ ██ ██  ██ ███████    ██    █████   ██████      █████   ██ ██   ███ ███████    ██    
██  ██  ██ ██    ██ ██  ██ ██      ██    ██    ██      ██   ██     ██      ██ ██    ██ ██   ██    ██    
██      ██  ██████  ██   ████ ███████    ██    ███████ ██   ██     ██      ██  ██████  ██   ██    ██    
                                                                                                        
                                                                                                        
";
    public static string bossFight = @"
 ▄▀▀█▄▄   ▄▀▀▀▀▄   ▄▀▀▀▀▄  ▄▀▀▀▀▄      ▄▀▀▀█▄    ▄▀▀█▀▄    ▄▀▀▀▀▄   ▄▀▀▄ ▄▄   ▄▀▀▀█▀▀▄ 
▐ ▄▀   █ █      █ █ █   ▐ █ █   ▐     █  ▄▀  ▀▄ █   █  █  █        █  █   ▄▀ █    █  ▐ 
  █▄▄▄▀  █      █    ▀▄      ▀▄       ▐ █▄▄▄▄   ▐   █  ▐  █    ▀▄▄ ▐  █▄▄▄█  ▐   █     
  █   █  ▀▄    ▄▀ ▀▄   █  ▀▄   █       █    ▐       █     █     █ █   █   █     █      
 ▄▀▄▄▄▀    ▀▀▀▀    █▀▀▀    █▀▀▀        █         ▄▀▀▀▀▀▄  ▐▀▄▄▄▄▀ ▐  ▄▀  ▄▀   ▄▀       
█    ▐             ▐       ▐          █         █       █ ▐         █   █    █         
▐                                     ▐         ▐       ▐           ▐   ▐    ▐         
";
}
    
    //Combat Related Class
    class PlayerInfo
{
    public class PlayerBaseStats
    {
        public string name { get; set; }
        public int hp { get; }
        public int atk { get; }
        public int def { get; }
        public int mp { get; }
        public PlayerBaseStats(string name, int hp, int atk, int def, int mp)
        {
            this.name = name;
            this.hp = hp;
            this.atk = atk;
            this.def = def;
            this.mp = mp;
        }
        public static PlayerBaseStats knight = new PlayerBaseStats("Knight", 200, 50, 40, 150);
        public static PlayerBaseStats mage = new PlayerBaseStats("Mage", 150, 40, 25, 150);
        public static PlayerBaseStats archer = new PlayerBaseStats("Archer", 125, 30, 15, 150);
        public static PlayerBaseStats bard = new PlayerBaseStats("Bard", 100, 15, 10, 150);
            
    }
    public class DefinedPlayerPool
    {
        public static PlayerBaseStats[] defaultParty =
            { PlayerBaseStats.knight, PlayerBaseStats.mage, PlayerBaseStats.archer, PlayerBaseStats.bard };
    }
    public class PlayerScaleModifier
    {
        public static double[] defaultScaleModifier = { 1.0, 1.0, 1.0, 1.0 };
    }
}
    class MonsterInfo
{
    public class MonsterBaseStats
    {
        public string name { get; }
        public int hp { get; }
        public int atk { get; }
        public int def { get; }
        // name = { Name, HP, ATK, DEF };
        public MonsterBaseStats(string name, int hp, int atk, int def)
        {
            this.name = name;
            this.hp = hp;
            this.atk = atk;
            this.def = def;
        }
        public static MonsterBaseStats slime = new MonsterBaseStats("Slime", 100, 40, 25);
        public static MonsterBaseStats goblin = new MonsterBaseStats("Goblin", 150, 65, 30);
        public static MonsterBaseStats skeleton = new MonsterBaseStats("Skeleton", 120, 50, 15);
    }

    public class DefinedMonsterPool
    {
        public static MonsterBaseStats[] oneToFive =
            { MonsterBaseStats.slime, MonsterBaseStats.goblin, MonsterBaseStats.skeleton };
    }

    public class MonsterScaleModifier
    {
        public static double[] defaultScaleModifier = { 1.0, 1.0, 1.0 };
    }
}
    class BossInfo
{
    public class BossBaseStats
    {
        public string name { get; }
        public int hp { get; }
        public int atk { get; }
        public int def { get; }
        public int gauge { get; }
        public BossBaseStats(string name, int hp, int atk, int def, int gauge)
        {
            this.name = name;
            this.hp = hp;
            this.atk = atk;
            this.def = def;
            this.gauge = gauge;
        }

        public static BossBaseStats leviathan = new BossBaseStats("Leviathan", 2000, 150, 100, 2);
        public static BossBaseStats megalodon = new BossBaseStats("Megalodon", 1000, 100, 50, 2);

    }
}
    class ItemInfo
{
    public class ItemStats
    {
        public string name { get; }
        public string description { get; }
        public int price { get; }
        public int hpValue { get; }
        public int atkValue { get; }
        public int defValue { get; }
        public int mpValue { get; }
        public bool revive { get; }
        public ItemStats(string name, string description, int price, int hpValue = 0, int atkValue = 0, int defValue = 0, int mpValue = 0, bool revive = false)
        {
            this.name = name;
            this.description = description;
            this.price = price;
            this.hpValue = hpValue;
            this.atkValue = atkValue;
            this.defValue = defValue;
            this.mpValue = mpValue;
            this.revive = revive;
        }

        public static ItemStats hpPotion = new ItemStats("Healing Potion", "Heals selected player", 35, hpValue: 40);
        public static ItemStats atkPotion = new ItemStats("Attack Boost Potion","Boosts ATK for selected player", 20, atkValue: 10);
        public static ItemStats defPotion = new ItemStats("Defense Boost Potion","Boosts DEF for selected player", 20, defValue: 10);
        public static ItemStats mpPotion = new ItemStats("Mana Potion","Recover MP for selected player", 45, mpValue: 40);
        public static ItemStats reviver = new ItemStats("Reviver","Revive and Heals selected player",100, hpValue: 30, revive: true);
        
    }

    class DefinedItemPool
    {
        public static ItemStats[] defaultItemPool = new ItemStats[]
            { ItemStats.hpPotion, ItemStats.atkPotion, ItemStats.defPotion, ItemStats.mpPotion, ItemStats.reviver };
    }

    /*
    class ItemStatModifier
    {
        public static double[] defaultScaleModifier = new[] { 1.0, 1.0, 1.0, 1.0 };
    }
    */
}
    class SkillInfo
{
    public class SkillBaseStats
    {
        public string name { get; }
        public string description { get; }
        public int mpCost { get; }
        public int hpValue { get; } //offensive: damage monster | inoffensive: heal member
        public int atkValue { get; } //offensive: reduce enemy ATK | inoffensive: boost member ATK
        public int defValue { get; } //offensive: reduce enemy DEF | inoffensive: boost member DEF
        public bool offensive { get; } //offensive skill will be used on enemy
        public bool aoe { get; } //offensive: damage all enemies | inoffensive: boost/heal all members
        public int percentageKill { get; } //Kill enemy with instantly with less that (percent)% HP
        public bool parry { get; } //Deflect attack back completely to the enemy

        SkillBaseStats(string name, string description, int mpCost, int hpValue = 0, int atkValue = 0, int defValue = 0,
            bool offensive = true, bool aoe = false, int percentageKill = 0, bool parry = false)
        {
            this.name = name;
            this.description = description;
            this.mpCost = mpCost;
            this.hpValue = hpValue;
            this.atkValue = atkValue;
            this.defValue = defValue;
            this.offensive = offensive;
            this.aoe = aoe;
            this.percentageKill = percentageKill;
            this.parry = parry;
        }

        //Knight
        public static SkillBaseStats superSlash = 
            new SkillBaseStats("Super Slash", "Deals damage to selected monsters", 120, hpValue: 120);
        public static SkillBaseStats steadyParry =
            new SkillBaseStats("Steady Parry",
                "Deflect attack back completely to the enemy, if it attacks you in the next turn", 100, parry: true);
        
        //Mage
        public static SkillBaseStats blizzard =
            new SkillBaseStats("Blizzard", "Deals AOE damage to monsters", 50, hpValue: 80, aoe: true);
        public static SkillBaseStats thunderbolt =
            new SkillBaseStats("Thunderbolt", "Deals damage to selected monsters", 100, hpValue: 100);
        
        //Archer
        public static SkillBaseStats arrowRain = 
            new SkillBaseStats("Arrow Rain", "Deals AOE damage to monsters", 90, hpValue: 120, aoe: true);
        public static SkillBaseStats cleanKill =
            new SkillBaseStats("Clean Kill",
                "Kill selected enemy instantly if its HP is less than specified percent. If not, deals damage instead",
                100, hpValue: 80, percentageKill: 25);
        
        //Bard
        public static SkillBaseStats encore =
            new SkillBaseStats("Encore", "Boost ATK to all party members", 100, atkValue: 50, offensive: false, aoe: true);
        public static SkillBaseStats remedy =
            new SkillBaseStats("Remedy", "Heals to all party members", 120, hpValue: 100, offensive: false, aoe: true);
        
        //Skill Pool
        public static SkillBaseStats[] knightSkills = new[] { superSlash, steadyParry };
        public static SkillBaseStats[] mageSkills = new[] { blizzard, thunderbolt};
        public static SkillBaseStats[] archerSkills = new[] { arrowRain, cleanKill };
        public static SkillBaseStats[] bardSkills = new[] { encore, remedy};
    }

    public class DefinedSkillPool
    {
        public static List<SkillBaseStats[]> defaultSkillPool = new List<SkillBaseStats[]>
        {
            SkillBaseStats.knightSkills, SkillBaseStats.mageSkills, SkillBaseStats.archerSkills,
            SkillBaseStats.bardSkills
        };
    }

    public class SkillScaleModifier
    {
        public static double[] defaultModifier = new double[] { 1.0, 1.0, 1.0 };
    }
}
    class BossSkillInfo
{
    public class SkillBaseStats
    {
        public string name { get; }
        public string description { get; }
        public int hpValue { get; } 
        public int atkValue { get; } 
        public int defValue { get; } 
        public bool aoe { get; }
        public bool debuff { get; }

        SkillBaseStats(string name, string description, int hpValue = 0, int atkValue = 0, int defValue = 0, bool aoe = true, bool debuff = false)
        {
            this.name = name;
            this.description = description;
            this.hpValue = hpValue;
            this.atkValue = atkValue;
            this.defValue = defValue;
            this.aoe = aoe;
            this.debuff = debuff;
        }

        public static SkillBaseStats whirlpool =
            new SkillBaseStats("Whirlpool", "Deals massive damage to the party", hpValue: 150);
        public static SkillBaseStats fang =
            new SkillBaseStats("Fang of The End", "Deals 'megalodon' damage to random member", hpValue: 250, aoe: false);
        public static SkillBaseStats king = new SkillBaseStats("King of The Sea", "Reduce DEF of the party and deal damage to the party", 
            hpValue: 100, defValue: 30, debuff: true);
        public static SkillBaseStats roar = new SkillBaseStats("Roar of The Ocean", "Reduce ATK of the party",
            atkValue: 25, debuff: true);
        public static SkillBaseStats[] megalodon = new[] { whirlpool, roar };
        public static SkillBaseStats[] leviathan = new[] { fang, king };

    }
}
    class WeaponInfo
{
    public class WeaponBaseStats
    {
        public string name { get; }
        public double baseModifier { get; set; }
        public int lvl { get; set; }
        WeaponBaseStats(string name, double baseModifier, int lvl = 1)
        {
            this.name = name;
            this.baseModifier = baseModifier;
            this.lvl = lvl;
        }

        public static WeaponBaseStats sword = new WeaponBaseStats("Great Sword", 1.0);
        public static WeaponBaseStats staff = new WeaponBaseStats("Mage Staff", 1.0);
        public static WeaponBaseStats bow = new WeaponBaseStats("Hunting Bow", 1.0);
        public static WeaponBaseStats lute = new WeaponBaseStats("Lute", 1.0);
    }
}
    class Inventory
{
    public class ItemInventory
    {
        public ItemInfo.ItemStats item;
        public int amount;
        ItemInventory(ItemInfo.ItemStats item, int amount = 0)
        {
            this.item = item;
            this.amount = amount;
        }
        public static ItemInventory hpPotionInventory = new ItemInventory(ItemInfo.ItemStats.hpPotion);
        public static ItemInventory atkPotionInventory = new ItemInventory(ItemInfo.ItemStats.atkPotion);
        public static ItemInventory defPotionInventory = new ItemInventory(ItemInfo.ItemStats.defPotion);
        public static ItemInventory mpPotionInventory = new ItemInventory(ItemInfo.ItemStats.mpPotion);
        public static ItemInventory reviverInventory = new ItemInventory(ItemInfo.ItemStats.reviver);
        public static ItemInventory[] currentInventory = new ItemInventory[]
            { hpPotionInventory, atkPotionInventory, defPotionInventory, mpPotionInventory, reviverInventory }; 
        public static void AddItem(ItemInventory itemSlot, int amount)
        {
            WriteLineWithSpeed($"{itemSlot.item.name} (x{amount}) was added to your inventory!");
            itemSlot.amount += amount;
        }

        public static void RemoveItem(ItemInventory itemSlot, int amount)
        {
            WriteLineWithSpeed($"{itemSlot.item.name} (x{amount}) was removed from your inventory!");
            itemSlot.amount -= amount;
        }
    }

    public class WeaponInventory
    {
        public static WeaponInfo.WeaponBaseStats[] weaponSets = new[]
        {
            WeaponInfo.WeaponBaseStats.sword, WeaponInfo.WeaponBaseStats.staff, WeaponInfo.WeaponBaseStats.bow,
            WeaponInfo.WeaponBaseStats.lute
        };
        public static void UpLevel(WeaponInfo.WeaponBaseStats weapon)
        {
            double statBefore = weapon.baseModifier;
            weapon.lvl += 1;
            weapon.baseModifier = 1.0 + (((double)weapon.lvl - 1) / 10);
            WriteLineWithSpeed($"{weapon.name} has been upgraded to level {weapon.lvl}!");
            WriteLineWithSpeed($"Attack Modifier | {statBefore} -> {weapon.baseModifier}");
        }
    }

    public class Wallet
    {
        public static int money = 0;

        public static void AddMoney(int amount)
        {
            money += amount;
            WriteLineWithSpeed($"{amount} coins has been added, you now have {money} coins!");
        }

        public static void RemoveMoney(int amount)
        {
            money -= amount;
            WriteLineWithSpeed($"{amount} coins has been removed, you have {money} coins left...");
        }
    }
}
}
