using UnityEngine;

// This script goes on any object that should play an animation once and then disappear
public class DestroyAfterAnimation : MonoBehaviour
{
    // Public method to be called by an Animation Event
    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}