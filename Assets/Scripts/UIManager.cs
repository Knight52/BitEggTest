using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

    [SerializeField]
    private Text levelText;
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text hpText;
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private Text gameOverScore;
    [SerializeField]
    private Button shareButton;
    [SerializeField]
    private Button inviteButton;
    [SerializeField]
    private Button submitButton;
    private int score;
	// Use this for initialization
	void Start () {
        scoreText.text = "SCORE: 0";
	}
    public void SetHP(int hp)
    {
        hpText.text = "HP: ";
        for (int i = 0; i < hp; i++)
            hpText.text += "|";
    }
    public void SetLevel(int level)
    {
        levelText.text = "LEVEL " + level;
    }
    public void SetScore(int score)
    {
        scoreText.text = "SCORE: " + score;
        this.score = score;
    }
    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        gameOverScore.text = score.ToString();
        shareButton.gameObject.SetActive(FacebookManager.Instance.IsInitiated);
        inviteButton.gameObject.SetActive(FacebookManager.Instance.IsInitiated);
    }
	// Update is called once per frame
	void Update ()
    {
	
	}
    public void SubmitScore()
    {
        if (!FacebookManager.Instance.IsInitiated) return;

        submitButton.gameObject.SetActive(false);
        if (!FacebookManager.Instance.IsLoggedIn)
        {
            FacebookManager.Instance.Login((loggedInFacebook, accessToken) =>
            {
                if (loggedInFacebook)
                {
                    GameSparksManager.Instance.LoginFacebook(accessToken, (loggedGameSpark) =>
                    {
                        submitButton.gameObject.SetActive(true);
                        if (loggedGameSpark)
                        {
                            GameSparksManager.Instance.SubmitHighscore(score, cb =>
                            {
                            });
                        }
                    });
                }
                else
                {
                    submitButton.gameObject.SetActive(true);
                }
            });
        }
        else
        {
            GameSparksManager.Instance.SubmitHighscore(score, cb =>
            {
                submitButton.gameObject.SetActive(true);
            });
        }
    }
}
