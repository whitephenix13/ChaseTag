using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { get; private set; }

    public enum CELL_TYPE { 
        EMPTY,WALL
    }

    public GameObject cat;
    public GameObject mouse;

    private CELL_TYPE[,] board; //board as a 2D array to reference static objects

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void Start()
    {
        int boardSize = BoardConfiguration.Instance.boardSize;
        board = new CELL_TYPE[boardSize, boardSize];
        for (int i = 0; i < boardSize; i++)
            for (int j = 0; j < boardSize; j++)
                board[i, j] = CELL_TYPE.EMPTY;
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
        Vector3 cellCenter = BoardConfiguration.Instance.cellCenter;
        float gridSize = BoardConfiguration.Instance.cellSize;
        float xSnap = cellCenter.x + (int)Mathf.Round((objPosition.x - cellCenter.x) / gridSize);
        float zSnap = cellCenter.z + (int)Mathf.Round((objPosition.z - cellCenter.z) / gridSize);
        return new Vector2(xSnap, zSnap);
    }

}
