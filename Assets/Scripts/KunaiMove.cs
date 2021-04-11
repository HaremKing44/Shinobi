using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KunaiMove : MonoBehaviour
{
    [SerializeField] float moveSpeed = 50f;

    Vector3 SpawnLocation;

    // Start is called before the first frame update
    void Start()
    {
        SpawnLocation = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * Time.deltaTime * moveSpeed);

        if(Mathf.Abs(transform.position.x - SpawnLocation.x) > 100f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(gameObject.name == "KunaiEnemy(Clone)")
        {
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<PlayerController>().AppplyDamage();
                Destroy(gameObject);
            }
        }
        
        if(gameObject.name == "Kunai(Clone)")
        {
            if (collision.CompareTag("Enemy"))
            {
                collision.GetComponent<MyEnemy>().AppplyDamage();
                Destroy(gameObject);
            }
        }
    }
}
