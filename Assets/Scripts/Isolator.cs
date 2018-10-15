using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Isolator : Tower
{
    public Isolator(Vector3 position) : base(position, TowerType.Isolator)
    {
        TargetEnd = true;
        Damage = 30;
        Range = 0;
        FireRate = 0.11f;

        rendered_tower = TowerTools.Instantiate(deploy_instance.tower_isolator, Position, Quaternion.identity);
        Color = rendered_tower.GetComponent<SpriteRenderer>().color;

        rendered_tower.transform.position = Position;
        //rendered_tower.GetComponent<Renderer>().sortingOrder = 2;
        rendered_tower.transform.position = new Vector3
            (
                rendered_tower.transform.position.x,
                rendered_tower.transform.position.y,
                -2
            );

        tower_gun = TowerTools.Instantiate(deploy_instance.tower_isolator_turret);
        tower_gun.transform.position = Position;
        //rendered_tower.GetComponent<Renderer>().sortingOrder = 2;
        tower_gun.transform.position = new Vector3
            (
                tower_gun.transform.position.x,
                tower_gun.transform.position.y,
                -3
            );

        AimLine = new GameObject();
        AimLine.AddComponent<LineRenderer>();
        AimLine.name = "Tower Aim Line";
        this.TowerObj.name = "Isolator Tower";

        //aim_line.transform.parent = this.TowerObj.transform;
    }

    public override void Fire(List<GameObject> enemy_queue)
    {
        //local fields store the closest game object and the distance of the object from the turret
        GameObject select_unit = null;
        float closest_distance = 0;
        TargetEnd = true;
        List<GameObject[]> navPoints = null;
        int NavPathNum;

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
            //get nodeindex
            gc = GameController.instance;

            navPoints = gc.navPoints;
            int HighNavPoint = 0;
        

            //goes through all of the enemy currently present on the board
            foreach (GameObject enemy in enemy_queue)
            {
                if (enemy_queue[0] == enemy)
                {
                    AimLine.GetComponent<LineRenderer>().SetPosition(1, Position);
                    AimLine.GetComponent<LineRenderer>().SetPosition(0, Position);
                    select_unit = enemy;
                    NavPathNum = enemy.GetComponent<AIController>().AIPathNum;
                    HighNavPoint = enemy.GetComponent<AIController>().NavPointNum;
                    closest_distance = Vector3.Distance(enemy.transform.position, navPoints[NavPathNum][HighNavPoint + 1].transform.position);

                }
                else
                {
                    NavPathNum = enemy.GetComponent<AIController>().AIPathNum;

                    if (enemy.GetComponent<AIController>().NavPointNum > HighNavPoint)
                    {
                        select_unit = enemy;
                        NavPathNum = enemy.GetComponent<AIController>().AIPathNum;

                        HighNavPoint = enemy.GetComponent<AIController>().NavPointNum;
                        closest_distance = Vector3.Distance(enemy.transform.position, navPoints[NavPathNum][HighNavPoint + 1].transform.position);
                    }
                    else if (enemy.GetComponent<AIController>().NavPointNum == HighNavPoint && Vector3.Distance(enemy.transform.position, navPoints[NavPathNum][HighNavPoint + 1].transform.position) < closest_distance)
                    {
                        select_unit = enemy;
                        closest_distance = Vector3.Distance(enemy.transform.position, navPoints[NavPathNum][HighNavPoint + 1].transform.position);
                    }
                }
            }

            //to prevent any errors, the isolator can only shoot object once they pass the first NAVPOINT
            if (select_unit != null && gc.WaveController.WaveObjSpawned > 1 && HighNavPoint > 0)
            {

                Vector3 temp;
                temp = new Vector3(select_unit.transform.position.x, select_unit.transform.position.y, Position.z);

                TowerRotation = temp - Position;

                select_unit.GetComponent<AIController>().data.ApplyDamage(Convert.ToInt32(Damage));
                if (select_unit.GetComponent<AIController>().data.Health <= 0)
                {
                    TowerTools.DestroyGameObj(select_unit);
                    enemy_queue.Remove(select_unit);
                    closest_distance = 0;
                }

                DrawLine(Position, select_unit.transform.position, Color.green);
                Shoot_Wait_Remaining = 1 / FireRate;
            }
        }
    }

    public override void Render()
    {
        base.Render();

        float speed = 5;
        // The step size is equal to speed times frame time.
        Vector3 dir, current, target;

        if (TowerRotation != null)
        {
            //code taken and modified from: https://answers.unity.com/questions/650460/rotating-a-2d-sprite-to-face-a-target-on-a-single.html

            float angle = Mathf.Atan2(-TowerRotation.x, TowerRotation.y) * Mathf.Rad2Deg;

            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            tower_gun.transform.rotation = Quaternion.Slerp(tower_gun.transform.rotation, q, Time.deltaTime * speed);

        }
        else
        {
            tower_gun.transform.rotation = Quaternion.LookRotation(Position - Position, Vector3.up);
        }
    }
}