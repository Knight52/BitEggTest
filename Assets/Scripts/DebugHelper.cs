using UnityEngine;
using System.Text;
using System.Collections;

public class DebugHelper : MonoBehaviour
{
    private StringBuilder log;
    public static DebugHelper Instance { get; private set; }
    void Awake()
    {
        Instance = this;
        log = new StringBuilder();
    }
    public void AppendLog(string text)
    {
        log.AppendLine(text);
    }
    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 1920, 1080), log.ToString());
    }
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
