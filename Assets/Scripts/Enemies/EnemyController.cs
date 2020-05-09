using UnityEngine;

public class EnemyController : MonoBehaviour {

    //Constant movement var
    public float speed;
    public float distance;
    private bool movingRight=true;
    public Transform groundDetection;
    
    
    //Constant movement update
    void Update() {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        RaycastHit2D groundInfo= Physics2D.Raycast(groundDetection.position, Vector2.down, distance);
        if(groundInfo.collider==false){
            if(movingRight){
                transform.eulerAngles= new Vector3(0,-180,0);
                movingRight=false;
            }
            else{
                transform.eulerAngles= new Vector3(0,0,0);
                movingRight=true;
            }
        }
    }
}