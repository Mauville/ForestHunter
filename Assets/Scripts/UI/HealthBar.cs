using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Gradient gradient;
    public Animator animator;
    public Image border;
    public float evaluation;

    private void Start() {
        animator=GetComponent<Animator>();
    }
    public void SetMaxHealth(){
        border.color=gradient.Evaluate(1f);
    }
    public void SetHealth(int health, int maxHealth){
        evaluation=health/maxHealth;

        //Here we have 2 animation for critical health, we can play either of both
        if(health>40){
            animator.SetBool("CriticalHealth",false);
        }else{
            animator.SetBool("CriticalHealth",true);
        }
        //We can also accelerate the animation speed depending on the health:
        //animator.speed= 1+(1-evaluation); 
        border.color=gradient.Evaluate(evaluation);
    }
}
