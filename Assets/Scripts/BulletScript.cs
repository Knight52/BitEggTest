using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

    [SerializeField]
    private Rigidbody2D rigidBody;
    [SerializeField]
    private float speed;
    private IBulletOwner owner;
    public Collider2D bulletCollider;
	// Use this for initialization
	void Start () {
        Destroy(gameObject, 3);
	}
    void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
        owner.OnBulletHit(collision.gameObject);
    }
    public void Launch(Vector2 direction, IBulletOwner owner)
    {
        this.owner = owner;
        direction.Normalize();
        direction *= speed;
        rigidBody.velocity = direction;
        Physics2D.IgnoreCollision(bulletCollider, owner.Collider2D);
    }
	// Update is called once per frame
	void Update ()
    {

	}
}
