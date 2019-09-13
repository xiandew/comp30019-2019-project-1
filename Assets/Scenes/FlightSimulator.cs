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
    private Bounds landBounds;

    // Start is called before the first frame update
    void Start() {
        MeshFilter landMesh = landscape.GetComponent<MeshFilter>();
        if (landMesh) {
            landBounds = landMesh.mesh.bounds;
        }
        rb = this.gameObject.GetComponent<Rigidbody>();
        rb.position = pivot = normalise(landBounds.extents.x, landBounds.size.y, landBounds.extents.z);
    }

    // Update is called once per frame
    void Update() {
        // Orientation
        yaw -= sensitivity * Input.GetAxis("Mouse X");
        pitch += sensitivity * Input.GetAxis("Mouse Y");
        this.transform.rotation = Quaternion.LookRotation(new Vector3(yaw, pitch, 0.0f) - pivot, Vector3.up);

        // Prevent tunneling
        RaycastHit hit;
        Vector3 p1 = transform.position + landBounds.center;
        if (Physics.SphereCast(p1, landBounds.size.y / 2, transform.forward, out hit, 10)) {
            speed = 8.0f;
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

        Vector3 p = this.transform.position + this.transform.rotation * new Vector3(dx, 0.0f, dz) * Time.deltaTime;
        rb.MovePosition(normalise(p.x, p.y, p.z));
    }

    // Set camera boundary
    Vector3 normalise(float x, float y, float z) {
        return new Vector3(
            Mathf.Clamp(x, landBounds.min.x + 2, landBounds.max.x - 2),
            Mathf.Clamp(y, landBounds.min.y, landBounds.max.y * 2),
            Mathf.Clamp(z, landBounds.min.z + 2, landBounds.max.z - 2));
    }
}
