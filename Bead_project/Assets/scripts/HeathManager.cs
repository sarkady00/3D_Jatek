using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HeathManager : MonoBehaviour
{

    public int maxHealth;
    public int currentHealth;

    public PlayerController thePlayer;

    public float invincibilityLength;
    private float invincibilityCounter;
    public float flashLength = 0.1f;
    private float flashCounter;

    public Renderer playerRenderer;

    private bool isRespawning;
    private Vector3 respawnPoint;
    public float respawnLength;

    public GameObject deathEffect;
    public Image blackScreen;
    private bool isFadeToBlack;
    private bool isFadeFromBlack;
    public float fadeSpeed;
    public float waitForFade;

    private int chance;

    private float Timer;
    private string timerString;
    

    // Start is called before the first frame update
    void Start()
    {
        Timer = 0;
        chance = PlayerPrefs.GetInt("chance"); // lekérjük a megmaradt esélyeink számát
        GetComponent<GameManager>().ChanceUpdate(chance);// frissítjük a kiíratást
        
        // ugyan ez a hp-ra
        currentHealth = PlayerPrefs.GetInt("currentHP");
        GetComponent<GameManager>().HpUpdate(currentHealth);
        
        respawnPoint = thePlayer.transform.position; // spawn beállítása
        
        if (SceneManager.GetActiveScene().buildIndex != 1) // az 1-es indexű a statisztika oldal
        {
            PlayerPrefs.SetInt("mapIndex", SceneManager.GetActiveScene().buildIndex); // elmentjük a scene indexet
        }
    }

    // Update is called once per frame
    void Update()
    {
        // idő mérése
        Timer += Time.deltaTime;
        int minutes = Mathf.FloorToInt(Timer / 60F);
        int seconds = Mathf.FloorToInt(Timer % 60F);
        int milliseconds = Mathf.FloorToInt((Timer * 100F) % 100F);
        timerString = minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + milliseconds.ToString("00");
        GetComponent<GameManager>().TimeUpdate(timerString); // idő kiíratása a UI-ra
        
        if (invincibilityCounter > 0) // ha sérhetetlenek vagyunk
        {
            invincibilityCounter -= Time.deltaTime;
            flashCounter -= Time.deltaTime;
            if (flashCounter <= 0)
            {
                playerRenderer.enabled = !playerRenderer.enabled;
                flashCounter = flashLength;
            }

            if (invincibilityCounter <= 0)
            {
                playerRenderer.enabled = true;
            }
        }

        if (isFadeToBlack) // elsötétítés
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b,
                Mathf.MoveTowards(blackScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
            if (blackScreen.color.a == 1f) 
            {
                isFadeToBlack = false;
            }
        }
        
        if (isFadeFromBlack) // sötétítés visszavonása
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b,
                Mathf.MoveTowards(blackScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
            if (blackScreen.color.a == 0f)
            {
                isFadeFromBlack = false;
            }
        }
    }

    public void HurtPlayer(int damage, Vector3 direction)
    {
        if (invincibilityCounter <= 0)
        {
            currentHealth -= damage;
            GetComponent<GameManager>().HpUpdate(currentHealth); // frissítjük a kiírást

            if (currentHealth <= 0) // ha meghalunk
            {
                chance -= 1;
                GetComponent<GameManager>().ChanceUpdate(chance);// frissítjük a kiírást
                
                if (chance == 0) // ha nincs több esélyünk
                {
                    Cursor.lockState = CursorLockMode.None; // felszabadítjuk az egeret
                    SceneManager.LoadScene("Hell"); // akkor vesztettünk
                }
                else
                {
                    Respawn(); // egyébként újra próbálkozunk
                }
            }
            else
            {
                thePlayer.KnockBack(direction);
                invincibilityCounter = invincibilityLength;

                playerRenderer.enabled = false;
                flashCounter = flashLength;
            }
        }
    }

    private void Respawn()
    {
        if (!isRespawning)
        {
            StartCoroutine("RespawnCo");
        }
        
    }

    public IEnumerator RespawnCo()
    {
        isRespawning = true;
        thePlayer.gameObject.SetActive(false); // eltüntetjük a karaktert
        Instantiate(deathEffect, thePlayer.transform.position, thePlayer.transform.rotation); // lejátszuk a halál effektet az adott helyen
        yield return new WaitForSeconds(respawnLength);
        isFadeToBlack = true; // elsötétül a kép
        
        invincibilityCounter = invincibilityLength; // újra éledéskor kis sebezhetetlenség
        playerRenderer.enabled = false;
        flashCounter = flashLength;
        
        yield return new WaitForSeconds(waitForFade);
        isFadeToBlack = false;
        isFadeFromBlack = true;

        isRespawning = false; 
        thePlayer.gameObject.SetActive(true);
        thePlayer.transform.position = respawnPoint; // a respawnpoint-hoz teleportáljuk a karaktert
        
        yield return new WaitForSeconds(0.00000000001f); // enélkül sokszor nem teleportált vissza a spawn helyre

        thePlayer.transform.position = respawnPoint; // biztos ami biztos
        currentHealth = maxHealth;
        GetComponent<GameManager>().HpUpdate(currentHealth); // frissítjük a kiírást
    }

    public void SetSpawnPoint(Vector3 newPosition)
    {
        respawnPoint = newPosition;
    }
    
    // érték lekérő függvények:
    public int GetHP()
    {
        return currentHealth;
    }
    
    public int GetChance()
    {
        return chance;
    }
    
    public string GetTimeStr()
    {
        return timerString;
    }
}
