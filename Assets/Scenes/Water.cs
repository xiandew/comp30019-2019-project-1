using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{

    public Shader shader;
    public Landscape landscape;
    public Sun sun;

    private float size;
    private int numDivisions;
    private Vector3[] vertices;
    private int numOfVerts;

    // Start is called before the first frame update
    void Start()
    {
        this.size = landscape.size;
        this.numDivisions = landscape.numDivisions;

        MeshFilter waterMesh = this.gameObject.AddComponent<MeshFilter>();
        waterMesh.mesh = this.CreateWater();

        MeshRenderer renderer = this.gameObject.AddComponent<MeshRenderer>();
        renderer.material.shader = shader;       
    }

    // Update is called once per frame
    void Update()
    {
        // Get renderer component (in order to pass params to shader)
        MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();

        // Pass updated light positions to shader
        renderer.material.SetColor("_SunLightColor", this.sun.color);
        renderer.material.SetVector("_SunLightPosition", this.sun.GetWorldPosition());
    }

    Mesh CreateWater() {
        Mesh water = new Mesh();
        water.name = "water";

        numOfVerts = (numDivisions + 1) * (numDivisions + 1);
        vertices = new Vector3[numOfVerts];
        Vector2[] uvs = new Vector2[numOfVerts];
        int[] triangles = new int[numDivisions * numDivisions * 6];

        float divisionSize = size / numDivisions;
        float halfSize = size * 0.5f;

        int triOffset = 0;

        // Generate all the verticies for the water
        for (int z= 0; z <= numDivisions; z++) {
            for (int x = 0; x <= numDivisions; x++) {
                vertices[z * (numDivisions + 1) + x] = new Vector3(-halfSize + x * divisionSize, 0.0f, halfSize - z * divisionSize);
                uvs[z * (numDivisions + 1) + x] = new Vector2((float) x / numDivisions, (float) z / numDivisions);

                // Generate the triangles for the corresponding vertex
                if (x < numDivisions && z < numDivisions) {

                    // The index of the verticies for a triangle
                    int topLeft = z * (numDivisions + 1) + x;
                    int botLeft = (z + 1) * (numDivisions + 1) + x;

                    triangles[triOffset] = topLeft;
                    triangles[triOffset + 1] = topLeft + 1;
                    triangles[triOffset + 2] = botLeft + 1;

                    triangles[triOffset + 3] = topLeft;
                    triangles[triOffset + 4] = botLeft + 1;
                    triangles[triOffset + 5] = botLeft;

                    triOffset += 6;
                }
            }
        }

        Color[] color = new Color[numOfVerts];
        Color sea = new Color(28.0f / 255.0f, 112.0f / 255.0f, 200.0f / 255.0f);

        for (int i = 0; i < numOfVerts; i++){
            color[i] = sea;
        }

        water.vertices = vertices;
        water.uv = uvs;
        water.triangles = triangles;
        water.colors = color;

        water.RecalculateBounds();
        water.RecalculateNormals();

        return water;
    }
}
