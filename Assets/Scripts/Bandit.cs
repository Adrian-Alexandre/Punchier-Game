using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bandit : MonoBehaviour
{
    [SerializeField] private Rigidbody[] _ragRb;
    [SerializeField] private Collider[] _ragColliders;
    [SerializeField] private BoxCollider mainCollider;
    [SerializeField] private AudioSource audioSource;

    public PlayerMovement player;
    void Awake()
    {
        _ragRb = GetComponentsInChildren<Rigidbody>();
        _ragColliders = GetComponentsInChildren<Collider>();
        GameObject audioObject = GameObject.Find("AudioPunch");
        audioSource = audioObject.GetComponent<AudioSource>();
        DisableRagdoll();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (player.isPunching == true && collision.collider.tag == "Hand")
        {
            EnableRagdoll();
            audioSource.Play();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (player.isPunching == true && collision.collider.tag == "Hand")
        {
            EnableRagdoll();
            audioSource.Play();
        }
    }

    public void DisableRagdoll()
    {
        foreach(Collider col in _ragColliders)
        {
            col.enabled = false;
        }

        foreach (Rigidbody rigid in _ragRb)
        {
            rigid.isKinematic = true;
        }

        GetComponent<Animator>().enabled = true;
        GetComponent<Collider>().enabled = true;
    }

    public void EnableRagdoll()
    {
        GetComponent<Animator>().enabled = false;
        
        foreach (Collider col in _ragColliders)
        {
            col.enabled = true;
        }
        
        foreach (Rigidbody rigid in _ragRb)
        {
            rigid.isKinematic = false;
        }

       GetComponent<Collider>().enabled = false;
    }
}
