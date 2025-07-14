using UnityEngine;
using UnityEngine.UI;

public class FreqChanger : MonoBehaviour
{
    public HologramController hologramController;
    public Text text;

    public void ChangeFreq(float amount)
    {
        hologramController.frequency += amount;
        text.text = $"{hologramController.frequency} Hz";
    }
}
