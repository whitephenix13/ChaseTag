using UnityEngine;

public class BoardManager : MonoBehaviour
{

    public static BoardManager Instance { get; private set; }

    //board is a 2d table where the first index is the line index (z position) and the second index is the column index (x position)
    //this is done to ensure that when iterating over the table, we iterate over the lines (as flattening the array is done by concatenating each lines)
    public CellType[][] board { get; private set; } //board as a 2D array to reference static objects
    //Flatten board is expected to be the concatenation of each lines of the board 
    public CellType[] flatBoard { 
        get {
            int lineSize = board.Length;
            int columnSize = board[0].Length;
            CellType[] res = new CellType[lineSize * columnSize];
            for (int i = 0; i < lineSize; ++i)
                for (int j = 0; j < columnSize; ++j)
                    res[i * columnSize + j] = board[i][j];
            return res;
        }
        private set { 
        }
    }
    public void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void Start()
    {
        board = new CellType[BoardConfiguration.Instance.BoardSize.x][];
        for (int i = 0; i < BoardConfiguration.Instance.BoardSize.x; i++)
        {
            board[i] = new CellType[BoardConfiguration.Instance.BoardSize.z];
            for (int j = 0; j < BoardConfiguration.Instance.BoardSize.z; j++)
                board[i][j] = CellType.EMPTY;
        }

        for (int i = 0; i < BoardConfiguration.Instance.flatAiBoardIndices.Count; ++i) {
            Vector3Int boardIndices = Vector3Int.FloorToInt(BoardConfiguration.Instance.flatAiBoardIndices[i]);
            CellType boardCellType = BoardConfiguration.Instance.flatAiBoard[i];
            board[boardIndices.x][boardIndices.z] = boardCellType;
        }
    }

    /// <summary>
    /// Check whether the object is on any of the grid positions given as parameter
    /// </summary>
    /// <param name="objectPosition">Position of the object</param>
    /// <param name="gridPositions">Positions of the cell on the grid</param>
    /// <returns>True on cell is equal to object positon</returns>
    public bool positionsContainsObject(Vector3 objectPosition,Vector2[] gridPositions) {
        bool objectFound = false;
        Vector2 objectGridPosition = BoardManager.GridPosition(objectPosition);
        for (int i = 0; i < gridPositions.Length; ++i)
            objectFound = objectFound || objectGridPosition.Equals(gridPositions[i]);
        return objectFound;
    }

    public bool positionsContainsCat(Vector2[] gridPositions)
    {
        return positionsContainsObject(GameMainManager.Instance.cat.transform.position, gridPositions);
    }
    public bool positionsContainsMouse(Vector2[] gridPositions)
    {
        return positionsContainsObject(GameMainManager.Instance.mouse.transform.position, gridPositions);
    }

    /// <summary>
    /// Return x and z position of the middle of the cell corresponding to the obj position
    /// </summary>
    /// <param name="obj">Gameobject from which the grid position should be computed</param>
    /// <returns>x and z position of the center of the cell</returns>
    public static Vector2 GridPosition(Vector3 objPosition) {
        float cellSize = BoardConfiguration.Instance.cellSize;
        float xSnap = BoardConfiguration.Instance.cellOffset.x + (int)Mathf.Round((objPosition.x - BoardConfiguration.Instance.cellOffset.x) / cellSize) * cellSize;
        float zSnap = BoardConfiguration.Instance.cellOffset.y + (int)Mathf.Round((objPosition.z - BoardConfiguration.Instance.cellOffset.y) / cellSize) * cellSize;
        return new Vector2(xSnap, zSnap);
    }

    /// <summary>
    /// Converts the world position to grid index position (value between 0 and board size for x and z axis), disregard y axis.
    /// </summary>
    /// <param name="worldPos">position in unity coordinate system</param>
    /// <returns></returns>
    public static Vector3Int ConvertPositionToGridIndex(Vector3 worldPos)
    {
        //Assume the following values for exemple: 
        //WorldPos = 4.5 ; boardSize = 6, cellSize = 3
        //Values are between -(boardSize-1)*cellSize /2 and (boardSize-1)*cellSize /2 -> -7.5 and 7.5 (so that total length is boardsize and step between cells is cellSize)
        //By doing  +(boardSize-1)*cellSize /2 and /cellSize we get values between 0 and boardSize-1
        //In that case, +7.5 and /3 gives 0 1 2 3 4 5
        //Also need to invert z axis as it points towards up (with want top left to be 0,0 so a z axis pointing down)
        worldPos.z *= -1; ;
        float cellSize = BoardConfiguration.Instance.cellSize;
        Vector3Int boardSize = BoardConfiguration.Instance.BoardSize;
        Vector3 minPosValue = (boardSize - Vector3.one) * cellSize / 2;
        Vector3 gridIndexFloat = (worldPos + minPosValue) / cellSize;
        return new Vector3Int(Mathf.RoundToInt(gridIndexFloat.z), Mathf.RoundToInt(gridIndexFloat.y), Mathf.RoundToInt(gridIndexFloat.x));
    }

    /// <summary>
    /// Convert a grid index vector (between 0 and boardSize -1 ) to a world position. 
    /// </summary>
    /// <param name="gridIndex">Value between 0 and board size for x and z axis), disregard y axis</param>
    /// <returns>World position (using unity coordinate system)</returns>
    public static Vector3 ConvertGridIndexToPosition(Vector3 gridIndex)
    {
        //Indices are betwwen 0 and boardSize-1
        //We want world positions between -(boardSize-1)*cellSize /2 and (boardSize-1)*cellSize /2 (so that total length is boardsize and step between cells is cellSize)
        //We can get this result by multiplying by cellSize (to make the cells "cellSize" appart from one another) and offset by (boardSize - 1) * cellSize / 2 to center the 0 value.
        //The coordinate system for the indices are (-z,y,x) so we have to invert z axis and swap x and z
        float cellSize = BoardConfiguration.Instance.cellSize;
        Vector3Int boardSize = BoardConfiguration.Instance.BoardSize;
        return ConvertGridIndexToPosition(gridIndex, cellSize, boardSize);
    }
    public static Vector3 ConvertGridIndexToPosition(Vector3 gridIndex, float cellSize, Vector3Int boardSize)
    {
        //Indices are betwwen 0 and boardSize-1
        //We want world positions between -(boardSize-1)*cellSize /2 and (boardSize-1)*cellSize /2 (so that total length is boardsize and step between cells is cellSize)
        //We can get this result by multiplying by cellSize (to make the cells "cellSize" appart from one another) and offset by (boardSize - 1) * cellSize / 2 to center the 0 value.
        //The coordinate system for the indices are (-z,y,x) so we have to invert z axis and swap x and z
        Vector3 minPosValue = (boardSize - Vector3.one) * cellSize / 2;
        Vector3 resultInGridCoordinateSystem = (gridIndex * cellSize) - minPosValue;
        return new Vector3(resultInGridCoordinateSystem.z, resultInGridCoordinateSystem.y, -resultInGridCoordinateSystem.x);
    }
}
