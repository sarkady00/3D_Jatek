using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private int prevGold;

    public int goldNeed; // győzelemhez szükséges arany mennyiség megadása
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
        // mindent visszaállítunk alap értékre vagy kiürítünk
        PlayerPrefs.SetInt("currentHP", 100);
        PlayerPrefs.SetInt("chance", 3);
        PlayerPrefs.SetString("timeStr1", "");
        PlayerPrefs.SetString("timeStr2", "");
        PlayerPrefs.SetString("timeStr3", "");
        
        PlayerPrefs.DeleteKey("gold");
        PlayerPrefs.DeleteKey("previousGold");
        
        SceneManager.LoadScene(scene); // betöltjük a scene-t
        
    }

    public void ExitGame()
    {
        Application.Quit(); // kilépünk
    }

    public void OnTriggerEnter(Collider other) // ha belemegyünk a fába
    {
        if (other.tag == "Player") // ha a játékos volt
        {
            // tovább lépés előtt elmentjük az eredményeket a statisztikához és a követekző pályára való átvitelhez
            PlayerPrefs.SetInt("gold", FindObjectOfType<GameManager>().GetGoldCount());
            prevGold = PlayerPrefs.GetInt("gold") - PlayerPrefs.GetInt("previousGold");
            PlayerPrefs.SetInt("previousGold", prevGold);
            PlayerPrefs.SetInt("currentHP", FindObjectOfType<HeathManager>().GetHP());
            PlayerPrefs.SetInt("chance", FindObjectOfType<HeathManager>().GetChance());


            // elmentjük minden pálya teljesítésének idejét
            if (SceneManager.GetActiveScene().name == "lvl3")
            {
                PlayerPrefs.SetString("timeStr3", FindObjectOfType<HeathManager>().GetTimeStr());
            }
            if (SceneManager.GetActiveScene().name == "lvl2")
            {
                PlayerPrefs.SetString("timeStr2", FindObjectOfType<HeathManager>().GetTimeStr());
            }
            if (SceneManager.GetActiveScene().name == "Labirinth")
            {
                PlayerPrefs.SetString("timeStr1", FindObjectOfType<HeathManager>().GetTimeStr());
            }
            
            // ha az utolsó pályán megyünk a fába
            if (SceneManager.GetActiveScene().name == "lvl3")
            {
                if (PlayerPrefs.GetInt("gold") < goldNeed) // ha nincs elég aranyunk
                {
                    Cursor.lockState = CursorLockMode.None;
                    SceneManager.LoadScene("Hell"); // akkor megyünk a pokolba
                }
                else
                {
                    Cursor.lockState = CursorLockMode.None;
                    SceneManager.LoadScene("Heaven"); // ha van elég akkor nyertünk
                }
            }
            else
            {
                SceneManager.LoadScene("Statistic"); // egyébként meg betöltjük az adott pálya statisztikáját
            }
        }
    }

    public void StatNextMap()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("mapIndex") + 1);
    }
    
    
}
