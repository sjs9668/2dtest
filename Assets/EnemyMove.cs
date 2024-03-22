using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rd;
    Animator anim;
    SpriteRenderer spr;

    public int nextMove;
    
    
    void Awake()
    {
        rd = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();

        Invoke("Think", 5);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rd.velocity = new Vector2(nextMove, rd.velocity.y);

        Vector2 frontVec = new Vector2(rd.position.x + nextMove*0.2f, rd.position.y);
            Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rh = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if(rh.collider == null)

            {
            Trun();
            }
        
        
    }

    void Think()
    {
        nextMove = Random.Range(-1, 2);

        anim.SetInteger("WalkSpeed", nextMove);
        if(nextMove != 0)
        {
            spr.flipX = nextMove == 1;
        }

        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);
    }

    void Trun()
    {
        nextMove *= -1;
        spr.flipX = nextMove == 1;

        CancelInvoke();
        Invoke("Think", 5);
    }
}
