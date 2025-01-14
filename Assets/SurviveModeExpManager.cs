public class SurviveModeExpManager
{
    int _killedEnemiesCount;
    ModeDifficult modeDifficultData;

    int _newWeaponCounter;

    public SurviveModeExpManager(ModeDifficult modeDifficult)
    {
        modeDifficultData = modeDifficult;
    }

    public void OnStartSurviveMode()
    {
        _killedEnemiesCount = 0;
        _newWeaponCounter = 0;
    }

    public void OnEnemyKilled()
    {
        _killedEnemiesCount++;

        if (_killedEnemiesCount % modeDifficultData.killAmountForNewWeapon == 0)
        {
            _newWeaponCounter++;
            GiveNewWeapon();
            return;
        }

        if ((_killedEnemiesCount + _newWeaponCounter) % modeDifficultData.killAmountForLvlUp == 0)
        {
            OnPlayerLvlUp();
        }
    }

    void OnPlayerLvlUp()
    {
        SurviveModeUpgradePanel.Instance.OnPlayerLevelUp();
    }

    public void GiveNewWeapon()
    {
        SurviveModeManager.Instance.OnChangeWeapon();
    }
}
