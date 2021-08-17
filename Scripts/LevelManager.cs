using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    //Configuration Parameters (things we need to know before the game)
    [SerializeField] int spawnEverySeconds = 10;
    Vector2[] spawningPoints;
    private Vector2 leftShelfSpawn1;
    private Vector2 rightShelfSpawn1;
    private Vector2 centerShelfSpawn1;
    private Vector2 leftShelfSpawn2;
    private Vector2 rightShelfSpawn2;
    private Vector2 centerShelfSpawn2;
    private float spawningPointYPos = 15.0f;

    
    [SerializeField] GameObject[] prefabsEasy;
    [SerializeField] GameObject[] prefabsNormal;
    [SerializeField] GameObject[] prefabsHard;
    [SerializeField] int maxPrefabnumber = 14;
    Quaternion zeroDegrees = Quaternion.Euler(new Vector2(0, 0));

    private Dictionary<GameObject, List<Collider2D>> bookInstances = new Dictionary<GameObject, List<Collider2D>>();
    public void RemovePrefabFromDictionary(GameObject inPrefab)
    {
        this.bookInstances.Remove(inPrefab);
    }   


    private List<Collider2D> booksTouchingLeftShelfBottom1 = new List<Collider2D>();
    private List<Collider2D> booksTouchingLeftShelfMiddle1 = new List<Collider2D>();
    private List<Collider2D> booksTouchingRightShelfBottom1 = new List<Collider2D>();
    private List<Collider2D> booksTouchingRightShelfMiddle1 = new List<Collider2D>();
    private List<Collider2D> booksTouchingLeftShelfBottom2 = new List<Collider2D>();
    private List<Collider2D> booksTouchingLeftShelfMiddle2 = new List<Collider2D>();
    private List<Collider2D> booksTouchingRightShelfBottom2 = new List<Collider2D>();
    private List<Collider2D> booksTouchingRightShelfMiddle2 = new List<Collider2D>();



    //Cached Component References (references to other game objects or components of game objects)

    [SerializeField] Collider2D leftShelfBottomTrigger1;
    [SerializeField] Collider2D leftShelfBottomTrigger2;
    [SerializeField] Collider2D leftShelfBottom1;
    [SerializeField] Collider2D leftShelfBottom2;
    [SerializeField] Collider2D leftShelfMiddleTrigger1;
    [SerializeField] Collider2D leftShelfMiddleTrigger2;
    [SerializeField] Collider2D leftShelfMiddle1;
    [SerializeField] Collider2D leftShelfMiddle2;
    [SerializeField] Collider2D leftShelfTop1;
    [SerializeField] Collider2D leftShelfTop2;
    [SerializeField] Collider2D rightShelfBottomTrigger1;
    [SerializeField] Collider2D rightShelfBottomTrigger2;
    [SerializeField] Collider2D rightShelfBottom1;
    [SerializeField] Collider2D rightShelfBottom2;
    [SerializeField] Collider2D rightShelfMiddleTrigger1;
    [SerializeField] Collider2D rightShelfMiddleTrigger2;
    [SerializeField] Collider2D rightShelfMiddle1;
    [SerializeField] Collider2D rightShelfMiddle2;
    [SerializeField] Collider2D rightShelfTop1;
    [SerializeField] Collider2D rightShelfTop2;
    [SerializeField] Collider2D centerShelf;

    private AudioSource backgroundMusic;
    [SerializeField] GameObject pauseScreen;



    //State variables (to keep track of the variables that govern states)
    private bool isInstanceCreated = false;
    private bool isBookOnLeftMiddleShelf1 = false;
    public bool IsBookOnLeftMiddleShelf1
    {
        get { return this.isBookOnLeftMiddleShelf1; }
    }
    public bool SetIsBookOnLeftMiddleShelf1(bool condition)
    {
        if ((condition.ToString().Trim().ToLower() == "true") ||
           (condition.ToString().Trim().ToLower() == "false"))
        {
            this.isBookOnLeftMiddleShelf1 = condition;
            return true;
        }
        else
            return false;
            
    }
    private bool isBookOnLeftMiddleShelf2 = false;
    public bool IsBookOnLeftMiddleShelf2
    {
        get { return this.isBookOnLeftMiddleShelf2; }
    }
    public bool SetIsBookOnLeftMiddleShelf2(bool condition)
    {
        if ((condition.ToString().Trim().ToLower() == "true") ||
           (condition.ToString().Trim().ToLower() == "false"))
        {
            this.isBookOnLeftMiddleShelf2 = condition;
            return true;
        }
        else
            return false;

    }
    private bool isBookOnRightMiddleShelf1 = false;
    public bool IsBookOnRightMiddleShelf1
    {
        get { return this.isBookOnRightMiddleShelf1; }
    }
    public bool SetIsBookOnRightMiddleShelf1(bool condition)
    {
        if ((condition.ToString().Trim().ToLower() == "true") ||
           (condition.ToString().Trim().ToLower() == "false"))
        {
            this.isBookOnRightMiddleShelf1 = condition;
            return true;
        }
        else
            return false;

    }
    private bool isBookOnRightMiddleShelf2 = false;
    public bool IsBookOnRightMiddleShelf2
    {
        get { return this.isBookOnRightMiddleShelf2; }
    }
    public bool SetIsBookOnRightMiddleShelf2(bool condition)
    {
        if ((condition.ToString().Trim().ToLower() == "true") ||
           (condition.ToString().Trim().ToLower() == "false"))
        {
            this.isBookOnRightMiddleShelf2 = condition;
            return true;
        }
        else
            return false;

    }

    private bool isGamePaused = false;
    public bool IsGamePaused
    {
        get { return this.isGamePaused; }
        set { this.isGamePaused = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1; //For scene reloading. If I put it in the scene loader, before loading the scene, it doesn't work for whatever reason. That is why it's here instead.
        this.backgroundMusic = this.gameObject.GetComponent<AudioSource>();

        this.leftShelfSpawn1 = new Vector2(2.0f, this.spawningPointYPos);
        this.leftShelfSpawn2 = new Vector2(5.0f, this.spawningPointYPos);
        this.rightShelfSpawn1 = new Vector2(14.0f, this.spawningPointYPos);
        this.rightShelfSpawn2 = new Vector2(17.0f, this.spawningPointYPos);
        this.centerShelfSpawn1 = new Vector2(8.0f, this.spawningPointYPos);
        this.centerShelfSpawn2 = new Vector2(11.0f, this.spawningPointYPos);

        this.spawningPoints = new Vector2[] { this.leftShelfSpawn1, this.rightShelfSpawn1, this.centerShelfSpawn1,
                                              this.leftShelfSpawn2, this.rightShelfSpawn2, this.centerShelfSpawn2 };

        this.CreateBookInstance();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.bookInstances.Count < this.maxPrefabnumber) //don't allow more than a maximum number of blocks at any given time to prevent bloating 
            this.SpawnBooks();

        this.PushBooksDown();

        this.PauseGame();
    }

    private void PauseGame()
    {
        if (!this.isGamePaused)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                Time.timeScale = 0;
                this.isGamePaused = true;
                this.backgroundMusic.Pause();
                this.pauseScreen.SetActive(true);
            }
        }
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1;
        this.isGamePaused = false;
        this.backgroundMusic.UnPause();
        this.pauseScreen.SetActive(false);
    }

    private void SpawnBooks()
    {
        if ((Mathf.CeilToInt(Time.time) % this.spawnEverySeconds == 0) && !this.isInstanceCreated)
        {
            this.CreateBookInstance();
        }
        else if (Mathf.CeilToInt(Time.time) % this.spawnEverySeconds != 0)
        {
            this.isInstanceCreated = false;
        } 
    }

    private void CreateBookInstance()
    {
        Vector2 rndSpawn;

        this.ChooseSpawningPoint(out rndSpawn);

        this.ChooseBookPrefab(rndSpawn);

        this.isInstanceCreated = true;
    }

    private void ChooseBookPrefab(Vector2 rndSpawn)
    {
        int prefabDifficulty = Random.Range(0, 21);
        int prefabNumber;

        GameObject temp;

        switch (prefabDifficulty)
        {
            case int i when i < 10:
                PickPrefab(rndSpawn, out prefabNumber, out temp, this.prefabsEasy);
                break;

            case int i when i >= 10 & i < 17:
                PickPrefab(rndSpawn, out prefabNumber, out temp, this.prefabsNormal);
                break;

            case int i when i >= 17:
                PickPrefab(rndSpawn, out prefabNumber, out temp, this.prefabsHard);
                break;

            default:
                temp = null;
                break;
        }
         
        this.bookInstances.Add(temp, temp.GetComponent<BlockPrefab>().BooksCollider);
    }

    private void PickPrefab(Vector2 rndSpawn, out int prefabNumber, out GameObject temp, GameObject[] prefabArray)
    {
        prefabNumber = Random.Range(0, prefabArray.Length);
        temp = GameObject.Instantiate<GameObject>(prefabArray[prefabNumber], rndSpawn, this.zeroDegrees);
    }

    private Vector2 ChooseSpawningPoint(out Vector2 rndSpawn)
    {
        rndSpawn = this.spawningPoints[Random.Range(0, this.spawningPoints.Length)]; //choose a spawning point at random

        if (rndSpawn == this.spawningPoints[2] || rndSpawn == this.spawningPoints[5]) //if the previous spawning point is on the center shelf, choose again
            rndSpawn = this.spawningPoints[Random.Range(0, this.spawningPoints.Length)]; //this way the game is a bit easier so center shelf doesn't get bloated
        
        return rndSpawn;
    }

    private void PushBooksDown()
    {
        for (int i = 0; i < this.bookInstances.Count; i++)
        {
            //First check the let shelves and push books down
            this.PushFromShelfToShelf(this.bookInstances, i, this.leftShelfMiddleTrigger1, this.booksTouchingLeftShelfMiddle1, this.leftShelfTop1,
                                      this.isBookOnLeftMiddleShelf1, this.leftShelfBottomTrigger1, this.booksTouchingLeftShelfBottom1, this.leftShelfMiddle1);
            this.PushFromShelfToShelf(this.bookInstances, i, this.leftShelfMiddleTrigger2, this.booksTouchingLeftShelfMiddle2, this.leftShelfTop2,
                                      this.isBookOnLeftMiddleShelf2, this.leftShelfBottomTrigger2, this.booksTouchingLeftShelfBottom2, this.leftShelfMiddle2);

            //second check right shelves and push books down
            this.PushFromShelfToShelf(this.bookInstances, i, this.rightShelfMiddleTrigger1, this.booksTouchingRightShelfMiddle1, this.rightShelfTop1,
                                      this.isBookOnRightMiddleShelf1, this.rightShelfBottomTrigger1, this.booksTouchingRightShelfBottom1, this.rightShelfMiddle1);
            this.PushFromShelfToShelf(this.bookInstances, i, this.rightShelfMiddleTrigger2, this.booksTouchingRightShelfMiddle2, this.rightShelfTop2,
                                      this.isBookOnRightMiddleShelf2, this.rightShelfBottomTrigger2, this.booksTouchingRightShelfBottom2, this.rightShelfMiddle2);
        }
    }

    private void PushFromShelfToShelf(Dictionary<GameObject, List<Collider2D>> prefabDic, int prefabListIndex,
                                      Collider2D middleShelfTrigger, List<Collider2D> booksInMiddleShelf,
                                      Collider2D topShelfCollider, bool isBookOnMiddleShelf, Collider2D shelfBottomTrigger,
                                      List<Collider2D> booksInBottomShelf, Collider2D middleShelfCollider)
    {
        if (Physics2D.GetContacts(middleShelfTrigger, booksInMiddleShelf) >= 1)
        {
            this.ManageIgnoreColliders(prefabDic, prefabListIndex, topShelfCollider, false);
        }

        if ((Physics2D.GetContacts(middleShelfTrigger, booksInMiddleShelf) < 1) &&
                 !isBookOnMiddleShelf)
        {
            this.ManageIgnoreColliders(prefabDic, prefabListIndex, topShelfCollider, true);
        }
        else if (Physics2D.GetContacts(shelfBottomTrigger, booksInBottomShelf) < 1)
        {
            this.ManageIgnoreColliders(prefabDic, prefabListIndex, middleShelfCollider, true);
        }
        else
        {
            this.ManageIgnoreColliders(prefabDic, prefabListIndex, middleShelfCollider, false);
        }
    }

    private void ManageIgnoreColliders(Dictionary<GameObject, List<Collider2D>> prefabDic, int prefabListIndex, Collider2D shelfCollider, bool isIgnored)
    {
        for (int j = 0; j < prefabDic.ElementAt(prefabListIndex).Value.Count; j++)
        {
            if (isIgnored)
            {
                if (!Physics2D.GetIgnoreCollision(shelfCollider, prefabDic.ElementAt(prefabListIndex).Value[j]))
                {
                    SetIgnoreCollider(prefabDic, prefabListIndex, shelfCollider, isIgnored, j);
                }
            }
            else
            {
                if (Physics2D.GetIgnoreCollision(shelfCollider, prefabDic.ElementAt(prefabListIndex).Value[j]))
                {
                    SetIgnoreCollider(prefabDic, prefabListIndex, shelfCollider, isIgnored, j);
                }
            }
                            
        }
    }

    private static void SetIgnoreCollider(Dictionary<GameObject, List<Collider2D>> prefabDic, int prefabListIndex,
                                          Collider2D shelfCollider, bool isIgnored, int colliderListIndex)
    {
        Physics2D.IgnoreCollision(shelfCollider, prefabDic.ElementAt(prefabListIndex).Value[colliderListIndex], isIgnored);
    }
}
