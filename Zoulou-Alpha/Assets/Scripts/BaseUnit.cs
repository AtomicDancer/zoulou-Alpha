using UnityEngine;
using UnityEngine.UI;

public abstract class BaseUnit : MonoBehaviour
{
    public string unitName;
    public Sprite icon;
    public int cost;
    public GameObject rangeVisualPrefab;
    public Image skillChargeBar;
    public ParticleSystem skillReadyFxAura;

    void Start()
    {
        skillChargeBar.GetComponentInParent<Canvas>().worldCamera = Camera.main;
        skillChargeBar.fillAmount = 0;
        skillReadyFxAura.Stop();
    }
}

