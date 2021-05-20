using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Statistic : MonoBehaviour
{
    public Text onMapGold;
    public Text AllCollected;
    public Text Progress;
    private int prog;
    public Text timeText;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        onMapGold.text = "On Map: " + PlayerPrefs.GetInt("previousGold") + "/6";
        AllCollected.text = "Collected so far: " + PlayerPrefs.GetInt("gold") + "/18";
        prog = Convert.ToInt32((Convert.ToDouble(PlayerPrefs.GetInt("gold"))  / 18 )* 100);
        Debug.Log(prog);
        Progress.text = "Progress: " + prog.ToString() + "%";
        
        if (PlayerPrefs.GetString("timeStr3") == "")
        {
            if (PlayerPrefs.GetString("timeStr2") == "")
            {
                timeText.text = PlayerPrefs.GetString("timeStr1");
            }
            else
            {
                timeText.text = PlayerPrefs.GetString("timeStr2");
            }
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
