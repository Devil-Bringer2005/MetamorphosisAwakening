using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public string gameOverScene;

    public int maxinmumLives = 3;
    public int currentLives = 3;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void ReduceLives()
    {
        currentLives--;
        UIManager.instance.UpdateLivesUI(currentLives);

        if (currentLives == 0)
        {
            if (gameOverScene != null)
            {
                SceneManager.LoadSceneAsync(gameOverScene, LoadSceneMode.Single);
            }
        }
    }
}
