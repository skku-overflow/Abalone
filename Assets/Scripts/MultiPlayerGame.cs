using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;

public class MultiPlayerGame : MonoBehaviour
{
    public GameManager gameManager;
    private FirebaseAuth auth;
    private FirebaseDatabase db;

    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://comit-abalone.firebaseio.com/");
        auth = FirebaseAuth.DefaultInstance;
        db = FirebaseDatabase.DefaultInstance;

    }

    // Update is called once per frame
    void Update()
    {

    }
}
