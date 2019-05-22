using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        Try();

    }

    void Update()
    {

    }

    private async void Try()
    {
        if (auth.CurrentUser == null)
        {
            SceneManager.LoadScene("Login", LoadSceneMode.Single);
            return;
        }

        var uid = auth.CurrentUser.UserId;
        rooms = db.GetReference("/matchings");

        await rooms.Child(uid).SetValueAsync(true);
        Debug.Log(rooms);
    }
}
