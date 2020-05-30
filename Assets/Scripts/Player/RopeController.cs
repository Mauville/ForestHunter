using UnityEngine;

public class RopeController : MonoBehaviour {
    
    [Header ("Scripts")]
    public Hunter pm;
    public SwingBlock swingBlock;

    [Header ("Other")]
    public LineRenderer lineRenderer;
    public GameObject ropeShooter;
    private DistanceJoint2D rope;
    private Animator anim;

    private bool swingBlockTriggered;

    private void Start() {
        anim= GetComponent<Animator>();
    }

    private void Update() {
        if(Input.GetMouseButtonDown(1) && swingBlockTriggered && pm.hasGrapplingUpgrade){
            Fire();
            anim.SetTrigger("isSwinging");
        }else if(Input.GetMouseButtonDown(1)){
            GameObject.DestroyImmediate(rope);
            lineRenderer.enabled= false;
            pm.enabled= true;
            pm.extraJumps=2;
        }
    }

    private void LateUpdate() {
        if(rope != null){
            lineRenderer.enabled = true;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, ropeShooter.transform.position);
            lineRenderer.SetPosition(1, rope.connectedAnchor);
        }else{
            lineRenderer.enabled=false;
        }
    }

    void Fire()
    {
        pm.enabled = false;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 position = ropeShooter.transform.position;
        Vector3 direction = mousePosition - position;

        RaycastHit2D hit = Physics2D.Raycast(position, direction, Mathf.Infinity);

        if (hit.collider != null)
        {
            DistanceJoint2D newRope = ropeShooter.AddComponent<DistanceJoint2D>();
            newRope.enableCollision = false;
            newRope.connectedAnchor = mousePosition;
            newRope.enableCollision = true;
            newRope.enabled = true;

            Destroy(rope);
            rope = newRope;
        }
        FindObjectOfType<AudioManager>().Play("Rope");
    }


    public void SwingBlockIsBeingTriggered(){
        swingBlockTriggered=true;
    }
    public void SwingBlockIsNotBeingTriggered(){
        swingBlockTriggered=false;
    }
}