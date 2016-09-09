using UnityEngine;
using System.Collections;
using System;

public class EnemyScript : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    private GameObject aura;
    [SerializeField]
    private GameObject body;
    [SerializeField]
    protected Rigidbody2D rigidBody;
    [SerializeField]
    protected float invincibleTime;
    [SerializeField]
    private int MaxHP;
    [SerializeField]
    protected Collider2D enemyCollider;
    public int score;
    private float starting;
    public event Action<EnemyScript> Die;
	public virtual void Start ()
    {
        starting = 1;
        HP = MaxHP;
        Destroy(this, 120);
	}
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            if (--HP == 0)
            {
                Destroy(gameObject);
                if (Die != null)
                    Die(this);
            }
            else
            {
                starting = invincibleTime;
            }
        }
    }
	// Update is called once per frame
    public int HP { get; set; }
	public virtual void Update ()
    {
        if (starting < 0) return;

        starting -= Time.deltaTime;
        if (starting > 0)
        {
            aura.SetActive(!aura.activeSelf);
            body.SetActive(aura.activeSelf);
        }
        else
        {
            gameObject.layer = 9;
            aura.SetActive(true);
            body.SetActive(true);
        }
	}
}