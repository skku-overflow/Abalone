using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Unity.Editor;
using UnityEngine;

public class MultiPlayerGame : MonoBehaviour
{
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://comit-abalone.firebaseio.com/");


    }

    // Update is called once per frame
    void Update()
    {

    }
}
