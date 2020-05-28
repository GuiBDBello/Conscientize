using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera MainCamera;
    public CharacterController CharacterController;
    public float Speed;
    public float DistanceToTalk;

    private Animator animator;
    private LayerMask layerMask;

    private GameObject civilianSelected;

    private void Start()
    {
        animator = GetComponent<Animator>();
        layerMask = LayerMask.GetMask(Layers.Clickable);

        civilianSelected = null;
    }

    private void Update()
    {
        CharacterController.SimpleMove(Move());
        CheckLeftMouseClick();

        if (civilianSelected != null)
        {
            if (!IsSelectedCivilianClose(civilianSelected))
            {
                //Debug.Log("GET AWAY");
                StopTalkingToCivilian();
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, DistanceToTalk);
    }

    private Vector3 Move()
    {
        Vector3 movement;
        if (CharacterController.isGrounded)
        {
            movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Speed;
            Animate(movement);
        }
        else movement = new Vector3(1, -1, 1);

        return movement;
    }

    private void Animate(Vector3 movement)
    {
        if (movement.magnitude > 0)
        {
            animator.SetBool(Animations.IsMoving, true);
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.LookRotation(movement), 0.1f);
            // break dance
            //gameObject.transform.Rotate(movement);
        }
        else animator.SetBool(Animations.IsMoving, false);
    }

    private void CheckLeftMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.UseGlobal))
            {
                GameObject gameObjectClicked = hit.collider.gameObject;

                //Debug.Log("CLICKED -> " + gameObjectClicked + " - " + Time.deltaTime);

                if (IsSelectedCivilianClose(gameObjectClicked))
                {
                    Debug.Log("CIVILIAN " + Time.deltaTime);
                    TalkToCivilian(gameObjectClicked);
                }
                else if (gameObjectClicked.tag.Equals(Tags.Trash))
                {
                    if (civilianSelected != null)
                    {
                        Debug.Log("TRASH " + Time.deltaTime);
                        TellCivilianToCollectTrash(gameObjectClicked);
                    }
                    else Debug.Log("SELECT A CIVILIAN FIRST");
                }
                else
                {
                    Debug.Log("CIVILIAN IS FAR AWAY");
                    civilianSelected = null;
                }
            }
            else
            {
                StopTalkingToCivilian();
            }
        }
    }

    private bool IsSelectedCivilianClose(GameObject civilian)
    {
        float distance = Vector3.Distance(civilian.transform.position, this.transform.position);

        return civilian.tag.Equals(Tags.Civilian) && distance < DistanceToTalk;
    }

    private void TalkToCivilian(GameObject civilian)
    {
        StopTalkingToCivilian();

        civilianSelected = civilian;
        civilian.GetComponent<CivilianController>().SetStopped(true);
    }

    private void StopTalkingToCivilian()
    {
        if (civilianSelected != null)
        {
            civilianSelected.GetComponent<CivilianController>().SetStopped(false);
            civilianSelected = null;
        }
    }

    private void TellCivilianToCollectTrash(GameObject trash)
    {
        civilianSelected.GetComponent<CivilianController>().CollectTrash(trash);
        //StopTalkingToCivilian();
    }
}
