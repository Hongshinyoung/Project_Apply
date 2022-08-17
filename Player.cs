using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed;
    float hAxis;
    float vAxis;
    bool ADown; // attack

    Vector3 moveVec;
    Rigidbody rigid;
    Animator anim;


    void Update()
    {
        Move();
        Jump();
        Attack();
        GetInput();
    }

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        ADown = Input.GetButtonDown("Attack");

    }


    void Move()
    {

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        transform.position += moveVec * Speed * Time.deltaTime;

    }
    void Jump()
    {

    }

    void Attack()
    {

    }

}
