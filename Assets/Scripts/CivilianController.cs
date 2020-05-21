using UnityEngine;
using UnityEngine.AI;

public class CivilianController : MonoBehaviour
{
    public NavMeshAgent Agent;

    private GameObject DestinationPoints;
    private int position = 0;
    
    private void Start()
    {
        DestinationPoints = GameObject.FindGameObjectWithTag(Tags.Respawn);
        position = (int) Random.Range(0, DestinationPoints.transform.childCount);
    }

    private void Update()
    {
        Agent.SetDestination(DestinationPoints.transform.GetChild(position).position);
    }

    private void FixedUpdate()
    {
        if (Agent.remainingDistance > 0 && Agent.remainingDistance < 1)
        {
            Destroy(this.gameObject, 1f);
        }
    }
}
