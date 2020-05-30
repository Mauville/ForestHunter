using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ArrowsPanel : MonoBehaviour
{
    public Image[] slots;
    private bool unlockedFire=false;
    private bool unlockedHeavy=false;
    public void activeGUINormalArrow(){
        slots[0].color= new Color32(255,255,255,80);
        if(unlockedFire){
            slots[1].color= new Color32(255,255,255,0);
        }
        if(unlockedHeavy){
            slots[2].color= new Color32(255,255,255,0);
        }
    }
    public void activeGUIFireArrow(){
        slots[0].color= new Color32(255,255,255,0);
        if(unlockedFire){
            slots[1].color= new Color32(255,255,255,80);
        }
        if(unlockedHeavy){
            slots[2].color= new Color32(255,255,255,0);
        }
    }
    public void activeGUIHeavyArrow(){
        slots[0].color= new Color32(255,255,255,0);
        if(unlockedFire){
            slots[1].color= new Color32(255,255,255,0);
        }
        if(unlockedHeavy){
            slots[2].color= new Color32(255,255,255,80);
        }
    }

    public void unlockGUIFireArrow(){
        unlockedFire=true;
        slots[1].color= new Color32(0,255,0,80);
        StartCoroutine("WaitInGreen",1);
    }
    public void unlockGUIHeavyArrow(){
        unlockedHeavy=true;
        slots[2].color= new Color32(0,255,0,80);
        StartCoroutine("WaitInGreen",2);
    }
    IEnumerator WaitInGreen(int index){
        yield return new WaitForSeconds(2);
        slots[index].color=new Color32(255,255,255,0);
    }
}
