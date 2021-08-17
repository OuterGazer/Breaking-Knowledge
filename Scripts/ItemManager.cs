using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    //Configuration Parameters (things we need to know before the game)
    [SerializeField] GameObject[] itemArray;
    [SerializeField] float itemLastingTime = 10.0f;
    [SerializeField] int spawnEverySeconds = 5;
    private float itemStartingTime;
    


    //Cached Component References (references to other game objects or components of game objects)
    private Ball gameBall;
    [SerializeField] GameObject bouncyBall;
    private Rigidbody2D gameBallRb;
    private Collider2D gameBallCol;
    private SpriteRenderer gameBallSprite;
    private GameObject gamePaddle;
    private LooseCollider toSubtractLifePoints;


    //State variables (to keep track of the variables that govern states)
    private bool isFastBallEnabled = false;
    public bool IsFastBallEnabled { get { return this.isFastBallEnabled; } }
    private bool isSlowBallEnabled = false;
    public bool IsSlowBallEnabled { get { return this.isSlowBallEnabled; } }

    private bool isInstanceCreated = false;

    private bool isItemActive = false;
    public bool IsItemActive
    {
        get { return this.isItemActive; }
        set { this.isItemActive = value; }
    }


    // Start is called before the first frame update
    void Start()
    {
        this.gameBall = GameObject.FindObjectOfType<Ball>();
        this.gameBallRb = GameObject.FindObjectOfType<Ball>().gameObject.GetComponent<Rigidbody2D>();
        this.gameBallCol = GameObject.FindObjectOfType<Ball>().gameObject.GetComponent<Collider2D>();
        this.gameBallSprite = GameObject.FindObjectOfType<Ball>().gameObject.GetComponent<SpriteRenderer>();
        this.gamePaddle = GameObject.Find("Paddle");

        this.toSubtractLifePoints = GameObject.FindObjectOfType<LooseCollider>();
    }

    private void Update()
    {
        if((this.isFastBallEnabled && !this.gameBall.HasGameStarted) || //If the ball is under slow or fast power up, if we loose a life, ball goes inmediatelly to standard speed
            (this.isSlowBallEnabled && !this.gameBall.HasGameStarted))
        {
            this.ReturnBallSpeedToNormal();
        }

        this.SpawnItem();

        if(this.isItemActive && (Time.time >= (this.itemStartingTime + this.itemLastingTime + 7.0f)))
        {
            this.isItemActive = false;
        }
    }

    public void SetItemStartingTime(float time)
    {        
        this.itemStartingTime = time;
    }

    private void SpawnItem()
    {
        if ((Mathf.CeilToInt(Time.time) % this.spawnEverySeconds == 0) && !this.isInstanceCreated && !this.isItemActive && this.gameBall.HasGameStarted)
        {
            this.CreateItemInstance();
        }
        else if(Mathf.CeilToInt(Time.time) % this.spawnEverySeconds != 0)
        {
            this.isInstanceCreated = false;
        }
    }

    private void CreateItemInstance()
    {
        Vector3 rndSpawn = new Vector3(Random.Range(1.0f, 18.0f), 15.0f, -2.0f);

        GameObject.Instantiate<GameObject>(this.itemArray[Random.Range(0, this.itemArray.Length)], rndSpawn, Quaternion.identity);

        
        this.isInstanceCreated = true;
        this.itemStartingTime = Time.time;
        this.isItemActive = true;
    }

    private void ReturnBallSpeedToNormal()
    {
        this.gameBall.SetBallSpeed("Standard");        
        this.isFastBallEnabled = false;
        this.isSlowBallEnabled = false;
    }

    private void ExpandBall()
    {
        this.StartCoroutine(BigBall());
    }
    private IEnumerator BigBall()
    {
        this.gameBall.gameObject.transform.localScale = new Vector3(1.20f, 1.20f, 1.20f);
        //int tempPowerLevel = this.gameBall.PowerLevel;
        //this.gameBall.SetBallPowerLevel(this.gameBall.PowerLevel + 1);

        yield return new WaitForSeconds(this.itemLastingTime);

        this.gameBall.gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        //this.gameBall.SetBallPowerLevel(tempPowerLevel);
    }

    private void ShrinkBall()
    {
        this.StartCoroutine(SmallBall());        
    }
    private IEnumerator SmallBall()
    {
        this.gameBall.gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        //int tempPowerLevel = this.gameBall.PowerLevel;
        //this.gameBall.SetBallPowerLevel(this.gameBall.PowerLevel - 1);

        yield return new WaitForSeconds(this.itemLastingTime);

        this.gameBall.gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        //this.gameBall.SetBallPowerLevel(tempPowerLevel);
    }

    private void FastBall()
    {
        this.StartCoroutine(AddBallSpeed());
    }
    private IEnumerator AddBallSpeed()
    {
        this.gameBall.SetBallSpeed("Fast");        
        this.isFastBallEnabled = true;        

        yield return new WaitForSeconds(this.itemLastingTime);

        this.ReturnBallSpeedToNormal();
    }

    private void SlowBall()
    {
        this.StartCoroutine(SubtractBallSpeed());
    }
    private IEnumerator SubtractBallSpeed()
    {
        this.gameBall.SetBallSpeed("Slow");        
        this.isSlowBallEnabled = true;

        yield return new WaitForSeconds(this.itemLastingTime);

        this.ReturnBallSpeedToNormal();
    }

    private void ExpandPaddle()
    {
        this.StartCoroutine(BigPaddle());
    }
    private IEnumerator BigPaddle()
    {
        this.gamePaddle.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

        yield return new WaitForSeconds(this.itemLastingTime);

        this.gamePaddle.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    private void ShrinkPaddle()
    {
        this.StartCoroutine(SmallPaddle());
    }
    private IEnumerator SmallPaddle()
    {
        this.gamePaddle.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

        yield return new WaitForSeconds(this.itemLastingTime);

        this.gamePaddle.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    private void FlipPaddle()
    {
        this.StartCoroutine(RotatePaddle());
    }
    private IEnumerator RotatePaddle()
    {
        this.gamePaddle.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

        yield return new WaitForSeconds(this.itemLastingTime);

        this.gamePaddle.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
    }

    private void LooseALife()
    {
        this.StartCoroutine(this.toSubtractLifePoints.StartAgain());
    }

    private void BouncyBall()
    {
        this.StartCoroutine(ChangeBall());
    }
    private IEnumerator ChangeBall()
    {
        Sprite tempSprite = this.gameBallSprite.sprite;
        Color tempColor = this.gameBallSprite.color;
        PhysicsMaterial2D tempMaterial = this.gameBallCol.sharedMaterial;

        this.gameBallSprite.sprite = this.bouncyBall.GetComponent<SpriteRenderer>().sprite;
        this.gameBallSprite.color = this.bouncyBall.GetComponent<SpriteRenderer>().color;
        this.gameBallCol.sharedMaterial = this.bouncyBall.GetComponent<Collider2D>().sharedMaterial;
        this.gameBallRb.freezeRotation = false;

        this.gameBall.IsBouncyBallEnabled = true;

        yield return new WaitForSeconds(this.itemLastingTime);

        this.gameBallSprite.sprite = tempSprite;
        this.gameBallSprite.color = tempColor;
        this.gameBallCol.sharedMaterial = tempMaterial;
        this.gameBall.gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
        this.gameBallRb.freezeRotation = true;

        this.gameBall.IsBouncyBallEnabled = false;
    }

    private void ExtraBalls()
    {
        GameObject[] ballClones = new GameObject[3];

        for(int i = 0; i < ballClones.Length; i++)
        {
            ballClones[i] = GameObject.Instantiate<GameObject>(this.gameBall.gameObject, this.gameBall.gameObject.transform.position, Quaternion.identity);
            ballClones[i].GetComponent<Ball>().HasGameStarted = true;

            ballClones[i].GetComponent<Rigidbody2D>().velocity = Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f)) * this.gameBallRb.velocity;
        }
    }
}
