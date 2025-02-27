using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;
    MeshCollider meshCollider;
    Color[] colors;
    Vector3[] vertices;
    int[] triangles;
    public Gradient gradient;
    public int xSize = 20;
    public int zSize = 20;
    public float minTerrainHeight = 0f;
    public float maxTerrainHeight = 5f;
    public float smoothness = 1f;  // Controls the smoothness of the terrain
    private float offsetX;
    private float offsetZ;

    void Start()
    {
        int seed = Random.Range(1000000, 10000000); // Generate a larger random seed
        Debug.Log("Terrain Seed: " + seed);
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        InitializeSeed(seed);
        meshCollider = gameObject.AddComponent<MeshCollider>();
        CreateShape();
        UpdateMesh();
    }

    public void InitializeSeed(int newSeed)
    {
        // Use the seed to generate consistent random offsets
        Random.InitState(newSeed);
        offsetX = Random.Range(-10000f, 10000f);
        offsetZ = Random.Range(-10000f, 10000f);
    }

    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        // Calculate the terrain height and smoothness
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                // Apply the offsets to the Perlin noise coordinates
                float perlinX = (x * smoothness) + offsetX;
                float perlinZ = (z * smoothness) + offsetZ;
                float y = Mathf.PerlinNoise(perlinX, perlinZ) * (maxTerrainHeight - minTerrainHeight) + minTerrainHeight;
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        // Create triangles
        triangles = new int[xSize * zSize * 6];
        int vert = 0;
        int tris = 0;
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;
                vert++;
                tris += 6;
            }
            vert++;
        }

        // Color the terrain based on height
        colors = new Color[vertices.Length];
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float height = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, vertices[i].y);
                colors[i] = gradient.Evaluate(height);
                i++;
            }
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
        mesh.RecalculateNormals();
        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = mesh;
    }
}