using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //enemy health
    public int health = 100;
    public bool Dead;// animation bool for dying
    public Animator anim; //reference to the animator
    public Transform playerLocation;
    public GameObject player;

    // Update is called once per frame
    void Update()
    {
       // Debug.Log(health);
        if(health <= 0)//if health is below 0 set animation to dead
        {
            Dead = true;
            anim.SetBool("dead", Dead);
        }
    }
    private void OnTriggerEnter(Collider collider)//lower health on hit
    {
        health -= 20;
    }
}
