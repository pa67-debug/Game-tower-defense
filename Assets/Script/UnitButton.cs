using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitButton : MonoBehaviour
{
    public UnitData data;

    [Header("Highlight (รูปเหลือง)")]
    public GameObject highlight;

    // 🔥 SOUND
    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip selectSound;
    public AudioClip unselectSound;

    static UnitButton current;

    void Start()
    {
        if (highlight != null)
            highlight.SetActive(false);

        if (highlight != null)
        {
            var img = highlight.GetComponent<Image>();
            if (img != null) img.raycastTarget = false;
        }

        var btn = GetComponent<Button>();
        if (btn != null)
            btn.navigation = new Navigation { mode = Navigation.Mode.None };
    }

    public void Click()
    {
        EventSystem.current.SetSelectedGameObject(null);

        // 🔁 กดซ้ำ = ยกเลิก
        if (current == this)
        {
            Debug.Log("Unselect: " + data.name);

            PlaySound(unselectSound);

            TowerBuildUI.instance.SelectUnit(null);

            if (highlight != null)
                highlight.SetActive(false);

            current = null;
            return;
        }

        // ❌ ปิดของเก่า
        if (current != null && current.highlight != null)
        {
            current.highlight.SetActive(false);
        }

        // 🔄 เลือกตัวนี้
        current = this;

        Debug.Log("Select: " + data.name);

        PlaySound(selectSound);

        TowerBuildUI.instance.SelectUnit(data);

        if (highlight != null)
            highlight.SetActive(true);
    }

    // 🔥 ฟังก์ชันเล่นเสียง (กัน null + random pitch)
    void PlaySound(AudioClip clip)
    {
        if (audioSource == null || clip == null) return;

        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(clip);
    }
}