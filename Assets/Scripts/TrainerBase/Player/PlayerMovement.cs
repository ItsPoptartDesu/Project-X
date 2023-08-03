using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour //TODO: use new unity input system. and seperate input from movement logic
{
    public float speed;
    public LayerMask grassLayer;
    private Animator animator;
    private Vector3 lastTilePosition;
    private PlayerController playerController;
    private void Start()
    {
    }
    public void FirstLoad()
    {
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        Vector2 dir = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
        {
            dir.x = -1;
            animator.SetInteger("Direction" , 3);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            dir.x = 1;
            animator.SetInteger("Direction" , 2);
        }

        if (Input.GetKey(KeyCode.W))
        {
            dir.y = 1;
            animator.SetInteger("Direction" , 1);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            dir.y = -1;
            animator.SetInteger("Direction" , 0);
        }

        dir.Normalize();
        animator.SetBool("IsMoving" , dir.magnitude > 0);

        GetComponent<Rigidbody2D>().velocity = speed * dir;
    }
}
