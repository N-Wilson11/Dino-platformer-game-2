using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Levelselector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SelectLevel1()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void SelectLevel2()
    {
        SceneManager.LoadScene("Level 2");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Level Select");
    }
    
}
