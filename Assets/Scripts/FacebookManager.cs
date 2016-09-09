using UnityEngine;
using System.Collections;
using System;
using Facebook.Unity;
using UnityEngine.SceneManagement;

public class FacebookManager : MonoBehaviour
{
    public static FacebookManager Instance { get; private set; }
    private Action<bool> startCallback;
    private bool login;
    private Action<bool, string> loginCallback;
    private bool share;
    private bool invite;
    void Awake()
    {
        Instance = this;
        login = false;
        share = false;
        invite = false;
    }
    void OnGUI()
    {
        if (login)
        {
            login = false;
            string[] permissions = new string[] { "public_profile" };
            FB.LogInWithReadPermissions(permissions, result =>
            {
                if (!result.Cancelled)
                {
                    FetchFacebookInformations(() =>
                    {
                        loginCallback(true, result.AccessToken.TokenString);
                        loginCallback = null;
                    });
                }
                else
                {
                    loginCallback(false, null);
                    loginCallback = null;
                }
            });
        }
        if (share)
        {
            share = false;
            FB.FeedShare("", new Uri("http://www.example.com/"), "LINK_NAME", "CAPTION", "DESC", null, "");
        }
        if (invite)
        {
            invite = false;
            FB.Mobile.AppInvite(new Uri("http://www.example.com/"));
        }
    }
    public bool IsLoggedIn { get { return FB.IsLoggedIn; } }
    public bool IsInitiated { get { return FB.IsInitialized; } }
    public string ID { get; private set; }
    public string FullName { get; private set; }
    public string FirstName { get; private set; }
	// Use this for initialization
	void Start ()
    {
    }
    public void Initialize(Action<bool> callback)
    {
        FB.Init(OnFacebookInit);
        startCallback = callback;
    }
    public void InviteFriends()
    {
        invite = true;
    }
    public void ShareScore()
    {
        share = true;
    }
	// Update is called once per frame
	void Update () {
	
	}
    public void Logout()
    {
        if (!FB.IsLoggedIn) return;

        FB.LogOut();
    }
    public void Login(Action<bool, string> callback)
    {
        login = true;
        loginCallback = callback;
    }
    private void FetchFacebookInformations(Action callback)
    {
        FB.API("/me?fields=name,id,first_name", HttpMethod.GET, graphResult =>
        {
            if (graphResult.ResultDictionary["id"] != null) ID = graphResult.ResultDictionary["id"].ToString();
            if (graphResult.ResultDictionary["name"] != null) FullName = graphResult.ResultDictionary["name"].ToString();
            if (graphResult.ResultDictionary["first_name"] != null) FirstName = graphResult.ResultDictionary["first_name"].ToString();
            callback();
        });
    }
    private void OnFacebookInit()
    {
        if (FB.IsLoggedIn)
        {
            FetchFacebookInformations(() =>
            {
                startCallback(true);
            });
        }
        else
        {
            startCallback(true);
        }
    }
}
