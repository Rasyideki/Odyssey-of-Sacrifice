using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : MonoBehaviour
{
    public float flightSpeed = 2f;
    public float waypointReachedDistance = 0.1f;
    public DetectionZone biteDetectionZone;
    public List<Transform> waypoints;
    public Collider2D deathCollider;

    Animator animator;
    Rigidbody2D rb;
    Damageable damageable;

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
        rb= GetComponent<Rigidbody2D>();
        damageable= GetComponent<Damageable>(); 
    }

    private void Start()
    {
        nextWaypoint = waypoints[waypointNum];
    }

    private void OnEnable()
    {
        damageable.damageableDeath.AddListener(OnDeath);
    }

    // Update is called once per frame
    void Update()
    {
        HasTarget = biteDetectionZone.detectedColliders.Count > 0; ;   
    }

    private void FixedUpdate()
    {
        if (damageable.IsAlive)
        {
            if (CanMove)
            {
                Flight();
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
    }
    private void Flight()
    {
        //terbang ke waypoint berikutnya
        Vector2 directionToWaypoint = (nextWaypoint.position - transform.position).normalized;

        //ngecek apakah sudah sampai di waypoint
        float distance = Vector2.Distance(nextWaypoint.position, transform.position);

        rb.velocity = directionToWaypoint * flightSpeed;
        UpdateDirection();

        //melihat apakah butuh bertukan waypoint
        if (distance <= waypointReachedDistance)
        {
            //tukar ke waypoint berikutnya
            waypointNum++;

            if(waypointNum >= waypoints.Count)
            {
                //balik ke original waypoint
                waypointNum = 0;
            }

            nextWaypoint = waypoints[waypointNum];
        }
    }

    private void UpdateDirection()
    {
        Vector3 loclScale = transform.localScale;
        if(transform.localScale.x > 0f)
        {
            if(rb.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1 * loclScale.x, loclScale.y, loclScale.z); 
            }
        }
        else
        {
            if(rb.velocity.x > 0)
            {
                transform.localScale = new Vector3(-1 * loclScale.x, loclScale.y, loclScale.z);
            }
        }
    }

    public void OnDeath()
    {   
            rb.gravityScale = 2f;
            rb.velocity = new Vector2(0, rb.velocity.y);
            deathCollider.enabled = true;
        
    }

}
