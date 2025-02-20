using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadTrigger : MonoBehaviour
{
    [SerializeField] private string[] _sceneToLoad;
    [SerializeField] private string[] _sceneToUnload;

    private GameObject _player;
    void Start()
    {
        _player = PlayerManager.instance.player.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            LoadScenes();
            UnloadScenes();
        }
    }

    private void UnloadScenes()
    {
        for (int i = 0; i < _sceneToUnload.Length; i++)
        {
            for (int j = 0; j < SceneManager.sceneCount; j++)
            {
                Scene loadedScene = SceneManager.GetSceneAt(j);
                if (loadedScene.name == _sceneToUnload[i])
                {
                    SceneManager.UnloadSceneAsync(_sceneToUnload[i]);
                }
            }
        }
    }

    private void LoadScenes()
    {
        for (int i = 0; i < _sceneToLoad.Length; i++)
        {
            bool isSceneLoaded = false;
            for (int j = 0; j < SceneManager.sceneCount; j++)
            {
                Scene loadedScene = SceneManager.GetSceneAt(j);
                Debug.Log(loadedScene.name);
                //Debug.Log(_sceneToLoad[j].SceneName);
                if (loadedScene.name == _sceneToLoad[i])
                {
                    isSceneLoaded = true;
                    break;
                }
            }
            if (!isSceneLoaded)
            {
                SceneManager.LoadSceneAsync(_sceneToLoad[i], LoadSceneMode.Additive);
            }
        }
    }
}
