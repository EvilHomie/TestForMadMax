using System.Collections;
using UnityEngine;

public class GarageBoxManager : MonoBehaviour
{
    public static GarageBoxManager Instance;

    [SerializeField] Animation _animation;
    
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void OnPlayerStartRaid(float startMoveDelay)
    {
        StartCoroutine(MoveGarage(startMoveDelay));
        _animation.Play();
    }

    public void OnPlayerEndRaid()
    {
        StopAllCoroutines();
        transform.position = Vector3.zero;
        gameObject.SetActive(true);
        //_animation.Re();
    }

    IEnumerator MoveGarage(float startMoveDelay)
    {
        yield return new WaitForSeconds(startMoveDelay);
        while (transform.position.x > -15000)
        {
            transform.position += 170 * RaidManager.Instance.PlayerMoveSpeed * Time.deltaTime * Vector3.left;
            yield return null;
        }
        gameObject.SetActive(false);
    }

    
}
