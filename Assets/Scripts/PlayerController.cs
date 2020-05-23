using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera MainCamera;
    public CharacterController CharacterController;
    public float Speed;

    private Vector3 movement;

    private void Update()
    {
        if (CharacterController.isGrounded)
        {
            movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Speed;
        }
        else
        {
            movement = new Vector3(1, -1, 1);
        }

        CharacterController.SimpleMove(movement);

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject gameObjectClicked = hit.collider.gameObject;
                float distance = Vector3.Distance(gameObjectClicked.transform.position, this.transform.position);
                Debug.Log(distance);

                if (gameObjectClicked.tag.Equals(Tags.Civilian) && distance < 5f)
                {
                    Debug.Log(gameObjectClicked);
                    gameObject.GetComponent<CivilianController>();
                }

                if (gameObjectClicked.tag.Equals(Tags.Trash))
                {
                    Debug.Log(gameObjectClicked);
                }
            }
        }
    }
}
