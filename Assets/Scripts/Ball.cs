using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public bool IsBlack()
    {
        return GetComponent<MeshRenderer>().material.color == Color.black;
    }
}