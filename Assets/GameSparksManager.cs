using UnityEngine;
using System.Collections;
using GameSparks.Core;
using GameSparks.Api;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using System;

public class GameSparksManager : MonoBehaviour {
    
    public static GameSparksManager Instance { get; private set; }
    void Awake()
    {
        IsAvailable = false;
        Instance = this;
    }
	// Use this for initialization
	void Start ()
    {
        
	}
    public void Logout()
    {
        GS.Reset();
        new DeviceAuthenticationRequest()
            .Send(cb => { });
    }
    public void Initialize(Action callback)
    {
        Action<bool> cb = (obj) =>
        {
            GS.GameSparksAvailable -= cb;
            IsAvailable = true;
            if (!GS.Authenticated)
            {
                new DeviceAuthenticationRequest()
                    .Send(deviceAuthenCallback =>
                    {
                        callback();
                    });
            }
            else
            {
                callback();
            }
        };
        GS.GameSparksAvailable += cb;
        gameObject.AddComponent<GameSparksUnity>();
    }
    public bool IsAvailable { get; private set; }
    public bool IsLoggedIn { get { return GS.Authenticated; } }
    // Update is called once per frame
    void Update () {
	
	}
    public void LoginFacebook(string accessToken, Action<bool> callback)
    {
        new FacebookConnectRequest()
            .SetAccessToken(accessToken)
            .SetDoNotLinkToCurrentPlayer(true)
            .Send(cb =>
            {
                callback(!cb.HasErrors);
            });
    }
    public void SubmitHighscore(int score, Action<bool> callback)
    {
        new LogEventRequest()
            .SetEventKey("SUBMIT_SCORE")
            .SetEventAttribute("score", score)
            .Send(cb =>
            {
                callback(!cb.HasErrors);
            });
    }
    public void GetHighScore(Action<HighScoreInfo[]> callback)
    {
        new LeaderboardDataRequest()
            .SetEntryCount(5)
            .SetLeaderboardShortCode("score")
            .Send(cb =>
            {
                HighScoreInfo[] outgoing = new HighScoreInfo[5];
                int i = 0;
                foreach (LeaderboardDataResponse._LeaderboardData dat in cb.Data)
                {
                    if (i >= outgoing.Length)
                        break;

                    outgoing[i] = new HighScoreInfo() { name = dat.UserName, score = dat.BaseData.GetInt("score").Value };
                    if (outgoing[i].name == "")
                        outgoing[i].name = "[redacted]";
                    i++;
                }
                
                for (; i < outgoing.Length; i++)
                {
                    outgoing[i] = new HighScoreInfo() { name = null, score = 0 };
                }
                callback(outgoing);
            });
    }
}
public struct HighScoreInfo
{
    public string name;
    public int score;
}