using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponMagazinePresentation : MonoBehaviour
{
    public static WeaponMagazinePresentation Instance;

    [SerializeField] Transform _bulletsContainer;
    [SerializeField] Image _UIbulletPF;
    [SerializeField] Transform _reloadIcon;
    [SerializeField] float _reloadIconRotateSpeed;
    [SerializeField] Sprite _filledBulletSprite;
    [SerializeField] Sprite _emptyBulletSprite;

    List<Image> _bulletsImages = new();
    int _lastMagCapacity;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
        gameObject.SetActive(false);
    }

    public void Init(int magCapacity)
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

    public void OnChangeMagCapacity(int magCapacity)
    {
        int difference = magCapacity - _lastMagCapacity;
        for (int i = 0; i < difference; i++)
        {
            _bulletsImages.Add(Instantiate(_UIbulletPF, _bulletsContainer));
        }
        _lastMagCapacity = magCapacity;
    }

    public void SetFullMagazine()
    {
        foreach (var image in _bulletsImages)
        {
            image.sprite = _filledBulletSprite;
        }
    }

    public void OnShoot(int leftBulletsCount)
    {
        _bulletsImages[leftBulletsCount].sprite = _emptyBulletSprite;
    }

    public void ReloadAnimation()
    {
        _reloadIcon.Rotate(Vector3.forward, _reloadIconRotateSpeed * Time.deltaTime);
    }

    public void DisablePanel()
    {
        gameObject.SetActive(false);
    }

}
