using UnityEngine;

public class DestroyableObject : MonoBehaviour, IReactive
{
    public void React()
    {
        gameObject.SetActive(false);
    }
}
