using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;

    Animator anim;
    SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        sprite.flipX = true;
        anim.SetLayerWeight(0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        Walk();
    }

    void Walk()
    {
        if (StateManager.Instance.state == State.AdvMove)
        {
            if (Input.GetKey("q") || Input.GetKey("a") || Input.GetKey("left"))
            {
                anim.SetLayerWeight(0, 0);
                anim.SetLayerWeight(1, 1);
                sprite.flipX = false;
                if (transform.position.x > -155)
                {
                    transform.position += speed * Time.deltaTime * Vector3.left;
                }
            }
            else if (Input.GetKey("d") || Input.GetKey("right"))
            {
                anim.SetLayerWeight(0, 0);
                anim.SetLayerWeight(1, 1);
                sprite.flipX = true;
                if (transform.position.x < 155)
                {
                    transform.position += speed * Time.deltaTime * Vector3.right;
                }
            }
            else
            {
                anim.SetLayerWeight(0, 1);
                anim.SetLayerWeight(1, 0);
            }
        } 
        else if (StateManager.Instance.state == State.AdvDialogue || StateManager.Instance.state == State.Fight)
        {
            anim.SetLayerWeight(0, 1);
            anim.SetLayerWeight(1, 0);
        }
            
    }
}
