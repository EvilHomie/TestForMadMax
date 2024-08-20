using UnityEngine;

public class WeaponPoint : MonoBehaviour
{
    [SerializeField] int _index;
    public int Index => _index;
    public Transform Transform => gameObject.transform;
}
