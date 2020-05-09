using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzyController : MonoBehaviour {

    public Hunter hunter;

    /*
    public AudioSource myFx;
    //Attract.mp3
    public AudioClip dizzy;
    */
    public float movingDistance = .2f;
    public float Speed = .4f;

    Vector3 _ping;
    Vector3 _pong;

    Vector3 _target;
    // Start is called before the first frame update
    void Start()
    {
        _ping = this.transform.position + new Vector3(0,this.transform.position.y+movingDistance,0);
        _pong = this.transform.position + new Vector3(0,this.transform.position.y-movingDistance,0);
        _target = _ping;
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }

    void setTarget(Vector3 target) {
        _target = target;
    }

    void move() {
        float distance = Vector3.Distance(gameObject.transform.position, _target);
        if (distance <= 0) {
            if (_target == _ping) {
                setTarget(_pong);
            } else {
                setTarget(_ping);
            }
        } else {
            transform.position = Vector3.Lerp(transform.position, _target, (Time.deltaTime * Speed) / distance);
        }
    }
    void OnCollisionEnter2D (Collision2D other) {
        if(other.gameObject.CompareTag("Player")){
            Destroy(this.gameObject);
            hunter.drug();
        }
    }
}