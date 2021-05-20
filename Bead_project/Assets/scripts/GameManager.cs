using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int currentGold;
    public Text goldText;

    public Text hpText;

    public Text chanceText;

    public Text timeText;
    
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

    public void HpUpdate(int hp)
    {
        hpText.text = "HP: " + hp;
    }

    public void ChanceUpdate(int ch)
    {
        chanceText.text = "Chance: " + ch;
    }

    public void TimeUpdate(string time)
    {
        timeText.text = time;
    }
    

    public int GetGoldCount()
    {
        return currentGold;
    }
    
    
}
