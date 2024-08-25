using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerExplosionBehaviour : MonoBehaviour
{
    void Start()
    {
        // i have some doubts about this being good solution
        Destroy(gameObject, 0.62f);
    }
}
