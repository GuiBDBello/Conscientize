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
    private Vector3 movement;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        movement = Move();

        CharacterController.SimpleMove(movement);

        CheckLeftMouseClick();
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
            animator.SetBool("Is Moving", true);
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.LookRotation(movement), 0.1f);
            // break dance
            //gameObject.transform.Rotate(movement);
        }
        else animator.SetBool("Is Moving", false);
    }

    private void CheckLeftMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Clickable"), QueryTriggerInteraction.UseGlobal))
            {
                GameObject gameObjectClicked = hit.collider.gameObject;

                //Debug.Log(distance);

                if (IsCivilianClose(gameObjectClicked, Vector3.Distance(gameObjectClicked.transform.position, this.transform.position)))
                {
                    Debug.Log("CIVIL " + Time.deltaTime);
                    TalkToCivilian(gameObjectClicked);
                }

                if (gameObjectClicked.tag.Equals(Tags.Trash))
                {
                    Debug.Log("LIXO " + Time.deltaTime);
                }
            }
        }
    }

    private bool IsCivilianClose(GameObject civilian, float distance)
    {
        return civilian.tag.Equals(Tags.Civilian) && distance < DistanceToTalk;
    }

    private void TalkToCivilian(GameObject civilian)
    {
        civilian.GetComponent<CivilianController>().Talk(true);
    }
}
