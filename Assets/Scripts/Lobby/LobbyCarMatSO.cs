using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MatSelect
{
    public Material mat;
    public bool active;
}

[CreateAssetMenu]
public class LobbyCarMatSO : ScriptableObject
{
    
    public List<GameObject> characterPrefabs;
    public List<Material> characterIconMat;

    public List<GameObject> hatPrefabs;

    public List<MatSelect> carMatsLobby;
    public List<Material> carMatsInGame;
    public List<GameObject> carPrefabs;
}
