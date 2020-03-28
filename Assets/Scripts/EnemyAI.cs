using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Methinks 'tis be the biggest turd I have ever created.
// This basically works as a state machine. The enemy is given a target, in this case, the player.
// The enemy has a home where it roams, looking for targets. (State.Roaming)
// When it finds a target, it starts chasing it (State.ChaseTarget), until it gets close enough to it to punch. Punch has not been implemented.
// Finally, when the player leaves the chasing zone, it returns to its home with (State.MovetoHome) and then the machine resets.

public class EnemyAI : MonoBehaviour {

    // This is public for debugging reasons. theres no need for you to change it.
    public enum State {
        Roaming,
        ChaseTarget,
        MoveToHome,
    }
    Vector3 startingPosition;
    Vector3 roamPosition;
    // Instance of player
    public Transform pl;
    public State state;
    // The speed that this uses to roam or get close to a target
    public float speed;
    // How many punches per frame
    public float nextPunchTime;

    // Move to the enemy
    void MoveToTimer (Vector3 position) {
        float distance = Vector3.Distance (gameObject.transform.position, position);
        transform.position = Vector3.Lerp (transform.position, position, (Time.deltaTime * speed) / distance);
    }

    void Awake () {
        // Default to roaming
        state = State.Roaming;
    }
    void Start () {
        startingPosition = transform.position;
    }

    void Update () {

        var reachedPositionDistance = 1f;
        switch (state) {
            case State.Roaming:
                MoveToTimer (roamPosition);
                // Roam around, picking points at random to move to 
                if (Vector3.Distance (transform.position, roamPosition) < reachedPositionDistance) {
                    roamPosition = GetRoamingPosition ();
                }
                FindTarget ();
                break;

            case State.ChaseTarget:
                // Close in on player
                MoveToTimer (pl.transform.position);
                float attackRange = 10f;
                // Target within range
                if (Vector3.Distance (transform.position, pl.transform.position) < attackRange) {

                    // Determine punch rate per second?
                    float fireRate = 1f;
                    if (Time.time > nextPunchTime) {
                        nextPunchTime = Time.time + fireRate;
                        // Punch ();
                        Debug.Log ("Punch Nazi");
                    }
                }
                // A radius to determine where to stop chase
                float stopChaseDistance = 15f;
                if (Vector3.Distance (transform.position, pl.transform.position) > stopChaseDistance) {
                    //Too far, stop chasing
                    state = State.MoveToHome;

                }
                break;

            case State.MoveToHome:
                // Once player is too far away return to roaming.
                MoveToTimer (startingPosition);
                if (Vector3.Distance (transform.position, startingPosition) < reachedPositionDistance) {
                    // Finally home, start roaming
                    state = State.Roaming;
                }
                break;

        }
    }

    // Random target position generator to get to
    private Vector3 GetRoamingPosition () {
        return startingPosition + new Vector3 (UnityEngine.Random.Range (-1f, 1f), 0).normalized * Random.Range (10f, 70f);
    }

    private void FindTarget () {
        // A radius to determine where the player will be 
        float targetRande = 50f;
        if (Vector3.Distance (transform.position, pl.transform.position) < targetRande) {
            state = State.ChaseTarget;
        }
    }

    // Merge with gameManager to remove player life and update HUM
    void Punch (GameObject other) {
        Debug.Log ("Punch Nazi");
    }

}



    // 
    // 
    //  DEPRECATED CODE
    // 
    // 
    // // Start is called before the first frame update
    // public float searchRange = 1;
    // public float stoppingDistance = .3f;

    // public enum States { patrol, pursuit }
    // public States state;
    // Rigidbody2D rb;

    // public Transform player;
    // Vector3 target;
    // Vector2 vel;

    // // This shit makes a and breaks patrol. For some reason, the limit to patrol is delimited by this, so sweetspot it until it works.
    // public float shaize = 30.97f;
    // float verticalSpeed;
    // float horizontalSpeed = 10f;

    // void Start () {
    //     rb = this.GetComponent<Rigidbody2D> ();
    //     InvokeRepeating ("SetTarget", 0, 5);
    //     state = States.patrol;
    //     // InvokeRepeating ("SendPunch", 0, 5);
    // }

    // // Update is called once per frame
    // void Update () {
    //     if (state == States.pursuit) {
    //         target = player.transform.position;
    //         if (Vector3.Distance (target, transform.position) > searchRange) {
    //             target = transform.position;
    //             state = States.patrol;
    //             return;
    //         }
    //     } else if (state == States.patrol) {
    //         var ob = Physics2D.CircleCast (transform.position, searchRange, Vector2.up);
    //         if (ob.collider != null) {
    //             if (ob.collider.CompareTag ("Player")) {
    //                 state = States.pursuit;
    //                 return;
    //             }
    //         }
    //     }
    //     vel = target - transform.position;
    //         Debug.Log ("HERE");
    //     if (vel.magnitude < stoppingDistance) {
    //         Debug.Log ("not here");
    //         vel = Vector2.zero;
    //         vel.Normalize ();
    //         rb.velocity = new Vector2 (horizontalSpeed, 0);
    //     }
    // }

    // // This creates nice circles for debugging areas and patrol radios.
    // void OnDrawGizmosSelected () {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere (transform.position, searchRange);
    //     Gizmos.DrawWireSphere (target, 0.2f);
    // }

    // void SetTarget () {
    //     if (state != States.patrol) {
    //         return;
    //     }
    //     // If target found, move to it. No movement in y
    //     target = new Vector2 (Random.Range (-searchRange, searchRange), transform.position.y);
    // }

    // void SendPunch () {
    //     if (state != States.pursuit)
    //         return;
    //     if (vel.magnitude != 0)
    //         return;
    //     // StartCoroutine (Punch ());
    // }

    // void Punch () {
    //     Debug.Log ("P");
    // }
    // }