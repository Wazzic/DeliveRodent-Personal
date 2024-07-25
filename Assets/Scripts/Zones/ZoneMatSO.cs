using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu]
public class ZoneMatSO : ScriptableObject
{
    public Material defaultMaterial;
    public List<ZoneMats> zoneMatsList;
    public Material defaultPickUpMat;
    public Material defaultDropOffMat;
    public Material defaultPlayerMat;
    public Material defaultHatMat;
}

[Serializable]
public class ZoneMats
{
    public Material material;
    public bool active;
}