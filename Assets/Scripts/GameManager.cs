using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        DontDestroyOnLoad(transform.gameObject);
    }

    public void OnMouseCatch() {
        SceneManager.LoadScene("DefaultLevelScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
