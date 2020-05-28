using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashController : MonoBehaviour
{
    public float WaitTime;

    private HUDController hudController;

    private IEnumerator explodeTrashCoroutine;
    
    private void Start()
    {
        hudController = GameObject.FindGameObjectWithTag(Tags.HUD).GetComponent<HUDController>();
        explodeTrashCoroutine = WaitAndExplodeTrash(WaitTime);

        StartCoroutine(explodeTrashCoroutine);
    }

    private IEnumerator WaitAndExplodeTrash(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        hudController.TakeDamage(10);
        Destroy(this.gameObject);
    }

    public void PickUp()
    {
        StopCoroutine(explodeTrashCoroutine);
        Destroy(this.gameObject);
    }
}
