using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeDistance : MonoBehaviour
{
    // Start is called before the first frame update
    public float distance;
    public Terrain terrain;
    void Start()
    {
        terrain.treeDistance = distance;
    }
}
