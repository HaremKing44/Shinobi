using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] GameObject playerRef;
    [SerializeField] bool isUsedForKunai;
    [SerializeField] bool isFinalPick;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isUsedForKunai)
        {
            playerRef.GetComponent<PlayerController>().KunaiHeld += 3;
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().KunaiPick();
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Player") && isFinalPick)
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().FinishGame();
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Player"))
        {
            playerRef.GetComponent<PlayerController>().hasDoubleJumpAbility = true;
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().EnableDoubleJump();
            Destroy(gameObject);
        }
    }
}
