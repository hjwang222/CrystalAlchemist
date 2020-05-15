using UnityEngine;

public class AnalyseInfo : MonoBehaviour
{
    [SerializeField]
    private AnalyseUI analyseUILoot;

    [SerializeField]
    private BoolValue isActive;

    private GameObject target;

    private void Start()
    {
        this.analyseUILoot.setTarget(this.target);
    }

    private void Update()
    {
        this.analyseUILoot.gameObject.SetActive(this.isActive.getValue());
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }
}
