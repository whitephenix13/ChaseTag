using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAIBrain : Brain
{
    public CatAIBrain() {
    }
    public override Actions brainUpdate(bool isPlayer1)
    {
        float[] actionsTable = new float[Actions.actionTableLength];
        actionsTable[0] = 0;
        actionsTable[1] = 0;
        actionsTable[2] = 0;
        actionsTable[3] = 0;
        actionsTable[4] = 0;

        return new Actions(actionsTable);
    }
}
