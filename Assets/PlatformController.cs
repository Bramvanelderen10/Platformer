using UnityEngine;
using System.Collections;

public class PlatformController : MonoBehaviour {

    private EdgeCollider2D ec2d;
    private BoxCollider2D bc2d;

	// Use this for initialization
	void Start () {
        ec2d = GetComponent<EdgeCollider2D>();
        bc2d = GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(ec2d, bc2d, true);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            transform.gameObject.layer = LayerMask.NameToLayer("Ignore");
            Physics2D.IgnoreCollision(ec2d, other.GetComponent<BoxCollider2D>(), true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            transform.gameObject.layer = LayerMask.NameToLayer("Ground");
            Physics2D.IgnoreCollision(ec2d, other.GetComponent<BoxCollider2D>(), false);
        }
    }
}
