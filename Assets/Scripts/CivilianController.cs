using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CivilianController : MonoBehaviour
{
    public NavMeshAgent Agent;
    public GameObject Trash;

    private GameObject destinationPoints;
    private int position;

    private IEnumerator coroutine;

    private float timePast = 0;
    
    private void Start()
    {
        destinationPoints = GameObject.FindGameObjectWithTag(Tags.Respawn);
        position = (int) Random.Range(0, destinationPoints.transform.childCount);

        coroutine = WaitAndInstantiateTrash();
    }

    private void Update()
    {
        Agent.SetDestination(destinationPoints.transform.GetChild(position).position);
    }

    private void FixedUpdate()
    {
        if (Agent.remainingDistance > 0 && Agent.remainingDistance < 1)
        {
            Destroy(this.gameObject, 1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals(Tags.PlayableArea))
            StartCoroutine(coroutine);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals(Tags.PlayableArea))
            StopCoroutine(coroutine);
    }

    private IEnumerator WaitAndInstantiateTrash()
    {
        while (true)
        {
            float waitTime = Random.Range(0, 5);
            //Debug.Log("WaitAndInstantiateTrash waitTime: " + waitTime);
            
            yield return new WaitForSeconds(waitTime);
            float chanceToInstantiate = Random.Range(0, 100);
            if (chanceToInstantiate <= 10)
            {
                Instantiate(Trash, this.transform.position, Quaternion.identity);
            }
        }
    }
}
