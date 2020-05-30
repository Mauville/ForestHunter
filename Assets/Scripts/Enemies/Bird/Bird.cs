using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [Header("Prefabs")]
    private Animator animator;
	public GameObject poop;
    public GameObject cannon;


    public float time;
    public int healthBird=20;
    private float originalXScale;
    private bool isFlipped;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        InvokeRepeating("Shoot", 0f,time);
        originalXScale= transform.localScale.x;
    }
    private void Update() {
        if(healthBird<=0){
            Die();
        }
    }
    private void Shoot(){
        GameObject drop= Instantiate(poop,cannon.transform.position,Quaternion.identity) as GameObject;
        Destroy(drop,2);
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Arrow")){
            healthBird-= other.gameObject.GetComponent<Arrow>().damage;
        }
        if(other.gameObject.CompareTag("FireArrow")){
            healthBird-= other.gameObject.GetComponent<FireArrow>().damage;
            StartCoroutine(Burn(other.gameObject.GetComponent<FireArrow>().burningLastInSeconds));
        }
        if(other.gameObject.CompareTag("HeavyArrow")){
            healthBird-= other.gameObject.GetComponent<HeavyArrow>().damage;
        }
    }
    public void Die(){
        animator.SetTrigger("isDead");
        StartCoroutine("Wait4MeToDie");
    }
    private IEnumerator Wait4MeToDie(){
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
        Hunter.givePoints(20);
    }
    IEnumerator Burn(int seconds){
        int i=0;
        while(i<seconds){
            yield return new WaitForSeconds(1);
            healthBird-=5;
        }
    }
}
