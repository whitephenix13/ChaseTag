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
    public int boardSize = 10; //number pof cells in the board
    public Vector3 cellCenter = new Vector3(0.5f, 0f, 0.5f);//The center of any cell of the grid
}
