using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PortalController : MonoBehaviour {

    public TextMesh selectText;
    public string sceneName;
    public float detectionRange = 2f;
    private bool isInRange = false;

	// Use this for initialization
	void Start () {
        selectText.text = "";
	}

    void Update()
    {
        if (isInRange)
        {
            if (Input.GetButtonDown("back"))
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, detectionRange, 1 << LayerMask.NameToLayer("Player"));
        if (collider)
        {
            selectText.text = "Press \"Back\" tp enter";
            isInRange = true;
        } else
        {
            selectText.text = "";
            isInRange = false;
        }
    }
}
