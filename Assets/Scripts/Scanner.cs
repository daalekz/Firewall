using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : Tower
{

    public Scanner(Vector3 position) : base(position, TowerType.Scanner)
    {
        Damage = 10;
        FireRate = 0.33f;
        Range = 3;
        this.TowerObj.name = "Scanner Tower";

    }

}
