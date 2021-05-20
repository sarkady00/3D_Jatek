using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private int prevGold;

    public int goldNeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewGame(string scene)
    {
        PlayerPrefs.SetInt("currentHP", 100);
        PlayerPrefs.SetInt("chance", 3);
        PlayerPrefs.DeleteKey("gold");
        PlayerPrefs.DeleteKey("previousGold");
        PlayerPrefs.SetString("timeStr1", "");
        PlayerPrefs.SetString("timeStr2", "");
        PlayerPrefs.SetString("timeStr3", "");
        
        SceneManager.LoadScene(scene);
        
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerPrefs.SetInt("gold", FindObjectOfType<GameManager>().GetGoldCount());
            prevGold = PlayerPrefs.GetInt("gold") - PlayerPrefs.GetInt("previousGold");
            PlayerPrefs.SetInt("previousGold", prevGold);
            PlayerPrefs.SetInt("currentHP", FindObjectOfType<HeathManager>().GetHP());
            PlayerPrefs.SetInt("chance", FindObjectOfType<HeathManager>().GetChance());
            //PlayerPrefs.SetString("timeStr", FindObjectOfType<HeathManager>().GetTimeStr());
            //PlayerPrefs.SetFloat("timeFloat", FindObjectOfType<HeathManager>().GetTimeFloat());

            if (SceneManager.GetActiveScene().name == "lvl3")
            {
                PlayerPrefs.SetString("timeStr3", FindObjectOfType<HeathManager>().GetTimeStr());
                //PlayerPrefs.SetFloat("timeFloat3", FindObjectOfType<HeathManager>().GetTimeFloat());
            }
            if (SceneManager.GetActiveScene().name == "lvl2")
            {
                PlayerPrefs.SetString("timeStr2", FindObjectOfType<HeathManager>().GetTimeStr());
                //PlayerPrefs.SetFloat("timeFloat2", FindObjectOfType<HeathManager>().GetTimeFloat());
            }
            if (SceneManager.GetActiveScene().name == "Labirinth")
            {
                PlayerPrefs.SetString("timeStr1", FindObjectOfType<HeathManager>().GetTimeStr());
                //PlayerPrefs.SetFloat("timeFloat1", FindObjectOfType<HeathManager>().GetTimeFloat());
            }
            
            
            if (SceneManager.GetActiveScene().name == "lvl3")
            {
                if (PlayerPrefs.GetInt("gold") < goldNeed)
                {
                    Cursor.lockState = CursorLockMode.None;
                    SceneManager.LoadScene("Hell");
                }
                else
                {
                    Cursor.lockState = CursorLockMode.None;
                    SceneManager.LoadScene("Heaven");
                }
            }
            else
            {
                SceneManager.LoadScene("Statistic");
            }
        }
    }

    public void StatNextMap()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("mapIndex") + 1);
    }
    
    
}
