using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private ItemManager targetObject;
    

    private void Start()
    {
        this.targetObject = GameObject.FindObjectOfType<ItemManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            this.ChooseItem();
            GameObject.Destroy(this.gameObject);
        }
        else if (collision.gameObject.name == "Loose Trigger")
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    private void ChooseItem()
    {
        switch (this.gameObject.name)
        {
            case "Expand Ball(Clone)":
                this.targetObject.SendMessage("ExpandBall");
                break;

            case "Shrink Ball(Clone)":
                this.targetObject.SendMessage("ShrinkBall");
                break;

            case "Fast Ball(Clone)":
                this.targetObject.SendMessage("FastBall");
                break;

            case "Slow Ball(Clone)":
                this.targetObject.SendMessage("SlowBall");
                break;

            case "Expand Paddle(Clone)":
                this.targetObject.SendMessage("ExpandPaddle");
                break;

            case "Shrink Paddle(Clone)":
                this.targetObject.SendMessage("ShrinkPaddle");
                break;

            case "Flip Paddle(Clone)":
                this.targetObject.SendMessage("FlipPaddle");
                break;

            case "Life Down(Clone)":
                this.targetObject.SendMessage("LooseALife");
                break;

            case "Gum Ball(Clone)":
                this.targetObject.SendMessage("BouncyBall");
                break;

            case "Extra Balls(Clone)":
                this.targetObject.SendMessage("ExtraBalls");
                break;
        }
        
    }
}
