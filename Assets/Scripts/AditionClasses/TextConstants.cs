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
            { StageName.UpgradeRotateSpeed, "The first thing to do is to increase the rotation speed.\r\nTo do this, press the <color=\"green\">“Improve”</color> button 3 times." },
            { StageName.UpgradeFireRate, "Now you need to improve your shooting speed.\r\nPress <color=\"green\">“Improve”</color> 2 times." },
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
            INVENTORY = "Инвентарь";
            RAID = "В Рейд";
            GARAGE = "Гараж";
            UNLOCKCOST = "Стоимость Открытия";
            NEWSCHEME = "Новая Схема";

            string[] tipscollectionRu =
            { "Не забывайте улучшать свое снаряжение во вкладке Инвентарь.",
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
            WAVEISAPPROACHING = "Приближается Волна";
            BOSSISAPPROACHING = "Приближается Босс";
            COMBO = "Комбо";


            Dictionary<StageName, string> tutorialTextsRu = new()
            {
                { StageName.Greetings, "Привет, боец!\r\nМеня зовут Мэг, я твой водитель. Я помогу тебе в твоем первом рейде по Пустоши." },
                { StageName.FirstRaidLaunch, "Наш склад сейчас пуст\r\nи нам срочно требуются ресурсы.\r\nДавай прокатимся за ними." },
                { StageName.ShowWaveWarning, "Осторожно, нас уже заметили! Советую стрелять по уязвимым местам.\r\nНапример, <color=\"red\">колеса</color> или <color=\"red\">балоны</color>." },
                { StageName.FirstLevelCompleted, "Это было очень просто!\r\nСледующий рейд будет сложнее, справишься?" },
                { StageName.ShowLevelStatisticPanel, "Отлично!\r\nМы собрали немного ресурсов." },
                { StageName.SecondLevelCompleted, "Давай их потратим на улучшение нашего инвентаря." },
                { StageName.FirstOpenInventory, "Здесь находится панель уже установленного инвентаря. Давай улучшим наше оружие. Нажми на него, чтобы выбрать." },
                { StageName.ShowUpgradeDiscription, "Здесь вы можете улучшить характеристики." },
                { StageName.UpgradeRotateSpeed, "Первым делом увеличим скорость поворота.\r\nДля это нужно нажать на кнопку <color=\"green\">“Улучшить”</color> 3 раза." },
                { StageName.UpgradeFireRate, "Теперь нужно улучшить скорость стрельбы.\r\nНажимай <color=\"green\">“Улучшить”</color> 2 раза." },
                { StageName.WishGoodluck, "Отлично, мы готовы к следующему рейду!" }
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