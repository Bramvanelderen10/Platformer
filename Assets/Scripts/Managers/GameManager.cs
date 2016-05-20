using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject player;
    public GameObject checkpointPrefab;
    public float checkpointSpawnTime = 0.5f;
    public Image healthBar;

    private PlayerController pc;

    private GameObject checkpoint;
    private bool isPressedCheckpoint = false;
    private float buttonPressedTimer = 0f;

    public float deathTime = 2f;
    private float deathTimer = 0f;

    public string spawnLocationName;

    // Use this for initialization
    void Start()
    {
        player.transform.position = GameObject.Find(spawnLocationName).transform.position;
        pc = player.GetComponent<PlayerController>();
        checkpoint = Instantiate(checkpointPrefab);
        checkpoint.transform.position = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("checkpoint") && !isPressedCheckpoint)
        {
            isPressedCheckpoint = true;
            buttonPressedTimer = Time.time;
        }

        if (Input.GetButtonUp("checkpoint"))
        {
            isPressedCheckpoint = false;
            if ((Time.time - buttonPressedTimer) >= checkpointSpawnTime)
            {
                Destroy(checkpoint);
                checkpoint = Instantiate(checkpointPrefab);
                checkpoint.transform.position = player.transform.position;
            }
        }


        if (pc.hitPoints <= 0)
        {
            if (Time.time - deathTimer > deathTime)
            {

                player.transform.gameObject.SetActive(false);
                deathTimer = Time.time;
                pc.hitPoints = 0.01f;
                player.transform.position = checkpoint.transform.position;
            }
        }
        else if (Time.time - deathTimer > deathTime && deathTimer != 0f)
        {
            deathTimer = 0f;
            pc.hitPoints = pc.maxHitPoints;
            player.transform.gameObject.SetActive(true);
        }

        UpdateIU();
    }

    private void UpdateIU()
    {
        healthBar.fillAmount = pc.hitPoints / pc.maxHitPoints;
    }
}
