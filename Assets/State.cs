using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class StatefulBehaviour: MonoBehaviour
{
    protected Animator anim;
    private int _state;
    
    // Caller gives a state of a particular type - convert it
    protected void SetState<T>(T state) where T:struct, System.IConvertible {
        _state = System.Convert.ToInt32(state);
        anim.SetInteger("AnimState", _state);
    }

    protected T GetState<T>() where T: struct, System.IConvertible
    {
        System.Object state = _state;
        return (T)state;
    }

    protected IEnumerator OnAnimationComplete(string name, Action callback)
    {
            Debug.Log(anim.GetCurrentAnimatorStateInfo(0).IsName(name));

        if (anim.GetCurrentAnimatorStateInfo(0).IsName(name))
        {
            while(anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f) {
                Debug.Log("while");
                yield return null;
            }
            Debug.Log("cb");
            callback();
        }
    }
}
