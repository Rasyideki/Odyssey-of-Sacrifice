using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DarkKnight : MonoBehaviour
{
    public float chaseDistance = 5f;
    public float waypointReachedDistance = 0.1f;
    public float returnDistance = 1.5f;
    public float speed = 3f;
    public Collider2D deathCollider;
    public List<Transform> waypoints; // Deklarasi variabel waypoints

    Animator animator;
    Rigidbody2D rb;
    Damageable damageable;

    GameObject player;
    Transform nextWaypoint;
    int waypointNum = 0;

    public bool _hasTarget = false;

    public bool HasTarget
    {
        get { return _hasTarget; }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        damageable = GetComponent<Damageable>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        nextWaypoint = waypoints[waypointNum];
    }

    private void OnEnable()
    {
        damageable.damageableDeath.AddListener(OnDeath);
    }

    private void Update()
    {
        if (player == null)
        {
            Flight();
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= chaseDistance)
        {
            HasTarget = true;
            if (damageable.IsAlive && CanMove)
            {
                ChasePlayer();
            }
            else
            {
                rb.velocity = Vector2.zero;
                if (!damageable.IsAlive)
                {
                    // gantiscene
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
            }
        }
        else
        {
            Flight();
            HasTarget = false;
        }
    }

    private void ChasePlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        UpdateDirection();
    }

    private void Flight()
    {
        //terbang ke waypoint berikutnya
        Vector2 directionToWaypoint = (nextWaypoint.position - transform.position).normalized;

        //ngecek apakah sudah sampai di waypoint
        float distance = Vector2.Distance(nextWaypoint.position, transform.position);

        rb.velocity = directionToWaypoint * speed;
        UpdateDirection();

        //melihat apakah butuh bertukan waypoint
        if (distance <= waypointReachedDistance)
        {
            //tukar ke waypoint berikutnya
            waypointNum++;

            if (waypointNum >= waypoints.Count)
            {
                //balik ke original waypoint
                waypointNum = 0;
            }

            nextWaypoint = waypoints[waypointNum];
        }
    }

    private void UpdateDirection()
    {
        Vector3 localScale = transform.localScale;
        if (transform.position.x > nextWaypoint.position.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(localScale.x), localScale.y, localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(localScale.x), localScale.y, localScale.z);
        }
    }

    public void OnDeath()
    {
        rb.gravityScale = 2f;
        rb.velocity = new Vector2(0, rb.velocity.y);
        deathCollider.enabled = true;
    }
}
