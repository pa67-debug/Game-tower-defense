using UnityEngine;

public class BuildConfirmUI : MonoBehaviour
{
    public static BuildConfirmUI instance;

    public GameObject panel;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip buildSuccessSound;
    public AudioClip failSound;

    BaseSlot currentSlot;

    void Awake()
    {
        instance = this;
        panel.SetActive(false);
    }

    public void Show(BaseSlot slot)
    {
        if (currentSlot != null)
            currentSlot.SetPreview(false);

        currentSlot = slot;

        currentSlot.SetPreview(true);
        panel.SetActive(true);
    }

    public void OnYes()
    {
        panel.SetActive(false);

        if (currentSlot != null)
        {
            currentSlot.SetPreview(false);

            bool success = TowerBuildUI.instance.Build(currentSlot);

            if (success)
                PlaySound(buildSuccessSound);
            else
                PlaySound(failSound);
        }

        currentSlot = null;
    }

    public void OnNo()
    {
        panel.SetActive(false);

        if (currentSlot != null)
            currentSlot.SetPreview(false);

        currentSlot = null;
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource == null || clip == null) return;

        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(clip);
    }
}