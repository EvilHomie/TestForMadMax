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
    public static string UPGRADE = "Upgrade";
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
            HULLDMG = "Урон Копусу";
            SHIELDDMG = "Урон Щитам";
            ROTATIONSPEED = "Скорость Поворота";
            FIRERATE = "Скорость Стрельбы";
            PERHIT = "Каждое Попадание";
            DGSINSECOND = "Градусов/секунду";
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
            COST = "Стоимость";
            UPGRADE = "Улучшить";
            MAXLEVEL = "Макс Уровень";
            NEWSCHEMES = "Новые Схемы";
            INVENTORY = "Склад";
            RAID = "В Рейд";
            GARAGE = "Гараж";
            UNLOCKCOST = "Стоимость Открытия";
            NEWSCHEME = "Новая Схема";

            string[] tipscollectionRu =
            { "Не забывайте улучшать свое снаряжение во вкладке склад.",
              "Новые схемы можно получить из врагов. Для открытия требуются ресурсы.",
              "разрушение взрывоопасной части нанесет корпусу значительные повреждения.",
              "Убегающий враг не принесет вам ресурсов. Враги убегают, если потеряли все свое оружие.",
              "колеса имеют меньшую прочность, чем кузов. Гусеница гораздо прочнее колес.",
              "Синее мерцание указывает на то, что урон наносится щиту. А красное указывает на повреждение корпуса.",
              "Имейте в виду, что оружие имеет два типа урона. Урон щитам и урон корпусу.",
              "Вы можете перепройти уровни для сбора дополнительных ресурсов. Автосмена уровней будет отключена."
            };
            TIPSCOLLECTION = tipscollectionRu;

            LEVELSTATISTIC = "Статистика по уровню";
            RESOURCESCOLLECTED = "Собранно ресурсов";
            DAMAGERECEIVED = "Получено урона";
            TOHULL = "Корпусу";
            TOSHIELD = "Щиту";
            DAMAGEDONE = "Нанесено урона";
            ELEMENTSDESTROYED = "Уничтожено Элементов";
            WHEELS = "Колес";
            CATERPILLARS = "Гусениц";
            BODIES = "Корпусов";
            OTHERS = "Других";
            EXPLOSIVEPARTS = "Взрывчатых частей";
            TIP = "Совет";
            NEXTTIP = "Далее";
            PREVIOUS = "Назад";
            RAIDDONE = "Пройдено";
            RAIDFAILED = "Неудачно";

        }

    }
}

public enum Language
{
    ru,
    en
}