using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Snake2 : MonoBehaviour
{ private static long intialLen = 1;
    public static long length = intialLen;

    Vector2 direction = Vector2.right;
    List<Transform> tail = new List<Transform>();

    bool ate = true;

    public GameObject tailPrefab;
    public ParticleSystem deathParticles;

    // Start is called before the first frame update
    void Start()
    {
        length = intialLen;
        InvokeRepeating(nameof(this.Move), 0.1f,0.1f);

       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            direction = Vector2.right;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            direction = Vector2.down;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            direction = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            direction = Vector2.up;
        }

    }

    private void Move()
    {
        if (GameController.isPaused) return;

        Vector2 currentPos = transform.position;
        transform.Translate(direction);

        if (ate)
        {
            var newPart = (GameObject)Instantiate(tailPrefab, currentPos, Quaternion.identity);
            tail.Insert(0, newPart.transform);
            length++;
            ate = false;
        }
        else if (tail.Count != 0)
        {
            tail.Last().position = currentPos;
            tail.Insert(0, tail.Last());
            tail.RemoveAt(tail.Count - 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("OnTriggerEnter2D");
        
        if (collision.name.StartsWith("Food"))
        {
            ate = true;
            Destroy(collision.gameObject);
            SpawnFood.EatOne();
        }
        else if(collision.tag.Contains("Border"))
        {
            Instantiate(deathParticles,transform.position,Quaternion.identity);
            GameController.FailGame();
            
        }
        else if (collision.tag.Contains("Tail"))
         {
             Instantiate(deathParticles,transform.position,Quaternion.identity);
            GameController.FailGame();
            
        }
    }
   
}
