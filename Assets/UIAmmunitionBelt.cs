using System.Collections;
using UnityEngine;

public class UIAmmunitionBelt : AbstractAmmunitionBelt
{
    public static UIAmmunitionBelt Instance;

    [SerializeField] RectTransform _ammunitionBeltRT;
    
    int _lastMagCapacity;
    float _ammunitionBeltRTWidth;
    int _loadedBulletNumber;
    float _bulletWidth;    

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
        gameObject.SetActive(false);
    }

    public override void Init()
    {        
        _bulletWidth = _UIbulletPF.GetComponent<RectTransform>().rect.width;
        _ammunitionBeltRTWidth = _ammunitionBeltRT.rect.width;
    }

    public override void OnStartSurviveMode(int magCapacity)
    {
        _isReloading = false;
        _loadedBulletNumber = 1;
        _bulletsImages.Clear();
        foreach (Transform child in _bulletsContainer) Destroy(child.gameObject);
        for (int i = 0; i < magCapacity; i++) _bulletsImages.Add(Instantiate(_UIbulletPF, _bulletsContainer));
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

    public override void OnShoot(int leftBulletsCount, float fireRate)
    {
        _bulletsImages[_loadedBulletNumber - 1].sprite = _emptyBulletSprite;
        StartCoroutine(BeltShootAnimation(1 / fireRate));
        _loadedBulletNumber++;
    }

    public override void OnReload(float reloadDuration = 0)
    {
        StartCoroutine(BeltReloadAnimation(reloadDuration));
    }

    public override void DisablePanel()
    {
        gameObject.SetActive(false);
    }

    IEnumerator BeltShootAnimation(float duration)
    {
        Vector3 startPos = _bulletsContainer.localPosition;
        Vector3 endPos = new(-_bulletWidth * _loadedBulletNumber, 0, 0);
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
        _isReloading = true;
        Vector3 startPos = _bulletsContainer.localPosition;
        Vector3 endPos = startPos + new Vector3(_ammunitionBeltRTWidth / 2, 0, 0);
        float t = 0;

        while (t < 0.5f)
        {
            t += Time.deltaTime / duration;
            Vector3 pos = Vector3.Lerp(startPos, endPos, t);
            _bulletsContainer.localPosition = pos;
            yield return null;
        }

        SetFullMagazine();
        startPos = _bulletsContainer.localPosition;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            Vector3 pos = Vector3.Lerp(startPos, Vector3.zero, t);
            _bulletsContainer.localPosition = pos;
            yield return null;
        }
        _loadedBulletNumber = 1;
        _isReloading = false;
    }
}
