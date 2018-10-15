using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Defender : Tower
{

    public Defender(Vector3 position) : base (position, TowerType.Defender)
        {
        FireRate = 0.5f;
        Range = 6;
        Damage = 5;
        this.TowerObj.name = "Defender Tower";
        }


}
