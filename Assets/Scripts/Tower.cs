using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Summary: 
 * Class is used for the different elements that will be placed on the screen and fire at enemies
*/
public class Tower
{
    private Vector3 _position;
    private GameObject rendered_tower;
    private bool _selected;
    private float range;
    private float fire_rate;

    //two different constructors to allow for greater user flexibility
    public Tower(int x, int y)
    {
        rendered_tower = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        _position.x = x;
        _position.y = y;
        _position.z = 0;
        var shape_color = rendered_tower.GetComponent<Renderer>().material.color;
        shape_color.a = 0;
    }

    public Tower(Vector3 position)
    {
        rendered_tower = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        _position = position;
        var shape_color = rendered_tower.GetComponent<Renderer>().material.color;
        shape_color.a = 0;
    }

    //tell the program how the tower should be drawn
    public void Render()
    {
        rendered_tower.transform.position = _position;

        if (_selected)
        {
            rendered_tower.GetComponent<Renderer>().material.color = Color.yellow;
        }
        else
        {
            rendered_tower.GetComponent<Renderer>().material.color = Color.cyan;
        }
    }

    //returns if the given tower has been selected
    public bool Selected
    {
        get
        {
            return _selected;
        }

        set
        {
            _selected = value;
        }
    }

    //returns the object component on the class
    public GameObject TowerObj
    {
        get
        {
            return rendered_tower;
        }

        set
        {
            rendered_tower = value;
        }
    }

    //return the position of the object
    public Vector3 Position
    {
        get
        {
            return _position;
        }

        //if you want to move tower position!
        set
        {
            _position = value;
        }
    }
}
