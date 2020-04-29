using TMPro;
using System.Collections;
using UnityEngine;

public class TitleScreenStart : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenu;

    [SerializeField]
    private SimpleSignal destroySignal;

    [SerializeField]
    private float delay = 0.3f;

    [SerializeField]
    private TextMeshProUGUI textfield;

    [SerializeField]
    private FloatSignal musicVolumeSignal;

    private bool inputPossible = false;

    private void Awake()
    {
        if (this.destroySignal != null) destroySignal.Raise();
        this.mainMenu.SetActive(false);
        StartCoroutine(this.delayInput());
    }

    private void Start()
    {
        SaveSystem.loadOptions();

        if (this.musicVolumeSignal != null)
            this.musicVolumeSignal.Raise(MasterManager.settings.backgroundMusicVolume);
    }

    private void Update()
    {
        if (Input.anyKeyDown && this.inputPossible)
        {
            this.mainMenu.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }

    private IEnumerator delayInput()
    {
        this.inputPossible = false;
        this.textfield.enabled = false;
        yield return new WaitForSeconds(this.delay);
        this.inputPossible = true;
        this.textfield.enabled = true;
    }
}
