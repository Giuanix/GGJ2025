using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomEventManager : MonoBehaviour
{
    [System.Serializable]
    public class WaveEvent
    {
        public float newWaveHeight;
        public float newWaveOffset;
        public float newWaveSpeed;
        public float newWaveFrequency;
        public float newAngularForce;
        public float eventDuration;
        public bool onlyOffset;
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
    [System.Serializable]
    public class SlipperyEvent
    {
        public float newWaveHeight;
        public float newWaveSpeed;
        public float newWaveFrequency;
        public float eventDuration;
        public Color color;
        public GameObject[] toSlippery;
        public float firstWaveDuration;
    }

    [SerializeField] private WaveEvent[] waveEvents;
    [SerializeField] private SpawnEvent spawnEvent;
    [SerializeField] private SlipperyEvent slipperyEvent;
    [SerializeField] private float countdownTime = 15f;
    [SerializeField] private float defaultSpawnTimer = 4f;


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
        StartCoroutine("DefaultSpawBubble");
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


    }

    void TriggerRandomEvent()
    {
        if (waveEvents.Length > 0 && Random.value < .289f)
        {
            int randomIndex = Random.Range(0, waveEvents.Length);
            WaveEvent selectedEvent = waveEvents[randomIndex];

            StartCoroutine(HandleWaveEvent(selectedEvent));
        }
        else if (spawnEvent != null && Random.value < .623f)
        {
            StartCoroutine("SpawnObjects");
        }else if(slipperyEvent != null)
        {
            StartCoroutine("SlipperyWave");
        }
    }

    private IEnumerator HandleWaveEvent(WaveEvent waveEvent)
    {
        if (!waveEvent.onlyOffset)
        {
            yield return StartCoroutine(LerpOffset(-250f, 5f));

            waterLevel.waterHeight = waveEvent.newWaveHeight;
            waterLevel.speed = waveEvent.newWaveSpeed;
            waterLevel.waterFrequency = waveEvent.newWaveFrequency;
            waterLevel.angularForce = waveEvent.newAngularForce;

        }

        yield return StartCoroutine(LerpOffset(waveEvent.newWaveOffset, 2f));

        yield return new WaitForSeconds(waveEvent.eventDuration);
        if (!waveEvent.onlyOffset)
        {
            yield return StartCoroutine(LerpOffset(-250f, 5f));


            waterLevel.waterHeight = originalWaveHeight;
            waterLevel.speed = originalWaveSpeed;
            waterLevel.waterFrequency = originalWaveFrequency;
            waterLevel.angularForce = originalAngularForce;


        }

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

    private IEnumerator SpawnObjects()
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

    private IEnumerator DefaultSpawBubble()
    {
        while (true)
        {
            GameObject randomObject = spawnEvent.objectsToSpawn[Random.Range(0, spawnEvent.objectsToSpawn.Length)];
            Vector3 spawnPosition = new Vector3(Random.Range(spawnEvent.SpawnRange.x, spawnEvent.SpawnRange.y), spawnEvent.spawnHeight, 0);
            Instantiate(randomObject, spawnPosition, Quaternion.identity);

            yield return new WaitForSeconds(defaultSpawnTimer);
        }
    }


    private IEnumerator SlipperyWave()
    {
        Color oldColor = waterLevel.fillColor;

        yield return StartCoroutine(LerpOffset(-250f, 5f));


        waterLevel.waterHeight = slipperyEvent.newWaveHeight;
        waterLevel.speed = slipperyEvent.newWaveSpeed;
        waterLevel.waterFrequency = slipperyEvent.newWaveFrequency;
        waterLevel.fillColor = slipperyEvent.color;


        yield return StartCoroutine(LerpOffset(originalOffset, 2f));
        List<string> oldTag = new List<string>();
        float timer = 0f;

        foreach (GameObject slip in slipperyEvent.toSlippery)
        {
            oldTag.Add(slip.tag);
        }

        Color colorObj = slipperyEvent.color;
        colorObj.a = 1;

        while (timer < slipperyEvent.firstWaveDuration)
        {
            timer += Time.deltaTime;
            
            foreach(GameObject slip in slipperyEvent.toSlippery)
            {
                if(slip.TryGetComponent<SpriteRenderer>(out SpriteRenderer spr))
                    spr.color = Color.Lerp(Color.white, colorObj, timer / slipperyEvent.firstWaveDuration);
            }
            yield return null;
        }
        foreach(GameObject slip in slipperyEvent.toSlippery)
        {
            slip.tag = "Slippery";
        }


        yield return StartCoroutine(LerpOffset(-250f, 2f));

        waterLevel.waterHeight = originalWaveHeight;
        waterLevel.speed = originalWaveSpeed;
        waterLevel.waterFrequency = originalWaveFrequency;
        waterLevel.angularForce = originalAngularForce;


        yield return StartCoroutine(LerpOffset(originalOffset, 2f));
     

        yield return new WaitForSeconds(slipperyEvent.eventDuration);

        yield return StartCoroutine(LerpOffset(-250f, 2f));
       
        waterLevel.waterHeight = slipperyEvent.newWaveHeight;
        waterLevel.speed = slipperyEvent.newWaveSpeed;
        waterLevel.waterFrequency = slipperyEvent.newWaveFrequency;
        waterLevel.fillColor = oldColor;

        yield return StartCoroutine(LerpOffset(originalOffset, 2f));

        while (timer < slipperyEvent.firstWaveDuration)
        {
            timer += Time.deltaTime;

            foreach (GameObject slip in slipperyEvent.toSlippery)
            {
                if (slip.TryGetComponent<SpriteRenderer>(out SpriteRenderer spr))
                    spr.color = Color.Lerp(colorObj, Color.white, timer / slipperyEvent.firstWaveDuration);
            }
            yield return null;
        }

        for(int i = 0; i < oldTag.Count; i++)
        {
            slipperyEvent.toSlippery[i].tag = oldTag[i];
        }

        yield return StartCoroutine(LerpOffset(-250f, 2f));

        waterLevel.waterHeight = originalWaveHeight;
        waterLevel.speed = originalWaveSpeed;
        waterLevel.waterFrequency = originalWaveFrequency;
        waterLevel.angularForce = originalAngularForce;


        yield return StartCoroutine(LerpOffset(originalOffset, 2f));


        ResetCountdown();
    }
}
