using UnityEngine;
using UnityEngine.EventSystems;
// Tillader at snakke til eventsystemet (source claude)

//IpointerEnterHandler og ExitHandler er så unity kan se hover funktioner (source claude)
public class PanelMove : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // hoveredPos = når musen er over, defaultPos = når musen ikke er, tagetPos = så den ikke snapper til position, men glider derhen (source claude)
    public Vector3 hoveredPosition;
    public Vector3 defaultPosition;
    private Vector3 targetPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Lerp beregner selve bevægelsen mellem den nuværende position og targetPosition, Time.deltaTime * 5f styrer hastigheden
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * 5f);
    }
    // Sætter targetPos når musen er over og når den ikke er, bevægelse sker i update.
    public void OnPointerEnter(PointerEventData eventData)
    {
        targetPosition = hoveredPosition;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetPosition = defaultPosition;
    }
}
