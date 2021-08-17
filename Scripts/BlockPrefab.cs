using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockPrefab : MonoBehaviour
{
    [SerializeField] List<Collider2D> booksCollider = new List<Collider2D>();
    private AudioSource forBookSfx;
    [SerializeField] AudioClip bookThump;

    private LevelManager levelManager;

    public List<Collider2D> BooksCollider
    {
        get { return this.booksCollider; }
        set { this.booksCollider = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        this.forBookSfx = this.gameObject.GetComponent<AudioSource>();
        this.forBookSfx.volume = 0.50f;

        this.levelManager = GameObject.FindObjectOfType<LevelManager>();
    }

    public void RemoveColliderObjectFromList(Collider2D collider)
    {
        this.booksCollider.Remove(collider);

        if (this.booksCollider.Count < 1)
        {            
            this.levelManager.RemovePrefabFromDictionary(this.gameObject);
            GameObject.Destroy(this.gameObject);
        }            
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            this.forBookSfx.PlayOneShot(this.bookThump, 0.15f);
        }
    }
}
