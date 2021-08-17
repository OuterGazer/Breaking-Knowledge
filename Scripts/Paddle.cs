using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    //Configuration Parameters (things we need to know before the game)
    
    private float posX;
    private float posY;
    private float resolutionWidth = 19.2f;
    //[SerializeField] float bumpAngle = 45.0f;
    private Vector3 rot_180;
    private Vector3 rot_210;
    private Vector3 rot_150;
    private Vector3 rot_0;
    private Vector3 rot_30;
    private Vector3 rot_330;


    //Cached Component References (references to other game objects or components of game objects)
    private LevelManager levelManager;
    private float paddleBounds;
    private Rigidbody2D paddleRb;


    //State variables (to keep track of the variables that govern states)


    // Start is called before the first frame update
    void Start()
    {
        this.levelManager = GameObject.FindObjectOfType<LevelManager>();

        this.rot_180 = new Vector3(0.0f, 0.0f, 180.0f);
        this.rot_210 = new Vector3(0.0f, 0.0f, 210.0f);
        this.rot_150 = new Vector3(0.0f, 0.0f, 150.0f);
        this.rot_0 = new Vector3(0.0f, 0.0f, 0.0f);
        this.rot_30 = new Vector3(0.0f, 0.0f, 30.0f);
        this.rot_330 = new Vector3(0.0f, 0.0f, 330.0f);

        this.paddleBounds = this.gameObject.GetComponent<SpriteRenderer>().sprite.bounds.extents.x;

        this.paddleRb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.levelManager.IsGamePaused)
        {
            this.PaddleMovement();

            this.PaddleBump();
        }
    }

    private void PaddleBump()
    {
        if(this.gameObject.transform.rotation.eulerAngles == this.rot_180 ||
           this.gameObject.transform.rotation.eulerAngles == this.rot_210 ||
           this.gameObject.transform.rotation.eulerAngles == this.rot_150)
        {
            this.ClickToRotatePaddle(180.0f, 210.0f, 150.0f);
        }
            

        else if(this.gameObject.transform.rotation.eulerAngles == this.rot_0||
                this.gameObject.transform.rotation.eulerAngles == this.rot_30 ||
                this.gameObject.transform.rotation.eulerAngles == this.rot_330)
            this.ClickToRotatePaddle(0.0f, 30.0f, 330.0f);
    }

    private void ClickToRotatePaddle(float initialRot, float leftClickRot, float rightClickRot)
    {
        if (Input.GetMouseButtonDown(0)) //rotates paddle to the left while left clicking
        {
            this.gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, leftClickRot);
        }
        else if (Input.GetMouseButtonDown(1)) //rotates paddle to the right while right clicking
        {
            this.gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rightClickRot);
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            this.gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, initialRot); //returns paddle to position when mouse buttons up
        }
        else if (Input.GetMouseButtonUp(1))
        {
            this.gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, initialRot);
        }
    }

    private void PaddleMovement()
    {
        this.posY = this.gameObject.transform.position.y; //Keep Y position always constant

        this.posX = (Input.mousePosition.x / Screen.width) * this.resolutionWidth; //The X position equals mouse position, converted into world units

        this.posX = Mathf.Clamp(this.posX, this.paddleBounds, (this.resolutionWidth - this.paddleBounds)); //the x-position is clamped within the level bounds

        this.gameObject.transform.position = new Vector2(this.posX, this.posY); //The line that actually moves the paddle left and right
    }
}
