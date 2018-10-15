using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Annihilator : Tower {
    public Annihilator(Vector3 position) : base(position, TowerType.Annihilator)
    {
        Damage = 30;
        Range = 0;
        FireRate = 0.11f;

    }

    public override void Render()
    {
        TowerObj.transform.position = Position;
        TowerObj.GetComponent<Renderer>().sortingOrder = 2;


        if (Selected)
        {
            TowerObj.GetComponent<Renderer>().material.color = Color.yellow;
        }
        else
        {
            TowerObj.GetComponent<Renderer>().material.color = Color.magenta;
        }
    }


    public override void Fire(List<GameObject> enemy_queue)
    {
        //local fields store the closest game object and the distance of the object from the turret
        GameObject select_unit = null;
        float closest_distance = 0;
        TargetEnd = true;
        GameObject[] navPoints = new GameObject[0];

        Shoot_Wait_Remaining -= Time.deltaTime;
        Display_Shot_Remaining -= Time.deltaTime;

        if (Display_Shot_Remaining <= 0)
        {
            AimLine.GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, 0, 0));
            AimLine.GetComponent<LineRenderer>().SetPosition(0, new Vector3(0, 0, 0));

            Display_Shot_Remaining = display_shot;
        }

        if (Shoot_Wait_Remaining <= 0f)
        {

            List<GameObject> Selected = new List<GameObject>();

            foreach (GameObject enemy in enemy_queue)
            {
                if (Vert)
                {
                    if (enemy.transform.position.x == Position.x)
                    {
                        Selected.Add(enemy);
                    }
                }
                else
                {
                    if (enemy.transform.position.y == Position.y)
                    {
                        Selected.Add(enemy);
                    }
                }
            }

            if (Selected.Count > 0)
            {
                foreach (GameObject enemy in Selected)
                {
                    enemy.GetComponent<AIController>().data.ApplyDamage(Convert.ToInt32(Damage));
                    if (enemy.GetComponent<AIController>().data.Health <= 0)
                    {
                        TowerTools.DestroyGameObj(enemy);

                        enemy_queue.Remove(enemy);
                    }
                }

                if (Vert)
                {
                    DrawLine(new Vector3(Position.x, -1000, -1), new Vector3(Position.x, 1000, -1), Color.green);
                }
                else
                {
                    DrawLine(new Vector3(-1000, Position.y, -1), new Vector3(1000, Position.y, -1), Color.green);
                }

                Shoot_Wait_Remaining = 1 / FireRate;

            }
        }
    }
}
