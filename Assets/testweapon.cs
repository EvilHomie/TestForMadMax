using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testweapon : MonoBehaviour
{
    [SerializeField] AbstractWeapon weapon;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            weapon.StartShoot();
        }
    }
}
