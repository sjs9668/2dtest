using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed = 5f;
    Rigidbody2D rb;
    SpriteRenderer spr;
    Animator anim;

   void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetButtonUp("Horizontal"))
        {
            rb.velocity = new Vector2(rb.velocity.normalized.x * 0.5f, rb.velocity.y);
        }

        if (Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") < 0)
        {
            if (rb.velocity.x <= 0)
                spr.flipX = true;
        }
        else if (Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") > 0)
        {
            if (rb.velocity.x >= 0)
                spr.flipX = false;
        }

        if (Mathf.Abs(rb.velocity.x) < 0.3)
        {
            anim.SetBool("isWalking", false);
        }
        else
        {
            anim.SetBool("isWalking", true);
        }
    }
     void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");

        
         rb.velocity = new Vector2(h * maxSpeed, rb.velocity.y);



        // 속도를 제한합니다.
        if (rb.velocity.x > maxSpeed)
        {
            rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
        }
        else if (rb.velocity.x < -maxSpeed)
        {
            rb.velocity = new Vector2(-maxSpeed, rb.velocity.y);
        }
    }
}