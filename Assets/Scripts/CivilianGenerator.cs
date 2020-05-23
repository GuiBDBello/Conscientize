using System.Collections;
using UnityEngine;

public class CivilianGenerator : MonoBehaviour
{
    public GameObject Civilian;
    public GameObject SpawnPoints;
    public float waitTime;

    private IEnumerator coroutine;

    private void Start()
    {
        coroutine = WaitAndInstantiateCivilian(waitTime);
        StartCoroutine(coroutine);
    }

    private IEnumerator WaitAndInstantiateCivilian(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            int index = (int)Random.Range(0, SpawnPoints.transform.childCount);
            Instantiate(Civilian, SpawnPoints.transform.GetChild(index).position, Quaternion.identity);
        }
    }
}
