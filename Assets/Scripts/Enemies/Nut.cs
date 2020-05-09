using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent (typeof (AudioSource))]
public class Nut : MonoBehaviour {

    public AudioSource myFx;
    //nuthit.mp3
    public AudioClip hit;

    public float speed;
    private Rigidbody2D rb;

    void Start () {

        rb = this.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2 (speed*1, 0);
        StartCoroutine ("WaitForSec");
    }
    IEnumerator WaitForSec () {
        yield return new WaitForSeconds(3);
        Destroy (this.gameObject);
    }


    void OnCollisionEnter2D (Collision2D other) {
        if(other.gameObject.CompareTag("Arrow") || other.gameObject.CompareTag("Player")){
            Destroy(this.gameObject);
        }
        myFx.PlayOneShot (hit);
    }

}