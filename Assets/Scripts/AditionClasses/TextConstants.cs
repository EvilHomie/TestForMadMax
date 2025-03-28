using System.Collections.Generic;
using YG;

public static class TextConstants
{
    public static Language Language;
    public static string HULLDMG = "Hull Damage";
    public static string DAMAGE = "Damage";
    public static string SHIELDDMG = "Shield Damage";
    public static string ROTATIONSPEED = "Rotation Speed";
    public static string FIRERATE = "FireRate";

    public static string PERHIT = "Per Hit";
    public static string DGSINSECOND = "DGS/Second";
    public static string INSECOND = "/Second";
    public static string ITEMNAME = "Item Name";

    public static string HULLHP = "Hull HP";
    public static string FULLHP = "Full HP";
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
    public static string MAX = "Max";
    public static string NEWSCHEMES = "New Schemes";
    public static string INVENTORY = "Inventory";
    public static string RAID = "Raid";
    public static string GARAGE = "Garage";
    public static string UNLOCKCOST = "UnlockCost";
    public static string NEWSCHEME = "New SCHEME";
    public static string QUICKIMPROVEMENT = "Quick improvement";
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
    public static string ACCEPT = "ACCEPT";
    public static string CANCEL = "CANCEL";
    public static string REFUSEIMPROVEMENT = "Refuse improvement";
    public static string REPAIR = "Repair";
    public static string EMERGENCYREPAIR = "Emergency repair";
    public static string CONTINUE = "Continue";
    public static string WITHOUT = "without";
    public static string SEC = "sec.";
    public static string UPGRADECOST = "Upgrade cost";

    public static string PRESSSPACE = "Press\r\nSpace";
    public static string UPGRADEDAMAGE = "Upgrade Damage";
    public static string UPGRADEFIRERATE = "Upgrade FireRate";
    public static string RELOADTIME = "ReloadTime";
    public static string UPGRADERELOADTIME = "Upgrade ReloadTime";
    public static string CAPACITY = "Capacity";
    public static string UPGRADECAPACITY = "Upgrade Capacity";
    public static string UPGRADEHULLHP = "Upgrade HullHP";
    public static string UPGRADESHIELDHP = "Upgrade ShieldHP";
    public static string UPGRADESHIELDREGRATE = "Upgrade Shield RegRate";
    public static string SURVIVEDRECORD = "Survived Record";
    public static string CURRENTSURVIVEDTIME = "CURRENT SURVIVED TIME";
    public static string NEWRECORD = "New Record";
    public static string COLLECT = "Collect";
    public static string BONUSCARDOFFERTEXT = "Collect Bonus\r\nCard?";
    public static string BONUSCARDCANCELEXT = "Continue without bonus";
    public static string ABORT = "Abort";

    public static Dictionary<StageName, string> TUTORIALSTAGESTEXTS = new()
        {
            { StageName.Greetings, "Hey, fighter!\r\nMy name is Meg, i'm your driver.\r\nI'll help you on your first raid in the Badlands." },
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

    public static Dictionary<RewardName, string> _rewardText = new()
    {
        {RewardName.RestoreHP, "Repair\r\n50% hull hp?" },
        {RewardName.FreeUpgrade, "Improve to max for watching ads?"}
    };

    public static Dictionary<string, string> LEVELSFULLNAMES = new()
    {
        {"Thug Lands", "Thug lands" }
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
            DAMAGE = "Урон";
            HULLDMG = "Урон Копусу";
            SHIELDDMG = "Урон Щитам";
            ROTATIONSPEED = "Скорость Поворота";
            FIRERATE = "Скорость Стрельбы";
            PERHIT = "Каждое Попадание";
            DGSINSECOND = "Градусов/секунду";
            INSECOND = "/секунду";
            ITEMNAME = "Имя предмета";
            HULLHP = "ХП Корпуса";
            FULLHP = "Максимальная прочность";
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
            MAX = "Max";
            NEWSCHEMES = "Новые Схемы";
            INVENTORY = "Инвентарь";
            RAID = "В Рейд";
            GARAGE = "Гараж";
            UNLOCKCOST = "Стоимость Открытия";
            NEWSCHEME = "Новая Схема";
            QUICKIMPROVEMENT = "Быстрая прокачка";

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
            ACCEPT = "Хорошо";
            CANCEL = "Нет";
            REFUSEIMPROVEMENT = "Отказ от прокачки";
            REPAIR = "Починить";
            EMERGENCYREPAIR = "Экстренный ремонт";
            CONTINUE = "Продолжить";
            WITHOUT = "без";
            SEC = "сек.";
            UPGRADECOST = "Стоимость Улучшения";
            PRESSSPACE = "Жми\r\nПробел";
            UPGRADEDAMAGE = "Улучшить Урон";
            UPGRADEFIRERATE = "Улучшить Скорострельность";
            RELOADTIME = "Время Перезарядки";
            UPGRADERELOADTIME = "Улучшить Время Перезарядки";
            CAPACITY = "Емкость";
            UPGRADECAPACITY = "Улучшить Емкость";
            UPGRADEHULLHP = "Улучшить ХП Корпуса";
            UPGRADESHIELDHP = "Улучшить ХП Щита";
            UPGRADESHIELDREGRATE = "Улучшить регенерацию щита";
            SURVIVEDRECORD = "Рекорд выживания";
            CURRENTSURVIVEDTIME = "Текущее время";
            NEWRECORD = "Новый Рекорд";
            COLLECT = "Получить";
            BONUSCARDOFFERTEXT = "Хотите получить\r\nбонусную карту?";
            BONUSCARDCANCELEXT = "Продолжить без бонуса";
            ABORT = "Прервать";


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

            Dictionary<RewardName, string> rewardTextRu = new()
            {
                {RewardName.RestoreHP, "Отремонтировать\r\n50% хп корпуса?" },
                {RewardName.FreeUpgrade, "Улучшить до максимума за просмотр рекламы?"}
            };
            _rewardText = rewardTextRu;

            Dictionary<string, string> levelsFullNames = new()
            {
                {"Thug Lands", "Земли головорезов" }
            };

            LEVELSFULLNAMES = levelsFullNames;
        }
    }
}

public enum Language
{
    ru,
    en
}