using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularMovement : MonoBehaviour
{
	public Transform[]targets;

	public int targetindex;

	public List<Vector3> _targets = new List<Vector3>();

	public Vector3 _target;

	public float Speed;

	private int direction=-1;
	private float originalXScale;
    
    void Start()
    {
    	foreach(var target in targets){
    		_targets.Add(target.position);
    	}
    	SetTarget(_targets[targetindex]);
        originalXScale=transform.localScale.x;
    }

    
    void Update()
    {
    	Move();
        
    }
    void SetTarget(Vector3 target){
    	_target=target;
    }
    void Move(){
    	float distance=Vector3.Distance(gameObject.transform.position,_target);
    	if(distance <=0){
    		targetindex++;
    		targetindex=targetindex%_targets.Count;
    		SetTarget(_targets[targetindex]);
			FlipDirection();
    	}
    	else{
    		gameObject.transform.position=Vector3.Lerp(gameObject.transform.position,_target,(Time.deltaTime*Speed/distance));
    	}
    }

	void FlipDirection(){
        direction*=-1;
        Vector3 scale= transform.localScale;
        scale.x=originalXScale*direction;
        transform.localScale=scale;
    }
}