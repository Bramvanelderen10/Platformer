using UnityEngine;
using System.Collections;

public class CollisionChecker : MonoBehaviour {

    public Transform groundCheckRight;
    public Transform groundCheckLeft;

    public Transform[] wallChecksLeft;
    public Transform[] wallChecksRight;

    public Transform[] doubleJumpWallCheckLeft;
    public Transform[] doubleJumpWallCheckRight;

    public bool facingRight = true;

    public bool IsNearWalls()
    {
        bool isNearWall = false;
        foreach (Transform tf in doubleJumpWallCheckLeft)
        {
            if (Physics2D.Linecast(transform.position, tf.position, 1 << LayerMask.NameToLayer("Ground")))
                isNearWall = true;
        }
        foreach (Transform tf in doubleJumpWallCheckRight)
        {
            if (Physics2D.Linecast(transform.position, tf.position, 1 << LayerMask.NameToLayer("Ground")))
                isNearWall = true;
        }

        return isNearWall;
    }

    public bool IsGrounded()
    {
        if (Physics2D.Linecast(transform.position, groundCheckRight.position, 1 << LayerMask.NameToLayer("Ground")) ||
            Physics2D.Linecast(transform.position, groundCheckLeft.position, 1 << LayerMask.NameToLayer("Ground")))
            return true;

        return false;
    }

    public Utils.Direction CheckSidesForWalls()
    {
        bool wallLeft = false;
        foreach (Transform tf in doubleJumpWallCheckLeft)
        {
            if (Physics2D.Linecast(transform.position, tf.position, 1 << LayerMask.NameToLayer("Ground")))
                wallLeft = true;
        }

        bool wallRight = false;
        foreach (Transform tf in doubleJumpWallCheckRight)
        {
            if (Physics2D.Linecast(transform.position, tf.position, 1 << LayerMask.NameToLayer("Ground")))
                wallRight = true;
        }

        if (wallRight && wallLeft)
        {
            return Utils.Direction.BOTH;
        } else if (wallLeft)
        {
            return Utils.Direction.LEFT;
        } else if (wallRight)
        {
            return Utils.Direction.RIGHT;
        } else
        {
            return Utils.Direction.NONE;
        }
    }

    public bool CheckGround(Utils.Direction check)
    {
        bool value = false;

        switch (check)
        {
            case Utils.Direction.LEFT:
                value = Physics2D.Linecast(transform.position, groundCheckLeft.position, 1 << LayerMask.NameToLayer("Ground"));
                break;
            case Utils.Direction.RIGHT:
                value = Physics2D.Linecast(transform.position, groundCheckRight.position, 1 << LayerMask.NameToLayer("Ground"));
                break;
            case Utils.Direction.BOTH:
                value = (Physics2D.Linecast(transform.position, groundCheckLeft.position, 1 << LayerMask.NameToLayer("Ground")) ||
                    Physics2D.Linecast(transform.position, groundCheckRight.position, 1 << LayerMask.NameToLayer("Ground")));
                break;
        }

        return value;
    }

    public bool CheckWall(Utils.Direction check)
    {
        bool hit = false;
        switch (check)
        {
            case Utils.Direction.LEFT:
                foreach (Transform wallCheck in wallChecksLeft)
                {
                    Debug.DrawLine(transform.position, wallCheck.position);
                    if (Physics2D.Linecast(transform.position, wallCheck.position, 1 << LayerMask.NameToLayer("Ground")))
                    {
                        hit = true;
                    }
                }
                break;
            case Utils.Direction.RIGHT:
                foreach (Transform wallCheck in wallChecksRight)
                {
                    if (Physics2D.Linecast(transform.position, wallCheck.position, 1 << LayerMask.NameToLayer("Ground")))
                    {
                        hit = true;
                    }
                }
                break;
        }

        return hit;
    }

    public float GetDistanceUntilCollision(Utils.Direction check, float distance)
    {
        float distanceHit = 0;
        Vector3 baseVector = transform.position;
        float i = -0.5f;
        int index = 0;
        switch (check)
        {
            case Utils.Direction.LEFT:
                distanceHit = distance * -1;
                Vector3[] leftVectors = new Vector3[10];
                i = -0.5f;
                index = 0;
                while (i < 0.48f)
                {
                    leftVectors[index] = new Vector3(baseVector.x - distance, baseVector.y + i);
                    i += 0.1f;
                    index++;
                }
                foreach (Vector3 leftVector in leftVectors)
                {
                    RaycastHit2D rc2d = Physics2D.Linecast(new Vector3(baseVector.x, leftVector.y), leftVector, 1 << LayerMask.NameToLayer("Ground"));
                    if (rc2d)
                        distanceHit = rc2d.distance * -1;
                }
                break;
            case Utils.Direction.RIGHT:
                distanceHit = distance;
                Vector3[] rightVectors = new Vector3[10];
                i = -0.5f;
                index = 0;
                while (i < 0.48f)
                {
                    rightVectors[index] = new Vector3(baseVector.x + distance, baseVector.y + i);
                    i += 0.1f;
                    index++;
                }
                foreach (Vector3 rightVector in rightVectors)
                {
                    RaycastHit2D rc2d = Physics2D.Linecast(new Vector3(baseVector.x, rightVector.y), rightVector, 1 << LayerMask.NameToLayer("Ground"));
                    if (rc2d)
                        distanceHit = rc2d.distance;
                }
                break;
        }

        return distanceHit;
    }
}
