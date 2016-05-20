using UnityEngine;
using System.Collections;

public class TrapController : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            PlayerController pc = coll.gameObject.GetComponent<PlayerController>();
            pc.AddDamage(1000f);
        }
    }
}
