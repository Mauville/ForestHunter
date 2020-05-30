using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextHelper : MonoBehaviour
{
    public TextMeshProUGUI helperText;
    public Animator animator;
    public void setText(string text){
        helperText.text=text;
        animator.SetTrigger("FadeIn");
    }

    public void deleteText(){
        animator.SetTrigger("FadeOut");
    }
}
