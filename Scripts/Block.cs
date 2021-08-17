using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    //Configuration Parameters (things we need to know before the game)
    [SerializeField] int blockPointsValue;
    private UI_Manager targetObject;

    private Vector3 gameCameraPos;
    private AudioSource forBookSfx;
    [SerializeField] AudioClip bookDestroyed;

    [SerializeField] int bookLives;
    [SerializeField] GameObject particles;
    [SerializeField] GameObject dustParticles;

    //Cached Component References (references to other game objects or components of game objects)
    private Rigidbody2D booksRb;
    private Collider2D blockCol;
    private BlockPrefab parent;
    private Ball gameBall;


    //State variables (to keep track of the variables that govern states)
    private int numberOfHits = 0;
    private SpriteRenderer[] bookGroup;
    private Color slightlyBurnt = new Color(1.0f, 0.486f, 0.176f);
    private Color almostDestroyed = new Color(0.965f, 0.165f, 0.051f);

    // Start is called before the first frame update
    void Start()
    {
        this.targetObject = GameObject.FindObjectOfType<UI_Manager>();

        this.gameCameraPos = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
        this.forBookSfx = this.gameObject.GetComponent<AudioSource>();
        this.forBookSfx.volume = 0.50f;

        this.bookGroup = this.gameObject.GetComponentsInChildren<SpriteRenderer>();
        this.booksRb = this.gameObject.GetComponentInParent<Rigidbody2D>();
        this.blockCol = this.gameObject.GetComponent<Collider2D>();
        this.parent = this.gameObject.GetComponentInParent<BlockPrefab>();
        this.gameBall = GameObject.Find("Game Ball").GetComponent<Ball>();

        Physics2D.IgnoreLayerCollision(10, 11, true); //The books ignore the screen bounds, this way I can spawn them above the ceiling and still fall down
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ball")
        {
            this.numberOfHits += this.gameBall.PowerLevel;

            if (this.numberOfHits > this.bookLives)
            {
                this.parent.RemoveColliderObjectFromList(this.blockCol); //Remove the book block unit from the list in the prefab

                //instantiate particles effect when book unit is destroyed and play destroying sound
                AudioSource.PlayClipAtPoint(this.bookDestroyed, this.gameCameraPos);
                GameObject destroyParticles = GameObject.Instantiate<GameObject>(this.particles, this.gameObject.transform.position, this.gameObject.transform.rotation);
                
                GameObject.Destroy(this.gameObject); //destry the book unit and call UI_Manager to add points to score
                this.targetObject.SendMessage("AddScore", this.blockPointsValue);

                GameObject.Destroy(destroyParticles, 1.0f); //destroy aprticle effect 1 second after instantiating
            }
            else
            { 
                foreach(SpriteRenderer item in this.bookGroup)
                {
                    if (this.bookLives == this.numberOfHits)
                        item.color = this.almostDestroyed;
                    else
                        item.color = this.slightlyBurnt;
                }
                
            }
        }

        //When books hit shelves make some dust appear
        if(collision.gameObject.layer == 9)
        {
            GameObject dust = GameObject.Instantiate(this.dustParticles, this.blockCol.bounds.min, this.gameObject.transform.rotation);
            dust.transform.position = new Vector3(this.blockCol.bounds.min.x, this.blockCol.bounds.min.y, -5.0f);

            GameObject.Destroy(dust, 1.0f);
        }
    }
}
