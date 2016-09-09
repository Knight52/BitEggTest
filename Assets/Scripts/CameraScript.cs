using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
    [SerializeField]
    private Transform player;
    private Camera game_camera;
        // Use this for initialization
    public virtual void Start ()
    {
        Debug.Assert(player != null);
        game_camera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	public virtual void Update ()
    {
        Vector2 player_pos = new Vector2(player.position.x, player.position.y);
        game_camera.transform.position = player_pos;
	}
}
