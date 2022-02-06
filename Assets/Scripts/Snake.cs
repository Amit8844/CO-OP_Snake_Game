﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Snake : MonoBehaviour
{
    private static long intialLen = 1;
    public static long length = intialLen;

    Vector2 direction = Vector2.left;
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
        if (Input.GetKey(KeyCode.D)&& direction!= Vector2.left)
        {
            direction = Vector2.right;
        }
        else if (Input.GetKey(KeyCode.S)&& direction!= Vector2.up)
        {
            direction = Vector2.down;
        }
        else if (Input.GetKey(KeyCode.A)&& direction!= Vector2.right)
        {
            direction = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.W)&& direction!= Vector2.down)
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
    private void ScreenWrap(Collider2D collision)
    {
        if ( direction == Vector2.right )
        {
            this.transform.position = new Vector3((-1 * collision.transform.position.x) + 1, this.transform.position.y, 0f);
        }
        else if (direction == Vector2.left)
        {
            this.transform.position = new Vector3((-1 * collision.transform.position.x) -1, this.transform.position.y, 0f);
        }
        else if (direction == Vector2.up)
        {
            this.transform.position = new Vector3(this.transform.position.x ,(-1 * collision.transform.position.y)+1, 0f);
        }
        else if (direction == Vector2.down)
        {
            this.transform.position = new Vector3(this.transform.position.x, (-1 * collision.transform.position.y)-1, 0f);
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("OnTriggerEnter2D");
        //CAN BE IMPROVE WITH TAG PROPERTY
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
            //TODO:YOU LOSE SCREEN
        }
        else if (collision.tag.Contains("Tail"))
         {
             Instantiate(deathParticles,transform.position,Quaternion.identity);
            GameController.FailGame();
            //TODO:YOU LOSE SCREEN
        }
        else if (collision.tag == "wall")
        {
            ScreenWrap(collision);
        }

       
    }
}
