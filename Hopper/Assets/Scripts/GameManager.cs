using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    private int score;
    private int lives;
    private Home[] homes;
    private Frog frog;
    private int nextScenetoLoad;
    private int time;
    public GameObject gameOver;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI timeText;


    private void Awake()
    {
        homes = FindObjectsOfType<Home>();
        frog = FindObjectOfType<Frog>();
    }

    private void Start()
    {
        NewGame();
    }

    private void NewGame()
    {
        gameOver.SetActive(false);
        SetScore(0);
        SetLives(3);
        NewLevel();
        nextScenetoLoad = SceneManager.GetActiveScene().buildIndex + 1;
    }

    private void NewLevel()
    {
        for (int i = 0; i < homes.Length; i++)
        {
            homes[i].enabled = false;
        }
        Respawn();
    }

    private IEnumerator PlayAgain()
    {
        bool playAgain = false;
        while(!playAgain)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                playAgain = true;
            }
            yield return null;
        }
        NewGame();
    }

    private void GameOver()
    {
        frog.gameObject.SetActive(false);
        gameOver.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(PlayAgain());

    }

    public void Died()
    {
        SetLives(lives - 1);
        if (lives > 0)
        {
            Invoke(nameof(Respawn), 1f);
        }
        else
        {
            Invoke(nameof(GameOver), 1f);
        }
    }

    public void HomeOccupied()
    {
        frog.gameObject.SetActive(false);

        int bonusPoints = time * 20;
        SetScore(score + bonusPoints + 50);

        if (Cleared())
        {
            SetScore(score + 1000);
            SetLives(lives + 1);
            SceneManager.LoadScene(nextScenetoLoad);
            Invoke(nameof(NewLevel), 1f);
        }
        else
        {
            Invoke(nameof(Respawn), 1f);
        }
    }

    private bool Cleared()
    {
        for (int i = 0; i < homes.Length; i++)
        {
            if (!homes[i].enabled)
            {
                return false;
            }
        }

        return true;
    }

    private void Respawn()
    {
        frog.Respawn();

        StopAllCoroutines();
        StartCoroutine(Timer(30));
    }

    private IEnumerator Timer(int duration)
    {
        time = duration;
        while (time >0)
        {
            yield return new WaitForSeconds(1);
            time--;
            timeText.text = time.ToString();
        }
        frog.Death();
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString();
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text = lives.ToString();
    }
}
