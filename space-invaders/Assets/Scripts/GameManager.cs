using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class GameManager : MonoBehaviour
{
    private Player player;
    private Invaders invaders;
    
    private NyanCat nyanCat;
    private Bunker_2_0[] bunkers;

    public GameObject gameOverUI;
    public Text scoreText;
    public Text livesText;

    public int score { get; private set; }
    public int lives { get; private set; }

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        invaders = FindObjectOfType<Invaders>();
        
        nyanCat = FindObjectOfType<NyanCat>();
        bunkers = FindObjectsOfType<Bunker_2_0>();
    }

    private void Start()
    {
        player.killed += OnPlayerKilled;
        
        nyanCat.killed += OnNyanCatKilled;
        invaders.killed += OnInvaderKilled;
        NewGame();
    }

    
    private void Update()
    {
        if (lives <= 0 && Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("Menu");
        }
    }
    

    private void NewGame()
    {
        gameOverUI.SetActive(false);

        SetScore(0);
        SetLives(3);
        NewRound();
    }

    private void NewRound()
    {
        invaders.ResetInvaders();
        invaders.gameObject.SetActive(true);

        for (int i = 0; i < bunkers.Length; i++)
        {
            bunkers[i].ResetBunker();
        }

        Respawn();
    }

    private void Respawn()
    {
        Vector3 position = player.transform.position;
        position.x = 0f;
        player.transform.position = position;
        player.gameObject.SetActive(true);
    }

    private void GameOver()
    {
        gameOverUI.SetActive(true);
        invaders.gameObject.SetActive(false);

        for (int i = 0; i < bunkers.Length; i++)
        {
            bunkers[i].DeactivateBunker();
        }

        SaveHighScore();

    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString().PadLeft(5, '0');
    }

    private void SetLives(int lives)
    {
        this.lives = Mathf.Max(lives, 0);
        livesText.text = lives.ToString();
    }

    private void OnPlayerKilled()
    {
        SetLives(lives - 1);

        player.gameObject.SetActive(false);

        if (lives > 0)
        {
            Invoke(nameof(NewRound), 1f);
        }
        else
        {
            GameOver();
        }
    }

    private void OnInvaderKilled(Invader invader)
    {
        SetScore(score + invader.score);

        if (invaders.amountKilled == invaders.totalAmount)
        {
            NewRound();
        }
    }
  
    private void OnNyanCatKilled(NyanCat nyanCat)
    {
        SetScore(score + nyanCat.score);
    }


    private void SaveHighScore() {

        int temp1;
        int temp2;


        if (this.score >= PlayerPrefs.GetInt("Score3", 0)) {

            if (this.score >= PlayerPrefs.GetInt("Score2", 0))
            {

                if (this.score >= PlayerPrefs.GetInt("Score1", 0))
                {

                    temp2 = PlayerPrefs.GetInt("Score2", 0);
                    PlayerPrefs.SetInt("Score3", temp2);

                    temp1 = PlayerPrefs.GetInt("Score1", 0);
                    PlayerPrefs.SetInt("Score2", temp1);

                    PlayerPrefs.SetInt("Score1", this.score);
                }
                else {

                    temp1 = PlayerPrefs.GetInt("Score2", 0);
                    PlayerPrefs.SetInt("Score3", temp1);

                    PlayerPrefs.SetInt("Score2", this.score);  
                }

            }
            else {
                PlayerPrefs.SetInt("Score3", this.score);
            }
        }  
    }
}
