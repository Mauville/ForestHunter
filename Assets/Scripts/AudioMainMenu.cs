using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioMainMenu : MonoBehaviour
{
    private static AudioMainMenu instance;
    void Awake() {
        if(instance!=null){
            Destroy(gameObject);
        }else{
            instance=this;
            DontDestroyOnLoad(instance);
        }
    }

    private void Update() {
        //This will play during the first two scenes, when we reach third scene (index 2) this will be destroyed.
        if(SceneManager.GetActiveScene().buildIndex==2){
            Destroy(gameObject);
        }
    }
}
