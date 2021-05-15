using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int currentGold;
    public Text goldText;
    
    // Start is called before the first frame update
    void Start()
    {
        currentGold = PlayerPrefs.GetInt("gold");
        GetComponent<GameManager>().AddGold(0); // frissítjük a kiíratást
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddGold(int goldToAdd)
    {
        currentGold += goldToAdd;
        goldText.text = "Gold: " + currentGold;
    }

    public int GetGoldCount()
    {
        return currentGold;
    }
}
