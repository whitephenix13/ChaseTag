using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class Brain  
{
    public abstract Actions brainUpdate(bool isPlayer1);
}
