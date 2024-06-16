using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamoDNA : MonoBehaviour
{
    #region Genes for color (chromosomes)
    public float r;
    public float g;
    public float b;
    #endregion

    #region Genes (chromosomes) for size

    public float size;
    #endregion

    private bool isDead = false;

    public float timeToDie = 0f;

    SpriteRenderer spriteRenderer;
    Collider2D sCollider;

    private void OnMouseDown()
    {
        isDead = true;
        timeToDie = CamoPopulationManager.elapsed;
        //Debug.Log("Died at: "+ timeToDie);
        spriteRenderer.enabled = false;
        sCollider.enabled = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        sCollider = GetComponent<Collider2D>();

        spriteRenderer.color = new Color(r,g,b);
        this.transform.localScale = Vector3.one * size;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
