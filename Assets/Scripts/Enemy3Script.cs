using UnityEngine;
using System.Collections;

public class Enemy3Script : Enemy1Script, IBulletOwner
{
    [SerializeField]
    private BulletScript bullet;
    [SerializeField]
    private float MaxShootCount;
    private float shootCount;
    public override void Start()
    {
        base.Start();
        shootCount = MaxShootCount;
    }
    public Collider2D Collider2D { get { return enemyCollider; } }
    public void OnBulletHit(GameObject target)
    { }
    public override void Update()
    {
        base.Update();
        if (shootCount > 0)
        {
            shootCount -= Time.deltaTime;
            if (shootCount <= 0)
            {
                shootCount = MaxShootCount;
                BulletScript bullet = (BulletScript)Instantiate(this.bullet, transform.position, Quaternion.identity);
                Vector2 velocity = rigidBody.velocity;
                if (velocity.y != 0)
                {
                    velocity.x = velocity.y;
                    velocity.y = 0;
                }
                else if (velocity.x != 0)
                {
                    velocity.y = velocity.x;
                    velocity.x = 0;
                }
                if (Random.Range(0, 100) >= 50)
                {
                    velocity.y *= -1;
                    velocity.x *= -1;
                }
                bullet.Launch(velocity, this);
            }
        }
    }
}
