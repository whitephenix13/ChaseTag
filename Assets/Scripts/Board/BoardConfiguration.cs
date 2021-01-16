using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardConfiguration : MonoBehaviour
{
    public static BoardConfiguration Instance { get; private set; }

    public void Awake()
    {
        if (Instance == null)
            Instance = this;

    }

    public float cellSize = 1;// x,z size of the cells of the board

    public List<CellType> flatAiBoard;
    public List<Vector3> flatAiBoardIndices;

    [SerializeField]
    [Tooltip("First index is the number of lines, second index is the number of columns")]
    private Vector3Int _boardSize;
    public Vector3Int BoardSize {
        get {
            return _boardSize;
        }
        set {
            _boardSize = value;
        }
    }


    private Vector3Int _boardSizeCached;

    /// <summary>
    /// Offset value to use to convert from world coordinate to cell coordinate
    /// </summary>
    private Vector2 _cellOffset;
    public Vector2 cellOffset { 
        get {
            if (!_boardSizeCached.Equals(_boardSize))
            {
                _boardSizeCached = _boardSize;
                _cellOffset = new Vector2((_boardSize.x + 1) % 2 / 2.0f, (_boardSize.z + 1) % 2 / 2.0f);
            }
            return _cellOffset;
        } 
        private set {
            _cellOffset = value;
        } 
    }
}
