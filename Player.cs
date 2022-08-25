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
    bool sDown1;
    bool sDown2;
    bool sDown3;


    bool isJump;
    bool isDodge;
    bool isSwap;


    Vector3 moveVec;
    Rigidbody rigid;
    Animator anim;

    GameObject nearObject;
    GameObject equipWeapon;
    int equipWeaponIndex = -1;


    void Update()
    {
        GetInput();
        Move();
        Jump();
        Attack();
        Turn();
        Dodge();
        Interation();
        Swap();
        SwapOut();
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
        iDown = Input.GetButton("Interation");
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");


    }


    void Move()
    {
        // if(isSwap)
        moveVec = new Vector3(hAxis, 0, vAxis).normalized; //노멀라이즈 = 대각선도 스피드 동일하게 적용

        transform.position += moveVec * Speed * (WDown ? 0.4f : 1f) * Time.deltaTime;

        anim.SetBool("isRun", moveVec != Vector3.zero); // 유니티 파라미터 변수명""
        anim.SetBool("isWalk", WDown); // 유니티 파라미터 변수명"" + 일단은 애니메이션 없어서 느리게만/
    }

    void Turn()
    {
        transform.LookAt(transform.position + moveVec);

    }

    void Jump()
    {
        if (JDown && moveVec == Vector3.zero && !isJump && !isDodge && !isSwap)
        {
            rigid.AddForce(Vector3.up * 22, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("DoJump");
            isJump = true;

        }

    }

    void Dodge()
    {
        if (JDown && moveVec != Vector3.zero && !isJump && !isDodge && !isSwap)
        {
            Speed *= 2;
            anim.SetTrigger("DoDodge");
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
        if(ADown && !isJump && !isDodge && !isSwap)
        {
            anim.SetBool("isAttack", true);
            anim.SetTrigger("doAttack");
        }

    }


    void OnTriggerStay(Collider other)
    {

        if (other.tag == "Weapon")
        {
            nearObject = other.gameObject;

            Debug.Log(nearObject.gameObject.name);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
        {
            nearObject = null;
        }
    }

    void Swap()
    {
        if (sDown1 && (!hasWeapons[0] || equipWeaponIndex == 0))
            return;
        if (sDown2 && (!hasWeapons[1] || equipWeaponIndex == 1))
            return;
        if (sDown3 && (!hasWeapons[2] || equipWeaponIndex == 2))
            return;

        int weaponIndex = -1;
        if (sDown1) weaponIndex = 0;
        if (sDown2) weaponIndex = 1;
        if (sDown3) weaponIndex = 2;

        if ((sDown1 || sDown2 || sDown3) && !isJump && !isDodge)
        {
            if (equipWeapon != null)
                equipWeapon.SetActive(false);

            equipWeaponIndex = weaponIndex;
            equipWeapon = weapons[weaponIndex];
            equipWeapon.SetActive(true);

            anim.SetTrigger("doSwap");

            isSwap = true;

            Invoke("SwapOut", 0.4f);
        }
    }

    void SwapOut()
    {
        isSwap = false;

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
