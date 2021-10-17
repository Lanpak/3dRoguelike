using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationBaker : MonoBehaviour
{
    public void BakeNavMesh(NavMeshSurface surf)
    { 
        surf.BuildNavMesh();
    }

}