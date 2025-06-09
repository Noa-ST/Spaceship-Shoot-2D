using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject scoreStarPrefab;
    [SerializeField] private float timeToDestroy;
    public AudioClip hitSound;
    public GameObject hitVFX;
    AudioSource aus;
    Rigidbody2D rb;
    GameController gc;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gc = FindObjectOfType<GameController>();
        aus = FindObjectOfType<AudioSource>();
        Destroy(gameObject, timeToDestroy);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = Vector2.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (aus && hitSound)
            {
                aus.PlayOneShot(hitSound);
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
            if (gc != null)
            {
                gc.TrySpawnPowerUp(collision.transform.position);
            }
        } else if (CompareTag("SceneToplimit"))
        {
            Destroy(gameObject);
        }
    }
}
