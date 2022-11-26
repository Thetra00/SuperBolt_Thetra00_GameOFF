using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    public static DialogSystem instance;
    private void Awake()
    {
        instance = this;
    }

    public AudioClip Audio;
    public GameObject TextField;
    public string textLine;
    List<string> savetexts = new List<string>();
    public float duration;
    public Text dialogtext;
   

    bool textActive, showall;

    private void Start()
    {
        dialogtext.text = "";
    }


    public void StartText(string text)
    {
        savetexts.Add(text);
    }
    
    private void Update()
    {
        if (!textActive && savetexts.Count != 0)
            StartCoroutine(ShowText());

    }

    IEnumerator ShowText()
    {
     
        TextField.SetActive(true);
        textActive = true;
        
        AudioManager.instance.PlaySFX(Audio);
 
        Time.timeScale = 1;

        foreach (char c in savetexts[0].ToCharArray())
        {
            dialogtext.text += c;
            
            yield return new WaitForSeconds(0.03f);
        }


        
        yield return new WaitForSeconds(duration);
    

        dialogtext.text = "";
        savetexts.Remove(savetexts[0]);
        textActive = false;
        //TextField.transform.localScale = Vector3.zero;
        TextField.SetActive(false);
        showall = false;
    }

    public void OnSkipText()
    {
        if (!showall)
        {
            showall = true;
            StopAllCoroutines();
            dialogtext.text = "";
            dialogtext.text = savetexts[0];
        }
        else
        {
            StopAllCoroutines();
            savetexts.Remove(savetexts[0]);
            dialogtext.text = "";
            textActive = false;
            //TextField.transform.localScale = Vector3.zero;
            TextField.SetActive(false);
            showall = false;
        }
    
    }
    
}
