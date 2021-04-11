using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    int KunaiHeld;
    int coins = -1;

    // Start is called before the first frame update
    void Start()
    {
        UpdateKunaiGUI();
        UpdateHealthGUI();
        UpdateCointGUI();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        Instantiate(coinExplosion, playerRef.transform).Play();
    }

    public void EnableDoubleJump()
    {
        Instantiate(powerupExplosion, playerRef.transform).Play();
    }

    public void KunaiPick()
    {
        Instantiate(kunaiExplosion, playerRef.transform).Play();
        UpdateKunaiGUI();
    }

    public void FinishGame()
    {
        Instantiate(finalExplosion, playerRef.transform).Play();
    }
}
