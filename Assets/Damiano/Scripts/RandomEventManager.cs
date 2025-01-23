using UnityEngine;
using System.Collections;

public class RandomEventManager : MonoBehaviour
{
    [System.Serializable]
    public class WaveEvent
    {
        public float newWaveHeight;
        public float newWaveSpeed;
        public float newWaveFrequency;
        public float newAngularForce;
        public float eventDuration;
    }

    [System.Serializable]
    public class SpawnEvent
    {
        public GameObject[] objectsToSpawn;  // Array of objects to spawn
        public int spawnCount;
        public float spawnDuration;
        public Vector2 SpawnRange = new Vector2(-9, 9);
        public float spawnHeight = -6f;
    }

    [SerializeField] private WaveEvent[] waveEvents;
    [SerializeField] private SpawnEvent[] spawnEvents;
    [SerializeField] private float countdownTime = 15f;
    [SerializeField] private float waterChangeDirectionTimer = 15f;
    private float currentWaterDirCountdown;

    private float currentCountdown;
    private WaterLevel waterLevel;

    private float originalOffset;
    private float originalWaveHeight;
    private float originalWaveSpeed;
    private float originalWaveFrequency;
    private float originalAngularForce;
    private bool canCountdown = true;

    void Start()
    {
        waterLevel = FindObjectOfType<WaterLevel>();
        if (waterLevel != null)
        {
            originalOffset = waterLevel.offset;
            originalWaveHeight = waterLevel.waterHeight;
            originalWaveSpeed = waterLevel.speed;
            originalWaveFrequency = waterLevel.waterFrequency;
            originalAngularForce = waterLevel.angularForce;
        }

        ResetCountdown();
    }

    void Update()
    {
        if (canCountdown)
        {
            currentCountdown -= Time.deltaTime;
            if (currentCountdown <= 0)
            {
                TriggerRandomEvent();
                canCountdown = false;
            }
        }

        currentWaterDirCountdown -= Time.deltaTime;
        if(currentWaterDirCountdown <= 0)
        {
            currentWaterDirCountdown = waterChangeDirectionTimer;
            waterLevel.speed *= -1;
        }
    }

    void TriggerRandomEvent()
    {
        if (waveEvents.Length > 0 && Random.value > 0.5f)
        {
            int randomIndex = Random.Range(0, waveEvents.Length);
            WaveEvent selectedEvent = waveEvents[randomIndex];

            StartCoroutine(HandleWaveEvent(selectedEvent));
        }
        else if (spawnEvents.Length > 0)
        {
            int randomIndex = Random.Range(0, spawnEvents.Length);
            SpawnEvent selectedEvent = spawnEvents[randomIndex];

            StartCoroutine(SpawnObjects(selectedEvent));
        }
    }

    private IEnumerator HandleWaveEvent(WaveEvent waveEvent)
    {
        yield return StartCoroutine(LerpOffset(-200f, 5f));

        waterLevel.waterHeight = waveEvent.newWaveHeight;
        waterLevel.speed = waveEvent.newWaveSpeed;
        waterLevel.waterFrequency = waveEvent.newWaveFrequency;
        waterLevel.angularForce = waveEvent.newAngularForce;

        yield return StartCoroutine(LerpOffset(originalOffset, 2f));

        yield return new WaitForSeconds(waveEvent.eventDuration);

        yield return StartCoroutine(LerpOffset(-200f, 2f));

        waterLevel.waterHeight = originalWaveHeight;
        waterLevel.speed = originalWaveSpeed;
        waterLevel.waterFrequency = originalWaveFrequency;
        waterLevel.angularForce = originalAngularForce;

        yield return StartCoroutine(LerpOffset(originalOffset, 2f));
        ResetCountdown();
    }

    private IEnumerator LerpOffset(float target, float duration)
    {
        float elapsedTime = 0f;
        float startOffset = waterLevel.offset;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            waterLevel.offset = Mathf.Lerp(startOffset, target, t);
            yield return null;
        }

        waterLevel.offset = target;
    }

    void ResetCountdown()
    {
        currentCountdown = countdownTime;
        canCountdown = true;
    }

    private IEnumerator SpawnObjects(SpawnEvent spawnEvent)
    {
        for (int i = 0; i < spawnEvent.spawnCount; i++)
        {
            if (spawnEvent.objectsToSpawn.Length > 0)
            {
                GameObject randomObject = spawnEvent.objectsToSpawn[Random.Range(0, spawnEvent.objectsToSpawn.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(spawnEvent.SpawnRange.x, spawnEvent.SpawnRange.y), spawnEvent.spawnHeight, 0);
                Instantiate(randomObject, spawnPosition, Quaternion.identity);
            }
            yield return new WaitForSeconds(spawnEvent.spawnDuration / spawnEvent.spawnCount);
        }
        ResetCountdown();
    }

    private IEnumerator DefaultSpawBubble(SpawnEvent spawnEvent)
    {
        for (int i = 0; i < spawnEvent.spawnCount; i++)
        {
            if (spawnEvent.objectsToSpawn.Length > 0)
            {
                GameObject randomObject = spawnEvent.objectsToSpawn[Random.Range(0, spawnEvent.objectsToSpawn.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(spawnEvent.SpawnRange.x, spawnEvent.SpawnRange.y), spawnEvent.spawnHeight, 0);
                Instantiate(randomObject, spawnPosition, Quaternion.identity);
            }
            yield return new WaitForSeconds(spawnEvent.spawnDuration / spawnEvent.spawnCount);
        }
        ResetCountdown();
    }
}
