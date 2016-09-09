using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour, IBulletOwner
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private Rigidbody2D rigidBody;
    [SerializeField]
    private BulletScript bullet;
    [SerializeField]
    private Camera game_camera;
    [SerializeField]
    private Collider2D playerCollider;
    [SerializeField]
    private GameObject aura;
    [SerializeField]
    private SpriteRenderer playerBody;
    [SerializeField]
    private Color invincibleColor;
    [SerializeField]
    private int MaxHP;
    [SerializeField]
    private UIManager uiManager;
    [SerializeField]
    private ControlPadController padController;
    [SerializeField]
    private ControlPadController fireController;
    private int score;

    private float invincibleCount;
    
    // Use this for initialization
    void Start()
    {
        HP = MaxHP;
        score = 0;
        uiManager.SetHP(HP);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.layer == 10 && (collision.gameObject.layer == 9 || collision.gameObject.layer == 11))
        {
            gameObject.layer = 8;
            invincibleCount = 3;
            aura.SetActive(false);
            playerBody.color = invincibleColor;
            uiManager.SetHP(--HP);
            if (HP == 0)
            {
                gameObject.SetActive(false);
                uiManager.ShowGameOver();
            }
        }
    }
    public int HP { get; set; }
    // Update is called once per frame
    public Collider2D Collider2D { get { return playerCollider; } }
    public void AddScore(int score)
    {
        this.score += score;
        uiManager.SetScore(this.score);
    }
    public void OnBulletHit(GameObject target)
    {
        EnemyScript script = target.GetComponent<EnemyScript>();
        if (script != null)
        {
            AddScore(script.score);
        }
    }
    void Update()
    {
        if (invincibleCount >= 0)
        {
            invincibleCount -= Time.deltaTime;
            if (invincibleCount < 0)
            {
                aura.SetActive(true);
                playerBody.color = Color.white;
                gameObject.layer = 10;
            }
        }
        bool hasChange = false;
        Vector2 velocity = rigidBody.velocity;
        Vector2 padVelocity = padController.Direction;
        if (padVelocity != Vector2.zero || (velocity != Vector2.zero && !Input.GetKey("w") && !Input.GetKey("s") && !Input.GetKey("a") && !Input.GetKey("d")))
        {
            hasChange = true;
            velocity = padVelocity;
        }
        else
        {
            if (Input.GetKeyDown("w"))
            {
                velocity.y += speed;
                hasChange = true;
            }
            if (Input.GetKeyDown("s"))
            {
                velocity.y -= speed;
                hasChange = true;
            }
            if (Input.GetKeyDown("a"))
            {
                velocity.x -= speed;
                hasChange = true;
            }
            if (Input.GetKeyDown("d"))
            {
                velocity.x += speed;
                hasChange = true;
            }

            if (Input.GetKeyUp("w") || Input.GetKeyUp("s"))
            {
                velocity.y = 0;
                hasChange = true;
                if (Input.GetKey("w")) velocity.y = -speed;
                else if (Input.GetKey("s")) velocity.y = speed;
            }
            if (Input.GetKeyUp("a") || Input.GetKeyUp("d"))
            {
                velocity.x = 0;
                hasChange = true;
                if (Input.GetKey("a")) velocity.x = -speed;
                else if (Input.GetKey("d")) velocity.x = speed;
            }
            velocity.Normalize();
        }
        if (hasChange)
        {
            velocity *= speed;
            rigidBody.velocity = velocity;
        }
        Vector2 fireDirection = fireController.Direction;
        if (
#if UNITY_EDITOR
            Input.GetMouseButton(0) ||
#endif
            fireDirection != Vector2.zero)
        {
            BulletScript bullet = (BulletScript)Instantiate(this.bullet, transform.position, Quaternion.identity);
            Vector2 pos;
            if (fireController.Direction == Vector2.zero)
            {
                pos = game_camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            }
            else
            {
                pos = fireDirection;
            }
            pos.Normalize();
            pos.x += Random.Range(-0.1f, 0.1f);
            pos.y += Random.Range(-0.1f, 0.1f);
            bullet.Launch(pos, this);
        }
    }
}