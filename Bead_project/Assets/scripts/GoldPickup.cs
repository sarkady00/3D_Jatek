using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GoldPickup : MonoBehaviour
{
    public int value;

    public GameObject pickupEffect;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            FindObjectOfType<GameManager>().AddGold(value);

            Instantiate(pickupEffect, transform.position, transform.rotation); // csillanás
            Destroy(gameObject);
        }
    }
}
