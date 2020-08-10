using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using static BoardManager;

public class MouseAIBrain : Brain
{
    [DllImport("ChaseTagAI", EntryPoint = "MouseAIAction")] //Use this to specify the name of the dll to import as well as the name of the function
    public static extern void MouseAIAction(CELL_TYPE[] board, int[] boardSize,float cellSize, float[] cellOffset, float[] mousePos, float[] catPos, int actionsSize, float[] outActions);

    public override Actions brainUpdate(float[] actionCooldowns, float[] lastActivationsTime)
    {
        int actionSize = Brain.Actions.actionTableLength;
        int[] boardSize = { BoardConfiguration.Instance.BoardSize.x, BoardConfiguration.Instance.BoardSize.y };
        float[] cellOffset = { BoardConfiguration.Instance.cellOffset.x, BoardConfiguration.Instance.cellOffset.y };
        float[] mousePos= { BoardManager.Instance.mouse.transform.position.x,BoardManager.Instance.mouse.transform.position.z };
        float[] catPos= { BoardManager.Instance.cat.transform.position.x, BoardManager.Instance.cat.transform.position.z};

        float[] actions = new float[actionSize];

        MouseAIAction(BoardManager.Instance.flatBoard, boardSize, BoardConfiguration.Instance.cellSize, cellOffset, mousePos, catPos, actionSize, actions) ;
        return new Actions(actions);
    }

}
