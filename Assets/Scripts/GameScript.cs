using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Physics2D.IgnoreLayerCollision(11, 8);
        Physics2D.IgnoreLayerCollision(10, 8);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void RestartGame()
    {
        SceneManager.LoadScene("GamePlay");
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void ShareToFacebook()
    {
        if (!FacebookManager.Instance.IsInitiated) return;

        if (!FacebookManager.Instance.IsLoggedIn)
        {
            FacebookManager.Instance.Login((result, accessToken) =>
            {
                if (!result) return;
                
                FacebookManager.Instance.ShareScore();
                GameSparksManager.Instance.LoginFacebook(accessToken, cb => { });
            });
        }
        else
        {
            FacebookManager.Instance.ShareScore();
        }
    }
    public void InviteFriends()
    {
        if (!FacebookManager.Instance.IsInitiated) return;

        if (!FacebookManager.Instance.IsLoggedIn)
        {
            FacebookManager.Instance.Login((result, accessToken) =>
            {
                if (!result) return;

                FacebookManager.Instance.InviteFriends();
                GameSparksManager.Instance.LoginFacebook(accessToken, cb => { });
            });
        }
        else
        {
            FacebookManager.Instance.InviteFriends();
        }
    }
}
