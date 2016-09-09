using UnityEngine;
using System.Collections;

public class Enemy1Script : EnemyScript {

    // Use this for initialization
    [SerializeField]
    private float speed;
	public override void Start () {
        base.Start();
        if (Random.Range(0, 100) >= 50)
        {
            rigidBody.velocity = new Vector2(Random.Range(0, 100) > 50 ? speed : -speed, 0);
        }
        else
        {
            rigidBody.velocity = new Vector2(0, Random.Range(0, 100) > 50 ? speed : -speed);
        }
	}
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.layer == 12)
        {
            Vector2 velocity = rigidBody.velocity;
            velocity.x *= -1;
            velocity.y *= -1;
            rigidBody.velocity = velocity;
        }
    }

}
