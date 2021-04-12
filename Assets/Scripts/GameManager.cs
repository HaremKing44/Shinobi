using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject playerRef;
    [SerializeField] TextMeshProUGUI kunaiText;
    [SerializeField] GameObject[] HealthUIIcon;
    [SerializeField] GameObject coinRef;
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] ParticleSystem coinExplosion;
    [SerializeField] ParticleSystem powerupExplosion;
    [SerializeField] ParticleSystem kunaiExplosion;
    [SerializeField] ParticleSystem finalExplosion;

    //Background Music
    [SerializeField] AudioClip inGameClip;
    [SerializeField] AudioClip inMainMenuClip;
    [SerializeField] AudioClip inGameOverClip;
    [SerializeField] AudioClip inGameFinishClip;

    //GameStates
    [HideInInspector]public bool isMainMenu;
    [HideInInspector] public bool isInGame;
    [HideInInspector] public bool isGameOver;
    [HideInInspector] public bool isGameFinish;
    bool isInstruction;
    bool isAbout;

    //UI Buttons
    [SerializeField] Button StartButton;
    [SerializeField] Button RestartButton;
    [SerializeField] Button VictoryButton;
    [SerializeField] Button InstructionButton;
    [SerializeField] Button AboutButton;
    [SerializeField] Button QuitButton;
    [SerializeField] Button BackButton;

    //UI
    [SerializeField] GameObject MainMenuUI;
    [SerializeField] GameObject GameUI;
    [SerializeField] GameObject GameOverUI;
    [SerializeField] GameObject VictoryUI;
    [SerializeField] GameObject InstructionUI;
    [SerializeField] GameObject AboutUI;

    public AudioClip coinCollectAudio;

    AudioSource mainAudioSource;
    int KunaiHeld;
    int coins = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Set GameStates
        isMainMenu = true;
        isInGame = false;
        isGameOver = false;
        isGameFinish = false;
        isInstruction = false;
        isAbout = false;

        mainAudioSource = gameObject.GetComponent<AudioSource>();

        //Set listener on UI Button Clicks
        StartButton.onClick.AddListener(StartGame);
        RestartButton.onClick.AddListener(RestartGame);
        VictoryButton.onClick.AddListener(RestartGame);
        InstructionButton.onClick.AddListener(InstructionMenu);
        AboutButton.onClick.AddListener(AboutMenu);
        BackButton.onClick.AddListener(GoBackToMainMenu);
        QuitButton.onClick.AddListener(QuitGame);

        if (isMainMenu)
        {
            MainMenuUI.SetActive(true);
            GameUI.SetActive(false);
            GameOverUI.SetActive(false);
            VictoryUI.SetActive(false);
            InstructionUI.SetActive(false);
            AboutUI.SetActive(false);
            BackButton.gameObject.SetActive(false);

            mainAudioSource.clip = inMainMenuClip;
            mainAudioSource.loop = true;
            mainAudioSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        MainMenuUI.SetActive(false);
        GameUI.SetActive(true);
        GameOverUI.SetActive(false);
        VictoryUI.SetActive(false);

        UpdateKunaiGUI();
        UpdateHealthGUI();

        //Set GameStates
        isMainMenu = false;
        isInGame = true;
        isGameOver = false;
        isGameFinish = false;

        //Stop Previous MusicClip and Start a new one
        mainAudioSource.Stop();
        mainAudioSource.clip = inGameClip;
        mainAudioSource.loop = true;
        mainAudioSource.Play();
    }

    public void GameOver()
    {
        MainMenuUI.SetActive(false);
        GameUI.SetActive(false);
        GameOverUI.SetActive(true);
        VictoryUI.SetActive(false);

        //Set GameStates
        isMainMenu = false;
        isInGame = false;
        isGameOver = true;
        isGameFinish = false;

        //Stop Previous MusicClip and Start a new one
        mainAudioSource.Stop();
        mainAudioSource.clip = inGameOverClip;
        mainAudioSource.loop = true;
        mainAudioSource.Play();
    }

    public void Victory()
    {
        MainMenuUI.SetActive(false);
        GameUI.SetActive(false);
        GameOverUI.SetActive(false);
        VictoryUI.SetActive(true);

        isMainMenu = false;
        isInGame = false;
        isGameOver = false;
        isGameFinish = true;

        mainAudioSource.Stop();
        mainAudioSource.clip = inGameFinishClip;
        mainAudioSource.loop = true;
        mainAudioSource.Play();
    }

    void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    void InstructionMenu()
    {
        isInstruction = true;
        MainMenuUI.SetActive(false);
        InstructionUI.SetActive(true);
        BackButton.gameObject.SetActive(true);
        //BackButton.onClick.AddListener(GoBackToMainMenu);
    }

    void AboutMenu()
    {
        isAbout = true;
        MainMenuUI.SetActive(false);
        AboutUI.SetActive(true);
        BackButton.gameObject.SetActive(true);
        //BackButton.onClick.AddListener(GoBackToMainMenu);
    }

    void GoBackToMainMenu()
    {
        if(isInstruction)
        {
            InstructionUI.SetActive(false);
            isInstruction = false;
        }

        if(isAbout)
        {
            AboutUI.SetActive(false);
            isAbout = false;
        }

        MainMenuUI.SetActive(true);
        BackButton.gameObject.SetActive(false);
    }

    public void UpdateKunaiGUI()
    {
        KunaiHeld = playerRef.GetComponent<PlayerController>().KunaiHeld;
        kunaiText.text = KunaiHeld.ToString();
    }

    public void UpdateHealthGUI()
    {
        if(HealthUIIcon.Length != 0)
        {
            for(int i = 0; i < HealthUIIcon.Length; i ++)
            {
                if(i < playerRef.GetComponent<PlayerController>().healthPoint)
                {
                    HealthUIIcon[i].SetActive(true);
                }
                else
                {
                    HealthUIIcon[i].SetActive(false);
                }
            }
        }
    }

    public void UpdateCointGUI()
    {
        coins++;
        coinText.text = "Coins: " + coins;
        mainAudioSource.PlayOneShot(coinCollectAudio);
        Instantiate(coinExplosion, playerRef.transform).Play();
    }

    public void EnableDoubleJump()
    {
        Instantiate(powerupExplosion, playerRef.transform).Play();
    }

    public void KunaiPick()
    {
        mainAudioSource.PlayOneShot(coinCollectAudio);
        Instantiate(kunaiExplosion, playerRef.transform).Play();
        UpdateKunaiGUI();
    }

    public void FinishGame()
    {
        Victory();
        Instantiate(finalExplosion, playerRef.transform).Play();
    }

    void QuitGame()
    {
        Application.Quit();
    }
}
