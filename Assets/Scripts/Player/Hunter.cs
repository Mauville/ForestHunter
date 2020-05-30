using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hunter : MonoBehaviour
{   
    [Header ("Status Effects")]
    [HideInInspector] public bool isGrounded;
    public int maxHealth=100;
    [HideInInspector] public static int score=0;
    [HideInInspector]int health;
    private bool isDrugged=false;
    public SpriteRenderer head;
    public Sprite normalHead;
    public Sprite druggedNormalHead;
    public Sprite skillHead;    

    public Sprite druggedSkillhead;


    [Header ("Scripts")]
    public HealthBar healthBar;
    public TextHelper textHelper;
    public ArrowsPanel arrowsPanel;

    [Header ("Movement")]
    public Transform groundCheck;
    public float speed;
    public float jumpForce;
    
    public static int direction=1;
    private float originalXScale;
    
    public float checkRadious;
    public LayerMask whatIsGround;
    [HideInInspector]public bool isFlipped=false;

    [HideInInspector] public int extraJumps;
    public int extraJumpsVal;
    [HideInInspector] public float moveInput;   

    [Header ("Attack")]
    public GameObject arrowPrefab;
    public GameObject fireArrowPrefab;
    public GameObject heavyArrowPrefab;
    public Transform bowAim;
    public SpriteRenderer quiverImage;
    public Sprite normalQuiver;
    public Sprite fireQuiver;
    public Sprite heavyQuiver;
    private bool hasFireArrow;
    private bool isTrigFireUpgrade;
    private GameObject triggeredFireUpgrade;
    private bool hasHeavyArrow;
    private bool isTrigHeavyUpgrade;
    private GameObject triggeredHeavyUpgrade;
    [HideInInspector] public bool hasGrapplingUpgrade=false;
    [HideInInspector] public bool isTrigGrapplingUpgrade;
    private GameObject triggeredGrapplingUpgrade;
    [HideInInspector] public ArrowType currentArrowType = ArrowType.Normal;

    [Header("UnityComponents")]
    public GameObject textHelperObject;
    private Rigidbody2D rb;
    private Animator anim;
    private GameMaster gameMaster;  

    public enum ArrowType{
        Normal,
        Fire,
        Heavy
    }



    void Start(){
        gameMaster= GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        //Checkpoint
        transform.position= gameMaster.lastCheckPointPos;


        //CheckForUpgrades
        hasGrapplingUpgrade=gameMaster.hasGrapplingUpgrade;
        if(hasGrapplingUpgrade){
            head.sprite=skillHead;
        }
        hasFireArrow=gameMaster.hasFireArrow;
        if(hasFireArrow){
            arrowsPanel.unlockGUIFireArrow();
        }
        hasHeavyArrow=gameMaster.hasHeavyArrow;
        if(hasHeavyArrow){
            arrowsPanel.unlockGUIHeavyArrow();
        }

        anim= GetComponent<Animator>();
        rb= GetComponent<Rigidbody2D>();
        originalXScale = transform.localScale.x;
        extraJumps= extraJumpsVal;


        health=maxHealth;
        healthBar.SetMaxHealth();
        textHelper=textHelperObject.GetComponent<TextHelper>();

        givePoints(0);

    }
    
    //Score
    public static void givePoints(int points){
        score+=points;
        FindObjectOfType<Score>().setScore(score);
        
    }




    //Mvement
    void FixedUpdate() {
        isGrounded=Physics2D.OverlapCircle(groundCheck.position, checkRadious, whatIsGround);
        moveInput= Input.GetAxis("Horizontal");
        if(isDrugged){
            rb.velocity= new Vector2(moveInput*-speed, rb.velocity.y);   
        }else{
            rb.velocity= new Vector2(moveInput*speed, rb.velocity.y);
        }

        if(moveInput == 0){
            anim.SetBool("isRunning", false);
            //FindObjectOfType<AudioManager>().Pause("LongWalk");
        }else {
            anim.SetBool("isRunning", true);
            //FindObjectOfType<AudioManager>().Play("LongWalk");
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
    void FlipDirection(){
        direction*=-1;
        Vector3 scale= transform.localScale;
        scale.x=originalXScale*direction;
        transform.localScale=scale;
    }
    void Update() {
        if(isGrounded){
            extraJumps= extraJumpsVal;
            anim.SetBool("isJumping",false);
        }else{
            anim.SetBool("isJumping", true);
        }

        if(Input.GetKeyDown(KeyCode.Space) && extraJumps>0 && isGrounded){
            anim.SetTrigger("TakeOff");
            rb.velocity= Vector2.up*jumpForce;
            extraJumps--;
            FindObjectOfType<AudioManager>().Play("GrassJump");
            
        }else if(Input.GetKeyDown(KeyCode.Space) && extraJumps > 0){
            rb.velocity= Vector2.up*jumpForce;
            extraJumps--;
            FindObjectOfType<AudioManager>().Play("Jump");
        }


        //Arrow Switching
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            arrowSwitching(ArrowType.Normal);
        }

        if(Input.GetKeyDown(KeyCode.Alpha2)){
            arrowSwitching(ArrowType.Fire);
        }
        
        if(Input.GetKeyDown(KeyCode.Alpha3)){
            arrowSwitching(ArrowType.Heavy);
        }

        if(Input.GetMouseButtonDown(0)){ 
            shootArrow(currentArrowType);   
        }
        if(Input.GetKeyDown(KeyCode.E)&&isTrigGrapplingUpgrade){
            hasGrapplingUpgrade=true;
            gameMaster.hasGrapplingUpgrade=true;
            setText("Try hanging from that rock", 5);
            head.sprite=skillHead;
            Destroy(triggeredGrapplingUpgrade);
            FindObjectOfType<AudioManager>().Play("PickUp");
            FindObjectOfType<AudioManager>().Play("Rope");
        }
        if(Input.GetKeyDown(KeyCode.E)&&isTrigFireUpgrade){
            hasFireArrow=true;
            gameMaster.hasFireArrow=true;
            setText("Maybe you could burn some plants", 5);
            arrowsPanel.unlockGUIFireArrow();
            Destroy(triggeredFireUpgrade);
            FindObjectOfType<AudioManager>().Play("PickUp");
            FindObjectOfType<AudioManager>().Play("FireArrow");
        }
        if(Input.GetKeyDown(KeyCode.E)&&isTrigHeavyUpgrade){
            hasHeavyArrow=true;
            gameMaster.hasHeavyArrow=true;
            setText("Try blowing up some rocks", 5);
            arrowsPanel.unlockGUIHeavyArrow();
            Destroy(triggeredHeavyUpgrade);
            FindObjectOfType<AudioManager>().Play("PickUp");
            FindObjectOfType<AudioManager>().Play("HeavyArrow");
        }
    }
    


    //Upgrade Picking
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("GrapplingUpgrade")){
            setText("Press E to collect item",2);
            isTrigGrapplingUpgrade=true;
            triggeredGrapplingUpgrade=other.gameObject;
        }
        if(other.gameObject.CompareTag("FireUpgrade")){
            setText("Press E to collect item",2);
            isTrigFireUpgrade=true;
            triggeredFireUpgrade=other.gameObject;
        }
        if(other.gameObject.CompareTag("HeavyUpgrade")){
            setText("Press E to collect item",2);
            isTrigHeavyUpgrade=true;
            triggeredHeavyUpgrade=other.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.CompareTag("GrapplingUpgrade")){
            isTrigGrapplingUpgrade=false;
        }
        if(other.gameObject.CompareTag("FireUpgrade")){
            isTrigFireUpgrade=false;
        }
        if(other.gameObject.CompareTag("HeavyUpgrade")){
            isTrigHeavyUpgrade=false;
        }
    }


    //Shooting Stuff

    void arrowSwitching(ArrowType arrowType){
        switch(arrowType){
            case ArrowType.Normal:
                currentArrowType=ArrowType.Normal;
                arrowsPanel.activeGUINormalArrow();
                quiverImage.sprite=normalQuiver;
                setText("Normal Arrow",1);
                break;
            case ArrowType.Fire:
                if(hasFireArrow){
                    arrowsPanel.activeGUIFireArrow();
                    currentArrowType=ArrowType.Fire;
                    quiverImage.sprite=fireQuiver;
                    setText("Fire Arrow",1);
                }else{
                    setText("You haven't found this weapon yet.",3);
                    FindObjectOfType<AudioManager>().Play("Denied");
                }
                break;
            case ArrowType.Heavy:
                if(hasHeavyArrow){
                    arrowsPanel.activeGUIHeavyArrow();
                    currentArrowType=ArrowType.Heavy;
                    quiverImage.sprite=heavyQuiver;
                    setText("Heavy Arrow",1);
                }else{
                    setText("You haven't found this weapon yet.",3);
                    FindObjectOfType<AudioManager>().Play("Denied");
                }
                break;
            
        }
    }

    void shootArrow(ArrowType currentArrowType){
        anim.SetTrigger("isAttacking");
        switch(currentArrowType){
            case ArrowType.Normal:
                GameObject arrow= Instantiate(arrowPrefab) as GameObject;
                arrow.transform.position = bowAim.position;
                FindObjectOfType<AudioManager>().Play("NormalArrow");
                if(isFlipped){
                    Vector3 scale= arrow.transform.localScale;
                    scale.x= -1*direction;
                    arrow.transform.localScale=scale;
                }
                Destroy(arrow,3);
                break;

            case ArrowType.Fire:
                GameObject fireArrow= Instantiate(fireArrowPrefab) as GameObject;
                fireArrow.transform.position = bowAim.position;
                FindObjectOfType<AudioManager>().Play("FireArrow");
                if(isFlipped){
                    Vector3 scale= fireArrow.transform.localScale;
                    scale.x= -1*direction;
                    fireArrow.transform.localScale=scale;
                }
                Destroy(fireArrow,3);
                break;

            case ArrowType.Heavy:
                GameObject heavyArrow= Instantiate(heavyArrowPrefab) as GameObject;
                heavyArrow.transform.position = bowAim.position;
                FindObjectOfType<AudioManager>().Play("HeavyArrow");
                if(isFlipped){
                    Vector3 scale= heavyArrow.transform.localScale;
                    scale.x= -1*direction;
                    heavyArrow.transform.localScale=scale;
                }
                Destroy(heavyArrow,3);
                break;
        }
    }

    //GUI Text Management
    private void setText(string text, int seconds){
        textHelper.setText(text);
        StartCoroutine("waitForSecondsText",seconds);
    }
    IEnumerator waitForSecondsText(int seconds){
        yield return new WaitForSeconds(seconds);
        textHelper.deleteText();
    }



    //Damage Taking
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Damage")){
            ouch(10);
        }
        if(other.gameObject.CompareTag("Enemy")){
            ouch(20);
        }
        if(other.gameObject.CompareTag("Spines")){
            ouch(50);
        }
        if(other.gameObject.CompareTag("Instakill")){
            kill();
        }
        if(other.gameObject.CompareTag("End")){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        }
    }
    void kill(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ouch(int damage){
        health-=damage;
        healthBar.SetHealth(health,maxHealth);
        FindObjectOfType<AudioManager>().Play("Ouch");
    }





    //Status Effects
    public void drug(){
        isDrugged=true;
        StartCoroutine("tripLength",10);
        if(hasGrapplingUpgrade){
            head.sprite=druggedSkillhead;
        }else{
            head.sprite=druggedNormalHead;
        }
    }

    IEnumerator tripLength(int seconds){
        yield return new WaitForSeconds(seconds);
        isDrugged=false;
        if(hasGrapplingUpgrade){
            head.sprite=skillHead;
        }else{
            head.sprite=normalHead;
        }
    }

}
