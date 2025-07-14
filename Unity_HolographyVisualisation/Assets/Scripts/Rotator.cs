using UnityEngine;
using UnityEngine.UI;

public class Rotator : MonoBehaviour
{
    public Transform target;
    public Text text;
    
    private float _angle;

    [ContextMenu("Rotate")] private void DebugRotate()
    {
        Rotate(20);
    }
    public void Rotate(float deg)
    {
        _angle += deg;
        text.text = $"{_angle} º";
        target.rotation *= Quaternion.Euler(deg, 0, 0);
    }
}
