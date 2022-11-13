namespace GI113_Final_Project;
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
    public static string[] combatOptions = {"Attack", "Defense", "Skill", "Item"};
    public static string[] startMenuOptions = { "Start", "Exit" };
}
class PlayerInfo
{
    public class PlayerBaseStats
    {
        public string name { get; }
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
        public static PlayerBaseStats knight = new PlayerBaseStats("Knight", 200, 50, 40, 75);
        public static PlayerBaseStats mage = new PlayerBaseStats("Mage", 150, 40, 25, 150);
        public static PlayerBaseStats archer = new PlayerBaseStats("Archer", 125, 30, 15, 100);
        public static PlayerBaseStats bard = new PlayerBaseStats("Bard", 100, 15, 10, 125);
            
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
        public static MonsterBaseStats slime = new MonsterBaseStats("Slime", 100, 15, 15);
        public static MonsterBaseStats goblin = new MonsterBaseStats("Goblin", 150, 25, 25);
        public static MonsterBaseStats skeleton = new MonsterBaseStats("Skeleton", 125, 10, 10);
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