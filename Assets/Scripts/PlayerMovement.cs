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
    [SerializeField] private float downFallBoost = 18f;
    private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private AudioSource jumpSoundEffect;
    private bool mobileLeftPressed;
    private bool mobileRightPressed;
    private bool mobileDownPressed;
    private bool mobileJumpQueued;
    
    
    
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
        float keyboardHorizontal = Input.GetAxisRaw("Horizontal");
        float mobileHorizontal = 0f;

        if (mobileLeftPressed)
        {
            mobileHorizontal -= 1f;
        }

        if (mobileRightPressed)
        {
            mobileHorizontal += 1f;
        }

        dirX = Mathf.Abs(mobileHorizontal) > 0.01f ? mobileHorizontal : keyboardHorizontal;

        rb.linearVelocity = new Vector2(dirX * moveSpeed , rb.linearVelocity.y);

        if (mobileDownPressed && !IsGrounded() && rb.linearVelocity.y < 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y - (downFallBoost * Time.deltaTime));
        }

        bool jumpPressed = Input.GetButtonDown("Jump") || mobileJumpQueued;

        if (jumpPressed && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            if (jumpSoundEffect != null)
            {
                jumpSoundEffect.Play();
            }
        }

        mobileJumpQueued = false;

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



    private bool IsGrounded()
    {
       return Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
        
    }

    public void SetMobileLeft(bool isPressed)
    {
        mobileLeftPressed = isPressed;
    }

    public void SetMobileRight(bool isPressed)
    {
        mobileRightPressed = isPressed;
    }

    public void SetMobileDown(bool isPressed)
    {
        mobileDownPressed = isPressed;
    }

    public void MobileJump()
    {
        mobileJumpQueued = true;
    }
}
