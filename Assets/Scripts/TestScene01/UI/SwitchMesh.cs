using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMesh : MonoBehaviour
{



    [Serializable]
    public struct MeshWithMaterial {
        public Mesh mesh;
        public Material material;
    }

    [SerializeField]
    private List<MeshWithMaterial> meshStorage;


    public static SwitchMesh instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this);
        }
    }
    //Changes the mesh based on an index that uses the mesh storage
    public void ChangeMesh(int index, MeshFilter mesh, MeshRenderer renderer)
    {
        if (index < meshStorage.Count && index >= 0)
        {
            mesh.mesh = meshStorage[index].mesh;
            renderer.material = meshStorage[index].material;
        }
    }

    //Used to switch the mesh you want with a random mesh in the storage
    public int ChangeRandomMesh(MeshFilter mesh, MeshRenderer renderer)
    {
        int randomNumber = UnityEngine.Random.Range(0, meshStorage.Count);
        if(meshStorage.Count > 0)
        {
            mesh.mesh = meshStorage[randomNumber].mesh;
            renderer.material = meshStorage[randomNumber].material;
        }
        return randomNumber;
    }

}
