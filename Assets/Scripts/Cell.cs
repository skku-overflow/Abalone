using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GetComponentInChildren<MeshRenderer>().material.color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
