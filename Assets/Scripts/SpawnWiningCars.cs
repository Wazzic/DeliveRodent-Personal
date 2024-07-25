using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class SpawnWiningCars : MonoBehaviour
{
    [SerializeField] PlayerConfigs playerConfigsSO;
    
    [SerializeField] public List<Transform> spawnPoints;

    [SerializeField] private Mesh playerModel1;
    [SerializeField] private Mesh playerModel2;
    [SerializeField] private Mesh playerModel3;
    [SerializeField] private Mesh playerModel4;

    [SerializeField] private GameObject tyres;

    [SerializeField] private Transform cam;

    [SerializeField] private ZoneMatSO zoneMatSO;


    void spawnCarMesh(Vector3 position, Quaternion rot, int index)
    { 
        //Create a gameobject to be spawned in this case the players selected car model
        GameObject go = new GameObject();
        //Create a mesh filter which stores the mesh data of the model to be rendered.
        go.AddComponent<MeshFilter>();
        //Create a renderer to render the obtained mesh
        MeshRenderer rend = go.AddComponent<MeshRenderer>();
        //Obtain the mesh of the players car
        Mesh carMesh = GameManager.instance.playerConfigs.carModels[index];
        //Obtain the material of the players car
        rend.material = GameManager.instance.playerConfigs.carMaterials[index];
        //Assign the obtained mesh
        go.GetComponent<MeshFilter>().mesh = carMesh;
        //Assign the spawn point
        go.transform.position = position;
        //Assign the rotation
        go.transform.LookAt(new Vector3(-1291f, -332f, 357.1f));
        //adjust scaling of the car
        go.transform.localScale = new Vector3(2f, 2f, 2f);
        SpawnTyres(go.transform);
    }
    void spawnCharacterMesh(Vector3 position, Quaternion rot, int index)
    {
        //Create a game object to be spawned in this case the players character model
        GameObject go = new GameObject();
        //Create a mesh filter which stores the mesh data of the model to be rendered
        go.AddComponent<MeshFilter>();
        //Create a renderer to render the obtained mesh
        MeshRenderer rend = go.AddComponent<MeshRenderer>();
        //Obtain the mesh of the players car
        Mesh characterMesh = GameManager.instance.playerConfigs.characterModels[index];
        //Obtain the material of the players car
        rend.material = GameManager.instance.playerConfigs.characterMaterials[index];
        //Assign the obtained mesh
        go.GetComponent<MeshFilter>().mesh = characterMesh;
        //Assign the spawn point
        go.transform.position = position;
        //Assign the rotation
        go.transform.LookAt(new Vector3(-1291f, -332f, 357.1f));
        go.transform.Rotate(0, 180, 0);
        //adjust scaling of the character
        go.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
       
    }


    private void SpawnTyres(Transform parent)
    {
        GameObject tyreGO = Instantiate(tyres, parent);
        tyreGO.transform.position += new Vector3(0.3f, -0.8f, 0.12f);
        tyreGO.transform.localScale = 0.35f * Vector3.one;
    }


    void spawnPlayer(Transform spawn, int index)
    {

        //Spawns the car and attaches it to the player for visual representation
        GameObject playerCarVisual = Instantiate(playerConfigsSO.carPrefabs[index], spawn);
        //Spawns the character and attaches it to the driver seat
        GameObject character = Instantiate(playerConfigsSO.characterPrefabs[index], playerCarVisual.GetComponent<CarVisualsHandler>().Driver.transform);

        character.transform.localPosition = character.transform.localPosition + new Vector3(0, 44, 0);

        //Spawns Hat
        GameObject hat = Instantiate(playerConfigsSO.characterHats[index], playerCarVisual.GetComponent<CarVisualsHandler>().Driver.transform.GetChild(0).GetChild(0));
        //Sets Hat Mat
        hat.GetComponent<MeshRenderer>().material = zoneMatSO.defaultHatMat;
        //Set car body mat
        playerCarVisual.transform.Find("CarBody/Body").GetComponent<MeshRenderer>().material = playerConfigsSO.carMaterials[index];
        
        ////Create a gameobject to be spawned in this case the players selected car model
        //GameObject go = new GameObject();
        ////Create a mesh filter which stores the mesh data of the model to be rendered.
        //go.AddComponent<MeshFilter>();
        ////Create a renderer to render the obtained mesh
        //MeshRenderer rend = go.AddComponent<MeshRenderer>();
        ////Obtain the mesh of the players car
        //Mesh carMesh = GameManager.instance.playerConfigs.carModels[index];
        ////Obtain the material of the players car
        //rend.material = GameManager.instance.playerConfigs.carMaterials[index];
        ////Assign the obtained mesh
        //go.GetComponent<MeshFilter>().mesh = carMesh;
        ////Assign the spawn point
        //go.transform.position = position;
        ////Assign the rotation
        //go.transform.LookAt(new Vector3(-1291f, -332f, 357.1f));
        ////adjust scaling of the car
        //go.transform.localScale = new Vector3(2f, 2f, 2f);
        //SpawnTyres(go.transform);
    }

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < playerConfigsSO.numberOfPlayers; i++)
        {
            int players2 = 0;
            if (playerConfigsSO.numberOfPlayers == 2)
            {
                players2 = 1;
            }
            spawnPlayer(spawnPoints[i + players2], i);
        }

        //PREVIOUS CODE
        //Debug.Log("test");
        //if(playerConfigsSO.numberOfPlayers == 2)
        //{
        //    for (int i = 0; i < playerConfigsSO.numberOfPlayers; i++)
        //    {
        //        if (i == 0)
        //        {
        //            spawnCarMesh(player2.transform.position, Quaternion.Euler(0, 250, 0), i);
        //            spawnCharacterMesh(player2.transform.position, Quaternion.Euler(0, 70, 0), i);
        //            Debug.Log("i am attempting to spawn car 2");
        //        }
        //        else if (i == 1)
        //        {
        //            spawnCarMesh(player3.transform.position, Quaternion.Euler(0, 290, 0), i);
        //            spawnCharacterMesh(player3.transform.position, Quaternion.Euler(0, 110, 0), i);
        //            Debug.Log("i am attempting to spawn car 3");
        //        }

        //        else
        //        {
        //            Debug.Log("error spawning cars");
        //        }
        //    }
        //}
        //else if(playerConfigsSO.numberOfPlayers == 3)
        //{
        //    for (int i = 0; i < playerConfigsSO.numberOfPlayers; i++)
        //    {
        //        if (i == 0)
        //        {
        //            spawnCarMesh(player1.transform.position, Quaternion.Euler(0, 230, 0), i);
        //            spawnCharacterMesh(player1.transform.position, Quaternion.Euler(0, 50, 0), i);
        //            Debug.Log("i am attempting to spawn car 1");
        //        }
        //        else if (i == 1)
        //        {
        //            spawnCarMesh(player2.transform.position, Quaternion.Euler(0, 250, 0), i);
        //            spawnCharacterMesh(player2.transform.position, Quaternion.Euler(0, 70, 0), i);
        //            Debug.Log("i am attempting to spawn car 2");
        //        }
        //        else if (i == 2)
        //        {
        //            spawnCarMesh(player3.transform.position, Quaternion.Euler(0, 290, 0), i);
        //            spawnCharacterMesh(player3.transform.position, Quaternion.Euler(0, 110, 0), i);
        //            Debug.Log("i am attempting to spawn car 3");
        //        }
        //        else
        //        {
        //            Debug.Log("error spawning cars");
        //        }
        //    }
        //}
        //else if(playerConfigsSO.numberOfPlayers == 4)
        //{
        //    for (int i = 0; i < playerConfigsSO.numberOfPlayers; i++)
        //    {
        //        if (i == 0)
        //        {
        //            spawnCarMesh(player1.transform.position, Quaternion.Euler(0, 230, 0), i);
        //            spawnCharacterMesh(player1.transform.position, Quaternion.Euler(0, 50, 0), i);
        //            Debug.Log("i am attempting to spawn car 1");
        //        }
        //        else if (i == 1)
        //        {
        //            spawnCarMesh(player2.transform.position, Quaternion.Euler(0, 250, 0), i);
        //            spawnCharacterMesh(player2.transform.position, Quaternion.Euler(0, 70, 0), i);
        //            Debug.Log("i am attempting to spawn car 2");
        //        }
        //        else if (i == 2)
        //        {
        //            spawnCarMesh(player3.transform.position, Quaternion.Euler(0, 290, 0), i);
        //            spawnCharacterMesh(player3.transform.position, Quaternion.Euler(0, 110, 0), i);
        //            Debug.Log("i am attempting to spawn car 3");
        //        }
        //        else if (i == 3)
        //        {
        //            spawnCarMesh(player4.transform.position, Quaternion.Euler(0, 320, 0), i);
        //            spawnCharacterMesh(player4.transform.position, Quaternion.Euler(0, 140, 0), i);
        //            Debug.Log("i am attempting to spawn car 4");
        //        }
        //        else
        //        {
        //            Debug.Log("error spawning cars");
        //        }
        //    }
        //}
    }
      
}
