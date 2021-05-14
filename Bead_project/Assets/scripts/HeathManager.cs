using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

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

    // Start is called before the first frame update
    void Start()
    {
        respawnPoint = thePlayer.transform.position;
        currentHealth = maxHealth;
        Debug.Log("Kiscica, krumpli, Lófasz");
        //thePlayer = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
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

            if (currentHealth <= 0)
            {
                Respawn();
                Debug.Log("Halott");
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
}
