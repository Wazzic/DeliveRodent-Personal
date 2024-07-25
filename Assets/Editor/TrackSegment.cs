using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;
using UnityEngine.XR;

[RequireComponent(typeof(MeshFilter))]
public class TrackSegment : MonoBehaviour
{
    [SerializeField] Mesh2D shape2D;

    [Range(2, 32)]
    [SerializeField] int edgeRingCount = 8;

    [Range(0, 1)]
    [SerializeField] float tTest = 0;
    [SerializeField] public Transform[] controlPoints = new Transform[4];

    Mesh segmentMesh;

    public Vector3 GetPos(int i) => (controlPoints[i].position);

    void Start()
    {
        segmentMesh = new Mesh();
        segmentMesh.name = "Segment";
        GetComponent<MeshFilter>().sharedMesh = segmentMesh;
        controlPoints = GetComponentsInChildren<Transform>();

        GenerateMesh();
        GenerateColl();
    }
    void GenerateMesh()
    {
        segmentMesh.Clear();

        //Verts
        List<Vector3> verts = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();

        for (int ring = 0; ring < edgeRingCount; ring++)
        {
            float t = ring / (edgeRingCount - 1f);

            OrientedPoint op = GetBezierOP(t);

            for (int i = 0; i < shape2D.vertices.Length; i++)
            {
                verts.Add(op.LocalToWorldPos(shape2D.vertices[i].point) - (this.transform.position));
                normals.Add(op.LocalToWorldVect(shape2D.vertices[i].normal));
                uvs.Add(new Vector2(shape2D.vertices[i].u, t));
            }
        }

        //Tris
        List<int> triIndices = new List<int>();
        for (int ring = 0; ring < edgeRingCount - 1; ring++)
        {
            int rootIndex = ring * shape2D.VertexCount;
            int rootIndexNext = (ring + 1) * shape2D.VertexCount;

            for (int line = 0; line < shape2D.LineCount - 1; line += 2)
            {
                int lineIndexA = shape2D.lineIndices[line];
                int lineIndexB = shape2D.lineIndices[line + 1];

                int currentA = rootIndex + lineIndexA;
                int currentB = rootIndex + lineIndexB;

                int nextA = rootIndexNext + lineIndexA;
                int nextB = rootIndexNext + lineIndexB;

                triIndices.Add(currentA);
                triIndices.Add(nextA);
                triIndices.Add(nextB);
                triIndices.Add(currentA);
                triIndices.Add(nextB);
                triIndices.Add(currentB);
            }
        }

        segmentMesh.SetVertices(verts);
        segmentMesh.SetNormals(normals);
        segmentMesh.SetUVs(0, uvs);
        segmentMesh.SetTriangles(triIndices, 0);
    }

    private void GenerateColl()
    {
        MeshCollider meshc = gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;

        meshc.sharedMesh = null;
        meshc.sharedMesh = segmentMesh;
    }

    public void OnDrawGizmos()
    {
        for (int i = 0; i < 4; i++)
        {
            Gizmos.DrawSphere(GetPos(i), 0.05f);
        }
        Handles.DrawBezier(GetPos(0), GetPos(3), GetPos(1), GetPos(2), Color.white, EditorGUIUtility.whiteTexture, 1f);

        Gizmos.color = Color.red;

        OrientedPoint testPoint = GetBezierOP(tTest);
        Handles.PositionHandle(testPoint.pos, testPoint.rot);

        //void DrawPoint(Vector2 localPos) => Gizmos.DrawSphere(testPoint.LocalToWorld(localPos), 0.15f);

        Vector3[] verts = shape2D.vertices.Select(v => testPoint.LocalToWorldPos(v.point)).ToArray();

        for (int i = 0; i < shape2D.lineIndices.Length; i += 2)
        {
            Vector3 a = verts[shape2D.lineIndices[i]];
            Vector3 b = verts[shape2D.lineIndices[i + 1]];

            Gizmos.DrawLine(a, b);
            //DrawPoint(shape2D.vertices[i].point);
        }
        Gizmos.color = Color.white;
    }


    OrientedPoint GetBezierOP(float t)
    {
        Vector3 p0 = GetPos(0);
        Vector3 p1 = GetPos(1);
        Vector3 p2 = GetPos(2);
        Vector3 p3 = GetPos(3);

        Vector3 a = Vector3.Lerp(p0, p1, t);
        Vector3 b = Vector3.Lerp(p1, p2, t);
        Vector3 c = Vector3.Lerp(p2, p3, t);

        Vector3 d = Vector3.Lerp(a, b, t);
        Vector3 e = Vector3.Lerp(b, c, t);

        Vector3 pos = Vector3.Lerp(d, e, t);
        Vector3 tangent = (e - d).normalized;

        return new OrientedPoint(pos, tangent);
    }


}