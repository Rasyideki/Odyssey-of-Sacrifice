using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirection),typeof(Damageable))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed =  9f;
    public float runspeed = 5f ;
    public float airWalkSpeed = 8f;
    public float jumpImpluse = 10f;
    FadeinOut fade;


    public GameObject loseScreenUI;

    Vector2 moveInput;
    TouchingDirection touchingDirections;
    Damageable damageable;

    public float CurrentMoveSpeed { get 
        {
            if (CanMove)
            {
                if (IsMoving && !touchingDirections.IsOnWall)
                {
                    if (touchingDirections.IsGrounded)
                    {
                        if (IsRunning)
                        {
                            return runspeed;
                        }
                        else
                        {
                            return walkSpeed;
                        }
                    }
                    else
                    {
                        return airWalkSpeed;
                    }
                }
                else
                {
                    //idle speed 0
                    return 0;
                }
            }
            else
            {
                //ngunci movement
                return 0;
            }
        }}

    [SerializeField]
    private bool _isMoving = false;


    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }
    [SerializeField]
    private bool _isRunning = false;

    public bool IsRunning
    {
        get { 
            return _isRunning; 
        }
        set { 
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }

    public bool _isFacingRight=true;
    public bool IsFacingRight { 
        get { 
            return _isFacingRight; 
        } private set {
            if(_isFacingRight != value) {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        }
    }
    
    public bool CanMove { get
        {
            return animator.GetBool(AnimationStrings.canMove);
        } 
    }
    public bool IsAlive
    {
        get
        {
            return animator.GetBool(AnimationStrings.isAlive);
        }       
    }

    Rigidbody2D rb;
    Animator animator;
    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirection>();
        damageable = GetComponent<Damageable>();
    }
    private void Start()
    {
        fade = FindObjectOfType<FadeinOut>();
    }

    private void Update()
    {
        PlayerLose();
    }
    
    private void PlayerLose()
    {
        if (!damageable.IsAlive && loseScreenUI != null)
        {
            loseScreenUI.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        if(!damageable.LockVelocity)
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);

        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMove(InputAction.CallbackContext context){
        moveInput = context.ReadValue<Vector2>();

        if (IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;

            SetFacingDirection(moveInput);
        }
        else
        {
            IsMoving = false;
        }
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if(moveInput.x > 0 && !IsFacingRight) 
        { 
            IsFacingRight = true;
        }else if(moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight= false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        }else if(context.canceled)
        {
            IsRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && touchingDirections.IsGrounded && CanMove) 
        {
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpluse);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }
    public void OnRangedAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.rangedAttackTrigger);
        }
    }
    public void OnEnterMap(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(1, 1), 0);
            foreach (Collider2D col in colliders)
            {
                if (col.CompareTag("Goal"))
                {
                    if (GoalManager.singleton.canEnterCastle)
                    {
                        EnterCastle();
                        break;
                    }
                    else
                    {
                       
                        Debug.Log("tes");
                    }
                }
                else if (col.CompareTag("Cave") && GoalManager.singleton.canEnterCave)
                {
                    EnterCave();
                    break;
                }
                else if (col.CompareTag("Gate") && GoalManager.singleton.canEnterGate)
                {
                    EnterGate();
                    break;
                }
                else if (col.CompareTag("ExitCave")&& GoalManager.singleton.canExitCave)
                {
                    ExitCave();
                    break;
                }
                else if (col.CompareTag("ExitCastle") && GoalManager.singleton.canExitCave)
                {
                    ExitCastle();
                    break;
                }
            }
        }
    }

    private void EnterCastle()
    {
        StartCoroutine(ChangeSceneEnterCastle());
        Debug.Log("Masuk Cave!");
        Damageable damageable = GetComponent<Damageable>();
        if (damageable != null)
        {
            damageable.OnSceneChange();
        }
    }

    //Fungsi
    private void EnterCave()
{
    StartCoroutine(ChangeSceneEnterCave());
    Debug.Log("Masuk Cave!");
    Damageable damageable = GetComponent<Damageable>();
    if (damageable != null)
    {
        damageable.OnSceneChange();
    }
}
    private void EnterGate()
    {
        Debug.Log("masuk Portal");
        StartCoroutine(ChangeSceneWithFade());
        Damageable damageable = GetComponent<Damageable>();
        if (damageable != null)
        {
            damageable.OnSceneChange();
        }
    }
    private void ExitCave()
    {
        Debug.Log("Keluar dari Cave!");
        StartCoroutine(ChangeSceneBackToForest());
        Damageable damageable = GetComponent<Damageable>();
        if (damageable != null)
        {
            damageable.OnSceneChange();
        }
    }
    private void ExitCastle()
    {
        Debug.Log("Keluar dari Castle!");
        StartCoroutine(ChangeSceneBackToForest());
        Damageable damageable = GetComponent<Damageable>();
        if (damageable != null)
        {
            damageable.OnSceneChange();
        }
    }

    //Pindah Scene
    private IEnumerator ChangeSceneWithFade()
    {
        Debug.Log("NormalFadeScene");
        fade.Fadein();

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    private IEnumerator ChangeSceneEnterCave()
    {
        Debug.Log("CaveFade");
        fade.Fadein();

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene("Cave");
    }
    private IEnumerator ChangeSceneEnterCastle()
    {
        Debug.Log("CastleFade");
        fade.Fadein();

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene("Castle");
    }
    private IEnumerator ChangeSceneBackToForest()
    {
        Debug.Log("BackToForestFade");
        fade.Fadein();

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene("SampleScene");
    }



    public void OnHit(int damage,Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }
}
