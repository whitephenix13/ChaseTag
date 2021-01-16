using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAiBoard : MonoBehaviour
{
    public Transform board;
    public BoardConfiguration boardConfiguration;
    void OnDrawGizmosSelected()
    {
        if (board != null && boardConfiguration != null)
        {
            //Ensure size is correct
            List<CellType> flatAiBoard = boardConfiguration.flatAiBoard;
            List<Vector3> flatAiBoardIndices = boardConfiguration.flatAiBoardIndices;

            int flatAiCountDiff = flatAiBoard.Count - flatAiBoardIndices.Count;
            if (flatAiCountDiff!= 0 ) {
                for (int i = 0; i < flatAiCountDiff; ++i) {
                    flatAiBoardIndices.Add(new Vector3());
                }
                for (int i = 0; i < -flatAiCountDiff; ++i)
                {
                    flatAiBoardIndices.RemoveAt(flatAiBoardIndices.Count-1);
                }
            }

            Vector3 gizmoSize = new Vector3(boardConfiguration.cellSize ,0.1f, boardConfiguration.cellSize);
            for (int i = 0; i < flatAiBoard.Count; ++i) {
                Vector3 cellPos = BoardManager.ConvertGridIndexToPosition(flatAiBoardIndices[i], boardConfiguration.cellSize,boardConfiguration.BoardSize);
                cellPos = new Vector3(cellPos.x, 0.1f, cellPos.z);
                switch (flatAiBoard[i]) {
                    case CellType.EMPTY:
                        break;
                    case CellType.WALL:
                        Gizmos.color = Color.red;
                        Gizmos.DrawCube(cellPos, gizmoSize);
                        break;
                    case CellType.BAR:
                        Gizmos.color = Color.blue;
                        Gizmos.DrawCube(cellPos, gizmoSize);
                        break;
                    case CellType.STEP:
                        Gizmos.color = Color.green;
                        Gizmos.DrawCube(cellPos, gizmoSize);
                        break;
                    case CellType.STEP_BAR:
                        Gizmos.color = Color.magenta;
                        Gizmos.DrawCube(cellPos, gizmoSize);
                        break;
                    default:
                        break;
                }
            }
        }
    }

  
}
