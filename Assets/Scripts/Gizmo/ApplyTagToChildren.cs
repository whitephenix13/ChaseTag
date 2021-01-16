using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyTagToChildren : MonoBehaviour
{
    void OnDrawGizmosSelected()
    {
        string tag = transform.tag; ;

        applyTagToChildrenRec(transform, tag);
    }

    void applyTagToChildrenRec(Transform t, string tag) {
        t.tag = tag;
        foreach (Transform child in t)
        {
            applyTagToChildrenRec(child, tag);
        }
    }
}
