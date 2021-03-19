using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{

    Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rigidbody.AddForce(Vector3.left * 10.0f);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rigidbody.AddForce(Vector3.right * 10.0f);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rigidbody.AddForce(Vector3.forward * 10.0f);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            rigidbody.AddForce(Vector3.back * 10.0f);
        }
    }
}
