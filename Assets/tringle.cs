#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
#endif

public class TrianglePrimitive
{
#if UNITY_EDITOR
    private static Mesh CreateMesh()
    {
        Vector3[] vertices = {
             new Vector3(-0.5f, -0.5f, 0),
             new Vector3(0.5f, -0.5f, 0),
             new Vector3(0f, 0.5f, 0)
         };

        Vector2[] uv = {
             new Vector2(0, 0),
             new Vector2(1, 0),
             new Vector2(0.5f, 1)
         };

        int[] triangles = { 0, 1, 2 };

        var mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        return mesh;
    }

    private static GameObject CreateObject()
    {
        var obj = new GameObject("Triangle");
        var mesh = CreateMesh();
        var filter = obj.AddComponent<MeshFilter>();
        var renderer = obj.AddComponent<MeshRenderer>();
        var collider = obj.AddComponent<MeshCollider>();

        filter.sharedMesh = mesh;
        collider.sharedMesh = mesh;
        renderer.sharedMaterial = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Material.mat");

        return obj;
    }

    [MenuItem("GameObject/3D Object/Triangle", false, 0)]
    public static void Create()
    {
        CreateObject();
    }
#endif
}