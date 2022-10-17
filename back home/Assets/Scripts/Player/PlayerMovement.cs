using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

//needs doing:
//fucntion to take damage

public class PlayerMovement : MonoBehaviour
{
    enum PlayerState
    {
        Idle = 0,
        Walk,
        Run,
        Attack,
        Dash,
        DashAttack,
        SpecialAttack
    }

    Rigidbody2D rb;
    Vector2 movement;
    public float walkSpeed = 3f;
    public float runSpeed = 5f;
    PlayerState state;
    Animator animator;
    public int HP = 25;
    public int TP = 15;
    public int CooldownTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        //walk = WASD
        //run = left click
        if (Input.GetKeyDown(KeyCode.Space) && Input.GetMouseButtonDown(1))
        {
            state = PlayerState.DashAttack;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            state = PlayerState.Dash;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            state = PlayerState.Attack;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            state = PlayerState.SpecialAttack;
        }
        else if (movement.x != 0 || movement.y != 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                state = PlayerState.Run;
            }
            else
            {
                state = PlayerState.Walk;
            }
        }
        else
        {
            state = PlayerState.Idle;
        }
    }

    private void LateUpdate() // animation
    {
        if (movement.x != 0 || movement.y != 0)
        {
            animator.SetFloat("horizontal", movement.x);
            animator.SetFloat("vertical", movement.y);
            animator.SetBool("idle", false);
        }
        else
        {
            animator.SetBool("idle", true);
        }
    }

    void FixedUpdate() // physics
    {
        //Idle,Walk,Run,Attack,Dash,DashAttack,SpecialAttack
        if (CooldownTime > 0)
        {
            CooldownTime--;
        }
        bool check = coolDownTimeZero(); //checks if cooldown is 0

        switch (state)
        {
            case PlayerState.Walk:
                Move(walkSpeed);
                break;
            case PlayerState.Run:
                Move(runSpeed);
                break;
            case PlayerState.Attack:
                Attack(check);
                break;
            case PlayerState.Dash:
                Dash(check);
                break;
            case PlayerState.DashAttack:
                DashAttack(check);
                break;
            case PlayerState.SpecialAttack:
                SpecialAttack(check);
                break;
        }

    }


    private void Move(float speed)
    {
        movement.x = movement.x * speed * Time.deltaTime;
        movement.y = movement.y * speed * Time.deltaTime;
        rb.position = new Vector2(rb.position.x + movement.x, rb.position.y + movement.y);
        Debug.Log("Move pressed");
    }
    private void Attack(bool check)
    {
        if (check == true)
        {
            Debug.Log("Attack initiated");
        }
        //check if character is touching an enemy
        //if so inflict damage on enemy
        
    }
    private void Dash(bool check)
    {
        if (check == true)
        {
            Debug.Log("Dash initiated");
        }
    }
    private void DashAttack(bool check)
    {
        if (check == true)
        {
            //check if enemy is in path of dash if so take away their hp
            //you can just call dash to get them to actual dash
            Debug.Log("DashAttack initiated");
        }
    }
    private void SpecialAttack(bool check)
    {
        if (TP - 5 >= 0 && check == true)
        {
            //create special attack in direction the player is facing (animator direction i guess cause it holds the
            // last frame data
        }
        Debug.Log("SpecialAttack initiated");
    }
    private bool coolDownTimeZero()
    {
        if (CooldownTime > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
