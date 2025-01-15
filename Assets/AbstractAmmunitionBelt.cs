using System;
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
    protected Action _onFinishReload;
    
    public bool IsReloading => _isReloading;
    public abstract void Init(Action OnFinishReload);

    public abstract void OnStartSurviveMode(int magCapacity);

    public abstract void OnChangeMagCapacity(int magCapacity);

    public abstract void OnShoot(int bulletIndex, float fireRate);

    public abstract void OnReload(float reloadDuration);

    public abstract void DisablePanel();
}
