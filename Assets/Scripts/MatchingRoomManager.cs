using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;

public class MatchingRoomManager : MonoBehaviour
{
    private FirebaseAuth auth;
    private FirebaseDatabase db;
    private DatabaseReference rooms;

    void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://comit-abalone.firebaseio.com/");
        auth = FirebaseAuth.DefaultInstance;
        db = FirebaseDatabase.DefaultInstance;

        rooms = db.GetReference("/matchings");

        Debug.Log(rooms);
    }

    void Update()
    {

    }
}
