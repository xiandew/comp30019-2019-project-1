/** The implementation of the Diamond Square Algorithm is mainly based on a
    YouTube video with the link: https://www.youtube.com/watch?v=1HV8GbFnCik
 */

using UnityEngine;

public class Landscape : MonoBehaviour {

    public int numDivisions;
    
    // The size of the landscape in the x and z direction.
    public float size;
    public float height;

    Vector3[] vertices;
    int numOfVerts;

    public Shader shader;
    public Sun sun;

    void Start() {
        MeshFilter landMesh = this.gameObject.AddComponent<MeshFilter>();
        landMesh.mesh = this.CreateLand();
        
        MeshCollider collider = this.gameObject.AddComponent<MeshCollider>();
        collider.sharedMesh = landMesh.mesh;

        MeshRenderer renderer = this.gameObject.AddComponent<MeshRenderer>();
        renderer.material.shader = shader;
    }

    void Update() {

        // Get renderer component (in order to pass params to shader)
        MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();

        // Pass updated light positions to shader
        renderer.material.SetColor("_SunLightColor", this.sun.color);
        renderer.material.SetVector("_SunLightPosition", this.sun.GetWorldPosition());

    }

    Mesh CreateLand() {
        Mesh land = new Mesh();
        land.name = "Landscape";

        numOfVerts = (numDivisions + 1) * (numDivisions + 1);
        vertices = new Vector3[numOfVerts];
        Vector2[] uvs = new Vector2[numOfVerts];
        int[] triangles = new int[numDivisions * numDivisions * 6];

        float divisionSize = size / numDivisions;
        float halfSize = size * 0.5f;
        float offset = this.height;

        int triOffset = 0;

        // Generate all the verticies for the landscape
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

        // Set the initial values of the four corners.
        vertices[0].y = Random.Range(-offset, offset);
        vertices[numDivisions].y = Random.Range(-offset, offset);
        vertices[vertices.Length - 1].y = Random.Range(-offset, offset);
        vertices[vertices.Length - 1 - numDivisions].y = Random.Range(-offset, offset);

        // The number of iterations where the diamond and square step is going
        // to be performed.
        int iterations = (int)Mathf.Log(numDivisions, 2);

        int numSquares = 1;
        int squareSize = numDivisions;

        // For each iteration, peform one Square step and Diamond Step
        for (int i = 0; i < iterations; i++) {
            
            int row = 0;

            // Iterate through rows of squares.
            for (int j = 0; j < numSquares; j++) {

                int col = 0;

                // Iterate through the squares within the same row.
                for (int k = 0; k < numSquares; k++) {

                    DiamondSquare(row, col, squareSize, offset);
                    col += squareSize;

                }

                row += squareSize;

            }

            numSquares *= 2;
            squareSize /= 2;
            offset *= 0.5f;
        }
        land.vertices = vertices;
        land.uv = uvs;
        land.triangles = triangles;

        land.RecalculateBounds();
        land.RecalculateNormals();

        Color[] color = new Color[numOfVerts];
        Color snow = new Color(206.0f / 255.0f, 211.0f / 255.0f, 217.0f / 255.0f);
        Color grass = new Color(126.0f / 255.0f, 200.0f / 255.0f, 80.0f /255.0f);
        Color sand = new Color(200.0f / 255.0f, 172.0f / 255.0f, 133.0f /255.0f);

        float snowHeight = height / 2;
        float grassHeight = height / 5;

        for (int i = 0; i < numOfVerts; i++){
            float vHeight = vertices[i].y;
            if (vHeight >= snowHeight) {
                color[i] = snow;
            } 
            else if (vHeight < snowHeight && vHeight >= grassHeight) {
                color[i] = grass;
            }
            else {                
                color[i] = sand;
            }
        }
        land.colors = color;

        return land;
    }

    void DiamondSquare(int row, int col, int squareSize, float offset) {
        int halfSize = (int)(squareSize * 0.5f);
        int topLeft = row * (numDivisions + 1) + col;
        int botLeft = (row + squareSize) * (numDivisions + 1) + col;

        // Perform the Diamond step
        int mid = (row + halfSize) * (numDivisions + 1) + (col + halfSize);
        vertices[mid].y = (vertices[topLeft].y + vertices[topLeft + squareSize].y + 
                           vertices[botLeft].y + vertices[botLeft + squareSize].y) * 0.25f +
                           Random.Range(-offset, offset);

        // Perform the Square step
        vertices[topLeft + halfSize].y = (vertices[topLeft].y + vertices[topLeft + squareSize].y + 
                                          vertices[mid].y) / 3 + Random.Range(-offset, offset);
        vertices[mid - halfSize].y = (vertices[topLeft].y + vertices[botLeft].y + 
                                      vertices[mid].y) / 3 + Random.Range(-offset, offset);
        vertices[mid + halfSize].y = (vertices[topLeft + squareSize].y + vertices[botLeft + squareSize].y + 
                                      vertices[mid].y) / 3 + Random.Range(-offset, offset);
        vertices[botLeft + halfSize].y = (vertices[botLeft].y + vertices[botLeft + squareSize].y + 
                                          vertices[mid].y) / 3 + Random.Range(-offset, offset);
    }

}

