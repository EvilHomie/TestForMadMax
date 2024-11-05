using System.Collections;
using UnityEngine;

public class EnemySpawnPos : MonoBehaviour
{
    bool _reservedStatus;

    public bool ReservedStatus => _reservedStatus;

    public void ChangeStatus()
    {
        _reservedStatus = true;
        StartCoroutine(ResetStatus());
    }

    public void ResetStatusImmediately()
    {
        StopAllCoroutines();
        _reservedStatus = false;
    }

    IEnumerator ResetStatus()
    {
        yield return new WaitForSeconds(3);
        _reservedStatus = false;
    }
}
