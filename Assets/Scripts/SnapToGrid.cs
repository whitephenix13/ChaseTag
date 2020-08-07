using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToGrid : MonoBehaviour
{
    public GameObject target;
 
    // Update is called once per frame
    void Update()
    {
        Vector2 xzGrid = BoardManager.GridPosition(target.transform.position);
        transform.position = new Vector3(xzGrid.x, transform.position.y, xzGrid.y);
    }
}
