using UnityEngine;

public class Landscape : MonoBehaviour {

    public int numDivisions;
    
    // The size of the landscape in the x and z direction.
    public float size;
    public float height;

    Vector3[] vertices;


    void Start() {
        MeshFilter landMesh = this.gameObject.AddComponent<MeshFilter>();
        landMesh.mesh = this.CreateLand();

        MeshRenderer landRender = this.gameObject.AddComponent<MeshRenderer>();
        landRender.material.shader = Shader.Find("Unlit/LandscapeShader");
    }

    void Update() {

    }

    Mesh CreateLand() {
        Mesh land = new Mesh();
        land.name = "Landscape";

        int numOfVerts = (numDivisions + 1) * (numDivisions + 1);
        this.vertices = new Vector3[numOfVerts];
        int[] triangles = new int[numDivisions * numDivisions * 6];

        float divisionSize = size / numDivisions;
        float halfSize = size * 0.5f;

        int triOffset = 0;

        // Generate all the verticies for the landscape
        for (int z= 0; z <= numDivisions; z++) {
            for (int x = 0; x <= numDivisions; x++) {
                vertices[z * (numDivisions + 1) + x] = new Vector3(-halfSize + x * divisionSize, 0.0f, halfSize - z * divisionSize);
                
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

        Color[] colors = new Color[numOfVerts];
        for (int i = 0; i < numOfVerts; i++) {
            colors[i] = Color.yellow;
        }

        land.vertices = vertices;
        land.triangles = triangles;
        land.colors = colors;

        return land;
    }

}

