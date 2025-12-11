using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScript : MonoBehaviour
{
    [SerializeField] GameObject blackScreen;
    [SerializeField] GameObject health;
    [SerializeField] GameObject player;
    [SerializeField] CameraFollowScript cameraFollowScript;
    TextMeshProUGUI gameOverText;
    [SerializeField] TextMeshProUGUI retryText;
    [SerializeField] Image retryButton;
    [SerializeField] TextMeshProUGUI quitText;
    [SerializeField] Image quitButton;

    Animator animator;
    // Start is called before the first frame update
    //void Start()
    //{
    //    gameOverAnimator = GetComponent<Animator>();
    //    gameObject.SetActive(false);
    //}

    private void FixedUpdate()
    {

    }

    private void OnEnable()
    {
        gameOverText = GetComponent<TextMeshProUGUI>();
        Color c = gameOverText.color;
        c.a = 0;
        gameOverText.color = c;
        retryText.color = c;
        retryButton.color = c;
        quitText.color = c;
        quitButton.color = c;
        animator = GetComponent<Animator>();
        cameraFollowScript.GameOver();

        Vector3 pos = player.transform.position;
        pos.z = 0;
        blackScreen.transform.position = pos;
        StartCoroutine(PlayWin());
    }

    IEnumerator PlayWin()
    {
        blackScreen.SetActive(true);
        health.SetActive(false);
        blackScreen.GetComponent<Animator>().Play("enable_blackscreen");
        DestroyAllEnemies();
        yield return new WaitForSecondsRealtime(1f);
        animator.Play("winscreen_enable");
    }

    void DestroyAllEnemies()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject obj in gameObjects)
        {
            Destroy(obj);
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
    #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
    #elif UNITY_STANDALONE
                Application.Quit();
    #endif
    }
}
