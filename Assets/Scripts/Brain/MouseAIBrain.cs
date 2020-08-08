using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class MouseAIBrain : Brain
{
    [DllImport("ChaseTagAI", EntryPoint = "Negate")] //Use this to specify the name of the dll to import as well as the name of the function
    public static extern bool Negate(bool val);

    public override Actions brainUpdate(float[] actionCooldowns, float[] lastActivationsTime)
    {
        float[] actionsTable = new float[Actions.actionTableLength];
        actionsTable[0] = Negate(false)?1:0;
        actionsTable[1] = 0;
        actionsTable[2] = 0;
        actionsTable[3] = 0;
        actionsTable[4] = 0;

        return new Actions(actionsTable);
    }
}
