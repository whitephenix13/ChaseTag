using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using static BoardManager;

public class MouseAIBrain : Brain
{
    private float fearStrength;
    private float fearArea;

    private float updateEvery = 0.1f;//0.1
    private float lastUpdate = float.MinValue;

    private List<CellType> travelCostKeys;
    private List<float> travelCostValues;

    private float sameCellTypePenalty;

    private List<Vector3> currentPath;
    private int currentPathIndex = 0;

    private MouseAIBrainMovement aiBrainMovement;

    [DllImport("ChaseTagAI", EntryPoint = "MouseAIAction")] //Use this to specify the name of the dll to import as well as the name of the function
    public static extern void MouseAIAction(CellType[] board, int[] boardSize,float cellSize, float[] cellOffset, float[] mousePos, float[] catPos, int actionsSize, float[] outActions);

    [DllImport("ChaseTagAI", EntryPoint = "FindPath")] //Use this to specify the name of the dll to import as well as the name of the function
    public static extern void FindPath(CellType[] board, int[] boardSize, float[] mousePos, float[] catPos, float fearStrength
    , float fearArea, float sameCellTypePenalty, CellType[] travelCostKey, float[] travelCostValue, int travelCostSize, int[] outResultPath, ref int outResultPathSize);
    public MouseAIBrain() {
        aiBrainMovement = new MouseAIBrainMovement();
    }

    public float GetLastUpdate()   {
        return lastUpdate;
    }
    public List<Vector3> GetCurrentPath() {
        return currentPath;
    }

    public void poolParameters(float fearStrength, float fearArea, List<CellType> travelCostKeys, List<float> travelCostValues, float sameCellTypePenalty) {
        this.fearStrength = fearStrength;
        this.fearArea = fearArea;
        this.travelCostKeys = travelCostKeys;
        this.travelCostValues = travelCostValues;
        this.sameCellTypePenalty = sameCellTypePenalty;
    }

    public override Actions brainUpdate(bool isPlayer1)
    {
        if (travelCostKeys.Count == 0)
        {
            //System is warming up OR travel cost is not set. Either way do nothing
            Debug.LogWarning("Travel cost not set");
            return new Actions(new float[Actions.actionTableLength]);
        }
        if ((Time.time - lastUpdate) > updateEvery)
        {
            lastUpdate = Time.time;
            int actionSize = Actions.actionTableLength;
            int[] boardSize = { BoardConfiguration.Instance.BoardSize.x, BoardConfiguration.Instance.BoardSize.z };

            //TODO: snap positions to center of cell
            Vector3 mousePosGridIndices = BoardManager.ConvertPositionToGridIndex(GameMainManager.Instance.mouse.transform.position);
            Vector3 catPosGridIndices = BoardManager.ConvertPositionToGridIndex(GameMainManager.Instance.cat.transform.position);

            //The dll function requires the first index to be the line index and the second one to be the column index
            float[] mousePos = { mousePosGridIndices.x, mousePosGridIndices.z };
            float[] catPos = { catPosGridIndices.x, catPosGridIndices.z };

            float[] actions = new float[actionSize];

            int outResultPathSize = 256;
            int[] outResultPath = new int[outResultPathSize];

            if (travelCostKeys.Count != travelCostValues.Count)
                Debug.LogError("Travel cost keys and values have different size");
           FindPath(BoardManager.Instance.flatBoard, boardSize, mousePos, catPos, fearStrength, fearArea, sameCellTypePenalty, travelCostKeys.ToArray(), travelCostValues.ToArray(), travelCostKeys.Count, outResultPath, ref outResultPathSize);

            currentPath = new List<Vector3>();
            currentPathIndex = -1;
            //TODO: for now, reverse the result path as it starts by the end point
            for (int i = (outResultPathSize / 2)-1; i >=0 ; --i) {
                Vector3 resultPathWorldPos = BoardManager.ConvertGridIndexToPosition(new Vector3(outResultPath[2 * i],0, outResultPath[2 * i + 1]));
                currentPath.Add(resultPathWorldPos);
            }
            return aiBrainMovement.DecideAction(currentPath, ref currentPathIndex);
        }
        else {
            return aiBrainMovement.DecideAction(currentPath, ref currentPathIndex);
        }
    }

   



}
