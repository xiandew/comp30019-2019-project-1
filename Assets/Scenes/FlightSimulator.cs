using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightSimulator : MonoBehaviour
{
    public Landscape landscape;
    public bool orthographic = false;
    public float speed;
    public float sensitivity;

    private float yaw = 45.0f;
    private float pitch = 30.0f;
    private Rigidbody rb;
    private Bounds landBounds;
    private float initDistance;

    // Start is called before the first frame update
    void Start() {
        MeshFilter landMesh = landscape.GetComponent<MeshFilter>();
        if (landMesh) {
            landBounds = landMesh.mesh.bounds;
        }
        rb = this.gameObject.GetComponent<Rigidbody>();
        rb.position = normalise(-landBounds.extents.x, landBounds.size.y, -landBounds.extents.z);

        // Initial orientation
        this.transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

        initDistance = Vector3.Distance(rb.position, landBounds.center);
    }

    // Update is called once per frame
    void Update() {
        // Ref: http://forum.unity3d.com/threads/how-to-lock-or-set-the-cameras-z-rotation-to-zero.68932/#post-441968
        // Update orientation
        yaw += sensitivity * Input.GetAxis("Mouse X");
        pitch -= sensitivity * Input.GetAxis("Mouse Y");

        // Clamp pitch:
        pitch = Mathf.Clamp(pitch, -90.0f, 90.0f);

        // Wrap yaw:
        while (yaw < 0.0f) {
            yaw += 360.0f;
        }
        while (yaw >= 360.0f) {
            yaw -= 360.0f;
        }

        this.transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

        // Prevent tunneling
        float delta = speed * Vector3.Distance(transform.position, landBounds.center) / initDistance;

        // Movement relative to the orientation
        float dx = 0.0f, dz = 0.0f;
        if (Input.GetKey(KeyCode.D))
            dx += delta;
        if (Input.GetKey(KeyCode.A))
            dx -= delta;
        if (Input.GetKey(KeyCode.W))
            dz += delta;
        if (Input.GetKey(KeyCode.S))
            dz -= delta;

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
