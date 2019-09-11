using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightSimulator : MonoBehaviour
{
    public Landscape landscape;
    public bool orthographic = false;
    public float speed;
    public float sensitivity;

    private Vector3 pivot;
    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private Rigidbody rb;
    private Vector3 landBounds;

    // Start is called before the first frame update
    void Start() {
        landBounds = new Vector3(landscape.size, landscape.height, landscape.size);
        MeshFilter landMesh = landscape.GetComponent<MeshFilter>();
        if (landMesh) {
            landBounds = landMesh.mesh.bounds.size;
        }
        rb = this.gameObject.GetComponent<Rigidbody>();
        rb.position = pivot = new Vector3(landBounds.x / 2, landBounds.y, landBounds.z / 2);
    }

    // Update is called once per frame
    void Update() {
        // Orientation
        yaw -= sensitivity * Input.GetAxis("Mouse X");
        pitch += sensitivity * Input.GetAxis("Mouse Y");
        this.transform.rotation = Quaternion.LookRotation(new Vector3(yaw, pitch, 0.0f) - pivot, Vector3.up);

        // Prevent tunneling
        RaycastHit hit;
        Vector3 p1 = transform.position + landscape.GetComponent<MeshFilter>().mesh.bounds.center;
        if (Physics.SphereCast(p1, landBounds.y / 2, transform.forward, out hit, 5))
        {
            speed = 10.0f;
        }

        // Movement relative to the orientation
        float dx = 0.0f, dz = 0.0f;
        if (Input.GetKey(KeyCode.D))
            dx += speed;
        if (Input.GetKey(KeyCode.A))
            dx -= speed;
        if (Input.GetKey(KeyCode.W))
            dz += speed;
        if (Input.GetKey(KeyCode.S))
            dz -= speed;

        rb.MovePosition(this.transform.position + this.transform.rotation * new Vector3(dx, 0.0f, dz) * Time.deltaTime);
    }
}
