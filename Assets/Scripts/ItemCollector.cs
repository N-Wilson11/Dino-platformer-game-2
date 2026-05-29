using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    private int coins = 0;

    [SerializeField] private TMPro.TextMeshProUGUI coinText;
    [SerializeField] private TMPro.TextMeshProUGUI coinCountText;
    [SerializeField] private AudioSource coinSoundEffect;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            coinSoundEffect.Play();
            Destroy(collision.gameObject);
            HandleCoinCount();
            coinText.text = "Coins: " + coins;
            Debug.Log("Amount of coins collected: " + coins);
            
            
        }

        
    }

    
    public void HandleCoinCount(){
        coins++;
        PlayerPrefs.SetInt("Coins", coins);
        CheckHighScore();

    }


    public void CheckHighScore()
    {
        if (coins > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", coins);
        }
    }
    public string getCoinCount()
    {
        
        return coinCountText.text = "Amount of coins collected: " + PlayerPrefs.GetInt("Coins");
    }    

    public string getHigscore()
    {
        return coinCountText.text = "Most amount of coins collected: " + PlayerPrefs.GetInt("HighScore");
    }
}
