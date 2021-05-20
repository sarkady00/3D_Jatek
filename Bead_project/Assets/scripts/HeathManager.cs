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
        chance = PlayerPrefs.GetInt("chance");
        GetComponent<GameManager>().ChanceUpdate(chance);
        
        currentHealth = PlayerPrefs.GetInt("currentHP");
        GetComponent<GameManager>().HpUpdate(currentHealth);
        
        respawnPoint = thePlayer.transform.position;
        
        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            PlayerPrefs.SetInt("mapIndex", SceneManager.GetActiveScene().buildIndex);
        }
        //Debug.Log("Kiscica, krumpli, Lófasz");
        //thePlayer = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        int minutes = Mathf.FloorToInt(Timer / 60F);
        int seconds = Mathf.FloorToInt(Timer % 60F);
        int milliseconds = Mathf.FloorToInt((Timer * 100F) % 100F);
        timerString = minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + milliseconds.ToString("00");
        GetComponent<GameManager>().TimeUpdate(timerString);
        
        if (invincibilityCounter > 0)
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

        if (isFadeToBlack)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b,
                Mathf.MoveTowards(blackScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
            if (blackScreen.color.a == 1f) 
            {
                isFadeToBlack = false;
            }
        }
        
        if (isFadeFromBlack)
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
            GetComponent<GameManager>().HpUpdate(currentHealth);

            if (currentHealth <= 0)
            {
                
                // Debug.Log("Halott");
                chance -= 1;
                GetComponent<GameManager>().ChanceUpdate(chance);
                
                if (chance == 0)
                {
                    Cursor.lockState = CursorLockMode.None;
                    SceneManager.LoadScene("Hell");
                }
                else
                {
                    Respawn();
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
        thePlayer.gameObject.SetActive(false);
        Instantiate(deathEffect, thePlayer.transform.position, thePlayer.transform.rotation);
        yield return new WaitForSeconds(respawnLength);
        isFadeToBlack = true;
        
        invincibilityCounter = invincibilityLength; // újra éledéskor kis sebezhetetlenség
        playerRenderer.enabled = false;
        flashCounter = flashLength;
        
        yield return new WaitForSeconds(waitForFade);
        isFadeToBlack = false;
        isFadeFromBlack = true;

        isRespawning = false; 
        thePlayer.gameObject.SetActive(true);
        thePlayer.transform.position = respawnPoint;
        
        yield return new WaitForSeconds(0.00000000001f);

        thePlayer.transform.position = respawnPoint;
        currentHealth = maxHealth;
        GetComponent<GameManager>().HpUpdate(currentHealth);
        
        
        
        
    }

    public void HealPlayer(int healAmount)
    {
        currentHealth += healAmount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void SetSpawnPoint(Vector3 newPosition)
    {
        respawnPoint = newPosition;
    }
    
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
    
    public float GetTimeFloat()
    {
        return Timer;
    }
    
}
