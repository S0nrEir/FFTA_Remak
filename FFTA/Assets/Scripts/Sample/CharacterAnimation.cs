using UnityEngine;
using System.Collections;
//-----------------------------------------------
//Animation切り替え
//-----------------------------------------------
public class CharacterAnimation : MonoBehaviour {
    public Animator[] CharacterList;
    public Transform[] CharacterTransform;


    public bool Front_Back;
    public bool Left_Right;
    void Start() {
        ChangeAnimation("CA_Idle_1");
        On_Front_Back(Front_Back);
        ChangeRotate(Left_Right);
    }
    public void On_Front_Back(bool flg) {
        Front_Back = flg;
        if (flg) {
            ChangeAnimation("CA_Idle_1");
        } else {
            ChangeAnimation("CA_Idle_2");
        }
    }
    public void On_Left_Right(bool flg) {
        Left_Right = flg;
        if (flg) {
            ChangeRotate(true);
        } else {
            ChangeRotate(false);
        }
    }
    public void OnButtonClick(int AnimationNo) {
        ChangeParameter("MoveFlg", false);
        On_Front_Back(Front_Back);
        if (AnimationNo == 0) {
            //On_Front_Back(Front_Back);
        } else if (AnimationNo == 1) {
            ChangeParameter("MoveFlg", true);
        } else if (AnimationNo == 2) {
            ChangeParameter("AttackFlg", true);
        } else if (AnimationNo == 3) {
            ChangeParameter("JumpFlg", true);
        } else if (AnimationNo == 4) {
            ChangeParameter("DamageFlg", true);
        } else if (AnimationNo == 5) {
            ChangeParameter("WinFlg", true);
        } else if (AnimationNo == 6) {
            ChangeParameter("DeathFlg", true);
        } else if (AnimationNo == 7) {
            ChangeParameter("SkillFlg", true);
        }
    }
    public void ChangeAnimation(string animationName) {
        for(int cnt = 0;cnt < CharacterList.Length; cnt++) {
            CharacterList[cnt].Play(animationName);
        }    
    }
    public void ChangeParameter(string name,bool flg) {
        for (int cnt = 0; cnt < CharacterList.Length; cnt++) {
            CharacterList[cnt].SetBool(name, flg);
        }
    }
    public void ChangeRotate(bool flg) {
        for (int cnt = 0; cnt < CharacterList.Length; cnt++) {
            Vector3 tmp = CharacterTransform[cnt].localScale;
            if (flg) {
                tmp.x = Mathf.Abs(tmp.x);
            } else {
                tmp.x = 0-tmp.x;
            }
            CharacterTransform[cnt].localScale = tmp;
        }
    }

}
