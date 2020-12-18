using UnityEngine;

public sealed class SceneDirector : MonoBehaviour
{
    public GameObject rain;
    public GameObject fire;
    public GameObject girlRainCloud;
    public GameObject girlTreeStumpHappy;
    public GameObject girlTreeStumpSad;
    public GameObject girlTreeStumpTalking;
    public GameObject girlTreeSitting;
    public GameObject girlTreeTalking;
    public GameObject girlTreeSwinging;
    public GameObject girlFireSitting;
    public GameObject girlFireStanding;
    public GameObject girlAlone;
    public GameObject boyTreePeaking;
    public GameObject boyTreeTalking;
    public GameObject boyTreeSwinging;
    public GameObject boyTreeStumpTalking;
    public GameObject boyFireSitting;
    public GameObject boyFireStanding;
    public GameObject boyHoldingRope;
    public GameObject tireBroken;
    public GameObject tireFixed;

    private enum GirlState
    {
        treeSitting,
        treeTalking,
        treeSwinging,
        treeStumpHappy,
        treeStumpSad,
        treeStumpTalking,
        fireSitting,
        fireStanding,
        alone,
        removed
    }

    private enum BoyState
    {
        treePeaking,
        treeTalking,
        treeSwinging,
        treeStumpTalking,
        fireSitting,
        fireStanding,
        holdingRope,
        removed
    }

    private void Start()
    {
        ResetScene();
    }

    public void SetScene(string direction)
    {
        Debug.Log(direction);

        if (direction.Contains("scene-reset")) {
            ResetScene();
        }

        if (direction.Contains("scene-rain-on")) {
            SetSceneRainActive(true);
        } else if (direction.Contains("scene-rain-off")) {
            SetSceneRainActive(false);
        }

        if (direction.Contains("scene-fire-on")) {
            SetSceneFireActive(true);
        } else if (direction.Contains("scene-fire-off")) {
            SetSceneFireActive(false);
        }

        if (direction.Contains("scene-tire-fixed")) {
            SetTireFixed(true);
        } else if (direction.Contains("scene-tire-broken")) {
            SetTireFixed(false);
        }

        if (direction.Contains("girl-rain-cloud-on")) {
            SetGirlRainCloudActive(true);
        } else if (direction.Contains("girl-rain-cloud-off")) {
            SetGirlRainCloudActive(false);
        }

        if (direction.Contains("girl-removed")) {
            SetGirlState(GirlState.removed);
        } else if (direction.Contains("girl-tree-sitting")) {
            SetGirlState(GirlState.treeSitting);
        } else if (direction.Contains("girl-tree-talking")) {
            SetGirlState(GirlState.treeTalking);
        } else if (direction.Contains("girl-tree-swinging")) {
            SetGirlState(GirlState.treeSwinging);
        } else if (direction.Contains("girl-tree-stump-happy")) {
            SetGirlState(GirlState.treeStumpHappy);
        } else if (direction.Contains("girl-tree-stump-sad")) {
            SetGirlState(GirlState.treeStumpSad);
        } else if (direction.Contains("girl-tree-stump-talking")) {
            SetGirlState(GirlState.treeStumpTalking);
        } else if (direction.Contains("girl-fire-sitting")) {
            SetGirlState(GirlState.fireSitting);
        } else if (direction.Contains("girl-fire-standing")) {
            SetGirlState(GirlState.fireStanding);
        } else if (direction.Contains("girl-alone")) {
            SetGirlState(GirlState.alone);
        }

        if (direction.Contains("boy-removed")) {
            SetBoyState(BoyState.removed);
        } else if (direction.Contains("boy-tree-peaking")) {
            SetBoyState(BoyState.treePeaking);
        } else if (direction.Contains("boy-tree-talking")) {
            SetBoyState(BoyState.treeTalking);
        } else if (direction.Contains("boy-tree-swinging")) {
            SetBoyState(BoyState.treeSwinging);
        } else if (direction.Contains("boy-tree-stump-talking")) {
            SetBoyState(BoyState.treeStumpTalking);
        } else if (direction.Contains("boy-fire-sitting")) {
            SetBoyState(BoyState.fireSitting);
        } else if (direction.Contains("boy-fire-standing")) {
            SetBoyState(BoyState.fireStanding);
        } else if (direction.Contains("boy-holding-rope")) {
            SetBoyState(BoyState.holdingRope);
        }
    }

