using UnityEngine;
using UnityEditor.Timeline.Actions;

public class Bunker_2_0 : MonoBehaviour
{
    public Sprite[] spriteArray;

    private SpriteRenderer _spriteRenderer;
    
    public int currentSprite;

    public Projectile missilePrefab;

    public float timeToWait;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(MissileAttack), this.timeToWait, this.timeToWait);
    }

    private void GotHit()
    {
        currentSprite++;

        if (currentSprite >= spriteArray.Length)
        {
            this.gameObject.SetActive(false);
            currentSprite = 0;

        }
        
        _spriteRenderer.sprite = spriteArray[currentSprite];
        

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Laser") || other.gameObject.layer == LayerMask.NameToLayer("Missile"))
        {
            GotHit();
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Invader"))
        {
            this.gameObject.SetActive(false);
        }

    }
    public void ResetBunker()
    {
        this.gameObject.SetActive(true);
        
        currentSprite = 0;
        _spriteRenderer.sprite = spriteArray[currentSprite];
        
    }

    public void DeactivateBunker() 
    {
        this.gameObject.SetActive(false);
    }

    private void MissileAttack()
    {
        if (Random.value < (0.07f) && this.gameObject.activeInHierarchy)
        {
            Instantiate(this.missilePrefab, this.transform.position, Quaternion.identity);

        }
    }

}
