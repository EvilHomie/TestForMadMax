using System.Collections.Generic;
using YG;

public static class TextConstants
{
    public static Language Language;
    public static string HULLDMG = "Hull Damage";
    public static string SHIELDDMG = "Shield Damage";
    public static string ROTATIONSPEED = "Rotation Speed";
    public static string FIRERATE = "FireRate";

    public static string PERHIT = "Per Hit";
    public static string DGSINSECOND = "DGS/Second";
    public static string INSECOND = "/Second";
    public static string ITEMNAME = "Item Name";

    public static string HULLHP = "Hull HP";
    public static string SHIELDHP = "Shield HP";
    public static string SHIELREGENRATE = "Shield RegenRate";
    public static string WEAPONSCOUNT = "Weapons Count";
    public static string UNIT = "Unit";
    public static string FROM = "From";
    public static string UNLOCKED = "Unlocked";
    public static string LOCKED = "Locked";
    public static string LEVELS = "Levels";
    public static string EQUIP = "Equip";
    public static string UNLOCK = "Unlock";
    public static string CHARACTERISTICS = "Characteristics";
    public static string UPGRADES = "Upgrades";
    public static string LEVEL = "Level";
    public static string UPEFFECT = "Up Effect";
    public static string COST = "Cost";
    public static string UPGRADE = "Improve";
    public static string MAXLEVEL = "MaxLevel";
    public static string NEWSCHEMES = "New Schemes";
    public static string INVENTORY = "Inventory";
    public static string RAID = "Raid";
    public static string GARAGE = "Garage";
    public static string UNLOCKCOST = "UnlockCost";
    public static string NEWSCHEME = "New SCHEME";
    public static string[] TIPSCOLLECTION =
        {
        "Don't forget to upgrade your equipment in the inventory tab.",
        "You can get new schemas from enemies. Unlocking them requires resources.",
        "destroying the explosive part will cause significant damage to the hull.",
        "An escaping enemy will not bring you resources. Enemies run away if they have lost all their guns.",
        "wheels have less strength than the body. Caterpillar is much stronger than wheels.",
        "Blue flickering indicates that the shield is taking damage. While red indicates damage to the body.",
        "Keep in mind that weapons have two types of damage. Shield Damage and Hull Damage.",
        "You can replay levels to collect additional resources. Automatic level change will be disabled."
        };

    public static string LEVELSTATISTIC = "Level Statistic";
    public static string RESOURCESCOLLECTED = "Resources Collected";
    public static string DAMAGERECEIVED = "DAMAGERECEIVED";
    public static string TOHULL = "TO HULL";
    public static string TOSHIELD = "TO SHIELD";
    public static string DAMAGEDONE = "DAMAGE DONE";
    public static string ELEMENTSDESTROYED = "ELEMENTS DESTROYED";
    public static string WHEELS = "WHEELS";
    public static string CATERPILLARS = "Caterpillars";
    public static string BODIES = "BODIES";
    public static string OTHERS = "OTHERS";
    public static string EXPLOSIVEPARTS = "EXPLOSIVE PARTS";
    public static string TIP = "TIP";
    public static string NEXTTIP = "NEXTTIP";
    public static string PREVIOUS = "PREVIOUS";
    public static string RAIDDONE = "RAID DONE";
    public static string RAIDFAILED = "RAID FAILED";
    public static string WAVEISAPPROACHING = "Wave is approaching";
    public static string BOSSISAPPROACHING = "Boss is approaching";
    public static string COMBO = "Combo";

