using System.Collections;
using UnityEngine;

public class EnemyVehicleMovementController : MonoBehaviour
{
    float _inGameZoneXPos;
    float _startZPos;
    float _currentOffset;

    bool _reachGameZone = false;

    Coroutine _runAwayCoroutine;
    Coroutine _translateToPlayerCoroutine;

    public void StartTranslateToPlayer()
    {
        _startZPos = transform.position.z;
        float translateDuration = Random.Range(GameLogicParameters.Instance.MinTranslateDuration, GameLogicParameters.Instance.MaxTranslateDuration);
        _translateToPlayerCoroutine = StartCoroutine(TranslateToPlayer(transform.position.x, translateDuration));
    }

    public void MotionSimulation()
    {
        if (!_reachGameZone) return;

        float offsetX = _currentOffset * Mathf.Cos(Time.time);

        float xPos = _inGameZoneXPos + offsetX;

        transform.position = new Vector3(xPos, transform.position.y, _startZPos);
    }

    IEnumerator TranslateToPlayer(float startXPos, float translateDuration)
    {
        float randomXPos = Random.Range(-GameLogicParameters.Instance.GameZoneXSize, GameLogicParameters.Instance.GameZoneXSize);

        float t = 0;
        while (t <= 1)
        {
            if (t >= GameLogicParameters.Instance.ValueToStartSlowTranslate)
            {
                t += Time.deltaTime / translateDuration / GameLogicParameters.Instance.SlowTranslateValue;
            }
            else
            {
                t += Time.deltaTime / translateDuration;
            }
            transform.position = Vector3.Lerp(new Vector3(startXPos, transform.position.y, _startZPos), new Vector3(randomXPos, transform.position.y, _startZPos), t);
            yield return null;
        }
        _inGameZoneXPos = randomXPos;

        InvokeRepeating(nameof(ChangeXPos), 0, GameLogicParameters.Instance.ChangeSlideOffsetDelay);
        _reachGameZone = true;
        _translateToPlayerCoroutine = null;
    }

    void ChangeXPos()
    {
        StartCoroutine(LerpXPos());
    }

    IEnumerator LerpXPos()
    {
        float randomXPos = Random.Range(-GameLogicParameters.Instance.SlideOffsetXValue, GameLogicParameters.Instance.SlideOffsetXValue);
        float startOffset = _currentOffset;

        float t = 0;
        while (t <= 1)
        {
            t += Time.deltaTime / (GameLogicParameters.Instance.ChangeSlideOffsetDelay - 1);
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
        while (transform.position.x > -GameLogicParameters.Instance.XOffsetForDestroyObject)
        {
            transform.Translate(GameLogicParameters.Instance.SpeedMod * RaidManager.Instance.PlayerMoveSpeed * Time.deltaTime * -Vector3.right, Space.World);
            yield return null;
        }
        Destroy(gameObject);
    }

    IEnumerator RunAway()
    {
        yield return new WaitForSeconds(Random.Range(GameLogicParameters.Instance.MinDelayForRun, GameLogicParameters.Instance.MaxDelayForRun));
        if (_translateToPlayerCoroutine != null)
            StopCoroutine(_translateToPlayerCoroutine);

        float runSpeedMod = Random.Range(GameLogicParameters.Instance.MinRunSpeed, GameLogicParameters.Instance.MaxRunSpeed);
        bool randomDirection = Random.value < 0.5f;

        runSpeedMod = randomDirection ? -runSpeedMod : runSpeedMod;

        while (transform.position.x > -GameLogicParameters.Instance.XOffsetForDestroyObject && transform.position.x < GameLogicParameters.Instance.XOffsetForDestroyObject)
        {
            transform.Translate(GameLogicParameters.Instance.SpeedMod * RaidManager.Instance.PlayerMoveSpeed * runSpeedMod * Time.deltaTime * Vector3.right, Space.World);
            yield return null;
        }
        _runAwayCoroutine = null;
        Destroy(gameObject);
    }
}
