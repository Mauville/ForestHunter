using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayGame(){

        if(FindObjectOfType<GameMaster>()==null){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);    
        }else{
            SceneManager.LoadScene(2);
        }
    }
    public void PlayGameplay(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    public void FirstScene(){
        SceneManager.LoadScene(0);
    }
    public void QuitGame(){
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
