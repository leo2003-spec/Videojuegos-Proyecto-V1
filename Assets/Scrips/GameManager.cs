using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Canvas canvas;
    public TextMeshProUGUI coinText;

    private int vidas = 3;
    private int monedas = 0;
    private int totalDamage = 0;
    public float tiempo;

    public int MonedasTotales { get { return monedas; } }
    public int TotalDamage { get { return totalDamage; } }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PerderVida()
    {
        vidas--;
        if (vidas <= 0)
        {
            SceneManager.LoadScene(2);
        }
        canvas.DesactivarVida(vidas);
    }

    public bool RecuperarVida()
    {
        if (vidas >= 3) return false;

        vidas++;
        canvas.ActivarVida(vidas);
        return true;
    }

    public void AddCoin(int valor)
    {
        monedas += valor;
        if (coinText != null)
        {
            coinText.text = ":0" + monedas.ToString();
        }
        else
        {
            Debug.LogWarning("coinText no está asignado en GameManager.");
        }
    }

    public void AddDamage(int damage)
    {
        totalDamage += damage;
    }

    public int GetMonedasTotales()
    {
        return monedas;
    }

    public void CompleteLevel()
    {
        SceneManager.LoadScene("score");
    }

    private void Update()
    {
        tiempo += Time.deltaTime;
    }

    public int GetTotalDamage()
    {
        return totalDamage;
    }

    public void ResetMonedas()
    {
        monedas = 0;
        if (coinText != null)
        {
            coinText.text = ":0" + monedas.ToString();
        }
        else
        {
            Debug.LogWarning("coinText no está asignado en GameManager.");
        }
    }
}
