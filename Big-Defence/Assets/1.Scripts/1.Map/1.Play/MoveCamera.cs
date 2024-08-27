using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    private Vector3 lastMousePosition;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(1))
        {
            Vector3 delta = (Vector3)Input.mousePosition - lastMousePosition;
            Camera.main.transform.position -= new Vector3(delta.x, delta.y) * 0.01f;
            lastMousePosition = Input.mousePosition;
        }
    }
}
