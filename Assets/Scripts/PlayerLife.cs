using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private int lives = 3;
    [SerializeField] private AudioSource dieSoundEffect;
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Spike" || collision.gameObject.tag == "Bomb")
        {
            
                lives--;
                dieSoundEffect.Play();
                Die();
            
        }
        else if(collision.gameObject.tag.Equals("EnemyKiller")){
            Debug.Log("Player has hit the enemy from above");
        }
    }

    private void Die()
    {
        rb.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("death");
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

   
}
