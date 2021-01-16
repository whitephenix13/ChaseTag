using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBrain : Brain
{
    private bool isSlidePressed = false;
    public PlayerBrain()
    {
    }
    public override Actions brainUpdate(bool isPlayer1)
    {
        string playerPostFix = isPlayer1 ? "P1" : "P2";
        if (Input.GetButtonDown("Slide" + playerPostFix))
            isSlidePressed = true;
        if (Input.GetButtonUp("Slide" + playerPostFix))
            isSlidePressed = false;
        float[] actionsTable = new float[Actions.actionTableLength];
        actionsTable[0] = Input.GetAxis("Horizontal" + playerPostFix);  //Input.GetKeyUp(KeyCode.C)
        actionsTable[1] = Input.GetAxis("Vertical" + playerPostFix);
        actionsTable[2] = isSlidePressed ? 1 : 0;
        actionsTable[3] = Input.GetButtonDown("Jump" + playerPostFix) ? 1 : 0;
        actionsTable[4] = Input.GetButtonDown("Dash" + playerPostFix) ? 1 : 0;

        return new Actions(actionsTable);
    }

}
