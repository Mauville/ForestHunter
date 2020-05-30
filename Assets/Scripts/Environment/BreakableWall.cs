using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    private Animator animator;
    private bool hasBeenTouched=false;
    public GameObject tuto1;
    public GameObject tuto2;

    private void Start() {
        animator=GetComponent<Animator>();
        tuto1=GameObject.FindGameObjectWithTag("Tuto1");
        tuto2=GameObject.FindGameObjectWithTag("Tuto2");
        StartCoroutine("WaitForLoading");
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("HeavyArrow")){
            if(!hasBeenTouched){
                tuto1.SetActive(false);
                tuto2.SetActive(true);
                animator.SetTrigger("Touched");
                hasBeenTouched=true;
            }
        }
        
    }

    /*
    This waits for all the scripts of type BreakableWall to load the tuto2 before disabling it. 
    Without this, some scripts may not load fast enough to get the tuto2 GameObject. 
    */
    IEnumerator WaitForLoading(){
        yield return new WaitForFixedUpdate();
        tuto2.SetActive(false);
    }
}
