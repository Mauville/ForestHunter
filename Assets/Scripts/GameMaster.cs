using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    private static GameMaster instance;
    
    //Checkpoint system
    [HideInInspector]public Vector2 lastCheckPointPos;
    //Hunter Save
    [HideInInspector] public bool hasGrapplingUpgrade=false;
    [HideInInspector] public bool hasFireArrow=false;
    [HideInInspector] public bool hasHeavyArrow=false;
    private bool algo;
    void Awake() {
        if(instance==null){
            instance=this;
            DontDestroyOnLoad(instance);
        }else{
            Destroy(gameObject);
        }
    }
}
