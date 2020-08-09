using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using static BoardManager;

public class MouseAIBrain : Brain
{
   [DllImport("ChaseTagAI", EntryPoint = "MouseAIAction")] //Use this to specify the name of the dll to import as well as the name of the function
    public static extern void MouseAIAction(CELL_TYPE[] board, int xBoardSize, int yBoardSize, float xMousePos, float zMousePos, float xCatPos, float zCatPos, int actionsSize, float[] outActions);

    public override Actions brainUpdate(float[] actionCooldowns, float[] lastActivationsTime)
    {
        int boardSize = BoardConfiguration.Instance.boardSize;
        int actionSize = Brain.Actions.actionTableLength;
        float xMousePos= BoardManager.Instance.mouse.transform.position.x;
        float zMousePos= BoardManager.Instance.mouse.transform.position.z;
        float xCatPos= BoardManager.Instance.cat.transform.position.x;
        float zCatPos= BoardManager.Instance.cat.transform.position.z;
        float[] actions = new float[actionSize];

        MouseAIAction(BoardManager.Instance.flatBoard, boardSize, boardSize, xMousePos, zMousePos, xCatPos, zCatPos, actionSize, actions);
        return new Actions(actions);
    }

}
