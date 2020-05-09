using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AudioSource))]

public class Squirrel : MonoBehaviour {
    public Transform firePoint;
    public GameObject bulletPrefab;
    public int life = 1;
    public float time;


    public AudioSource myFx;
    //Camouflage.mp3
    public AudioClip throwFX;

    private void Start() {
        InvokeRepeating("Shoot", 0f,time);
    }

    private void Shoot(){
        Debug.Log("shoot");
        GameObject drop= Instantiate(bulletPrefab,firePoint.transform.position,Quaternion.identity) as GameObject;
        Destroy(drop,4);
    }
        //Vector3 position = new Vector3 (firePoint.position.x, firePoint.position.y + 1, firePoint.position.z);
        //Instantiate (bulletPrefab, position, firePoint.rotation);
    
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Arrow")){
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }

}