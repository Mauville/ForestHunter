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
    // Start is called before the first frame update
    void Start()
    {
    	foreach(var target in targets){
    		_targets.Add(target.position);
    	}
    	SetTarget(_targets[targetindex]);
        
    }

    // Update is called once per frame
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
    	}
    	else{
    		gameObject.transform.position=Vector3.Lerp(gameObject.transform.position,_target,(Time.deltaTime*Speed/distance));
    	}
    }
}