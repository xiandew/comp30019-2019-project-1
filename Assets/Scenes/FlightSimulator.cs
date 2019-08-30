using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightSimulator : MonoBehaviour
{
    public Landscape landscape;
    public bool orthographic = false;

    private Vector3 pivot;
    private float yaw = 0.0f;
    private float pitch = 0.0f;

    // Start is called before the first frame update
    void Start() {
        Vector3 meshBounds = new Vector3(landscape.size, landscape.height, landscape.size);
        MeshFilter landMesh = landscape.GetComponent<MeshFilter>();
        if (landMesh) {
            meshBounds = landMesh.mesh.bounds.size;
        }
        this.transform.position = pivot = new Vector3(meshBounds.x / 2, meshBounds.y, meshBounds.z / 2);
    }

    // Update is called once per frame
    void Update() {
        // Orientation
        yaw -= Input.GetAxis("Mouse X");
        pitch += Input.GetAxis("Mouse Y");
        this.transform.rotation = Quaternion.LookRotation(new Vector3(yaw, pitch, 0.0f) - pivot, Vector3.up);

        // Movement relative to the orientation
        float dx = 0.0f, dz = 0.0f;
        if (Input.GetKey(KeyCode.D))
            dx += 1.0f;
        if (Input.GetKey(KeyCode.A))
            dx -= 1.0f;
        if (Input.GetKey(KeyCode.W))
            dz += 1.0f;
        if (Input.GetKey(KeyCode.S))
            dz -= 1.0f;
        this.transform.position += this.transform.rotation * new Vector3(dx, 0.0f, dz) * Time.deltaTime;
    }
}
