using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : Singleton<Hover>
{
    private SpriteRenderer spriteRenderer; //reference to the tower icon sprite renderer

    private SpriteRenderer soldierRangeSpriteRenderer; //reference to the tower range sprite renderer

    public bool IsVisible {get; private set;}
    void Start()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();

        this.soldierRangeSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        FollowMouse();
    }

    private void FollowMouse() //makes the tower icon follow the mouse when positioning towers
    {
        if(spriteRenderer.enabled)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }

    public void Activate(Sprite sprite) //activates the hover icon
    {
        this.spriteRenderer.sprite = sprite; //sets the sprite
        spriteRenderer.enabled = true; //enables tower icon renderer  
        //soldierRangeSpriteRenderer.enabled = true; //enables tower range renderer

        IsVisible = true;
    }

    public void Deactivate() //deactivates hover icon
    {
        spriteRenderer.enabled = false; //disables tower icon renderer
        //soldierRangeSpriteRenderer.enabled = false; //disables tower range renderer

        GameManager.Instance.ClickedTowerBtn = null; //unclicks the button

        IsVisible = false;
    }
}
