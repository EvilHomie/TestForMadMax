public static class TextConstants
{
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
    public static string UPGRADE = "Upgrade";
    public static string MAXLEVEL = "MaxLevel";
    public static string NEWSCHEMES = "New Schemes";
    public static string INVENTORY = "Inventory";
    public static string RAID = "Raid";
    public static string GARAGE = "Garage";



    public static void SetLanguage(Language language)
    {
        if (language == Language.en) return;
        else if (language == Language.ru)
        {
            HULLDMG = "Урон Копусу";
            SHIELDDMG = "Урон Щитам";
            ROTATIONSPEED = "Скорость Поворота";
            FIRERATE = "Скорость Стрельбы";
            PERHIT = "Каждое Попадание";
            DGSINSECOND = "Градус/секунду";
            INSECOND = "/секунду";
            ITEMNAME = "Имя предмета";
            HULLHP = "ХП Корпуса";
            SHIELDHP = "ХП Щита";
            SHIELREGENRATE = "Регенерация Щита";
            WEAPONSCOUNT = "Количество Орудий";
            UNIT = "Едениц";
            FROM = "Из";
            UNLOCKED = "Открыт";
            LOCKED = "Закрыт";
            LEVELS = "Уровни";
            EQUIP = "Оборудовать";
            UNLOCK = "Открыть";
            CHARACTERISTICS = "Характеристики";
            UPGRADES = "Улучшения";
            LEVEL = "Уровень";
            UPEFFECT = "Эффект";
            COST = " Стоимость";
            UPGRADE = " Улучшить";
            MAXLEVEL = " Макс Уровень";
            NEWSCHEMES = "Новые Схемы";
            INVENTORY = "Инвентарь";
            RAID = "Набег";
            GARAGE = "Гараж";
        }
    }
}

public enum Language
{
    ru,
    en
}