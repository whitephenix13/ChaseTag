using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatPlayerBrain : Brain
{
    public override Actions brainUpdate(float[] actionCooldowns, float[] lastActivationsTime)
    {
        float[] actionsTable = new float[Actions.actionTableLength];
        actionsTable[0] = Input.GetAxis("Horizontal"); 
        actionsTable[1] = Input.GetAxis("Vertical");
        actionsTable[2] = Input.GetButtonDown("Fire1")?1:0;
        actionsTable[3] = 0;
        actionsTable[4] = 0;

        return new Actions(actionsTable) ;
    }

}
