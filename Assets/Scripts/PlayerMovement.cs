using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
   
    //Check the environment
    public bool drawDebugRaycasts= true;

    [Header("Movement Properties")] //Only glancy loockin' header
    public float speed= 8f;
    public float crouchSpeedDivisor= 3f; //Crouch speed reduction factor
    public float coyoteDuration= 0.05f; //Time to jump even if not in platform. Meek Meek
    public float maxFallSpeed=-25f;

    [Header("Jump Properties")]
    public float jumpForce= 6.3f;
    public float crouchJumpBoost= 2.5f; //Jump higher if crouch
    public float hangingJumpForce=15f;
    public float jumpHoldForce=1.9f;
    public float jumpHoldDuration=0.1f; //higher if keep pressed

    [Header("Environment Check Properties")]
	public float footOffset = .4f;			
	public float eyeHeight = 0f;			
	public float reachOffset = .7f;			
	public float headClearance = .5f;		
	public float groundDistance = .2f;		
	public float grabDistance = .4f;		
	public LayerMask groundLayer;			

	[Header ("Status Flags")]
	public bool isOnGround;
	public bool isJumping;
	public bool isHanging;				
	public bool isCrouching;
	public bool isHeadBlocked;

    PlayerInput input;
    BoxCollider2D bodyCollider;
    Rigidbody2D rigidBody;		

    float jumpTime;
    float coyoteTime;
    float playerHeight;

    float originalXScale;
    int direction=1;

    Vector2 colliderStandSize;
    Vector2 colliderStandOffset;
    Vector2 colliderCrouchSize;
    Vector2 colliderCrouchOffset;

    const float smallAmount= 0.05f;

    private void Start() {
		input = GetComponent<PlayerInput>();
		rigidBody = GetComponent<Rigidbody2D>();
		bodyCollider = GetComponent<BoxCollider2D>();

		originalXScale = transform.localScale.x;
		playerHeight = bodyCollider.size.y-1.8f;

		colliderStandSize = bodyCollider.size;
		colliderStandOffset = bodyCollider.offset;

		colliderCrouchSize = new Vector2(bodyCollider.size.x, bodyCollider.size.y / 2f);
		colliderCrouchOffset = new Vector2(bodyCollider.offset.x, bodyCollider.offset.y / 2f);
    }

    private void FixedUpdate() {
		
        PhysicsCheck();
		GroundMovement();
		MidAirMovement();
    }

    void PhysicsCheck(){
        //We asume nothing is blocking our head
        isOnGround=false;
        isHeadBlocked= false;

        //Two feet, two ray.
        RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset, 0f), Vector2.down, groundDistance);
		RaycastHit2D rightCheck = Raycast(new Vector2(footOffset, 0f), Vector2.down, groundDistance);

        if(leftCheck||rightCheck){
            isOnGround=true;
        }

        RaycastHit2D headCheck= Raycast(new Vector2(0f, bodyCollider.size.y-1.5f    ), Vector2.up, headClearance);
        if(headCheck){
            isHeadBlocked=true;
        }

        Vector2 grabDir= new Vector2(direction, 0f);
        //Grab rays for wall grab
        RaycastHit2D blockedCheck = Raycast(new Vector2(footOffset * direction, playerHeight+1f), grabDir, grabDistance);
		RaycastHit2D ledgeCheck = Raycast(new Vector2(reachOffset * direction, playerHeight+1f), Vector2.down, grabDistance);
		RaycastHit2D wallCheck = Raycast(new Vector2(footOffset * direction, eyeHeight), grabDir, grabDistance);

        //Condiciones pa trepar el muro
        if(!isOnGround && !isHanging && rigidBody.velocity.y<0f && ledgeCheck && wallCheck && !blockedCheck){
            Vector3 pos= transform.position;
            pos.x+= (wallCheck.distance - smallAmount)* direction;
            pos.y-=ledgeCheck.distance;
            transform.position=pos;
            rigidBody.bodyType= RigidbodyType2D.Static;
            isHanging=true;
        }

    }

    void GroundMovement(){
        //Can't move while hanging
        if(isHanging)
            return;

        if(input.crouchHeld && !isCrouching && isOnGround){
            Crouch();
        }else if(!input.crouchHeld&&isCrouching){
            StandUp();
        }else if(!isOnGround && isCrouching){
            StandUp();
        }

        float xVelocity= speed* input.horizontal;
        if(xVelocity*direction<0f){ 
            FlipDirection();
        }
        
        if(isCrouching){
            xVelocity/=crouchSpeedDivisor;
        }
        rigidBody.velocity=new Vector2(xVelocity, rigidBody.velocity.y);

        if(isOnGround){
            coyoteTime=Time.time+coyoteDuration;
        }
    }

    void MidAirMovement(){
        if(isHanging){
            if(input.crouchPressed){
                isHanging=false;
                rigidBody.bodyType=RigidbodyType2D.Dynamic;
                return;
            }
            if(input.jumpPressed){
                isHanging=false;
                rigidBody.bodyType= RigidbodyType2D.Dynamic;
                rigidBody.AddForce(new Vector2(0f, hangingJumpForce),ForceMode2D.Impulse);
                return;
            }
        }
        
        //Don't jump if you have already jumped
        if(input.jumpPressed && !isJumping &&(isOnGround||coyoteTime>Time.time)){
            if(isCrouching&&!isHeadBlocked){
                StandUp();
                rigidBody.AddForce(new Vector2(0f, crouchJumpBoost), ForceMode2D.Impulse);
            }
            isOnGround=false;
            isJumping=true;

            jumpTime=Time.time+jumpHoldDuration;

            rigidBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        } else if(isJumping){
            if(input.jumpHeld){
                rigidBody.AddForce(new Vector2(0f, jumpHoldForce), ForceMode2D.Impulse);
            }
            if(jumpTime<= Time.time){
                isJumping=false;
            }
        }
        if(rigidBody.velocity.y<maxFallSpeed){
            rigidBody.velocity= new Vector2(rigidBody.velocity.x, maxFallSpeed);
        }
    }
    void FlipDirection(){
        direction*=-1;
        Vector3 scale= transform.localScale;
        scale.x=originalXScale*direction;
        transform.localScale=scale;
    }

    void Crouch(){
        isCrouching=true;

        bodyCollider.size=colliderCrouchSize;
        bodyCollider.offset=colliderCrouchOffset;

    }
    void StandUp(){
        if(isHeadBlocked)
            return;
        
        isCrouching=false;
        bodyCollider.size=colliderStandSize;
        bodyCollider.offset=colliderCrouchOffset;
    }

    //Raycasts improvement:
    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length){
		return Raycast(offset, rayDirection, length, groundLayer);
	}

	RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask mask){
		Vector2 pos = transform.position;
		RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, mask);
		if (drawDebugRaycasts){
			Color color = hit ? Color.red : Color.green;
			Debug.DrawRay(pos + offset, rayDirection * length, color);
		}
		return hit;
	}
}
