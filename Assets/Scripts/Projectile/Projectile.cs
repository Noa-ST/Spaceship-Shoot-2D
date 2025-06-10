using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject scoreStarPrefab;
    [SerializeField] private float timeToDestroy;
    public GameObject hitVFX;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, timeToDestroy);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = Vector2.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameTag.Enemy.ToString()))
        {
            if (AudioController.Ins)
            {
                AudioController.Ins.PlaySound(AudioController.Ins.hitSound);
            }

            if (hitVFX)
            {
               GameObject vfx = Instantiate(hitVFX, collision.transform.position, Quaternion.identity);
                Destroy(vfx, 1f);
            }

            if (scoreStarPrefab)
            {
                Enemy enemy = collision.GetComponent<Enemy>();
                int value = 1;
                if (enemy != null)
                    value = enemy.scoreValue;

                GameObject star = Instantiate(scoreStarPrefab, collision.transform.position, Quaternion.identity);
                star.GetComponent<ScoreStar>().scoreValue = value;
            }

            Destroy(gameObject);

            Destroy(collision.gameObject);
            if (GameController.Ins != null)
            {
                GameController.Ins.TrySpawnPowerUp(collision.transform.position);
            }
        } else if (CompareTag("SceneToplimit"))
        {
            Destroy(gameObject);
        }
    }
}
