using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Annihilator : Tower {
    public List<GameObject> TargetedTowers;
    private bool select_vertical = false;
    private List<GameObject> SelectedTowers;

    public Annihilator(Vector3 position, bool selected_vertical) : base(position, TowerType.Annihilator)
    {
        Damage = 30;
        Range = 0;
        FireRate = 0.11f;

        select_vertical = selected_vertical;    

        AimLine = new GameObject();
        AimLine.AddComponent<LineRenderer>();
        AimLine.name = "Tower Aim Line";

        //aim_line.transform.parent = this.TowerObj.transform;
    }

    public override void Render()
    {
        if (rendered_tower == null)
        {
            rendered_tower = TowerTools.Instantiate(deploy_instance.tower_annihilator, Position, Quaternion.identity);
            Color = rendered_tower.GetComponent<SpriteRenderer>().color;

            rendered_tower.transform.position = Position;
            //rendered_tower.GetComponent<Renderer>().sortingOrder = 2;
            rendered_tower.transform.position = new Vector3
            (
                rendered_tower.transform.position.x,
                rendered_tower.transform.position.y,
                -2
            );
            this.TowerObj.name = "Annihilator Tower";
        }

        if (tower_gun == null) { 

            tower_gun = TowerTools.Instantiate(deploy_instance.tower_annihilator_turret);
            tower_gun.transform.position = Position;
            //rendered_tower.GetComponent<Renderer>().sortingOrder = 2;
            tower_gun.transform.position = new Vector3
            (
                tower_gun.transform.position.x,
                tower_gun.transform.position.y,
                -3
            );
        }

        TowerObj.transform.position = new Vector3(Position.x, Position.y, -2);
        
        if (Selected)
        {
            TowerObj.GetComponent<Renderer>().material.color = Color.yellow;
        }
        else
        {
            TowerObj.GetComponent<Renderer>().material.color = Color.magenta;
        }
    }

    public List<GameObject> TargetRow(List<GameObject> enemy_queue)
    {
        TargetedTowers = new List<GameObject>();
        
        foreach (GameObject enemy in enemy_queue)
        {
            if (select_vertical)
            {
                if (enemy.transform.position.x == Position.x)
                {
                    TargetedTowers.Add(enemy);
                }
            }
            else
            {
                if (enemy.transform.position.y == Position.y)
                {
                    TargetedTowers.Add(enemy);
                }
            }
        }

        return TargetedTowers;
    }

    public override bool Fire(List<GameObject> enemy_queue)
    {
        //local fields store the closest game object and the distance of the object from the turret
        SelectLast = true;
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
            SelectedTowers = TargetRow(enemy_queue);

            if (SelectedTowers.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }

    public override void Attack(List<GameObject> enemy_queue)
    {
        if (Fire(enemy_queue) && Active)
        {
            foreach (GameObject enemy in SelectedTowers)
            {
                enemy.GetComponent<AIController>().data.ApplyDamage(Convert.ToInt32(Damage));
                if (enemy.GetComponent<AIController>().data.Health <= 0)
                {
                    TowerTools.DestroyGameObj(enemy);

                    enemy_queue.Remove(enemy);
                }
            }

            if (select_vertical)
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
