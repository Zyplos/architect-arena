using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Animator myDoor = null;
    [SerializeField] private bool openTrigger = false;
    public AudioSource source;
    
    private void OnTriggerEnter(Collider other)
    {
  
        if(other.CompareTag("Player"))
        {
            if (openTrigger)
            {
                source.Play();
                myDoor.Play("DoorOpen");
            }
        }
    }

}
