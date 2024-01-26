using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TriggerParticleActivation : MonoBehaviour
{
    public ParticleSystem passaros;
    public AudioSource audioSource;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Change "Player" to the tag of the object you want to trigger the particle system
        {
            passaros.Play();
            audioSource.Play();
        }
    }
}

