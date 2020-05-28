using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class CivilianController : MonoBehaviour
{
    public NavMeshAgent Agent;
    public GameObject Trash;

    private Animator animator;

    private GameObject player;
    private GameObject waypoints;
    private GameObject[] trashBins;
    private ArrayList trashesThrown;
    private GameObject pickedUpTrash;

    private HUDController hudController;

    private int position;
    private float timePast;

    private bool isRoaming;
    private bool isAware;
    private bool isTrashPicked;

    private IEnumerator throwTrashCoroutine;

    private void Start()
    {
        animator = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag(Tags.Player);
        waypoints = GameObject.FindGameObjectWithTag(Tags.Waypoints);
        trashBins = GameObject.FindGameObjectsWithTag(Tags.TrashBin);

        if (SceneManager.GetActiveScene().buildIndex == 0)
            hudController = null;
        else
            hudController = GameObject.FindGameObjectWithTag(Tags.HUD).GetComponent<HUDController>();

        position = Random.Range(0, waypoints.transform.childCount);
        timePast = 0;

        isRoaming = true;
        isAware = false;
        isTrashPicked = false;

        trashesThrown = new ArrayList();

        throwTrashCoroutine = WaitAndInstantiateTrash(2, 5, 10);
        Agent.SetDestination(waypoints.transform.GetChild(position).position);
    }

    private void Update()
    {
        Animate();
    }

    private void FixedUpdate()
    {
        CheckIfHasReachedDestination();

        if (Agent.isStopped)
        {
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.LookRotation(player.transform.position - gameObject.transform.position), 0.1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals(Tags.PlayableArea) && !isAware)
            StartCoroutine(throwTrashCoroutine);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals(Tags.PlayableArea))
            StopCoroutine(throwTrashCoroutine);
    }

    private IEnumerator WaitAndInstantiateTrash(int min, int max, int percentage)
    {
        while (true)
        {
            float waitTime = Random.Range(min, max);

            yield return new WaitForSeconds(waitTime);

            if (Random.Range(0, 100) <= percentage)
                trashesThrown.Add(Instantiate(Trash, this.transform.position, Quaternion.identity));
        }
    }

    private void Animate()
    {
        animator.SetBool(Animations.IsStopped, Agent.isStopped);
    }

    private void CheckIfHasReachedDestination()
    {
        //Debug.Log("Distância do destino: " + Agent.remainingDistance);

        if (Agent.remainingDistance > 0 && Agent.remainingDistance < 1)
        {
            if (isRoaming)
            {
                Destroy(this.gameObject, 1f);
            }
            else if (!isTrashPicked)
            {
                Destroy(pickedUpTrash);

                if (SceneManager.GetActiveScene().buildIndex == 1)
                    hudController.AddPoints(10);

                PickUpTrash();
            }
            else
            {
                ThrowTrashToBin();
            }
        }
    }

    private bool IsThrownByThisCivilian(GameObject trash)
    {
        foreach (GameObject trashThrown in trashesThrown)
        {
            Debug.Log("VAI PEGA O LIXO OTÁRIO");
            if (trash.Equals(trashThrown))
                return true;
        }

        return false;
    }

    public void PickUpTrash()
    {
        GameObject closestTrashBin = trashBins[0];
        float closestTrashBinDistance = float.MaxValue;
        foreach(GameObject trashBin in trashBins)
        {
            float trashBinDistance = Vector3.Distance(gameObject.transform.position, trashBin.transform.position);
            if (trashBinDistance < closestTrashBinDistance)
            {
                closestTrashBinDistance = trashBinDistance;
                closestTrashBin = trashBin;
            }
        }
        Debug.Log("CATOU O LIXO DO CHÃO");
        Agent.SetDestination(closestTrashBin.transform.position);
        isTrashPicked = true;
    }

    private void ThrowTrashToBin()
    {
        Debug.Log("JOGOU O LIXO NO LIXO");

        if (SceneManager.GetActiveScene().buildIndex == 1)
            hudController.AddPoints(10);

        Agent.SetDestination(waypoints.transform.GetChild(position).position);
        isRoaming = true;
    }

    public void SetStopped(bool isStopped)
    {
        Agent.isStopped = isStopped;

        if (isStopped || isAware)
            StopCoroutine(throwTrashCoroutine);
        else
            StartCoroutine(throwTrashCoroutine);
    }

    public void CollectTrash(GameObject trashClicked)
    {
        if (IsThrownByThisCivilian(trashClicked))
        {
            pickedUpTrash = trashClicked;
            Agent.SetDestination(trashClicked.transform.position);
            isRoaming = false;
            isAware = true;
        }
        SetStopped(false);
    }
}
