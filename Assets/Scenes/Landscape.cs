using UnityEngine;

public class Landscape : MonoBehaviour {

    public int xLength;
    public int zLength;

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

        int numOfVerticies = xLength * zLength * 2 * 3;
        Vector3[] verticies = new Vector3[numOfVerticies];
        Color[] colors = new Color[numOfVerticies];

        int count = 0;
        for (int x = 0; x < xLength; x++) {
            for (int z = 0; z < zLength; z++) {
                verticies[count] = new Vector3((float) x, 0, (float) z);
                verticies[count + 1] = new Vector3((float) x, 0, (float) z + 1);
                verticies[count + 2] = new Vector3((float) x + 1, 0, (float) z);

                verticies[count + 3] = new Vector3((float) x + 1, 0, (float) z + 1); 
                verticies[count + 4] = new Vector3((float) x + 1, 0, (float) z); 
                verticies[count + 5] = new Vector3((float) x, 0, (float) z + 1);

                colors[count] = Color.yellow; 
                colors[count + 1] = Color.yellow; 
                colors[count + 2] = Color.yellow; 

                colors[count + 3] = Color.yellow; 
                colors[count + 4] = Color.yellow; 
                colors[count + 5] = Color.yellow; 

                count += 6;
            }
        }

        land.vertices = verticies;
        land.colors = colors;

        int[] triangles = new int[land.vertices.Length];
        for (int i = 0; i < land.vertices.Length; i++) {
            triangles[i] = i;
        }

        land.triangles = triangles;

        return land;
    }

}