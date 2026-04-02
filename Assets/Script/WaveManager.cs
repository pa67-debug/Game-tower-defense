using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    [Header("Enemy Prefabs")]
    public GameObject normalPrefab;
    public GameObject armoredPrefab;
    public GameObject flyingPrefab;
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
        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {
        // 🔥 นับถอยหลังก่อนเริ่มเกม
        yield return StartCoroutine(StartCountdown());

        while (currentWave < 15)
        {
            yield return StartCoroutine(RunWave());

            // 💰 ให้เงิน
            PlayerMoney.instance.Add(120);

            currentWave++;
        }

        Debug.Log("YOU WIN!");
    }

    // ⏱ นับถอยหลัง 10 วิ
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

        var data = GetWaveData(waveNumber);

        // 🔥 Spawn มอน
        yield return StartCoroutine(SpawnEnemies(normalPrefab, data.normal));
        yield return StartCoroutine(SpawnEnemies(armoredPrefab, data.armored));
        yield return StartCoroutine(SpawnEnemies(flyingPrefab, data.flying));
        yield return StartCoroutine(SpawnEnemies(bossPrefab, data.boss));

        // 🔥 หลัง spawn เสร็จ → รอ
        float timer = 0f;
        bool skipShown = false;

        while (true)
        {
            // ✔ ถ้าตายหมด → ไปเวฟถัดไป
            if (enemiesAlive <= 0)
                break;

            timer += Time.deltaTime;

            // 🔥 ครบ 20 วิ → โชว์ skip
            if (timer >= timeBetweenWaves && !skipShown)
            {
                skipShown = true;
                ShowSkipUI();
            }

            // ✔ กด YES → ข้ามทันที
            if (skipPressed)
                break;

            yield return null;
        }

        HideSkipUI();
    }

    void ShowSkipUI()
    {
        if (skipPanel != null)
            skipPanel.SetActive(true);
    }

    void HideSkipUI()
    {
        if (skipPanel != null)
            skipPanel.SetActive(false);
    }

    // 🔘 YES
    public void OnSkipYes()
    {
        skipPressed = true;
    }

    // 🔘 NO
    public void OnSkipNo()
    {
        skipPanel.SetActive(false);
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

        // 🔥 ใส่ path จาก WaveManager
        enemy.waypoints = waypoints;

        // 🔥 รีเซ็ต waypoint (กัน bug)
        enemy.SendMessage("Start");

        enemiesAlive++;
    }

    public void EnemyDied()
    {
        enemiesAlive--;
    }

    // 📊 ตารางเวฟ
    WaveData GetWaveData(int wave)
    {
        List<WaveData> table = new List<WaveData>()
        {
            new WaveData(8,0,0,0),
            new WaveData(10,0,0,0),
            new WaveData(12,1,0,0),
            new WaveData(14,2,0,0),
            new WaveData(16,3,2,0),
            new WaveData(18,4,3,0),
            new WaveData(20,5,4,0),
            new WaveData(22,6,5,0),
            new WaveData(24,8,6,0),
            new WaveData(24,8,10,0),
            new WaveData(26,10,6,0),
            new WaveData(28,12,8,0),
            new WaveData(30,14,10,0),
            new WaveData(32,16,12,0),
            new WaveData(25,15,10,1)
        };

        return table[wave - 1];
    }

    [System.Serializable]
    public class WaveData
    {
        public int normal, armored, flying, boss;

        public WaveData(int n, int a, int f, int b)
        {
            normal = n;
            armored = a;
            flying = f;
            boss = b;
        }
    }
}