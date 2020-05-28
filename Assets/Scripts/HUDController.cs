using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUDController : MonoBehaviour
{
    public Slider HealthSlider;
    public Text PointsText;
    public Text TimeText;
    public GameObject panelGameover;
    public Text GameoverText;

    private int health;
    private int points;
    private double time;

    private string message;

    private void Start()
    {
        health = 100;
        points = 0;
        time = 120;
    }

    private void Update()
    {
        time -= Time.deltaTime;

        if (CheckGameOver())
        {
            panelGameover.SetActive(true);

            if (time <= 0)
            {
                message = "Parabéns! Você salvou o Meio Ambiente!";
                GameoverText.text = message + "\n\nPontuação: " + this.points;
            }
            else
            {
                message = "Que pena, dessa vez você não conseguiu salvar o Meio Ambiente.";
                GameoverText.text = message + "\n\nPontuação: " + this.points + "\nTempo restante: " + this.time;
            }

            Time.timeScale = 0;
        }


        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            HealthSlider.value = health;
            PointsText.text = points.ToString();
            TimeText.text = System.Math.Round(time, 1).ToString();
        }
    }

    private bool CheckGameOver()
    {
        return health <= 0 || time <= 0;
    }

    public void TakeDamage(int damage)
    {
        this.health -= damage;
    }

    public void AddPoints(int points)
    {
        this.points += points;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
