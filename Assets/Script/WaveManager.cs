using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    [Header("Enemy Prefabs")]
    public GameObject normalPrefab;
    public GameObject armoredPrefab;
    public GameObject bossPrefab;

    [Header("Spawn")]
    public Transform spawnPoint;
    public Transform[] waypoints;

    [Header("Timing")]
    public float spawnDelay = 0.5f;
    public float timeBetweenWaves = 20f;
    public float startCountdown = 10f;

    [Header("UI")]
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI countdownText;
    public GameObject skipPanel;

    [Header("Enemy Count UI")]
    public TextMeshProUGUI enemyCountText;
    public TextMeshProUGUI skipWarningText;

    [Header("WIN UI")]
    public GameObject winUI;

    // 🔥 SOUND
    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip newWaveSound;

    [Header("Win Sound")]
    public AudioClip winSound;   // 🔊 เสียงชนะ
    public AudioClip winMusic;   // 🎼 เพลงชนะ (ถ้ามี)

    int currentWave = 0;
    int enemiesAlive = 0;

    bool skipPressed = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        skipPanel.SetActive(false);

        if (winUI != null)
            winUI.SetActive(false);

        UpdateEnemyUI();
        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {
        yield return StartCoroutine(StartCountdown());

        while (currentWave < 15)
        {
            yield return StartCoroutine(RunWave());

            GiveFarmIncome();
            PlayerMoney.instance.Add(120);

            currentWave++;
        }

        WinGame();
    }

    void WinGame()
    {
        Debug.Log("YOU WIN!");

        if (winUI != null)
            winUI.SetActive(true);

        // 🔥 หยุดเสียงทั้งหมดในเกม (รวม SFX + BGM)
        AudioSource[] allAudio = FindObjectsOfType<AudioSource>();
        foreach (AudioSource a in allAudio)
        {
            a.Stop();
        }

        // 🔥 เล่นเสียงชนะ (SFX)
        if (audioSource != null && winSound != null)
        {
            audioSource.ignoreListenerPause = true; // 🔥 กันโดน TimeScale
            audioSource.PlayOneShot(winSound);
        }

        // 🔥 เล่นเพลงชนะ (แทน BGM เดิม)
        if (audioSource != null && winMusic != null)
        {
            audioSource.clip = winMusic;
            audioSource.loop = true;
            audioSource.PlayDelayed(0.2f); // หน่วงนิดให้เสียงชนะเล่นก่อน
        }

        // 🔥 หยุดเกม (หลังจากเสียงเริ่มเล่นแล้ว)
        Time.timeScale = 0f;
    }
    public void Replay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator StartCountdown()
    {
        float t = startCountdown;

        while (t > 0)
        {
            countdownText.text = "Start in " + Mathf.CeilToInt(t);
            t -= Time.deltaTime;
            yield return null;
        }

        countdownText.text = "";
    }

    IEnumerator RunWave()
    {
        skipPressed = false;

        int waveNumber = currentWave + 1;
        waveText.text = "Wave " + waveNumber;

        // 🔥 เล่นเสียงขึ้นเวฟ
        PlayWaveSound();

        var data = GetWaveData(waveNumber);

        yield return StartCoroutine(SpawnEnemies(normalPrefab, data.normal));
        yield return StartCoroutine(SpawnEnemies(armoredPrefab, data.armored));
        yield return StartCoroutine(SpawnEnemies(bossPrefab, data.boss));

        float timer = 0f;
        bool skipShown = false;

        // 🔥 ถ้าเป็นเวฟสุดท้าย → ปิด Skip ไปเลย
        if (currentWave >= 14)
        {
            HideSkipUI();
        }

        while (true)
        {
            if (enemiesAlive <= 0)
                break;

            timer += Time.deltaTime;

            // 🔥 แก้ตรงนี้: เวฟสุดท้ายจะไม่โชว์ Skip
            if (timer >= timeBetweenWaves && !skipShown && currentWave < 14)
            {
                skipShown = true;
                ShowSkipUI();
            }

            if (skipPressed)
                break;

            yield return null;
        }

        HideSkipUI();
    }

    // 🔥 ฟังก์ชันเล่นเสียง (กัน null + random pitch)
    void PlayWaveSound()
    {
        if (audioSource == null || newWaveSound == null) return;

        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(newWaveSound);
    }

    IEnumerator SpawnEnemies(GameObject prefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Spawn(prefab);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void Spawn(GameObject prefab)
    {
        GameObject e = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        Enemy enemy = e.GetComponent<Enemy>();
        enemy.waypoints = waypoints;

        enemiesAlive++;

        UpdateEnemyUI();
        UpdateSkipState();
    }

    public void EnemyDied()
    {
        enemiesAlive--;

        UpdateEnemyUI();
        UpdateSkipState();
    }

    void UpdateEnemyUI()
    {
        if (enemyCountText != null)
            enemyCountText.text = "Enemies: " + enemiesAlive;
    }

    void ShowSkipUI()
    {
        if (skipPanel != null)
        {
            skipPanel.SetActive(true);
            UpdateSkipState();
        }
    }

    void HideSkipUI()
    {
        if (skipPanel != null)
            skipPanel.SetActive(false);
    }

    void UpdateSkipState()
    {
        if (skipWarningText == null) return;

        if (enemiesAlive > 20)
            skipWarningText.text = "Too many enemies! ( > 20 )";
        else
            skipWarningText.text = "";
    }

    public void OnSkipYes()
    {
        if (enemiesAlive > 20)
        {
            Debug.Log("Skip blocked");
            return;
        }

        skipPressed = true;
    }

    public void OnSkipNo()
    {
        skipPanel.SetActive(false);
    }

    void GiveFarmIncome()
    {
        Tower[] towers = FindObjectsOfType<Tower>();

        foreach (Tower t in towers)
            t.GiveFarmIncome();
    }

    WaveData GetWaveData(int wave)
    {
        List<WaveData> table = new List<WaveData>()
        {
            new WaveData(8,0,0),
            new WaveData(10,0,0),
            new WaveData(12,1,0),
            new WaveData(14,2,0),
            new WaveData(16,4,0),
            new WaveData(18,8,0),
            new WaveData(20,10,0),
            new WaveData(22,13,0),
            new WaveData(24,14,0),
            new WaveData(24,16,0),
            new WaveData(26,18,0),
            new WaveData(28,20,0),
            new WaveData(30,22,0),
            new WaveData(32,24,1),
            new WaveData(25,26,2)
        };

        return table[wave - 1];
    }

    [System.Serializable]
    public class WaveData
    {
        public int normal, armored, boss;

        public WaveData(int n, int a, int b)
        {
            normal = n;
            armored = a;
            boss = b;
        }
    }

    Dictionary<UnitType, int> unitCount = new Dictionary<UnitType, int>();

    Dictionary<UnitType, int> unitLimit = new Dictionary<UnitType, int>()
    {
        { UnitType.Farm, 5 },
        { UnitType.Support, 3 }
    };

    public bool CanBuild(UnitType type)
    {
        if (!unitLimit.ContainsKey(type)) return true;

        int current = unitCount.ContainsKey(type) ? unitCount[type] : 0;

        return current < unitLimit[type];
    }

    public void RegisterTower(UnitType type)
    {
        if (!unitCount.ContainsKey(type))
            unitCount[type] = 0;

        unitCount[type]++;
    }

    public void RemoveTower(UnitType type)
    {
        if (!unitCount.ContainsKey(type)) return;

        unitCount[type]--;
    }


}