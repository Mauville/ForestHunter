using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("Pause Menu")]
    public GameObject pauseUI;
    public static bool gameIsPaused;
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(gameIsPaused){
                Resume();
            }else{
                Pause();
            }

        }
    }

    private void Start() {
        pauseUI=GameObject.FindGameObjectWithTag("Pause");
        Resume();
    }
    public void Resume(){
        pauseUI.SetActive(false);
        Time.timeScale=1f;
        gameIsPaused=false;
    }
    public void Pause(){
        pauseUI.SetActive(true);
        Time.timeScale=0f;
        gameIsPaused=true;
    }
    public void Quit(){
        Resume();
        SceneManager.LoadScene(0);
    }
}
