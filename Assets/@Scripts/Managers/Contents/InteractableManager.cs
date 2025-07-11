using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : BaseObject
{

    public override bool Init()
    {




        return true;
    }

    public override void OnClick()
    {

        Destroy(gameObject);
    }
}

public class InteractableManager
{
    public List<Interactable> interactables = new List<Interactable>();

    public void Init()
    {
        //Managers.Object.Spawn<Interactable>(new Vector3(38 - Random.Range(0,10), 0.616f, 27 - Random.Range(0, 10)), Managers.Data.InteractableDic[]);
        //Managers.Object.Spawn<Interactable>(new Vector3(38 - Random.Range(0, 10), 0.616f, 27 - Random.Range(0, 10)), Managers.Data.InteractableDic);
    }

}