    private void ResetScene()
    {
        ResetGirl();
        ResetBoy();

        SetSceneRainActive(false);
        SetSceneFireActive(false);
        SetGirlRainCloudActive(false);
        SetTireFixed(false);
    }

    private void ResetGirl()
    {
        this.girlTreeStumpHappy.SetActive(false);
        this.girlTreeStumpSad.SetActive(false);
        this.girlTreeStumpTalking.SetActive(false);
        this.girlTreeSitting.SetActive(false);
        this.girlTreeTalking.SetActive(false);
        this.girlTreeSwinging.SetActive(false);
        this.girlFireSitting.SetActive(false);
        this.girlFireStanding.SetActive(false);
        this.girlAlone.SetActive(false);
    }

    private void ResetBoy()
    {
        this.boyTreePeaking.SetActive(false);
        this.boyTreeTalking.SetActive(false);
        this.boyTreeSwinging.SetActive(false);
        this.boyTreeStumpTalking.SetActive(false);
        this.boyFireSitting.SetActive(false);
        this.boyFireStanding.SetActive(false);
        this.boyHoldingRope.SetActive(false);
    }

    private void SetSceneRainActive(bool active)
    {
        this.rain.SetActive(active);
    }

    private void SetSceneFireActive(bool active)
    {
        this.fire.SetActive(active);
    }

    private void SetGirlRainCloudActive(bool active)
    {
        this.girlRainCloud.SetActive(active);
    }

    private void SetTireFixed(bool isFixed)
    {
        if (isFixed)
        {
            this.tireFixed.SetActive(true);
            this.tireBroken.SetActive(false);
        }
        else
        {
            this.tireFixed.SetActive(false);
            this.tireBroken.SetActive(true);
        }
    }

    private void SetGirlState(GirlState state)
    {
        ResetGirl();

        switch (state)
        {
            case GirlState.treeSitting:
                this.girlTreeSitting.SetActive(true);
                break;
            case GirlState.treeTalking:
                this.girlTreeTalking.SetActive(true);
                break;
            case GirlState.treeSwinging:
                this.girlTreeSwinging.SetActive(true);
                break;

            case GirlState.treeStumpHappy:
                this.girlTreeStumpHappy.SetActive(true);
                break;
            case GirlState.treeStumpSad:
                this.girlTreeStumpSad.SetActive(true);
                break;
            case GirlState.treeStumpTalking:
                this.girlTreeStumpTalking.SetActive(true);
                break;

            case GirlState.fireSitting:
                this.girlFireSitting.SetActive(true);
                break;
            case GirlState.fireStanding:
                this.girlFireStanding.SetActive(true);
                break;

            case GirlState.alone:
                this.girlAlone.SetActive(true);
                break;
        }
    }

    private void SetBoyState(BoyState state)
    {
        ResetBoy();

        switch (state)
        {
            case BoyState.treePeaking:
                this.boyTreePeaking.SetActive(true);
                break;
            case BoyState.treeTalking:
                this.boyTreeTalking.SetActive(true);
                break;
            case BoyState.treeSwinging:
                this.boyTreeSwinging.SetActive(true);
                break;

            case BoyState.treeStumpTalking:
                this.boyTreeStumpTalking.SetActive(true);
                break;

            case BoyState.fireSitting:
                this.boyFireSitting.SetActive(true);
                break;
            case BoyState.fireStanding:
                this.boyFireStanding.SetActive(true);
                break;

            case BoyState.holdingRope:
                this.boyHoldingRope.SetActive(true);
                break;
        }
    }

}
