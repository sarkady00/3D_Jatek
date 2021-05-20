using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public HeathManager theHealthMan;

    public Renderer theRend;

    public Material cpOff;

    public Material cpOn;

    // Start is called before the first frame update
    void Start()
    {
        theHealthMan = FindObjectOfType<HeathManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckpointOn() // aktiváljuk a checkpointot
    {
        Checkpoint[] checkpoints = FindObjectsOfType<Checkpoint>();
        foreach (Checkpoint cp in checkpoints)
        {
            cp.CheckpointOff(); // az összeset deaktiváljuk
        }
        
        theRend.material = cpOn; // kivéve amelyikbe utoljára belementünk
    }

    public void CheckpointOff()
    {
        theRend.material = cpOff;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player")) // ha belemegyünk akkor az fog aktiválódni a többi pedig deaktiválódni
        {
            theHealthMan.SetSpawnPoint(transform.position);
            CheckpointOn();
        }
    }
}
