using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Array of actions to execute each represented by an int. The array is expected to be of size 5:
/// 0 -> horizontal speed
/// 1 -> vertical speed
/// 2 -> special action 1
/// 3 -> special action 2
/// 4 -> special action 3
/// </summary>
public struct Actions
{
    public const int actionTableLength = 5;
    public float[] table { get; private set; }
    public Actions(float[] table)
    {
        Debug.Assert(table.Length == actionTableLength);
        this.table = table;
    }
    public float getHorizontalAxis() { return Mathf.Clamp(table[0], -1, 1); }
    public float getVerticalAxis() { return Mathf.Clamp(table[1], -1, 1); }
    public bool isSlide() { return table[2] != 0; }
    public bool isJump() { return table[3] != 0; }
    public bool isDash() { return table[4] != 0; }
}
