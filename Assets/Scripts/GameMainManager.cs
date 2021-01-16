using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameMainManager : MonoBehaviour
{
    public static GameMainManager Instance { get; private set; }

    public GameObject cat;
    public GameObject mouse;

    public Vector3 catStartPos;
    public Vector3 mouseStartPos;


    public Text fpsText;
    public Text timeText;
    public Text chaserScoreText;
    public Text evaderScoreText;

    private int chaserScore = 0;
    private int evaderScore = 0;

    private const float TIMER_START_TIME = 12.0f;
    private float time;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null){
            Instance = this;

            timeText.text = "Time: " + time;
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        RestartGame();
    }

    public void OnMouseCatch() {
        chaserScore += 1;
        RestartGame();
    }

    private void OnTimeout() {
        evaderScore += 1;
        RestartGame();
    }

    private void RestartGame() {

        //Reset objects animations
        cat.GetComponentInChildren<AvatarController>().OnGameReset();
        mouse.GetComponentInChildren<AvatarController>().OnGameReset();

        //Set objects position
        cat.transform.position = catStartPos;
        cat.transform.rotation = Quaternion.identity;

        mouse.transform.position = mouseStartPos;
        mouse.transform.rotation = Quaternion.identity;

        chaserScoreText.text = "Chaser: " + chaserScore;
        evaderScoreText.text = "Evader: " + evaderScore;

        //Set timer
        time = TIMER_START_TIME;
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;

        int currentFps = (int)(1f / Time.unscaledDeltaTime);
        fpsText.text = "fps: "+currentFps.ToString();
        timeText.text = "Time: " + Math.Round(time,2);
        if (time < 0)
            OnTimeout();
    }
}
