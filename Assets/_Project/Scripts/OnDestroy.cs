using UnityEngine;

public class OnDestroyObject : MonoBehaviour
{

    public GameObject animationPrefab;

 
    void OnDestroy()
    {
        GameObject fx = Instantiate(animationPrefab, transform.position, Quaternion.identity);
        fx.transform.SetParent(null); // détaché du parent
    }
}
