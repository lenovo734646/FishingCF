using UnityEngine;

public class LiveEff : MonoBehaviour
{
    [Header("存活时间")]
    public float LiveTime = 0.1f;

    private void OnEnable()
    {
        Invoke("UnspawnEff", LiveTime);
    }

    private void UnspawnEff()
    {
        if (gameObject.activeSelf)
            ObjectPoolManager.Instance.Unspawn(gameObject);
    }
}
