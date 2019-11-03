using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{

    public float raiseSpeed;
    public Color color;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.RotateAround(Vector3.zero, Vector3.right, raiseSpeed * Time.deltaTime);
        this.transform.LookAt(Vector3.zero);
    }

    public Vector3 GetWorldPosition()
    {
        return this.transform.position;
    }
}
