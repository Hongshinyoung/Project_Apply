using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed;
    public GameObject[] weapons;
    public bool[] hasWeapons;

    float hAxis;
    float vAxis;

    bool WDown;
    bool JDown;
    bool ADown; // attack
    bool iDown; // 줍기

    bool isJump;
    bool isDodge;

    Vector3 moveVec;
    Rigidbody rigid;
    Animator anim;

    GameObject nearObject;


    void Update()
    {
        Move();
        Jump();
        Attack();
        GetInput();
        Turn();
        Dodge();
    }

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal"); //Input = 키보드,마우스 입력 받는 함수
        vAxis = Input.GetAxisRaw("Vertical"); // GetAxisRaw = Axis 값을 정수로 반환하는 함수
        JDown = Input.GetButtonDown("Jump");
        WDown = Input.GetButton("Walk");
        ADown = Input.GetButtonDown("Attack");


    }


    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized; //노멀라이즈 = 대각선도 스피드 동일하게 적용

        transform.position += moveVec * Speed * (WDown ? 0.4f : 1f) * Time.deltaTime;

        anim.SetBool("IsRun", moveVec != Vector3.zero); // 유니티 파라미터 변수명""
        anim.SetBool("isWalk", WDown); // 유니티 파라미터 변수명"" + 일단은 애니메이션 없어서 느리게만/
    }

    void Turn()
    {
        transform.LookAt(transform.position + moveVec);

    }

    void Jump()
    {
        if (JDown && moveVec == Vector3.zero && !isJump && !isDodge)
        {
            rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;

        }

    }

    void Dodge()
    {
        if (JDown && moveVec != Vector3.zero && !isJump && !isDodge)
        {
            Speed *= 2;
            anim.SetTrigger("doDodge");
            isDodge = true;

            Invoke("DodgeOut", 0.4f);

        }
    }

    void DodgeOut()
    {
        Speed *= 0.5f;
        isDodge = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            anim.SetBool("isJump", false);
            isJump = false;

        }
    }

    void Attack()
    {
        if(ADown && !isJump && !isDodge)
        {
            anim.SetBool("IsAttack", true);
            anim.SetTrigger("DoAttack");
        }

    }


    void OnTriggerStay(Collider other)
    {

        if (other.tag == "Weapon")
        {
            nearObject = other.gameObject;

            Debug.Log(nearObject.name);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
        {
            nearObject = null;
        }
    }

    void Interation()
    {
        if (iDown && nearObject != null && !isJump && !isDodge)
        {
            if (nearObject.tag == "Weapon")
            {
                Item item = nearObject.GetComponent<Item>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true;

                Destroy(nearObject);
            }
        }
    }

}
