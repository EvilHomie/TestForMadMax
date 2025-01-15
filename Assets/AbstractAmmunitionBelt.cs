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

    
    public abstract void Init();

    public abstract void OnStartSurviveMode(int magCapacity);

    public abstract void OnChangeMagCapacity(int magCapacity);

    public abstract void OnShoot(int bulletIndex, float fireRate);

    public abstract void OnReload(float reloadDuration, Action OnFinishReload);

    public abstract void DisablePanel();
}
