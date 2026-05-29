using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private ItemCollector itemCollector;
    [SerializeField] private TMPro.TextMeshProUGUI coinCountText;
    void Start()
    {
        itemCollector = GetComponent<ItemCollector>();
    }

    public void Update()
    {
        Debug.Log("Muntjessss: " + itemCollector.getCoinCount());
            // Haal de hoeveelheid munten op van ItemCollector en werk de UI bij
            string coinCount = itemCollector.getCoinCount();
            coinCountText.text = coinCount;
        
        
    }
    
}
