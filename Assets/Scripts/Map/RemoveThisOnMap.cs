using UnityEngine;

public class RemoveThisOnMap : MonoBehaviour
{
    private void OnEnable()
    {
        NetworkEvents.NewMapLoaded += OnNewMapLoaded;
    }
    private void OnDisable()
    {
        NetworkEvents.NewMapLoaded -= OnNewMapLoaded;
    }

    private void OnNewMapLoaded()
    {
        Destroy(gameObject);
    }
}
