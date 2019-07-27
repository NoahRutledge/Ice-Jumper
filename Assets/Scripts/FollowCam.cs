using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour {

    public float widthOffset = 3f;
    public float heightOffset = 8f;
    public float xMoveSpeed;
    public float yMoveSpeed;
    public GameObject player;
	
	// Update is called once per frame
	void Update () {
		if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            return;
        }

        float targetX = transform.position.x;
        float targetY = transform.position.y;
        float targetZ = transform.position.z;
        float speed = 0;

        //if the player is further to the right than the camera position plus offset
        if(player.transform.localPosition.x > this.transform.position.x + widthOffset)
        {
            targetX = transform.localPosition.x + (widthOffset / 2);
            speed = xMoveSpeed;
        }
        else if (player.transform.localPosition.x < transform.position.x - widthOffset)
        {
            targetX = transform.localPosition.x - (widthOffset / 2);
            speed = xMoveSpeed;
        }

        //if the player is farther back than should be from the camera
        if (player.transform.localPosition.z > transform.position.z + (heightOffset*1.5))
        {
            targetZ = transform.localPosition.z + (heightOffset / 2);
            speed = yMoveSpeed;
        }
        else if (player.transform.localPosition.z < transform.position.z + heightOffset)
        {
            targetZ = transform.localPosition.z - (heightOffset / 2);
            speed = yMoveSpeed;
        }

        Vector3 target = new Vector3(targetX, targetY, targetZ);
        transform.position = Vector3.Lerp(transform.position, target, speed);
	}
}
