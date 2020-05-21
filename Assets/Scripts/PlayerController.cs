using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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
    }
}
