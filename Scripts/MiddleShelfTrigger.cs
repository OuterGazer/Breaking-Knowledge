using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleShelfTrigger : MonoBehaviour
{
    private LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        this.levelManager = GameObject.FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
            if(this.gameObject.name == "Left Shelf Trigger Middle Up")
                this.levelManager.SetIsBookOnLeftMiddleShelf1(true);
            else if(this.gameObject.name == "Left Shelf Trigger Middle Up 2")
                this.levelManager.SetIsBookOnLeftMiddleShelf2(true);
            else if (this.gameObject.name == "Right Shelf Trigger Middle Up")
                this.levelManager.SetIsBookOnRightMiddleShelf1(true);
            else if (this.gameObject.name == "Right Shelf Trigger Middle Up 2")
                this.levelManager.SetIsBookOnRightMiddleShelf2(true);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            if (this.gameObject.name == "Left Shelf Trigger Middle Up")
                this.levelManager.SetIsBookOnLeftMiddleShelf1(false);
            else if (this.gameObject.name == "Left Shelf Trigger Middle Up 2")
                this.levelManager.SetIsBookOnLeftMiddleShelf2(false);
            else if (this.gameObject.name == "Right Shelf Trigger Middle Up")
                this.levelManager.SetIsBookOnRightMiddleShelf1(false);
            else if (this.gameObject.name == "Right Shelf Trigger Middle Up 2")
                this.levelManager.SetIsBookOnRightMiddleShelf2(false);
        }            
    }
}
