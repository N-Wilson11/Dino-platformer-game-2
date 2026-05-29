using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private float dirX = 0;
    private float moveSpeed = 7f;
   [SerializeField] private float jumpForce = 11f;
    private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private AudioSource jumpSoundEffect;
    
    
    
    // Start is called before the first frame update
    private void Start()
    { 
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
       
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        

        rb.linearVelocity = new Vector2(dirX * moveSpeed , rb.linearVelocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpSoundEffect.Play();
        }

        UpdateAnimationState();
        
         
    }
    private void UpdateAnimationState()
    {
        if (dirX > 0)
        {
            anim.SetBool("walking", true);
            sprite.flipX = false;
        }

        else if (dirX < 0)
        {
            anim.SetBool("walking", true);
            sprite.flipX = true;
        }

        else
        {
            anim.SetBool("walking", false);
        }
    }



    private bool isGrounded()
    {
       return Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
        
    }
}
