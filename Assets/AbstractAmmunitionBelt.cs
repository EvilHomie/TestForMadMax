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
    protected NewWeaponData _weaponData;
    protected int _bulletIndex;

    public abstract void Init();

    public abstract void OnStartSurviveMode(NewWeaponData currentWeaponData);

    public abstract void OnChangeMagCapacity();

    public abstract void OnChangeWeapon(NewWeaponData currentWeaponData);

    public abstract void OnShoot();

    public abstract void OnReload();

    public abstract void DisablePanel();
}
