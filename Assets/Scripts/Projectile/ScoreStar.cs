using UnityEngine;

public class ScoreStar : MonoBehaviour
{
    public int scoreValue = 1;
    public float fallSpeed = 1f;
    Transform playerTransform;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag(GameTag.Player.ToString());
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
        if (other.CompareTag(GameTag.Player.ToString()))
        {

            GameController.Ins.ScoreIncrement(scoreValue);
            AudioController.Ins.PlaySound(AudioController.Ins.gotCollectable);

            Destroy(gameObject);
        }
    }
}
