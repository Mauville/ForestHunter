using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnableBush : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("FireArrow")){
            Destroy(this.gameObject);
        }
    }
}
