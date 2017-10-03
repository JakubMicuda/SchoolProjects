using UnityEngine;
using System.Collections;

public class TryController : MonoBehaviour {

    [SerializeField]
    private tk2dSprite [] tries = new tk2dSprite[6];
    [SerializeField]
    private string tryUsed = "pokus02";
    [SerializeField]
    private string tryNotUsed = "pokus01";
    private int nextIndex = 0;
    private int endIndex = 6;

    public bool HasTries { get { return nextIndex < endIndex; } }

    //resets all tries
    public void ResetTries(int endindex = 6)
    {
        for(int i = 0; i < tries.Length; i++)
        {
            tries[i].SetSprite(tryNotUsed);
            tries[i].gameObject.SetActive(true);
        }

        if(endindex <= 6 && endindex > 0)
        {
            for(int i = endindex; i<tries.Length; i++)
            {
                tries[i].gameObject.SetActive(false);
            }
            this.endIndex = endindex;
        }
        nextIndex = 0;
    }

    //uses try if available
    public void UseTry()
    {
        if(!HasTries)
            return;
        tries[nextIndex].SetSprite(tryUsed);
        nextIndex++;
    }
}
