using System.Collections;
using UnityEngine;

public class OldHangarManager : MonoBehaviour
{
    public static OldHangarManager Instance;

    [SerializeField] Animation _animation;
    [SerializeField] float _speed;

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
    }

    IEnumerator MoveGarage(float startMoveDelay)
    {
        yield return new WaitForSeconds(startMoveDelay);
        while (transform.position.x > -15000)
        {
            transform.position += _speed * InRaidManager.Instance.PlayerMoveSpeed * Time.deltaTime * Vector3.left;
            yield return null;
        }
        gameObject.SetActive(false);
    }

    public void DisableGarage()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }
}
