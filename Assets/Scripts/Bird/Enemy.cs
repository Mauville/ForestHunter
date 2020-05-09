using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public GameObject poop;
    public GameObject cannon;
    public float time;
    
    void Start()
    {
    	InvokeRepeating("Shoot", 0f,time);   
    }
    private void Shoot(){
        GameObject drop= Instantiate(poop,cannon.transform.position,Quaternion.identity) as GameObject;
        Destroy(drop,4);
    }
}
