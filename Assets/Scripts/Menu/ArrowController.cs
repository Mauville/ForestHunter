using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float speed;

    private Rigidbody2D rb;

    void Start(){
        rb=this.GetComponent<Rigidbody2D>();
        rb.velocity= new Vector2(speed * Hunter.direction,0);
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(!other.gameObject.CompareTag("Player")){
            Destroy(this.gameObject);
        }
    }
}
