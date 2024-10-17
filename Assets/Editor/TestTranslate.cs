using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestTranslate : MonoBehaviour
{
    [SerializeField] float speed;

    Rigidbody rb;

    private void Awake()
    {
        //NavMesh.avoidancePredictionTime = 5;
        rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
        rb.AddForce(Vector3.left* speed * Time.deltaTime, ForceMode.VelocityChange);
    }
}
