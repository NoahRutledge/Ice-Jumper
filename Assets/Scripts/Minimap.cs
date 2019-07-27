using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour {

    public float waitTime;
    public float minSize;
    public Vector2 viewportXYMin;
    public Vector2 viewportWHMin;

    private Camera camera;
    private bool isMax = false;
    private bool isWaiting = false;
    private float waitUntil;

    void Start()
    {
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update () {
        //if we're not waiting on a cooldown
        if (!isWaiting)
        {
            //if space is pressed and the camera is currently maxed
            if (Input.GetKey(KeyCode.Space) && isMax)
            {
                camera.rect = new Rect(viewportXYMin.x, viewportXYMin.y, viewportWHMin.x, viewportWHMin.y);
                camera.orthographicSize = minSize;
                isMax = false;

                waitUntil = Time.time + waitTime;
            }
            //else expand the minimap
            else if (Input.GetKey(KeyCode.Space))
            {
                camera.rect = new Rect(0, 0, 1, 1);
                camera.orthographicSize = LevelInfo.S.getLevel(IceHopping.S.currentLevel).size;
                isMax = true;

                waitUntil = Time.time + waitTime;
            }
        }

        if(waitUntil > Time.time)
        {
            isWaiting = true;
        }
        else
        {
            isWaiting = false;
        }
	}
}
