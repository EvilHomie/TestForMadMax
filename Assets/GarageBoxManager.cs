using System.Collections;
using UnityEngine;

public class GarageBoxManager : MonoBehaviour
{
    public static GarageBoxManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void OnPlayerStartRaid()
    {
        StartCoroutine(MoveGarage());
    }

    public void OnPlayerEndRaid()
    {
        StopAllCoroutines();
        transform.position = Vector3.zero;
        gameObject.SetActive(true);
    }

    IEnumerator MoveGarage()
    {
        while (transform.position.x > -10000)
        {
            transform.position += 170 * RaidObjectsManager.Instance.PlayerMoveSpeed * Time.deltaTime * Vector3.left;
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
