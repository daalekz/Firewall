using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Annihilator : Tower {
    public List<GameObject> TargetedTowers;
    private bool select_vertical = false;
    private List<GameObject> SelectedTargets;

    public Annihilator(Vector3 position, bool selected_vertical) : base(position, TowerType.Annihilator)
    {
        Damage = 30;
        Range = 0;
        FireRate = 0.11f;

        select_vertical = selected_vertical;    

        AimLine = new GameObject();
        AimLine.AddComponent<LineRenderer>();
        AimLine.name = "Tower Aim Line";
    }

    //updates tower data, and then displays it on the game screen
    public override void Render()
    {
        //initalizes the render_tower object if it doesn't exist yet!
        if (rendered_tower == null)
        {
            rendered_tower = TowerTools.Instantiate(deploy_instance.tower_annihilator, Position, Quaternion.identity);
            Color = rendered_tower.GetComponent<SpriteRenderer>().color;

            rendered_tower.transform.position = Position;
            rendered_tower.transform.position = new Vector3
            (
                rendered_tower.transform.position.x,
                rendered_tower.transform.position.y,
                -2
            );
            this.TowerObj.name = "Annihilator Tower";
        }

        //initalizes the tower_gun object if it doesn't exist yet!
        if (tower_gun == null) { 

            tower_gun = TowerTools.Instantiate(deploy_instance.tower_annihilator_turret);
            tower_gun.transform.position = Position;
            tower_gun.transform.position = new Vector3
            (
                tower_gun.transform.position.x,
                tower_gun.transform.position.y,
                -3
            );
        }

        //ensures that the tower is visible (hence changes z values to be seen on top of other objects)
        TowerObj.transform.position = new Vector3(Position.x, Position.y, -2);
        
        //changes the displayed object color, to yellow, if it is selected
        if (Selected)
        {
            TowerObj.GetComponent<Renderer>().material.color = Color.yellow;
        }
        else
        {
            TowerObj.GetComponent<Renderer>().material.color = Color.magenta;
        }
    }

    /*
    Summary:
    This is the target method for the Annihilator Tower Type
    Selected either all of the enemy for a given x or y value
    */
    public List<GameObject> TargetRow(List<GameObject> enemy_queue)
    {
        TargetedTowers = new List<GameObject>();
        
        foreach (GameObject enemy in enemy_queue)
        {
            //checks the orientation that the tower is allowed to shoot in
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

    //utilies shoot to check if there are any enemies in range
    //if there are enemy within the tower's range shoots them (and set timers, to display line and wait, if shot is taken)
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
            SelectedTargets = TargetRow(enemy_queue);

            if (SelectedTargets.Count > 0)
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

    //applies damage to the enemys and destroy them (and all their associated data) if necessary
    public override void Attack(List<GameObject> enemy_queue)
    {
        //checks if the object itself is active
        if (Fire(enemy_queue) && Active)
        {
            //the tower has returned true, that there are enemy that it can shoot, it loops through all of the enemys on the map
            foreach (GameObject enemy in SelectedTargets)
            {
                //applies damage to enemy
                enemy.GetComponent<AIController>().data.ApplyDamage(Convert.ToInt32(Damage));
                //destroys enemy if their health is below 0
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
