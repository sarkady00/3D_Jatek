using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
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
        PlayerPrefs.DeleteKey("gold");
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        }
    }
    
    
}
