using System.Collections;
using System;
using UnityEngine;

public class StatefulBehaviour: MonoBehaviour
{
    protected Animator anim;
    private int _state;
    
    // Caller gives a state of a particular type - convert it
    protected void SetState<T>(T state) where T:struct, IConvertible {
        _state = Convert.ToInt32(state);
        anim.SetInteger("AnimState", _state);
    }

    protected T GetState<T>() where T: struct, IConvertible
    {
        System.Object state = _state;
        return (T)state;
    }

    protected IEnumerator OnAnimationComplete(string name, Action cb)
    {     
        yield return new WaitWhile(() => !anim.GetCurrentAnimatorStateInfo(0).IsName(name));
        yield return new WaitWhile(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.99f);
        cb();
    }
}
