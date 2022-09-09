using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private HasHealth playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GetComponent<HasHealth>();
    }

    public void RespawnPlayer(Vector2 spawnPosition)
    {
        Vector3 spawnPoint = spawnPosition;
        spawnPoint.z = gameObject.transform.position.z;
        gameObject.transform.position = spawnPoint;
        playerHealth.ResetHealth();
        gameObject.SetActive(true);
    }
}
