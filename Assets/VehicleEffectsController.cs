using UnityEngine;

public class VehicleEffectsController : MonoBehaviour
{
    [SerializeField] Transform[] _wheels;
    [SerializeField] ParticleSystem[] _wheelsDustPS;
    float _lastMoveSpeed = 0;

    public void PlayMoveEffects()
    {
        TrackSpeed();
        RotateWheels();
    }

    void TrackSpeed()
    {
        if (_lastMoveSpeed != RaidManager.Instance.PlayerMoveSpeed)
        {
            _lastMoveSpeed = RaidManager.Instance.PlayerMoveSpeed;
            UpdateDustsSettings(_lastMoveSpeed);
        }
    }

    void RotateWheels()
    {
        foreach (var wheel in _wheels)
        {
            if (wheel == null) continue;
            wheel.Rotate(_lastMoveSpeed * GameConfig.Instance.WheelsRotateSpeedMod * Time.deltaTime, 0, 0, Space.Self);
        }
    }

    void UpdateDustsSettings(float newSpeed)
    {
        foreach (var dust in _wheelsDustPS)
        {
            if (dust == null) continue;
            var main = dust.main;
            main.startSpeed = GameConfig.Instance.SpeedMod * newSpeed;

            var emmision = dust.emission;
            emmision.rateOverTime = GameConfig.Instance.DustPSEmmisionRateMod * newSpeed;
        }
    }

    public void StopDustEmmiting()
    {
        foreach (var dust in _wheelsDustPS)
        {
            if (dust == null) continue;
            var emmision = dust.emission;
            emmision.enabled = false;
        }
    }

    public void CutDustPS()
    {
        foreach (var dust in _wheelsDustPS)
        {
            dust.Stop();
        }
    }

    public void PlayDustPS()
    {
        foreach (var dust in _wheelsDustPS)
        {
            dust.Play();
        }
    }
}
