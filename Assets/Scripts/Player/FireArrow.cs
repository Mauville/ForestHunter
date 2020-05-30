using System.Collections;
using UnityEngine;
public class FireArrow : MonoBehaviour {
    public float speed=20f;
    public int damage=20;
    private Rigidbody2D rb;

    public int burningLastInSeconds=5;

    void Start(){
        rb=this.GetComponent<Rigidbody2D>();
        rb.velocity= new Vector2(speed * Hunter.direction,0);
        StartCoroutine("DieInSeconds");
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Damage")||other.gameObject.CompareTag("Ground")||other.gameObject.CompareTag("Damage")||other.gameObject.CompareTag("Arrow")||other.gameObject.CompareTag("Grippable")||other.gameObject.CompareTag("End")||other.gameObject.CompareTag("FireArrow")||other.gameObject.CompareTag("HeavyArrow")||other.gameObject.CompareTag("Damage")||other.gameObject.CompareTag("GrapplingUpgrade")||other.gameObject.CompareTag("FireUpgrade")||other.gameObject.CompareTag("HeavyUpgrade")){
            Destroy(this.gameObject);
        }
    }
    IEnumerator DieInSeconds(){
        yield return new WaitForSeconds(0.8f);
        Destroy(gameObject);
    }

}