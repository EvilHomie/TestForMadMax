using UnityEngine;

public class CollisionLogic : MonoBehaviour
{
    [SerializeField] Collider2D Collider;
    int specificObjectID;

    //void Start()
    //{
    //    specificObjectID = gameObject.GetInstanceID();


    //    Collider.ist

    //}


    //private void OnCollisionEnter(Collision collision)
    //{

    //    if(collision.c)


    //    Debug.LogWarning(collision.gameObject.name);
    //}


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(gameObject.name);
    }



    private void OnParticleCollision(GameObject other)
    {
        Debug.Log(other.name);
    }

    private void OnParticleTrigger()
    {
        Debug.Log(gameObject.name);
    }
}
