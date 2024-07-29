using System.Collections;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] float _rotateSpeed;
    [SerializeField] Transform[] _wheels;
    [SerializeField] ParticleSystem[] _wheelsDustPS;

    Vector3 _startPos;
    Vector3 _maxRightPos;
    Vector3 _maxLeftPos;
    Vector3 _offsetX = new(500,0,0);
    float _maxSlideSpeed = 100;
    float _changeDirectionMaxDelay = 5f;

    float _currentDelay;
    float _curentSpeed;


    [SerializeField] float _xOffset;
    [SerializeField] float _currentOffset;

    float _lastXPos;
    float _diferenceInPos;

    float _currentRotateSpeed;


    private void Start()
    {
        _startPos = transform.position;
        _maxRightPos = transform.position + _offsetX;
        _maxLeftPos = transform.position - _offsetX;
        if(gameObject.name == "MilitaryTruck2") return;
        _currentDelay = Random.Range(0, _changeDirectionMaxDelay);
        _curentSpeed = Random.Range(-_maxSlideSpeed, _maxSlideSpeed);
        //Invoke(nameof(ChangeDirection), _currentDelay);
        InvokeRepeating(nameof(ChangeOffset), 0, 6);
    }

    void FixedUpdate()
    {
        foreach (var wheel in _wheels)
        {
            wheel.Rotate(_rotateSpeed * Manager.GlobalMoveSpeed, 0, 0, Space.Self);
        }

        if (_wheelsDustPS != null)
            foreach (var dust in _wheelsDustPS)
            {
                var main = dust.main;
                main.startSpeed = 185 * Manager.GlobalMoveSpeed;
                var emmision = dust.emission;

                emmision.rateOverTime = 10 * Manager.GlobalMoveSpeed;
            }

        if (gameObject.name == "MilitaryTruck2") return;

        
        float offsetX = _currentOffset * Mathf.Cos(Time.time);

        float xPos = _startPos.x + offsetX;

        transform.position = new Vector3(xPos, transform.position.y, _startPos.z);


        _diferenceInPos = transform.position.x - _lastXPos;

        if (_diferenceInPos > 0)
        {            
            _lastXPos = transform.position.x;
        }
        else
        {            
            _lastXPos = transform.position.x;
        }

        Debug.Log(_diferenceInPos);

        //_currentRotateSpeed = _rotateSpeed + _diferenceInPos / _rotateSpeed;




        //float newXPos = _curentSpeed * Time.fixedDeltaTime;
        ////transform.position += _curentSpeed * Time.fixedDeltaTime * Vector3.right;

        //float clampedX = Mathf.Clamp(newXPos, - 500, 500);
        ////transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);

        //transform.Translate(new Vector3(newXPos, 0, 0), Space.World);
    }

    void ChangeOffset()
    {
        StartCoroutine(Lerp());
    }

    IEnumerator Lerp()
    {
        float _targetOffset = Random.Range(-_xOffset, _xOffset);
        float startOffset = _currentOffset;

        float t = 0;
        while (t <= 1)
        {
            t += Time.deltaTime / 5;
            _currentOffset = Mathf.Lerp(startOffset, _targetOffset, t);
            yield return null;
        }
    }


    //void ChangeDirection()
    //{
    //    _currentDelay = Random.Range(2, _changeDirectionMaxDelay);
    //    _curentSpeed = Random.Range(-_maxSlideSpeed, _maxSlideSpeed);
    //    Invoke(nameof(ChangeDirection), _currentDelay);
    //}

    
}
