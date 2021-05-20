using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heaven : MonoBehaviour
{
    public Text firstText;

    public Text secondText;

    public Text thirdText;
    // Start is called before the first frame update
    void Start()
    {
        firstText.text = "LVL1\n" + PlayerPrefs.GetString("timeStr1");
        secondText.text = "LVL2\n" + PlayerPrefs.GetString("timeStr2");
        thirdText.text = "LVL3\n" + PlayerPrefs.GetString("timeStr3");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
