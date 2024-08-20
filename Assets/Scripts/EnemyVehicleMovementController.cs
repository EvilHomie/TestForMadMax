using System.Collections;
using UnityEngine;

public class EnemyVehicleMovementController : MonoBehaviour
{
    float _inGameZoneXPos;
    float _startZPos;
    float _currentOffset;

    bool _reachGameZone = false;
    bool _tryRun = false;

    Coroutine _runAwayCoroutine;
    Coroutine _translateToPlayerCoroutine;

    public void StartTranslateToGameZone()
    {
        _startZPos = transform.position.z;
        float translateDuration = Random.Range(GameConfig.Instance.MinTranslateDuration, GameConfig.Instance.MaxTranslateDuration);
        _translateToPlayerCoroutine = StartCoroutine(TranslateToPlayer(transform.position.x, translateDuration));
    }

    public void MotionSimulation()
    {
        if (!_reachGameZone || _tryRun) return;

        float offsetX = _currentOffset * Mathf.Cos(Time.time);

        float xPos = _inGameZoneXPos + offsetX;

        transform.position = new Vector3(xPos, transform.position.y, _startZPos);
    }

    IEnumerator TranslateToPlayer(float startXPos, float translateDuration)
    {
        float randomXPos = Random.Range(-GameConfig.Instance.GameZoneXSize, GameConfig.Instance.GameZoneXSize);

        float t = 0;
        while (t <= 1)
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
        _inGameZoneXPos = randomXPos;

        InvokeRepeating(nameof(ChangeXPos), 0, GameConfig.Instance.ChangeSlideOffsetDelay);
        _reachGameZone = true;
        _translateToPlayerCoroutine = null;
    }

    void ChangeXPos()
    {
        StartCoroutine(LerpXPos());
    }

    IEnumerator LerpXPos()
    {
        float randomXPos = Random.Range(-GameConfig.Instance.SlideOffsetXValue, GameConfig.Instance.SlideOffsetXValue);
        float startOffset = _currentOffset;

        float t = 0;
        while (t <= 1)
        {
            t += Time.deltaTime / (GameConfig.Instance.ChangeSlideOffsetDelay - 1);
            _currentOffset = Mathf.Lerp(startOffset, randomXPos, t);
            yield return null;
        }
    }

    public void OnDieMovementLogic()
    {
        if (_translateToPlayerCoroutine != null)
            StopCoroutine(_translateToPlayerCoroutine);

        if (_runAwayCoroutine != null)
            StopCoroutine(_runAwayCoroutine);

        StartCoroutine(MoveOnDie());
    }

    public void OnRunMovementLogic()
    {
        if (_translateToPlayerCoroutine != null)
            StopCoroutine(_translateToPlayerCoroutine);
        _runAwayCoroutine = StartCoroutine(RunAway());
    }

    IEnumerator MoveOnDie()
    {
        while (transform.position.x > -GameConfig.Instance.XOffsetForDestroyObject)
        {
            transform.Translate(GameConfig.Instance.SpeedMod * RaidManager.Instance.PlayerMoveSpeed * Time.deltaTime * -Vector3.right, Space.World);
            yield return null;
        }
        Destroy(gameObject);
    }

    IEnumerator RunAway()
    {
        yield return new WaitForSeconds(Random.Range(GameConfig.Instance.MinDelayForRun, GameConfig.Instance.MaxDelayForRun));
        if (_translateToPlayerCoroutine != null)
            StopCoroutine(_translateToPlayerCoroutine);
        _tryRun = true;

        float runSpeedMod = Random.Range(GameConfig.Instance.MinRunSpeed, GameConfig.Instance.MaxRunSpeed);
        bool randomDirection = Random.value < 0.5f;

        runSpeedMod = randomDirection ? -runSpeedMod : runSpeedMod;

        while (transform.position.x > -GameConfig.Instance.XOffsetForDestroyObject && transform.position.x < GameConfig.Instance.XOffsetForDestroyObject)
        {
            transform.Translate(GameConfig.Instance.SpeedMod * RaidManager.Instance.PlayerMoveSpeed * runSpeedMod * Time.deltaTime * Vector3.right, Space.World);
            yield return null;
        }
        _runAwayCoroutine = null;
        Destroy(gameObject);
    }
}
