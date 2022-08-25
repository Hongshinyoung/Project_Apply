using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
public enum Type { Coin, Heart, Weapon }; // enum 열거형 타입
    public Type type;
    public int value;

    void Update()
    {
        // transform.Rotate(Vector3.up * 20 * Time.deltaTime);
        transform.Rotate(Vector3.back * 20 * Time.deltaTime);
    }
}
