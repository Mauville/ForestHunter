using UnityEngine;

public class SwingBlock : MonoBehaviour {
    public bool cursorIsTrig;

    public RopeController ropeController;
    private void OnMouseEnter() {
        cursorIsTrig=true;
        ropeController.SwingBlockIsBeingTriggered();
    }
    private void OnMouseExit() {
        cursorIsTrig=false;
        ropeController.SwingBlockIsNotBeingTriggered();
    }

}