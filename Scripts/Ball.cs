using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    //Configuration Parameters (things we need to know before the game)
    private AudioSource forBallSounds;
    [SerializeField] AudioClip ballBounceSfx;
    [SerializeField] AudioClip bouncyBallBounceSfx;

    private TrailRenderer ballTrail;

    private float posX;
    private float posY;
    [SerializeField] float startingImpulse = 10.0f;
    [SerializeField] float slowBallSpeed = 10.0f;
    public float SlowBallSpeed { get { return this.slowBallSpeed; } }
    [SerializeField] float standardSpeed = 15.0f;
    public float StandardSpeed { get { return this.standardSpeed; } }
    [SerializeField] float fastSpeed = 18.0f;
    [SerializeField] float maxSpeed = 20.0f;
    public float MaxSpeed { get { return this.maxSpeed; } }
    [SerializeField] float maxSpeedWithPower = 25.0f;
    public float MaxSpeedWithPower { get { return this.maxSpeedWithPower; } }
    [SerializeField] float startingMinAngle = 10.0f;
    [SerializeField] float startingMaxAngle = 45.0f;
    [SerializeField] float minBounceAngle = 10.0f;
    [SerializeField] float bouncyBallBounceAngle = 90.0f;

    [SerializeField] int powerLevel = 1;
    public int PowerLevel
    {
        get { return this.powerLevel; }
    }
    public void SetBallPowerLevel(int inPowerLevel)
    {
        this.powerLevel = inPowerLevel;
    }
    public void SetBallSpeed(string speedLevel)
    {
        switch (speedLevel)
        {
            case "Standard":
                this.ballRb.velocity = Vector2.ClampMagnitude(this.ballRb.velocity, this.standardSpeed);
                break;

            case "Fast":
                this.ballRb.velocity = this.ballRb.velocity.normalized * this.maxSpeedWithPower;
                break;

            case "Slow":
                this.ballRb.velocity = this.ballRb.velocity.normalized * this.slowBallSpeed;
                break;
        }
    }


    //Cached Component References (references to other game objects or components of game objects)
    private GameObject paddle;
    private Rigidbody2D ballRb;
    private ItemManager managePowerUps;


    //State variables (to keep track of the variables that govern states)
    private bool hasGameStarted = false;
    public bool HasGameStarted
    {
        get { return this.hasGameStarted; }
        set { this.hasGameStarted = value; }
    }

    private bool isBouncyBallEnabled;
    public bool IsBouncyBallEnabled
    {
        get { return this.isBouncyBallEnabled; }
        set { this.isBouncyBallEnabled = value; }
    }


    // Start is called before the first frame update
    void Start()
    {
        this.forBallSounds = this.gameObject.GetComponent<AudioSource>();

        this.ballTrail = this.gameObject.GetComponent<TrailRenderer>();
        this.ballTrail.emitting = false;

        this.paddle = GameObject.FindObjectOfType<Paddle>().gameObject;
        this.ballRb = this.gameObject.GetComponent<Rigidbody2D>();
        this.managePowerUps = FindObjectOfType<ItemManager>();

        Physics2D.IgnoreLayerCollision(8, 9, true); //The ball ignores the colliders on the shelfs used to stop the books from falling
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.hasGameStarted)
            this.MovementStartingPosition();

        if (!this.isBouncyBallEnabled)
            this.CheckMinBounce();

        this.CheckBallSpeedWithinLimits();

        this.CheckBallPowerLevel();

        this.ManageBallTrail();
    }

    Color orange = new Color(1.0f, 0.48f, 0.016f);
    private void ManageBallTrail()
    {
        if (this.ballRb.velocity.magnitude <= this.standardSpeed)
        {
            this.ballTrail.emitting = false;
        }
        else if (this.ballRb.velocity.magnitude >= this.standardSpeed)
        {            
            this.ballTrail.emitting = true;
        }        
        
        
        if (this.ballRb.velocity.magnitude >= this.maxSpeedWithPower)
        {
            this.ballTrail.startColor = Color.red;
        }
        else if (this.ballRb.velocity.magnitude >= this.maxSpeed)
        {
            this.ballTrail.startColor = this.orange;
        }
        else if (this.ballRb.velocity.magnitude <= this.fastSpeed)
        {
            this.ballTrail.startColor = Color.yellow;
        }
    }

    private void CheckBallPowerLevel()
    {
        if (this.ballRb.velocity.magnitude >= this.maxSpeedWithPower)
            this.powerLevel = 3;
        else if (this.ballRb.velocity.magnitude >= this.maxSpeed)
            this.powerLevel = 2;
        else if (this.ballRb.velocity.magnitude <= this.standardSpeed)// &&
                 //(!this.managePowerUps.IsFastBallEnabled && !this.managePowerUps.IsSlowBallEnabled))
            this.powerLevel = 1;
        /*else if (this.ballRb.velocity.magnitude <= this.standardSpeed &&
                 this.managePowerUps.IsFastBallEnabled)
            this.powerLevel = 2;*/ //I got this piece of code wrong, I wanted to increase the power if the ball was made big!
    }

    private void CheckBallSpeedWithinLimits()
    {
        //Avoid that the ball either goes super slow or super fast due to physics' shenanigans,and taking in account if fast ball or slow ball is active
        if (((this.ballRb.velocity.magnitude < this.standardSpeed) || (this.ballRb.velocity.magnitude > this.maxSpeed)) &&
            (!this.managePowerUps.IsFastBallEnabled && !this.managePowerUps.IsSlowBallEnabled))
        {
            this.ballRb.velocity = this.ballRb.velocity.normalized * this.standardSpeed; //it's the same logic as ClampMagnitude() but increasing the vector to a minimum value
        }            
        else if (((this.ballRb.velocity.magnitude < this.standardSpeed) || (this.ballRb.velocity.magnitude > this.maxSpeedWithPower)) &&
            this.managePowerUps.IsFastBallEnabled)
        {
            this.ballRb.velocity = this.ballRb.velocity.normalized * this.maxSpeedWithPower;
        }
        else if (((this.ballRb.velocity.magnitude < this.slowBallSpeed) || (this.ballRb.velocity.magnitude > this.slowBallSpeed)) &&
            this.managePowerUps.IsSlowBallEnabled)
        {
            this.ballRb.velocity = this.ballRb.velocity.normalized * this.slowBallSpeed;
        }
    }

    /// <summary>
    /// Checks that the ball always bounce at a minimum angle to avoid perfect horizontal/vertical loops depending on the quadrant the local velocity vector of the RB is in.
    /// </summary>
    private void CheckMinBounce()
    {
        if (this.ballRb.velocity.x < 0)
        {
            float ballMoveAngle = Vector2.Angle(this.ballRb.velocity, Vector2.left);

            if(ballMoveAngle < this.minBounceAngle)
            {
                if (this.ballRb.velocity.y <= 0)
                {
                    this.AddMinAngle();
                }
                else if (this.ballRb.velocity.y >= 0)
                {
                    this.SubtractMinAngle();
                }
            }
        }
        if (this.ballRb.velocity.x > 0)
        {
            float ballMoveAngle = Vector2.Angle(this.ballRb.velocity, Vector2.right);

            if(ballMoveAngle < this.minBounceAngle)
            {
                if (this.ballRb.velocity.y <= 0)
                {
                    this.SubtractMinAngle();
                }
                else if (this.ballRb.velocity.y >= 0)
                {
                    this.AddMinAngle();
                }
            }
        }
        if (this.ballRb.velocity.y > 0)
        {
            float ballMoveAngle = Vector2.Angle(this.ballRb.velocity, Vector2.up);

            if(ballMoveAngle < this.minBounceAngle)
            {
                if (this.ballRb.velocity.x <= 0)
                {
                    this.AddMinAngle();
                }
                else if (this.ballRb.velocity.x >= 0)
                {
                    this.SubtractMinAngle();
                }
            }
        }
        if (this.ballRb.velocity.y < 0)
        {
            float ballMoveAngle = Vector2.Angle(this.ballRb.velocity, Vector2.down);

            if(ballMoveAngle < this.minBounceAngle)
            {
                if (this.ballRb.velocity.x >= 0)
                {
                    this.SubtractMinAngle();
                }
                else if (this.ballRb.velocity.x <= 0)
                {
                    this.AddMinAngle();
                }
            }
        }
    }

    private void SubtractMinAngle()
    {
        this.ballRb.velocity = Quaternion.Euler(0, 0, -this.minBounceAngle) * this.ballRb.velocity;
    }

    private void AddMinAngle()
    {
        this.ballRb.velocity = Quaternion.Euler(0, 0, this.minBounceAngle) * this.ballRb.velocity;
    }

    private void MovementStartingPosition()
    {
        this.ballRb.velocity = new Vector2(0, 0); //When the ball goes through the floor and automatically is put on the paddle, it still "remembers" its old velocity

        this.posY = 1.08f;/*this.paddle.transform.position.y +
                        this.paddle.GetComponent<Collider2D>().bounds.extents.y +
                        this.gameObject.GetComponent<Collider2D>().bounds.extents.y;*///0.757f; //If player hasn't clicked, keep the ball always slightly above the paddle so colliders don't touch
        this.posX = this.paddle.transform.position.x;
        this.gameObject.transform.position = new Vector2(this.posX, this.posY);

        if (Input.GetMouseButtonDown(0) ||
            Input.GetMouseButtonDown(1))
        {
            //left or right click to impulse the ball vertically
            this.ballRb.AddForce(new Vector2(0, this.startingImpulse),
                                 ForceMode2D.Impulse);

            //if right or left click, rotate the vertical velocity vector by a random angle up to 45 degrees to the appropriate side
            if (Input.GetMouseButtonDown(0))
                this.ballRb.velocity = Quaternion.Euler(0, 0, Random.Range(this.startingMinAngle, this.startingMaxAngle)) * this.ballRb.velocity;
            else if(Input.GetMouseButtonDown(1))
                this.ballRb.velocity = Quaternion.Euler(0, 0, Random.Range(-this.startingMinAngle, -this.startingMaxAngle)) * this.ballRb.velocity;

            this.hasGameStarted = true; //prevents this piece of code from running once the player has clicked to start the game
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If bouncy ball is enabled, make crazy bounces
        if (this.isBouncyBallEnabled)
        {
            this.ballRb.velocity = Quaternion.Euler(0, 0, Random.Range(-this.bouncyBallBounceAngle, this.bouncyBallBounceAngle)) * this.ballRb.velocity;
            this.Play_SFX(this.bouncyBallBounceSfx);
            return;
        }
        else
        {
            this.Play_SFX(this.ballBounceSfx);
        }

        if (collision.gameObject.tag == "Player" && this.hasGameStarted)
        {
            if ((this.paddle.transform.rotation.eulerAngles.z == 0.0f) || (this.paddle.transform.rotation.eulerAngles.z == 180.0f))
            {
                if (!this.managePowerUps.IsFastBallEnabled && !this.managePowerUps.IsSlowBallEnabled)
                {
                    this.ballRb.velocity = Vector2.ClampMagnitude(this.ballRb.velocity, this.standardSpeed); //if paddle is horizontal, set ball speed to standard
                }
            }
            else
            {
                //in 2 strokes we should be able to achieve maximum speed of 20.0f velocity magnitude
                if(this.ballRb.velocity.magnitude < this.fastSpeed)
                    this.ballRb.velocity = this.ballRb.velocity.normalized * this.fastSpeed;
                else if(this.ballRb.velocity.magnitude < this.maxSpeed)
                    this.ballRb.velocity = this.ballRb.velocity.normalized * this.maxSpeed;

                //if the fast ball or slow ball power up are NOT enabled, we don't want to limit the speed
                if (!this.managePowerUps.IsFastBallEnabled && !this.managePowerUps.IsSlowBallEnabled)
                    this.ballRb.velocity = Vector2.ClampMagnitude(this.ballRb.velocity, this.maxSpeed); //then clamp it so we don't tresspass a certain max value
                else if (this.managePowerUps.IsFastBallEnabled)
                    this.ballRb.velocity = Vector2.ClampMagnitude(this.ballRb.velocity, this.maxSpeedWithPower);
                else if (this.managePowerUps.IsSlowBallEnabled)
                    this.ballRb.velocity = Vector2.ClampMagnitude(this.ballRb.velocity, this.slowBallSpeed);
            }
        }
    }

    private void Play_SFX(AudioClip clip)
    {
        this.forBallSounds.pitch = Random.Range(0.90f, 1.10f);
        this.forBallSounds.PlayOneShot(clip);
    }
}
