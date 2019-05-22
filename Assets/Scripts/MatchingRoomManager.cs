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
        var current = db.GetReference("/matchings").Child("current");

        //await rooms.RunTransaction((tr) =>
        //{
        //    var v = tr.Value;
        //    if (v == null)
        //    {
        //        tr.Value = uid;
        //    }
        //    else
        //    {
        //        tr.Value = null;
        //    }

        //    return TransactionResult.Success(tr.Value)

        //});

        var enemyId = await current.GetValueAsync();

        if (enemyId == null)
        {
            await rooms.SetValueAsync(uid);
        }
        else
        {
            // TODO:
            SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
        }
    }
}
