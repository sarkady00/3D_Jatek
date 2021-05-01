using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
    

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        thePlayer = FindObjectOfType<PlayerController>();
        
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
    }

    public void HurtPlayer(int damage, Vector3 direction)
    {
        if (invincibilityCounter <= 0)
        {
            currentHealth -= damage;
            thePlayer.KnockBack(direction);
            invincibilityCounter = invincibilityLength;

            playerRenderer.enabled = false;
            flashCounter = flashLength;
        }
        
        
    }

    public void HealPlayer(int healAmount)
    {
        currentHealth += healAmount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
