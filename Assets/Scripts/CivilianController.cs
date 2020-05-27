using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CivilianController : MonoBehaviour
{
    public NavMeshAgent Agent;
    public GameObject Trash;

    private GameObject player;
    private GameObject destinationPoints;
    private Animator animator;
    private int position;
    private float timePast;
    public bool isTalking;
    private bool isRoaming;

    private IEnumerator coroutine;

    private void Start()
    {
        isTalking = false;

        player = GameObject.FindGameObjectWithTag(Tags.Player);
        destinationPoints = GameObject.FindGameObjectWithTag(Tags.Respawn);
        animator = GetComponent<Animator>();
        position = (int) Random.Range(0, destinationPoints.transform.childCount);
        timePast = 0;

        coroutine = WaitAndInstantiateTrash();

        Agent.SetDestination(destinationPoints.transform.GetChild(position).position);
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (Agent.remainingDistance > 0 && Agent.remainingDistance < 1)
        {
            Destroy(this.gameObject, 1f);
        }
    }

    private void Move()
    {

    }

    public void Talk(bool isTalking)
    {
        Agent.isStopped = isTalking;
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
