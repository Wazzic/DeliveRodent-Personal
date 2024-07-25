using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpringBehaviour : MonoBehaviour
{
    [SerializeField]
    [Range(0.01f, 100f)]
    protected float Damping = 26;
    [SerializeField]
    [Range(0, 500)]
    protected float Stiffness = 169f;
}
