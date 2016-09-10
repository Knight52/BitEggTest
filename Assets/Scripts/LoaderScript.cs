using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class LoaderScript : MonoBehaviour {

    // Use this for initialization
    private DateTime startAt;
	void Start ()
    {
        startAt = DateTime.Now;
        FacebookManager.Instance.Initialize(cb =>
        {
            GameSparksManager.Instance.Initialize(() =>
            {
                if (!FacebookManager.Instance.IsLoggedIn && GameSparksManager.Instance.IsLoggedIn)
                    GameSparksManager.Instance.Logout();
                SceneManager.LoadScene("MainMenu");
            });
        });
    }
	
	// Update is called once per frame
	void Update ()
    {
        //In case of no internet connection, GameSparks will keep reconnect, and will never finish initializing. So we need to time out.
        if ((DateTime.Now - startAt).TotalSeconds > 7)
        {
            SceneManager.LoadScene("MainMenu");
        }
	}
}
