using UnityEngine;

public enum AsteroidType { Big, Medium, Little }

public class Asteroid : MonoBehaviour
{
    public AsteroidType asteroidType;
    public GameObject explosionPrefab;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet")  && !collision.CompareTag("EnemyBullet")) return;

        Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);
        Destroy(collision.gameObject);
        if (collision.CompareTag("Bullet")) { AddScore(); }
        DestroyAsteroid();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            DestroyAsteroid();
        }
    }

    private void AddScore()
    {
        switch (asteroidType)
        {
            case AsteroidType.Big:
                LevelManager.Instance.AddScore(20);
                break;
            case AsteroidType.Medium:
                LevelManager.Instance.AddScore(50);
                break;
            case AsteroidType.Little:
            default:
                LevelManager.Instance.AddScore(100);
                break;
        }
    }

    private void DestroyAsteroid()
    {
        switch(asteroidType)
        {
            case AsteroidType.Big:
                AsteroidsManager.Instance.DestroyAsteroid(this.gameObject);
                AsteroidsManager.Instance.CreateMediumAsteroid(GetRandomPosition(0.8f));
                AsteroidsManager.Instance.CreateMediumAsteroid(GetRandomPosition(0.8f));
                break;
            case AsteroidType.Medium:
                AsteroidsManager.Instance.DestroyAsteroid(this.gameObject);
                AsteroidsManager.Instance.CreateLittleAsteroid(GetRandomPosition(0.5f));
                AsteroidsManager.Instance.CreateLittleAsteroid(GetRandomPosition(0.5f));
                AsteroidsManager.Instance.CreateLittleAsteroid(GetRandomPosition(0.5f));
                break;
            case AsteroidType.Little:
            default:
                AsteroidsManager.Instance.DestroyAsteroid(this.gameObject);
                break;
        }

        if (LevelManager.Instance.IsLevelFinished())
        {
            LevelManager.Instance.LevelFinished();
        }
    }

    private Vector2 GetRandomPosition(float _radius)
    {
        Vector2 position = (Vector2)this.transform.position + (Random.insideUnitCircle * _radius);
        return position;
    }
    
}
