using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerTools : MonoBehaviour {
    public static void DestroyGameObj(GameObject obj)
    {
        //due to some of the class being unable to inherit monobehaviour
        //destroy is moved over to an externally accessible static method
        Destroy(obj);
    }
}

