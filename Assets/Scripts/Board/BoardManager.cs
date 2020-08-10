using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { get; private set; }

    public enum CELL_TYPE { 
        EMPTY,WALL
    }

    public GameObject cat;
    public GameObject mouse;

    public CELL_TYPE[][] board { get; private set; } //board as a 2D array to reference static objects
    public CELL_TYPE[] flatBoard { 
        get {
            int xSize = board.Length;
            int ySize = board[0].Length;
            CELL_TYPE[] res = new CELL_TYPE[xSize*ySize];
            for (int i = 0; i < xSize; ++i)
                for (int j = 0; j < ySize; ++j)
                    res[i * ySize + j] = board[i][j];
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
        board = new CELL_TYPE[BoardConfiguration.Instance.BoardSize.x][];
        for (int i = 0; i < BoardConfiguration.Instance.BoardSize.x; i++)
        {
            board[i] = new CELL_TYPE[BoardConfiguration.Instance.BoardSize.y];
            for (int j = 0; j < BoardConfiguration.Instance.BoardSize.y; j++)
                board[i][j] = CELL_TYPE.EMPTY;
        }
        //TODO: parse game hierarchy to fill the board 
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
        return positionsContainsObject(cat.transform.position, gridPositions);
    }
    public bool positionsContainsMouse(Vector2[] gridPositions)
    {
        return positionsContainsObject(mouse.transform.position, gridPositions);
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

}
