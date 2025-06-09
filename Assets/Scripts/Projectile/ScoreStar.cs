using UnityEngine;

public class ScoreStar : MonoBehaviour
{
    public int scoreValue = 1;
    public float fallSpeed = 1f;
    Transform playerTransform;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        if (playerTransform == null)
            return;

        transform.position = Vector3.Lerp(transform.position, playerTransform.position, fallSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameController gc = FindObjectOfType<GameController>();
            if (gc != null)
            {
                gc.ScoreIncrement(scoreValue);
            }

            Destroy(gameObject);
        }
    }
}
