public class SurviveModeExpManager
{
    int _killedEnemiesCount;
    ModeDifficult modeDifficultData;

    int _newWeaponLvlsCount;

    public SurviveModeExpManager(ModeDifficult modeDifficult)
    {
        modeDifficultData = modeDifficult;
        UIExpPresentationManager.Instance.Init(modeDifficultData.killAmountForLvlUp);
    }

    public void OnStartSurviveMode()
    {
        _killedEnemiesCount = 0;
        _newWeaponLvlsCount = 0;
        UIExpPresentationManager.Instance.OnStartSurviveMode();
    }

    public void OnEnemyKilled()
    {
        _killedEnemiesCount++;

        if ((_killedEnemiesCount + _newWeaponLvlsCount) % modeDifficultData.killAmountForNewWeapon == 0)
        {
            if (GiveNewWeapon())
            {
                _newWeaponLvlsCount++;
                return;
            }            
        }

        if ((_killedEnemiesCount + _newWeaponLvlsCount) % modeDifficultData.killAmountForLvlUp == 0)
        {
            OnPlayerLvlUp();
        }

        UIExpPresentationManager.Instance.OnKillEnemy(_killedEnemiesCount - _newWeaponLvlsCount);
    }

    void OnPlayerLvlUp()
    {
        SurviveModeUpgradePanel.Instance.OnPlayerLevelUp();
    }

    public bool GiveNewWeapon()
    {
        return SurviveModeManager.Instance.OnChangeWeapon();
    }
}
