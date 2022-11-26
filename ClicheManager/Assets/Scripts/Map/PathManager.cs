using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class PathManager : MonoBehaviour
{
    
    [Header("StageSetup")]
    public int StageID;
    [SerializeField] int StageSceneID;

    [SerializeField] bool StartNode;
    public bool isUnlocked;
    public bool stageFinished;
    public bool multiExit;
    public PathManager multiExit2Node;

    public GameObject UnLockablePath;
    public Transform[] PathLeft, PathRight, PathUp, PathDown;

    public SpriteRenderer Arrowleft, Arrowright, Arrowup, Arrowdown;
    public PathManager left, right, up, down;

    //UI

    [Header("UI")]
    [SerializeField] GameObject LevelNamePlate;
    public Sprite Pu_F, Pu_L, Pu_I, Pu_W, Pu_Empty;
    [SerializeField] string StageName;
    
    [SerializeField] Image PuToFind;
    [SerializeField] Text StageNameField, exitsFoundText;

    
 

    public AudioClip moveSFX, startSFX;


    Animator anim;
    private void Start()
    {
       if (MapManager.instance.finishedIDs.Contains(StageID))
            stageFinished = true;
   
       if(MapManager.instance.unlockedIDs.Contains(StageID))
                isUnlocked = true;

        anim = GetComponent<Animator>();
    }

    

    private void Update()
    {
        
        SpriteUpdate();

        // UNLOCK ORDER

        if (playerIsHere())
        {
            ShowLevelName();
            if (Input.GetButtonDown("Jump"))
            {
                if (startSFX != null && AudioManager.instance != null)
                    AudioManager.instance.PlaySFX(startSFX);

                GameManager.activLevelID = StageID;
                SceneManager.LoadScene(StageSceneID);
            }


            //if (stageFinished == true)
            //{
            //    if (left != null)
            //        left.UnlockStage();

            //    if (right != null)
            //        right.UnlockStage();

            //    if (down != null)
            //        down.UnlockStage();

            //    if (up != null)
            //        up.UnlockStage();
            //}

            // MOVE ORDER

            if (left != null)
                if (left.isUnlocked) 
                {
                    Arrowleft.enabled = true;
                    if (Input.GetAxisRaw("Horizontal") < 0)
                    {
                        PathPlayer.instance.MoveTo(PathLeft);
                        PlayMoveAudio();
                    } 
                }
            if (right != null)
                if (right.isUnlocked) 
                {
                    Arrowright.enabled = true;
                    if (Input.GetAxisRaw("Horizontal") > 0)
                    {
                        PathPlayer.instance.MoveTo(PathRight);
                        PlayMoveAudio();
                    } 
                }
            if (down != null)
                if (down.isUnlocked)
                {
                    Arrowdown.enabled = true;

                    if (Input.GetAxisRaw("Vertical") < 0)
                    {
                        PathPlayer.instance.MoveTo(PathDown);
                        PlayMoveAudio();
                    } 
                }
            if (up != null)
                if (up.isUnlocked)
                {
                    Arrowup.enabled = true;
                    if (Input.GetAxisRaw("Vertical") > 0)
                    {
                        PathPlayer.instance.MoveTo(PathUp);
                        PlayMoveAudio();
                    }
                }
        }
        else
            HideLevelName();
        anim.SetBool("Player", playerIsHere());
    }

    void PlayMoveAudio()
    {
        if (moveSFX != null && AudioManager.instance != null)
            AudioManager.instance.PlaySFX(moveSFX);


    }

    public void SpriteUpdate()
    {
        if (stageFinished)
        {
            anim.SetBool("Fin", true);
            if(!MapManager.instance.finishedIDs.Contains(StageID))
            MapManager.instance.finishedIDs.Add(StageID);
        }
        if (isUnlocked)
        {
            if(!StartNode)
                UnLockablePath.SetActive(true);
            anim.SetBool("Open", true);
            if(!MapManager.instance.unlockedIDs.Contains(StageID))
                MapManager.instance.unlockedIDs.Add(StageID);
        }
    }

    void HideLevelName()
    {
        if (StageNameField.text.Contains(StageName) && LevelNamePlate != null)
        {
            LevelNamePlate.SetActive(false);
        }

    
        
    }
    void ShowLevelName()
    {
        if (LevelNamePlate == null)
            return;

        StageNameField.text = StageName;
        //Set Image PowerUp in UI
        if (Pu_F != null || Pu_I != null || Pu_L != null || Pu_W != null)
        {
            PuToFind.enabled = true;
            if (GameManager.instance.fire && Pu_F != null)
                PuToFind.sprite = Pu_F;
            else if (GameManager.instance.wind && Pu_W != null)
                PuToFind.sprite = Pu_W;
            else if (GameManager.instance.ice && Pu_I != null)
                PuToFind.sprite = Pu_I;
            else if (GameManager.instance.lighning && Pu_L != null)
                PuToFind.sprite = Pu_L;
            else
                PuToFind.sprite = Pu_Empty;
        }
        else
            PuToFind.enabled= false;


        if (multiExit)
        {
            if (!stageFinished)
                exitsFoundText.text = "Exits Found: 0/2";
            if (stageFinished)
                exitsFoundText.text = "Exits Found: 1/2";
            if (multiExit2Node.isUnlocked)
                exitsFoundText.text = "Exits Found: 2/2";
        }
        else
        {
            if (!stageFinished)
                exitsFoundText.text = "Exits Found: 0/1";
            if (stageFinished)
                exitsFoundText.text = "Exits Found: 1/1";
        }



        LevelNamePlate.SetActive(true);
    }


    public void UnlockStage()
    {
        isUnlocked = true;
    }

    private bool  playerIsHere()
    {
        if (Vector2.Distance(PathPlayer.instance.transform.position, transform.position) < 1f)
            return true;
        else
            return false;
    }
}
