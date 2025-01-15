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

    public override void OnStartSurviveMode(int magCapacity)
    {
        _bulletsImages.Clear();
        SetFullMagazine();
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
            _bulletsImages.Add(Instantiate(_UIbulletPF, _bulletsContainer));
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
        _bulletsImages[bulletIndex].sprite = _emptyBulletSprite;
    }

    public override void OnReload(float reloadDuration, Action OnFinishReload)
    {
        StartCoroutine(ReloadLogic(reloadDuration, OnFinishReload));
    }

    public override void DisablePanel()
    {
        gameObject.SetActive(false);
    }

    IEnumerator ReloadLogic(float reloadDuration, Action OnFinishReload)
    {
        float t = 0;
        while (t < reloadDuration)
        {
            t += Time.deltaTime;
            _reloadIcon.Rotate(Vector3.forward, _reloadIconRotateSpeed * Time.deltaTime);
            yield return null;
        }
        SetFullMagazine();
        OnFinishReload?.Invoke();
    }

}
