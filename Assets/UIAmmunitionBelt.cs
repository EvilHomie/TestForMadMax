using System.Collections;
using UnityEngine;

public class UIAmmunitionBelt : AbstractAmmunitionBelt
{
    [SerializeField] RectTransform _ammunitionBeltRT;

    int _lastMagCapacity;
    float _ammunitionBeltRTWidth;
    float _bulletWidth;

    public override void Init()
    {
        _bulletWidth = _UIbulletPF.GetComponent<RectTransform>().rect.width;
        _ammunitionBeltRTWidth = _ammunitionBeltRT.rect.width;
        gameObject.SetActive(false);
    }

    public override void OnStartSurviveMode(NewWeaponData weaponData)
    {
        _weaponData =  weaponData;
        _bulletsImages.Clear();
        foreach (Transform child in _bulletsContainer)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < _weaponData.magCapacity; i++)
        {
            _bulletsImages.Add(Instantiate(_UIbulletPF, _bulletsContainer));
        }
        _lastMagCapacity = _weaponData.magCapacity;
        SetFullMagazine();
        _weaponData.bulletInMagLeft = _weaponData.magCapacity;
        _bulletsContainer.localPosition = Vector3.zero;
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
            var bullet = Instantiate(_UIbulletPF, _bulletsContainer);
            _bulletsImages.Add(bullet);
            bullet.sprite = _filledBulletSprite;
        }
        _lastMagCapacity = _weaponData.magCapacity;
    }

    void SetFullMagazine()
    {
        foreach (var image in _bulletsImages)
        {
            image.sprite = _filledBulletSprite;
        }
    }

    public override void OnShoot()
    {
        _bulletIndex = _weaponData.magCapacity - _weaponData.bulletInMagLeft;
        _bulletsImages[_bulletIndex].sprite = _emptyBulletSprite;
        StartCoroutine(BeltShootAnimation(1 / _weaponData.fireRate));
    }

    public override void OnReload()
    {
        StartCoroutine(BeltReloadAnimation());
    }

    public override void DisablePanel()
    {
        gameObject.SetActive(false);
    }

    IEnumerator BeltShootAnimation(float duration)
    {
        Vector3 startPos = _bulletsContainer.localPosition;
        Vector3 endPos = new(-_bulletWidth * (_bulletIndex + 1), 0, 0);
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            Vector3 pos = Vector3.Lerp(startPos, endPos, t);
            _bulletsContainer.localPosition = pos;
            yield return null;
        }
    }

    IEnumerator BeltReloadAnimation()
    {
        _weaponData.isReloading = true;
        _weaponData.bulletInMagLeft = 0;
        Vector3 startPos = _bulletsContainer.localPosition;
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime / (_weaponData.reloadTime / 2);
            Vector3 pos = Vector3.Lerp(startPos, new Vector3(_ammunitionBeltRTWidth, 0, 0), t);
            _bulletsContainer.localPosition = pos;
            yield return null;
        }

        SetFullMagazine();
        startPos = _bulletsContainer.localPosition;
        t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / (_weaponData.reloadTime / 2);
            Vector3 pos = Vector3.Lerp(startPos, Vector3.zero, t);
            _bulletsContainer.localPosition = pos;
            yield return null;
        }
        _weaponData.bulletInMagLeft = _weaponData.magCapacity;
        _weaponData.isReloading = false;
    }
}
