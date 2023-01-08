using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderBoardScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score1Text;
    [SerializeField] private TextMeshProUGUI score2Text;
    [SerializeField] private TextMeshProUGUI score3Text;
    
 
    private int score1;
    private int score2;
    private int score3;

    private void Awake()
    {
        GetScores();
        UpdateScores();
    }

    private void GetScores() {
        score1 = PlayerPrefs.GetInt("Score1", 0);
        score2 = PlayerPrefs.GetInt("Score2", 0);
        score3 = PlayerPrefs.GetInt("Score3", 0);
    }

    private void UpdateScores() {
        score1Text.text = $"{PlayerPrefs.GetInt("Score1", 0)}";
        score2Text.text = $"{PlayerPrefs.GetInt("Score2", 0)}";
        score3Text.text = $"{PlayerPrefs.GetInt("Score3", 0)}";
    }


}
