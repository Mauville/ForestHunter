using System.Collections;
using UnityEngine;

public class HeavyArrow : MonoBehaviour {
    public float speed=9f;
    public int damage=40;
    private Rigidbody2D rb;

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
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}