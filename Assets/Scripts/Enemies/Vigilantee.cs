using System.Collections;
using UnityEngine;

public class Vigilantee : MonoBehaviour {

    [Header("Movement")]
    public float speed;
    public float distance;
    public float health=50f;
    private bool movingRight=true;
    public Transform groundDetection;
    
    
    //Constant movement update
    void Update() {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        RaycastHit2D groundInfo= Physics2D.Raycast(groundDetection.position, Vector2.down, distance);
        if(groundInfo.collider==false){
            if(movingRight){
                transform.eulerAngles= new Vector3(0,-180,0);
                movingRight=false;
            }
            else{
                transform.eulerAngles= new Vector3(0,0,0);
                movingRight=true;
            }
        }

        if(health<=0){
            Destroy(this.gameObject);
            Hunter.givePoints(10);
        }
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Arrow")){
            health-= other.gameObject.GetComponent<Arrow>().damage;
        }
        if(other.gameObject.CompareTag("FireArrow")){
            health-= other.gameObject.GetComponent<FireArrow>().damage;
            Burn(5);
            StartCoroutine(Burn(other.gameObject.GetComponent<FireArrow>().burningLastInSeconds));
        }
        if(other.gameObject.CompareTag("HeavyArrow")){
            health-= other.gameObject.GetComponent<HeavyArrow>().damage;
        }
        logOnSec();
    }


    IEnumerator logOnSec(){
        yield return new WaitForSeconds(1);
        Debug.Log("asdf");
    }

    IEnumerator Burn(int seconds){
        int i=0;
        while(i<seconds){
            yield return new WaitForSeconds(1);
            health-=5;
        }
    }
}