using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DeploymentStates
{
    add = 1,
    select = 2,
    move = 3,
    delete = 4,
    //reset is used if the tower is currently being moved and you wish to abort the move
    reset = 5
}

