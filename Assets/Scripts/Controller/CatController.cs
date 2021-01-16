using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Brain;

public class CatController : AvatarController
{
    protected new void Awake()
    {
        base.Awake();
        if (brainType.Equals(BrainType.PLAYER))
            brain = new PlayerBrain();
        else
            brain = new CatAIBrain();
    }
    // Update is called once per frame
    void Update()
    {
        bool isPlayer1 =true;
        UpdateAvatar(isPlayer1);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag.Equals("Mouse")) {
            GameMainManager.Instance.OnMouseCatch();
        }
    }
}
