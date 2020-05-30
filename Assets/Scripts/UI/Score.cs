using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Score : MonoBehaviour
{
    public TextMeshProUGUI textScore;

    public void setScore(int scoreValue){
        textScore.text=scoreValue.ToString();
    }


}
