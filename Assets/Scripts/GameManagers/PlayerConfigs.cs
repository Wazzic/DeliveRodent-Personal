using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[CreateAssetMenu]
public class PlayerConfigs : ScriptableObject
{
    public int numberOfPlayers;
    public int roundTime;
    public int activeZones;

    public List<Mesh> carModels;
    public List<Material> carMaterials;
    public List<GameObject> carPrefabs;
    public List<Mesh> characterModels;
    public List<Material> characterMaterials;
    public List<GameObject> characterPrefabs;
    public List<Material> characterIcon;

    public List<GameObject> characterHats;

    public List<InputDevice> controllers;
    public float hapticLevel;
}
