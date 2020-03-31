using UnityEngine;

public class AnalyseInfo : MonoBehaviour
{
    [SerializeField]
    private AnalyseUI analyseUILoot;

    [SerializeField]
    private GameObject analyseUISecret;

    public GameObject activeAnalyse;
    public GameObject target;

    private void Start()
    {
        this.analyseUILoot.gameObject.SetActive(false);
        this.analyseUISecret.gameObject.SetActive(false);
        this.SetAnalyseUI(false);
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;

        if (this.target != null)
        {
            this.analyseUILoot.setTarget(this.target);
            this.activeAnalyse = this.analyseUILoot.gameObject;
        }
        else
        {
            this.activeAnalyse = this.analyseUISecret;
        }
    }

    public void SetAnalyseUI(bool value)
    {
        //Signal
        this.activeAnalyse.SetActive(value);
    }
}
