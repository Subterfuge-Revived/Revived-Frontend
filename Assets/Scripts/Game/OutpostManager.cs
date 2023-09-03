﻿using Subterfuge.Remake.Core.Entities.Components;
using Subterfuge.Remake.Core.Entities.Positions;
using UnityEngine;
 using TMPro;

 public class OutpostManager : MonoBehaviour
{

    private Animator OutpostAnimator;
    public string ID;
    private float downtime;
    private bool expanded = false;
    public TextMeshPro drillerCountText;
    public TextMeshPro outpostNameText;
    public int zoomsSkipped = 0;

    public Outpost outpost;

    public readonly Color subOlive = new Color(0.439f, 0.463f, 0.290f);
    public readonly Color subRed = new Color(0.667f, 0.122f, 0.137f);
    public readonly Color subSky = new Color(0.482f, 0.659f, 0.851f);
    public readonly Color subOrange = new Color(0.824f, 0.553f, 0.161f);
    public readonly Color subTeal = new Color(0.353f, 0.6f, 0.541f);
    public readonly Color subBiege = new Color(0.608f, 0.565f, 0.482f);
    public readonly Color subPink = new Color(0.784f, 0.467f, 0.694f);
    public readonly Color subPurple = new Color(0.42f, 0.333f, 0.557f);
    public readonly Color subNavy = new Color(0.227f, 0.294f, 0.639f);
    public readonly Color subBrown = new Color(0.545f, 0.369f, 0.235f);
    public readonly Color subGrey = new Color(0.3f, 0.3f, 0.3f);

    // Start is called before the first frame update
    void Start()
    {
        OutpostAnimator = gameObject.GetComponent<Animator>();
        drillerCountText = gameObject.GetComponentInChildren<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        // Set color based on the owner
        var drillerCount = outpost.GetComponent<DrillerCarrier>().GetDrillerCount().ToString();
        var shieldCount = outpost.GetComponent<ShieldManager>().GetShields();
        var shieldCapacity = outpost.GetComponent<ShieldManager>().GetShieldCapacity();
        drillerCountText.text = drillerCount + "(" + shieldCount + "/" + shieldCapacity + ")";
        outpostNameText.text = outpost.GetComponent<IdentityManager>().GetName();
        
        
        // // Determine the outpost vision mask.
        // if (outpost.GetOwner()?.GetId() == ApplicationState.player.GetId())
        // {
        //     // Apply vision mask.
        //     gameObject.GetComponentInChildren<SpriteMask>().transform.localScale = new Vector3(outpost.GetVisionRange() / 15.0f, outpost.GetVisionRange() / 15.0f, 1);
        //     gameObject.GetComponentInChildren<TextMeshPro>().transform.localScale = new Vector3(1, 1, 1);
        // }
        // else
        // {
        //     // remove vision mask.
        //     gameObject.GetComponentInChildren<SpriteMask>().transform.localScale = new Vector3(0, 0, 1);
        //
        //     if (ApplicationState.CurrentGame.TimeMachine.GetState().isInVisionRange(outpost, ApplicationState.player))
        //     {
        //         gameObject.GetComponentInChildren<TextMeshPro>().transform.localScale = new Vector3(1, 1, 1);
        //     }
        //     else
        //     {
        //         gameObject.GetComponentInChildren<TextMeshPro>().transform.localScale = new Vector3(0, 0, 1);
        //     }
        // }

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        int playerId = 0;
        if (outpost.GetComponent<DrillerCarrier>().GetOwner() != null)
        {
            playerId = ApplicationState.CurrentGame.TimeMachine.GetState().GetPlayers().IndexOf(outpost.GetComponent<DrillerCarrier>().GetOwner()) + 1;
        }
        switch (playerId)
        {
            case 0:
                renderer.color = subGrey;
                break;
            case 1:
                renderer.color = subRed;
                break;
            case 2:
                renderer.color = subOlive;
                break;
            case 3:
                renderer.color = subOrange;
                break;
            case 4:
                renderer.color = subPink;
                break;
            case 5:
                renderer.color = subBrown;
                break;
            case 6:
                renderer.color = subPurple;
                break;
            case 7:
                renderer.color = subBiege;
                break;
            case 8:
                renderer.color = subSky;
                break;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            downtime = Time.time;
            return;
        }

        if (Input.GetMouseButtonUp(0))
        {
            downtime = Time.time - downtime;
        }
        
        if (Input.GetMouseButton(0))
        {
            return;
        }

        if (downtime < 0.25 && downtime > 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject == gameObject && expanded == false)
                {
                    hit.collider.gameObject.GetComponent<OutpostManager>().Expand();
                    //gameManager.SelectedOutpost = hit.transform.GetComponent<OutpostManager>().ID;
                }
                else
                {
                    gameObject.GetComponent<OutpostManager>().Contract();
                }
            }
            else
            {
                gameObject.GetComponent<OutpostManager>().Contract();
            }
            
            downtime = 0;
        }
        
        // If the mouse wheel is being scrolled, scale text attached to the object.
        if (Input.mouseScrollDelta.magnitude > 0)
        {
            ScaleTextToZoom();
        }
    }

    private void ScaleTextToZoom()
    {
        float finalSize = (float)Screen.width/20.0f;
        outpostNameText.fontSize = (int)finalSize;
        drillerCountText.fontSize = (int)finalSize;
        // guiText.pixelOffset = new Vector2( offset.x * Screen.width, offset.y * Screen.height);
        
        // if (Input.mouseScrollDelta.y > 0)
        // {
        //     if (zoomsSkipped < 0 || !(outpostNameText.fontSize > 6))
        //     {
        //         zoomsSkipped++;
        //         return;
        //     }
        //
        //     // Zooming In, decrease font size.
        //     outpostNameText.fontSize -= 2;
        //     drillerCountText.fontSize -= 2;
        // }
        // else
        // {
        //     if (zoomsSkipped > 0 || !(outpostNameText.fontSize < 18))
        //     {
        //         zoomsSkipped--;
        //         return;
        //     }
        //     
        //     outpostNameText.fontSize += 2;
        //     drillerCountText.fontSize += 2;
        // }
    }

    public void Expand()
    {
        expanded = true;
        OutpostAnimator.ResetTrigger("Contract");
        OutpostAnimator.SetTrigger("Expand");
    }
    
    public void Contract()
    {
        expanded = false;
        OutpostAnimator.ResetTrigger("Expand");
        OutpostAnimator.SetTrigger("Contract");
    }
}
