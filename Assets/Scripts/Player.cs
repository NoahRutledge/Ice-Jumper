using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum action
{
    idle,
    waiting,
    moving,
    start,
    end
}

public class Player : MonoBehaviour {
    public static Player S;

    public BlockStack currentBlock;
    public action state; 
    public float movementTime;

    private float moveStartTime;
    private BlockStack moveTo;
    private Vector3 midPoint;
    private Vector3 midPoint2;
    private Vector3 endPoint;
    private bool callOnce;
    private Transform topDownCamera;
    private BlockStack midBlock;

	// Use this for initialization
	void Awake () {
        S = this;
        state = action.start;
        currentBlock = IceHopping.S.getStart();
        topDownCamera = GameObject.Find("Top Down Camera").transform;
	}

    public void init()
    {
        state = action.start;
        if(currentBlock == null)
        {
            currentBlock = IceHopping.S.getStart();
        }
        transform.position = new Vector3(currentBlock.transform.localPosition.x, currentBlock.transform.position.y + currentBlock.blocks.Count - (1 - Block.heightOffset), currentBlock.transform.localPosition.z);
        GetComponent<Rigidbody>().isKinematic = true;
    }

    // Update is called once per frame
    void Update () {
        if (currentBlock.isEnd && IceHopping.S.stacksLeft() == 1 && state == action.idle)
        {
            state = action.end;
            Invoke("nextLevel", 0.7f);
        }

        topDownCamera.position = new Vector3(this.transform.position.x, 5, this.transform.position.z);

        //check movement keys
        if(state == action.idle)
        {
            GetComponent<Rigidbody>().isKinematic = false;

            //IF LEFT ARROW
            if (Input.GetAxisRaw("Horizontal") == -1)
            {
                //IF HOLDING SHIFT TO JUMP 2
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    if (currentBlock.left != null && currentBlock.left.left != null)
                    {
                        moveTo = currentBlock.left.left;
                        midBlock = currentBlock.left;
                        movementStart();
                    }
                }
                else
                {
                    if (currentBlock.left != null)
                    {
                        moveTo = currentBlock.left;
                        movementStart();
                    }
                    else
                    {
                        midPoint = new Vector3(transform.position.x - .75f, transform.position.y + 0.5f, transform.position.z);
                        endPoint = new Vector3(transform.position.x - 1.25f, -.13f, transform.position.z);
                        state = action.moving;
                        moveStartTime = Time.time;
                    }
                }
            }
            //IF RIGHT ARROW
            else if(Input.GetAxisRaw("Horizontal") == 1)
            {
                //IF HOLDING SHIFT TO JUMP 2
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    if (currentBlock.right != null && currentBlock.right.right != null)
                    {
                        moveTo = currentBlock.right.right;
                        midBlock = currentBlock.right;
                        movementStart();
                    }
                }
                else
                {
                    if (currentBlock.right != null)
                    {
                        moveTo = currentBlock.right;
                        movementStart();
                    }
                    //if its not valid, set the point yourself and start the timer
                    else
                    {
                        midPoint = new Vector3(transform.position.x + .75f, transform.position.y + 0.5f, transform.position.z);
                        endPoint = new Vector3(transform.position.x + 1.25f, -.13f, transform.position.z);
                        state = action.moving;
                        moveStartTime = Time.time;
                    }
                }
            }
            //IF DOWN ARROW
            else if (Input.GetAxisRaw("Vertical") == -1)
            {
                //IF HOLDING SHIFT TO JUMP 2
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    if (currentBlock.down != null && currentBlock.down.down != null)
                    {
                        moveTo = currentBlock.down.down;
                        midBlock = currentBlock.down;
                        movementStart();
                    }
                }
                else
                {
                    if (currentBlock.down != null)
                    {
                        moveTo = currentBlock.down;
                        movementStart();
                    }
                    else
                    {
                        midPoint = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z - .75f);
                        endPoint = new Vector3(transform.position.x, -.13f, transform.position.z - 1.25f);
                        state = action.moving;
                        moveStartTime = Time.time;
                    }
                }
            }
            //IF UP ARROW
            else if (Input.GetAxisRaw("Vertical") == 1)
            {
                //IF HOLDING SHIFT TO JUMP 2
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    if (currentBlock.up != null && currentBlock.up.up != null)
                    {
                        moveTo = currentBlock.up.up;
                        midBlock = currentBlock.up;
                        movementStart();
                    }
                }
                else
                {
                    if (currentBlock.up != null)
                    {
                        moveTo = currentBlock.up;
                        movementStart();
                    }
                    else
                    {
                        midPoint = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z + .75f);
                        endPoint = new Vector3(transform.position.x, -.13f, transform.position.z + 1.25f);
                        state = action.moving;
                        moveStartTime = Time.time;
                    }
                }
            }
        }

        if(state == action.moving)
        {
            float u = (Time.time - moveStartTime) / movementTime;
            if(u > 1)
            {
                //Actually set the positional values to the exact values
                this.transform.localPosition = new Vector3(moveTo.transform.localPosition.x, this.transform.localPosition.y, moveTo.transform.localPosition.z);

                //state = action.idle;
                currentBlock = moveTo;
                callOnce = false;
                moveTo = null;
                midPoint2 = Vector3.zero;
                if(state == action.waiting)
                {
                    state = action.idle;
                }
                else
                {
                    state = action.waiting;
                }
                return;
            }
            else if(u < 0)
            {
                return;
            }
            else
            {
                if(u > .25 && !callOnce)
                {
                    //dont lower more if there are none left to lower
                    if (currentBlock.blocks.Count > 0)
                    {
                        currentBlock.lower();
                        callOnce = true;
                    }
                }
                Vector3 p01, p12, p23, p012, p123;
                p01 = (1 - u) * transform.position + u * midPoint;
                p12 = (1 - u) * midPoint + u * midPoint2;
                p23 = (1 - u) * midPoint2 + u * endPoint;
                p012 = (1 - u) * p01 + u * p12;
                p123 = (1 - u) * p12 + u * p23;

                transform.position = (1 - u) * p012 + u * p123;
            }
        }
	}

    private void movementStart()
    {
        state = action.moving;

        float x = 0;
        float z = 0;
        float y;

        if (midBlock != null)
        {
            //set the y as it will always be this
            y = Mathf.Max(midBlock.blocks.Count, Mathf.Max(transform.position.y, moveTo.blocks.Count)) + 1f;

            //if the x's are the same then the movement was in z
            if (transform.localPosition.x == moveTo.transform.localPosition.x)
            {
                //find the direction: (Down-up || up-down)
                //MOVING DOWN-UP
                if(moveTo.transform.localPosition.z > transform.localPosition.z)
                {
                    midPoint = new Vector3(moveTo.transform.localPosition.x, y, transform.localPosition.z);
                    midPoint2 = new Vector3(moveTo.transform.localPosition.x, y, moveTo.transform.localPosition.z);
                }
                //MOVING UP-DOWN
                else
                {
                    midPoint = new Vector3(moveTo.transform.localPosition.x, y, transform.localPosition.z);
                    midPoint2 = new Vector3(moveTo.transform.localPosition.x, y, moveTo.transform.localPosition.z);
                }
            }
            else
            {
                //find the direction: (left-right || right-left)
                //MOVING LEFT-RIGHT
                if (moveTo.transform.localPosition.x > transform.localPosition.x)
                {
                    midPoint = new Vector3(transform.localPosition.x, y, moveTo.transform.localPosition.z);
                    midPoint2 = new Vector3(moveTo.transform.localPosition.x, y, moveTo.transform.localPosition.z);
                }
                //MOVING RIGHT-LEFT
                else
                {
                    midPoint = new Vector3(transform.localPosition.x, y, moveTo.transform.localPosition.z);
                    midPoint2 = new Vector3(moveTo.transform.localPosition.x, y, moveTo.transform.localPosition.z);
                }
            }
        }
        else
        {
            x = (currentBlock.transform.localPosition.x + moveTo.transform.localPosition.x) / 2;
            y = Mathf.Max(transform.position.y, moveTo.blocks.Count) + 1f;
            z = (currentBlock.transform.localPosition.z + moveTo.transform.localPosition.z) / 2;
            midPoint = new Vector3(x, y, z);
            midPoint2 = midPoint;
        }

        
        endPoint = new Vector3(moveTo.transform.localPosition.x, moveTo.blocks.Count - (1 - Block.heightOffset ), moveTo.transform.localPosition.z);

        midBlock = null;

        moveStartTime = Time.time;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Water")
        {
            state = action.end;
            Invoke("Restart", 0.7f);
        }
    }

    public void Restart()
    {
        IceHopping.S.Restart();
    }

    public void nextLevel()
    {
        IceHopping.S.nextLevel();
    }
    
}
