using System.Collections.Generic;
using UnityEngine;

public class VehicleVisualEffectsLogic : MonoBehaviour
{
    List<Transform> _wheels;
    List<ParticleSystem> _wheelsDustPS;

    bool _isInit = false;
    public void Init(List<Transform> wheels, List<ParticleSystem> wheelsDustPS, bool autoStart = true)
    {
        _wheels = wheels;
        _wheelsDustPS = wheelsDustPS;
        SetDustsParameters();
        if (autoStart)
        {
            PlayDustPS();
        }
        _isInit = true;
    }

    public void CustomUpdate()
    {
        if(!_isInit) return;
        RotateWheels();
    }

    void RotateWheels()
    {
        foreach (var wheel in _wheels)
        {
            if (wheel == null) continue;
            wheel.Rotate(GameConfig.Instance.WheelsRotateSpeedMod * Time.deltaTime, 0, 0, Space.Self);
        }
    }

    void SetDustsParameters()
    {
        foreach (var dust in _wheelsDustPS)
        {
            if (dust == null) continue;
            var main = dust.main;
            main.startSpeed = GameConfig.Instance.SpeedMod;

            var emmision = dust.emission;
            emmision.rateOverTime = GameConfig.Instance.DustPSEmmisionRateMod;
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
