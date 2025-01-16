using System;
using System.Collections;
using UnityEngine;

public class WeaponMagazinePresentation : AbstractAmmunitionBelt
{
    [SerializeField] Transform _reloadIcon;
    [SerializeField] float _reloadIconRotateSpeed;
    int _lastMagCapacity;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public override void Init()
    {
    }

    public override void OnStartSurviveMode(NewWeaponData currentWeaponData)
    {
        _weaponData = currentWeaponData;
        _bulletsImages.Clear();
        SetFullMagazine();
        _bulletsImages.Clear();
        foreach (Transform child in _bulletsContainer) Destroy(child.gameObject);
        for (int i = 0; i < _weaponData.magCapacity; i++) _bulletsImages.Add(Instantiate(_UIbulletPF, _bulletsContainer));
        _lastMagCapacity = _weaponData.magCapacity;
        SetFullMagazine();
        gameObject.SetActive(true);
    }

    public override void OnChangeWeapon(NewWeaponData currentWeaponData)
    {
        _weaponData = currentWeaponData;
    }

    public override void OnChangeMagCapacity()
    {
        int difference = _weaponData.magCapacity - _lastMagCapacity;
        _weaponData.bulletInMagLeft += difference;
        for (int i = 0; i < difference; i++)
        {
            _bulletsImages.Add(Instantiate(_UIbulletPF, _bulletsContainer));
        }
        _lastMagCapacity = _weaponData.magCapacity;
    }

    void SetFullMagazine()
    {
        foreach (var image in _bulletsImages)
        {
            image.sprite = _filledBulletSprite;
        }
        _weaponData.bulletInMagLeft = _weaponData.magCapacity;
    }

    public override void OnShoot()
    {
        _bulletIndex = _weaponData.magCapacity - _weaponData.bulletInMagLeft;
        _bulletsImages[_bulletIndex].sprite = _emptyBulletSprite;
    }

    public override void OnReload()
    {
        StartCoroutine(ReloadLogic());
    }

    public override void DisablePanel()
    {
        gameObject.SetActive(false);
    }

    IEnumerator ReloadLogic()
    {
        _weaponData.isReloading = true;
        _weaponData.bulletInMagLeft = 0;
        float t = 0;
        while (t < _weaponData.reloadTime)
        {
            t += Time.deltaTime;
            _reloadIcon.Rotate(Vector3.forward, _reloadIconRotateSpeed * Time.deltaTime);
            yield return null;
        }
        SetFullMagazine();
        
        _weaponData.isReloading = false;
    }

}
