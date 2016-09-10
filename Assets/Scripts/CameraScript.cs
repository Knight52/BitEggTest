using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
    [SerializeField]
    private Transform player;
    [SerializeField]
    private Camera gameCamera;
        // Use this for initialization
    public virtual void Start ()
    {

	}
	
	// Update is called once per frame
	public virtual void Update ()
    {
        Vector2 player_pos = new Vector2(player.position.x, player.position.y);
        gameCamera.transform.position = player_pos;
	}
}
