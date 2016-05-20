using UnityEngine;

abstract public class AiInterface : MonoBehaviour
{
    public enum AIStatus
    {
        STOPPED,
        STARTED,
        INITIALIZING
    }

    protected AIStatus aiStatus = AIStatus.INITIALIZING;

    protected CollisionChecker cc;
    protected Animator anim;
    protected Rigidbody2D rb2d;
    public GameObject sprite;
    public Utils.ObjectType type = Utils.ObjectType.ENEMY;

    protected bool facingLeft = true;
    protected bool isAttacking = false;

    void FixedUpdate()
    {
        Move();
        Attack();
    }

    abstract protected void Move();

    abstract protected void Attack();

    public AIStatus GetAIStatus()
    {

        return aiStatus;
    }

    public void Flip(Utils.Direction direction)
    {
        if ((direction == Utils.Direction.LEFT && !facingLeft) || (direction == Utils.Direction.RIGHT && facingLeft))
        {
            facingLeft = !facingLeft;
            Vector3 scale = sprite.transform.localScale;
            scale.x *= -1;
            sprite.transform.localScale = scale;
        }
    }
}