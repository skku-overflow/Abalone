using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Unity.Editor;

public class Login : MonoBehaviour
{
    public InputField email;
    public InputField password;
    public Button loginButton;

    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://comit-abalone.firebaseio.com/");
        loginButton.onClick.AddListener(HandleLogin);
    }

    // Update is called once per frame
    void Update()
    {

    }

    async void HandleLogin()
    {
        try
        {
            var user = await FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(email.text, password.text);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            var user = await FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(email.text, password.text);

        }


        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }
}
