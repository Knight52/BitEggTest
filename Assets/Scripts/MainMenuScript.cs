using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text;

public class MainMenuScript : MonoBehaviour {

    [SerializeField]
    private Button facebookButton;
    [SerializeField]
    private Text facebookText;
    [SerializeField]
    private Button playButton;
    [SerializeField]
    private Button logoutButton;
    [SerializeField]
    private Text highScoreText;
    // Use this for initialization
    void Start()
    {
        if (!string.IsNullOrEmpty(FacebookManager.Instance.FirstName))
        {
            facebookText.gameObject.SetActive(true);
            logoutButton.gameObject.SetActive(true);
            facebookButton.gameObject.SetActive(false);
            facebookText.text = "Hi, " + FacebookManager.Instance.FirstName;
        }
        if (!FacebookManager.Instance.IsInitiated)
        {
            facebookButton.gameObject.SetActive(false);
        }
        highScoreText.text = "Fetching records...";
        GameSparksManager.Instance.GetHighScore(cb =>
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < cb.Length; i++)
            {
                if (!string.IsNullOrEmpty(cb[i].name)) 
                    builder.AppendLine((i + 1) + ". " + cb[i].name + " - " + cb[i].score);
            }
            highScoreText.text = builder.ToString();
        });
    }

    // Update is called once per frame
    void Update() {

    }
    public void StartGame()
    {
        SceneManager.LoadScene("GamePlay");
    }
    public void LogoutFacebook()
    {
        FacebookManager.Instance.Logout();
        GameSparksManager.Instance.Logout();

        logoutButton.gameObject.SetActive(false);
        facebookText.gameObject.SetActive(false);
        facebookButton.gameObject.SetActive(true);
    }
    public void LoginFacebook()
    {
        facebookButton.gameObject.SetActive(false);
        facebookText.gameObject.SetActive(true);
        playButton.gameObject.SetActive(false);
        facebookText.text = "Logging in...";
        FacebookManager.Instance.Login((result, accessToken) =>
        {
            playButton.gameObject.SetActive(true);
            Debug.Log(result);
            if (!result)
            {
                facebookButton.gameObject.SetActive(true);
                facebookText.gameObject.SetActive(false);
            }
            else
            {
                GameSparksManager.Instance.LoginFacebook(accessToken, cb =>
                {
                    facebookText.text = "Hi, " + FacebookManager.Instance.FirstName;
                    logoutButton.gameObject.SetActive(true);
                });
            }
        });
    }
}
