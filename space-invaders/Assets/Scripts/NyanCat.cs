using UnityEngine;

public class NyanCat : MonoBehaviour
{
    //animacja
    public Sprite[] animationSprites;
    public float animationTime = 0.1f;

    private SpriteRenderer _spriteRenderer;
    private int _animationFrame;
    private float flip = 1.0f;

    //dzwiek
    [SerializeField] private AudioSource nyanSoundEffect;

    public float speed = 5f;
    public float appearAfterTime = 30f;
    public int score = 200;
    public System.Action<NyanCat> killed;

    public Vector3 leftBorder { get; private set; }
    public Vector3 rightBorder { get; private set; }
    public int direction { get; private set; } = -1;
    public bool alive { get; private set; }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {      
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        Vector3 left = transform.position;
        left.x = leftEdge.x - 1.5f;
        leftBorder = left;

        Vector3 right = transform.position;
        right.x = rightEdge.x + 1.5f;
        rightBorder = right;

        transform.position = leftBorder;
        Disappear();
    }

    private void Update()
    {
        if (!alive)
        {
            return;
        }

        if (direction == 1)
        {
            GoRight();
        }
        else
        {
            GoLeft();
        }
    }

    private void AnimateSprite()
    {
        _animationFrame++;

        if (_animationFrame >= this.animationSprites.Length)
        {
            _animationFrame = 0;
        }
        _spriteRenderer.sprite = this.animationSprites[_animationFrame];
    }

    private void GoRight()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;

        if (transform.position.x >= rightBorder.x)
        {
            Disappear();
        }
    }

    private void GoLeft()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x <= leftBorder.x)
        {
            Disappear();
        }
    }

    private void Appear()
    {
        direction *= -1;

        if (direction == 1)
        {
            transform.position = leftBorder;
        }
        else
        {
            transform.position = rightBorder;
        }

        alive = true;
        nyanSoundEffect.Play();
        InvokeRepeating(nameof(AnimateSprite), this.animationTime, this.animationTime);
        
    }

    private void Disappear()
    {
        alive = false;

        if (direction == 1)
        {
            transform.position = rightBorder;
        }
        else
        {
            transform.position = leftBorder;
        }

        Invoke(nameof(Appear), appearAfterTime);

        transform.localScale = new Vector3(1.0f * flip, 1.0f, 1.0f);
        flip *= -1.0f;

        nyanSoundEffect.Stop();
        CancelInvoke(nameof(AnimateSprite));

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Laser") || other.gameObject.layer == LayerMask.NameToLayer("BunkerMissile"))
        {
            Disappear();

            if (killed != null)
            {
                killed.Invoke(this);
            }
        }
    }
}