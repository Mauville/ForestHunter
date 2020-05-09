using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hunter : MonoBehaviour
{   
    public int healthVal=5;

    [Header ("Movement")]
    public Transform groundCheck;
    public float speed;
    public float jumpForce;
    
    public static int direction=1;
    private float originalXScale;
    
    public float checkRadious;
    public LayerMask whatIsGround;

    [HideInInspector]   
    public int extraJumps;
    public int extraJumpsVal;

    [Header ("Attack")]
    public GameObject arrowPrefab;
    public Transform bowAim;
    


    [Header("UnityComponents")]
    private Rigidbody2D rb;
    private Animator anim;
    private GameMaster gameMaster;


    public static int health;   
    [HideInInspector]
    public float moveInput;    
    [HideInInspector]
    public bool isGrounded, isFlipped=false, hasGrapplingUpgrade=false;

    public bool isDrugged=false;


    void Start(){
        gameMaster= GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        transform.position= gameMaster.lastCheckPointPos;

        anim= GetComponent<Animator>();
        rb= GetComponent<Rigidbody2D>();
        originalXScale = transform.localScale.x;
        extraJumps= extraJumpsVal;
        health=healthVal;
    }

    void FixedUpdate() {
        isGrounded=Physics2D.OverlapCircle(groundCheck.position, checkRadious, whatIsGround);
        moveInput= Input.GetAxis("Horizontal");
        if(isDrugged){
            Debug.Log(isDrugged);
            rb.velocity= new Vector2(moveInput*-speed, rb.velocity.y);   
        }else{
            rb.velocity= new Vector2(moveInput*speed, rb.velocity.y);
        }

        if(moveInput == 0){
            anim.SetBool("isRunning", false);
        }else {
            anim.SetBool("isRunning", true);
        }
        float xVelocity= speed* moveInput;
        if(xVelocity*direction<0f){ 
            FlipDirection();
            isFlipped= !isFlipped;
        }


        if(health<= 0){
            kill();
        }
    }
    void Update() {
        if(isGrounded){
            extraJumps= extraJumpsVal;
            anim.SetBool("isJumping",false);
        }else{
            anim.SetBool("isJumping", true);
        }

        if(Input.GetKeyDown(KeyCode.Space) && extraJumps>0){
            anim.SetTrigger("TakeOff");
            rb.velocity= Vector2.up*jumpForce;
            extraJumps--;
        }else if(Input.GetKeyDown(KeyCode.Space) && extraJumps == 0 && isGrounded){
            rb.velocity= Vector2.up*jumpForce;
        }

        if(Input.GetMouseButtonDown(0)){ 
            shootArrow();   
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Damage")){
            ouch();
            Destroy(other.gameObject);
        }
    }

    void shootArrow(){
        anim.SetTrigger("isAttacking");
        GameObject arrow= Instantiate(arrowPrefab) as GameObject;
        arrow.transform.position = bowAim.position;
        if(isFlipped){
            Vector3 scale= arrow.transform.localScale;
            scale.x= -1*direction;
            arrow.transform.localScale=scale;
        }
        Destroy(arrow,3);
    }


    void FlipDirection(){
        direction*=-1;
        Vector3 scale= transform.localScale;
        scale.x=originalXScale*direction;
        transform.localScale=scale;
    }

    void kill(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ouch(){
        health--;
    }
    public void bigOuch(){
        health-=2;
    }

    public void drug(){
        isDrugged=true;
        Debug.Log("dale");
        StartCoroutine("getHigh");
    }

    IEnumerator getHigh(){
        yield return new WaitForSeconds(10);
        isDrugged=false;
    }

}
