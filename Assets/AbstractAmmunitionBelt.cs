using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractAmmunitionBelt : MonoBehaviour
{
    [SerializeField] protected Sprite _filledBulletSprite;
    [SerializeField] protected Sprite _emptyBulletSprite;
    [SerializeField] protected RectTransform _bulletsContainer;
    [SerializeField] protected Image _UIbulletPF;

    protected List<Image> _bulletsImages = new();

    protected bool _isReloading = false;

    public bool IsReload => _isReloading;

    public abstract void Init();

    public abstract void OnStartSurviveMode(int magCapacity);

    public abstract void OnChangeMagCapacity(int magCapacity);

    public abstract void OnShoot(int leftBulletsCount, float fireRate);

    public abstract void OnReload(float reloadDuration = 0);

    public abstract void DisablePanel();
}
