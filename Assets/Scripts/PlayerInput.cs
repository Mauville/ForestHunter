using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script shoul run before anything else
[DefaultExecutionOrder(-100)]
public class PlayerInput : MonoBehaviour{
    [HideInInspector] public float horizontal;		
	[HideInInspector] public bool jumpHeld;			
	[HideInInspector] public bool jumpPressed;		
	[HideInInspector] public bool crouchHeld;		
	[HideInInspector] public bool crouchPressed;	

    bool readyToClear;

    private void Update() {
         ClearInput();

        //If we die, we shouldn't keep running the update
         //if(GameManager.IsGameOver())
           // return;
        
        Movement();
        
        //Always make this var between -1 & 1
        horizontal= Mathf.Clamp(horizontal, -1f, 1f);
    }

    //Slower refresh than update, avoid trouble with movement
    private void FixedUpdate() {
        readyToClear=true;
    }
	
    void ClearInput(){
        if(readyToClear){
            horizontal		= 0f;
            jumpPressed		= false;
            jumpHeld		= false;
            crouchPressed	= false;
            crouchHeld		= false;

            readyToClear	= false;
        }
    }
    
    
    //Character's movement
    void Movement(){

        horizontal += Input.GetAxis("Horizontal");

        //This is for us to know if we have pressed it even if we have already jumped
        jumpPressed	= jumpPressed || Input.GetButtonDown("Jump");
		jumpHeld = jumpHeld || Input.GetButton("Jump");

        crouchPressed	= crouchPressed || Input.GetButtonDown("Crouch");
		crouchHeld		= crouchHeld || Input.GetButton("Crouch");
    }
}
