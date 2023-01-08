
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Projectile laserPrefab;
    public float speed = 5.0f;
    private bool _laserActive;
    public System.Action killed;

    [SerializeField] private AudioSource laserSoundEffect;


    private void Update()
    {
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        if (Input.GetKey(KeyCode.A ) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (this.transform.position.x < leftEdge.x + 0.5f)
            {
                this.transform.position = new Vector3(leftEdge.x + 0.5f, this.transform.position.y, this.transform.position.z);
            }
            else {
                this.transform.position += Vector3.left * this.speed * Time.deltaTime;

            }
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (this.transform.position.x > rightEdge.x - 0.5f)
            {
                this.transform.position = new Vector3(rightEdge.x - 0.5f, this.transform.position.y, this.transform.position.z);
            }
            else {
                this.transform.position += Vector3.right * this.speed * Time.deltaTime;
            }
        }

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (!_laserActive)
        {
            laserSoundEffect.Play();

            Projectile projectile = Instantiate(this.laserPrefab, this.transform.position, Quaternion.identity);
            projectile.destroyed += LaserDestroyed;
            _laserActive = true;
        }
        
    }
    private void LaserDestroyed()
    {
        _laserActive = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Invader") || other.gameObject.layer == LayerMask.NameToLayer("Missile"))
        {
            if (killed != null)
            {
                killed.Invoke();
            }
        }
    }
}
