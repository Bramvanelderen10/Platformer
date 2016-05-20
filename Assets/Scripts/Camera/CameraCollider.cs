using UnityEngine;
using System.Collections;
using System;

public class CameraCollider : MonoBehaviour {

    private new Camera camera;

    // Use this for initialization
    void Start ()
    {
        camera = transform.parent.gameObject.GetComponent<Camera>();
        Vector3 position = transform.localPosition;
        Vector3 cameraPosition = transform.parent.transform.localPosition;
        if (cameraPosition.z != 0)
            position.z += (cameraPosition.z * -1);

        transform.localPosition = position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        Vector3[] corners = new Vector3[4];
        GetCorners(camera, 0, ref corners);

        Vector3 scale = new Vector3();

        float distance_x = Vector3.Distance(corners[0], corners[1]);
        float distance_y = Vector3.Distance(corners[0], corners[2]);

        scale.x = distance_x;
        scale.y = distance_y;
        scale.z = 0;

        transform.localScale = scale;
    }

    public static void GetCorners(Camera camera, float distance, ref Vector3[] corners)
    {
        Array.Resize(ref corners, 4);

        // Top left
        corners[0] = camera.ViewportToWorldPoint(new Vector3(0, 1, distance));

        // Top right
        corners[1] = camera.ViewportToWorldPoint(new Vector3(1, 1, distance));

        // Bottom left
        corners[2] = camera.ViewportToWorldPoint(new Vector3(0, 0, distance));

        // Bottom right
        corners[3] = camera.ViewportToWorldPoint(new Vector3(1, 0, distance));
    }
}