    public static Dictionary<StageName, string> TUTORIALSTAGESTEXTS = new()
        {
            { StageName.Greetings, "Hey, fighter!\r\nMy name is Meg,\r\nI'm your driver.\r\nI'll help you on your first raid in the Badlands." },
            { StageName.FirstRaidLaunch, "Our warehouse is empty right now and we're in urgent need of resources.\r\nLet's go get them." },
            { StageName.ShowWaveWarning, "Careful, we've been spotted!\r\nI suggest you to shoot at vulnerable areas.\r\nLike <color=\"red\">wheels</color> or <color=\"red\">canisters</color>." },
            { StageName.FirstLevelCompleted, "That was very easy!\r\nThe next raid will be more difficult, can you handle it?" },
            { StageName.ShowLevelStatisticPanel, "All right!\r\nWe've gathered some resources." },
            { StageName.SecondLevelCompleted, "Let's spend it on improving our inventory." },
            { StageName.FirstOpenInventory, "Here is the panel of the already installed inventory.\r\nLet's improve our weapon.\r\nClick on it to select it." },
            { StageName.ShowUpgradeDiscription, "Here you can improve the characteristics." },
            { StageName.UpgradeRotateSpeed, "The first thing to do is to increase the rotation speed.\r\nTo do this, press the <color=\"green\">�Improve�</color> button 3 times." },
            { StageName.UpgradeFireRate, "Now you need to improve your shooting speed.\r\nPress <color=\"green\">�Improve�</color> 2 times." },
            { StageName.WishGoodluck, "All right,\r\nwe're ready for the next raid!" }
        };
    public static void SetLanguage()
    {
        string language = YandexGame.EnvironmentData.language;
        if (language == "en")
        {
            Language = Language.en;
            return;
        }
        else if (language == "ru")
        {
            Language = Language.ru;
            HULLDMG = "���� ������";
            SHIELDDMG = "���� �����";
            ROTATIONSPEED = "�������� ��������";
            FIRERATE = "�������� ��������";
            PERHIT = "������ ���������";
            DGSINSECOND = "��������/�������";
            INSECOND = "/�������";
            ITEMNAME = "��� ��������";
            HULLHP = "�� �������";
            SHIELDHP = "�� ����";
            SHIELREGENRATE = "����������� ����";
            WEAPONSCOUNT = "���������� ������";
            UNIT = "������";
            FROM = "��";
            UNLOCKED = "������";
            LOCKED = "������";
            LEVELS = "������";
            EQUIP = "�����������";
            UNLOCK = "�������";
            CHARACTERISTICS = "��������������";
            UPGRADES = "���������";
            LEVEL = "�������";
            UPEFFECT = "������";
            COST = "���������";
            UPGRADE = "��������";
            MAXLEVEL = "���� �������";
            NEWSCHEMES = "����� �����";
            INVENTORY = "���������";
            RAID = "� ����";
            GARAGE = "�����";
            UNLOCKCOST = "��������� ��������";
            NEWSCHEME = "����� �����";

            string[] tipscollectionRu =
            { "�� ��������� �������� ���� ���������� �� ������� ���������.",
              "����� ����� ����� �������� �� ������. ��� �������� ��������� �������.",
              "���������� ������������� ����� ������� ������� ������������ �����������.",
              "��������� ���� �� �������� ��� ��������. ����� �������, ���� �������� ��� ���� ������.",
              "������ ����� ������� ���������, ��� �����. �������� ������� ������� �����.",
              "����� �������� ��������� �� ��, ��� ���� ��������� ����. � ������� ��������� �� ����������� �������.",
              "������ � ����, ��� ������ ����� ��� ���� �����. ���� ����� � ���� �������.",
              "�� ������ ���������� ������ ��� ����� �������������� ��������. ��������� ������� ����� ���������."
            };
            TIPSCOLLECTION = tipscollectionRu;

            LEVELSTATISTIC = "���������� �� ������";
            RESOURCESCOLLECTED = "�������� ��������";
            DAMAGERECEIVED = "�������� �����";
            TOHULL = "�������";
            TOSHIELD = "����";
            DAMAGEDONE = "�������� �����";
            ELEMENTSDESTROYED = "���������� ���������";
            WHEELS = "�����";
            CATERPILLARS = "�������";
            BODIES = "��������";
            OTHERS = "������";
            EXPLOSIVEPARTS = "���������� ������";
            TIP = "�����";
            NEXTTIP = "�����";
            PREVIOUS = "�����";
            RAIDDONE = "��������";
            RAIDFAILED = "��������";
            WAVEISAPPROACHING = "������������ �����";
            BOSSISAPPROACHING = "������������ ����";
            COMBO = "�����";


            Dictionary<StageName, string> tutorialTextsRu = new()
            {
                { StageName.Greetings, "������, ����!\r\n���� ����� ���, � ���� ��������. � ������ ���� � ����� ������ ����� �� �������." },
                { StageName.FirstRaidLaunch, "��� ����� ������ ����\r\n� ��� ������ ��������� �������.\r\n����� ���������� �� ����." },
                { StageName.ShowWaveWarning, "���������, ��� ��� ��������! ������� �������� �� �������� ������.\r\n��������, <color=\"red\">������</color> ��� <color=\"red\">������</color>." },
                { StageName.FirstLevelCompleted, "��� ���� ����� ������!\r\n��������� ���� ����� �������, ����������?" },
                { StageName.ShowLevelStatisticPanel, "�������!\r\n�� ������� ������� ��������." },
                { StageName.SecondLevelCompleted, "����� �� �������� �� ��������� ������ ���������." },
                { StageName.FirstOpenInventory, "����� ��������� ������ ��� �������������� ���������. ����� ������� ���� ������. ����� �� ����, ����� �������." },
                { StageName.ShowUpgradeDiscription, "����� �� ������ �������� ��������������." },
                { StageName.UpgradeRotateSpeed, "������ ����� �������� �������� ��������.\r\n��� ��� ����� ������ �� ������ <color=\"green\">����������</color> 3 ����." },
                { StageName.UpgradeFireRate, "������ ����� �������� �������� ��������.\r\n������� <color=\"green\">����������</color> 2 ����." },
                { StageName.WishGoodluck, "�������, �� ������ � ���������� �����!" }
            };

            TUTORIALSTAGESTEXTS = tutorialTextsRu;
        }
    }
}

public enum Language
{
    ru,
    en
}