using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField]
    private Text multiplierLabel;
    [SerializeField]
    private Text scoreLabel;

    private float _score;
    public float Score
    {
        get => _score;
        private set
        {
            _score = value;
            scoreLabel.text = $"Score: {_score}";
        }
    }

    public float Multiplier => gameManager.CurrentSpeed / 25;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Score = 0;
    }

    void OnGUI()
    {
        multiplierLabel.text = $"{Multiplier}X Multiplier!";
    }

    public void AddScore(float amount)
    {
        Score += amount * Multiplier;
    }
}
