using UnityEngine;

public class SwingBlock : MonoBehaviour {
    public bool cursorIsTrig;
    private void OnMouseEnter() {
        cursorIsTrig=true;
    }
    private void OnMouseExit() {
        cursorIsTrig=false;
    }

}