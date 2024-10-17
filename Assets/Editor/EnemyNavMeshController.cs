using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMeshController : MonoBehaviour
{
    [SerializeField] bool SlideIsActive = false;
    NavMeshAgent agent;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        //Debug.LogWarning("1");
        //List<PosInEnemyGameZone> avaleable = EnemyGameZone.Instance.PosInEnemyGameZones.FindAll(pos => pos.IsReserved == false);
        //if (avaleable != null)
        //{
        //    Debug.LogWarning("2");
        //    int index = Random.Range(0, avaleable.Count);
        //    avaleable[index].IsReserved = true;
        //    agent.SetDestination(avaleable[index].transform.position);
        //}
        //else
        //{
        //    Debug.LogWarning("3");
        //    int index = Random.Range(0, EnemyGameZone.Instance.PosInEnemyGameZones.Count);
        //    agent.SetDestination(EnemyGameZone.Instance.PosInEnemyGameZones[index].transform.position);
        //}

        //agent.updateRotation = true;
        //agent.dis

        //int index = Random.Range(0, EnemyGameZone.Instance.PosInEnemyGameZones.Count);
        //agent.SetDestination(EnemyGameZone.Instance.PosInEnemyGameZones[index].transform.position);
        //InvokeRepeating(nameof(SetNewPos), 0, 5);
    }

    void SetNewPos()
    {
        if(!SlideIsActive) return;
        //int index = Random.Range(0, EnemyGameZone.Instance.PosInEnemyGameZones.Count);
        //agent.SetDestination(EnemyGameZone.Instance.PosInEnemyGameZones[index].transform.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.LogWarning("WDAWD");
    }
}
