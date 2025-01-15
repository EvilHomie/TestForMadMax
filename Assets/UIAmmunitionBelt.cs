using System;
using System.Collections;
using UnityEngine;

public class UIAmmunitionBelt : AbstractAmmunitionBelt
{
    [SerializeField] RectTransform _ammunitionBeltRT;

    int _lastMagCapacity;
    float _ammunitionBeltRTWidth;
    int _bulletIndex;
    float _bulletWidth;
    private void Start()
    {
        gameObject.SetActive(false);
    }

    public override void Init(Action OnFinishReload)
    {
        _bulletWidth = _UIbulletPF.GetComponent<RectTransform>().rect.width;
        _ammunitionBeltRTWidth = _ammunitionBeltRT.rect.width;
        _onFinishReload = OnFinishReload;
    }

    public override void OnStartSurviveMode(int magCapacity)
    {
        _bulletsImages.Clear();
        foreach (Transform child in _bulletsContainer)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < magCapacity; i++)
        {
            _bulletsImages.Add(Instantiate(_UIbulletPF, _bulletsContainer));
        }
        _lastMagCapacity = magCapacity;
        SetFullMagazine();
        gameObject.SetActive(true);
    }

    public override void OnChangeMagCapacity(int magCapacity)
    {
        int difference = magCapacity - _lastMagCapacity;
        for (int i = 0; i < difference; i++)
        {
            var bullet = Instantiate(_UIbulletPF, _bulletsContainer);
            _bulletsImages.Add(bullet);
            bullet.sprite = _filledBulletSprite;
        }
        _lastMagCapacity = magCapacity;
    }

    void SetFullMagazine()
    {
        foreach (var image in _bulletsImages)
        {
            image.sprite = _filledBulletSprite;
        }
    }

    public override void OnShoot(int bulletIndex, float fireRate)
    {
        _bulletIndex = bulletIndex;
        _bulletsImages[bulletIndex].sprite = _emptyBulletSprite;
        StartCoroutine(BeltShootAnimation(1 / fireRate));
    }

    public override void OnReload(float reloadDuration)
    {
        _isReloading = true;
        StartCoroutine(BeltReloadAnimation(reloadDuration));
    }

    public override void DisablePanel()
    {
        gameObject.SetActive(false);
    }

    IEnumerator BeltShootAnimation(float duration)
    {
        Vector3 startPos = _bulletsContainer.localPosition;
        Vector3 endPos = new(-_bulletWidth * (_bulletIndex + 1), 0, 0);
        
        //Debug.LogWarning(startPos);
        //Debug.LogWarning(endPos);
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            Vector3 pos = Vector3.Lerp(startPos, endPos, t);
            _bulletsContainer.localPosition = pos;
            yield return null;
        }
    }

    IEnumerator BeltReloadAnimation(float duration)
    {
        Vector3 startPos = _bulletsContainer.localPosition;
        //Vector3 endPos = startPos + new Vector3(_ammunitionBeltRTWidth, 0, 0);
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime / (duration / 2);
            Vector3 pos = Vector3.Lerp(startPos, new Vector3(_ammunitionBeltRTWidth, 0, 0), t);
            _bulletsContainer.localPosition = pos;
            yield return null;
        }

        SetFullMagazine();
        startPos = _bulletsContainer.localPosition;
        t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / (duration / 2);
            Vector3 pos = Vector3.Lerp(startPos, Vector3.zero, t);
            _bulletsContainer.localPosition = pos;
            yield return null;
        }
        _onFinishReload?.Invoke();
        _isReloading = false;
    }
}
