using System.Collections;
using UnityEngine;

public class EnemyVehicleMovementController : MonoBehaviour
{
    EnemyVehicleManager _enemyVehicleManager;


    float _inGameZoneDeffXPos;
    float _startZPos;
    float _currentOffset;

    Coroutine _slidingCoroutine;
    Coroutine _translateToGameZoneCoroutine;

    public void StartMovementLogic(EnemyVehicleManager enemyVehicleManager)
    {
        _enemyVehicleManager = enemyVehicleManager;
        _startZPos = transform.position.z;
        _translateToGameZoneCoroutine = StartCoroutine(TranslateInGameZone());
    }

    IEnumerator TranslateInGameZone()
    {
        float startXPos = transform.position.x;
        float translateDuration = Random.Range(GameConfig.Instance.MinTranslateDuration, GameConfig.Instance.MaxTranslateDuration);
        float randomXPos = Random.Range(-GameConfig.Instance.GameZoneXSize, GameConfig.Instance.GameZoneXSize);
        _inGameZoneDeffXPos = randomXPos;

        float t = 0;
        while (t < 1)
        {
            if (t >= GameConfig.Instance.ValueToStartSlowTranslate)
            {
                t += Time.deltaTime / translateDuration / GameConfig.Instance.SlowTranslateValue;
            }
            else
            {
                t += Time.deltaTime / translateDuration;
            }
            transform.position = Vector3.Lerp(new Vector3(startXPos, transform.position.y, _startZPos), new Vector3(randomXPos, transform.position.y, _startZPos), t);
            yield return null;
        }
        _translateToGameZoneCoroutine = null;
        OnReachGameZone();
    }

    void OnReachGameZone()
    {
        _enemyVehicleManager.OnReachGameZone();
        InvokeRepeating(nameof(ChangeSlideOffset), 0, GameConfig.Instance.ChangeSlideOffsetDelay);
        _slidingCoroutine = StartCoroutine(CosineSliding());
    }

    IEnumerator CosineSliding()
    {
        while (true) 
        {
            float offsetX = _currentOffset * Mathf.Cos(Time.time);
            float xPos = _inGameZoneDeffXPos + offsetX;
            transform.position = new Vector3(xPos, transform.position.y, _startZPos);
            yield return null;
        }
    }

    void ChangeSlideOffset()
    {
        StartCoroutine(LerpSlideOffset());
    }

    IEnumerator LerpSlideOffset()
    {
        float startOffset = _currentOffset;
        float slideMod = LevelManager.Instance.GetSelectedLevelinfo().EnemySlideDistanceMod;
        float newOffset = Random.Range(-GameConfig.Instance.SlideOffsetXValue * slideMod, GameConfig.Instance.SlideOffsetXValue * slideMod);

        float t = 0;
        while (t <= 1)
        {
            t += Time.deltaTime / (GameConfig.Instance.ChangeSlideOffsetDelay);
            _currentOffset = Mathf.Lerp(startOffset, newOffset, t);
            yield return null;
        }
    }

    public void StartDieLogic()
    {
        CancelInvoke();
        StopAllCoroutines();
        StartCoroutine(OnDieTranslation());
    }

    public void OnTryRunMovementLogic()
    {
        StartCoroutine(RunAwayTranslation());
    }

    IEnumerator OnDieTranslation()
    {
        float t = 0;
        float mod;

        while (transform.position.x > -GameConfig.Instance.XOffsetForDestroyObject)
        {
            t += Time.deltaTime/2;
            mod = Mathf.Lerp(0,1,t);

            transform.Translate(mod * GameConfig.Instance.SpeedMod * RaidManager.Instance.PlayerMoveSpeed * Time.deltaTime * Vector3.left, Space.World);
            yield return null;
        }
        Destroy(gameObject);
    }

    IEnumerator RunAwayTranslation()
    {
        yield return new WaitForSeconds(Random.Range(GameConfig.Instance.MinDelayForRun, GameConfig.Instance.MaxDelayForRun));

        CancelInvoke();

        if (_translateToGameZoneCoroutine != null) StopCoroutine(_translateToGameZoneCoroutine);
        if (_slidingCoroutine != null) StopCoroutine(_slidingCoroutine);

        float runSpeedMod = Random.Range(GameConfig.Instance.MinRunSpeed, GameConfig.Instance.MaxRunSpeed);
        bool randomDirection = Random.value < 0.5f;

        runSpeedMod = randomDirection ? -runSpeedMod : runSpeedMod;

        while (transform.position.x > -GameConfig.Instance.XOffsetForDestroyObject && transform.position.x < GameConfig.Instance.XOffsetForDestroyObject)
        {
            transform.Translate(GameConfig.Instance.SpeedMod * RaidManager.Instance.PlayerMoveSpeed * runSpeedMod * Time.deltaTime * Vector3.right, Space.World);
            yield return null;
        }

        Destroy(gameObject);
    }
}
