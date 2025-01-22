using UnityEngine;
using UnityEngine.Events;

public class RandomEventManager : MonoBehaviour
{
    [System.Serializable]
    public class EventData
    {
        public string eventName; // Nome dell'evento (per sapere cosa succede)
        public UnityEvent onEventTriggered; // Evento che viene attivato
    }

    public EventData[] events; // Lista degli eventi configurabili dall'Inspector
    public float countdownTime = 5f; // Durata del countdown (in secondi)

    private float currentCountdown; // Valore attuale del countdown

    void Start()
    {
        ResetCountdown(); // Imposta il countdown iniziale
    }

    void Update()
    {
        // Aggiorna il countdown
        currentCountdown -= Time.deltaTime;
        if (currentCountdown <= 0)
        {
            TriggerRandomEvent(); // Attiva un evento casuale
            ResetCountdown(); // Ripristina il countdown
        }
    }

    void TriggerRandomEvent()
    {
        if (events.Length == 0) return; // Se non ci sono eventi, non fare nulla

        // Seleziona un evento casuale
        int randomIndex = Random.Range(0, events.Length);
        EventData selectedEvent = events[randomIndex];

        Debug.Log($"Evento selezionato: {selectedEvent.eventName}"); // Mostra l'evento selezionato nella console
        selectedEvent.onEventTriggered?.Invoke(); // Attiva l'evento
    }

    void ResetCountdown()
    {
        currentCountdown = countdownTime; // Imposta il countdown al valore iniziale
    }
}
