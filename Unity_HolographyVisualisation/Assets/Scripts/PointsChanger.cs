using UnityEngine;
using UnityEngine.UI;

public class PointsChanger : MonoBehaviour
{
    public GameObject pointPrefab;
    public Bounds spawnBounds;
    public HologramController hologramController;
    public Text text;

    private Vector3 RandomPointInBounds()
    {
        Vector3 randomPoint;
        do
        {
            randomPoint = Random.insideUnitSphere * spawnBounds.extents.magnitude;
            randomPoint += spawnBounds.center;
        } while (!spawnBounds.Contains(randomPoint));

        return randomPoint;
    }
    
    [ContextMenu("AddPoint")] public void AddPoint()
    {
        if (hologramController.count >= hologramController.maxCount)
            return;
        var point = Instantiate(pointPrefab, RandomPointInBounds(), Quaternion.identity);
        point.transform.SetParent(transform);
        hologramController.points[hologramController.count] = point.transform;
        hologramController.count++;
        text.text = $"{hologramController.count}";
    }
    
    public void RemovePoint()
    {
        if (hologramController.count <= 1)
            return;
        hologramController.count--;
        Destroy(hologramController.points[hologramController.count].gameObject);
        text.text = $"{hologramController.count}";
    }
}