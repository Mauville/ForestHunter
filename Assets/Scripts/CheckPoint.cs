using UnityEngine;

public class CheckPoint : MonoBehaviour {
    private GameMaster gameMaster;
    
    private void Start() {
        gameMaster= GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            gameMaster.lastCheckPointPos = transform.position;
            //FindObjectOfType<AudioManager>().Play("Rope");
        }
    }
}