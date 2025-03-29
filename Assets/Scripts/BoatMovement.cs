using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoatMovement : MonoBehaviour
{
    [Header("Movement Variables")]
    public float speed;
    private Vector2 movementDir;
    public float maxBoatFuel;
    public float boatFuel;
    [SerializeField] private float fuelPerMove;

    [Header("Referenced Objects")]
    [SerializeField] private TextMeshProUGUI fuelText;
    [SerializeField] private Slider fuelSlider;

    // PRIVATE VARIABLES
    private Rigidbody2D rb;
    private Transform myCamera;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        boatFuel = maxBoatFuel;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (boatFuel > 0)
        {
            movementDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                SwitchRotation();
                boatFuel -= fuelPerMove * Time.deltaTime;
            }
        }
        else { }
        fuelText.text = boatFuel.ToString("F1") + "%";
        fuelSlider.value = boatFuel;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = movementDir.normalized * speed;
    }

    void SwitchRotation()
    {
        if (movementDir.x > 0)
        {
            Debug.Log("D");
            transform.rotation = Quaternion.identity;
        }
        else if (movementDir.x < 0)
        {
            Debug.Log("A");
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (movementDir.y > 0)
        {
            Debug.Log("W");
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (movementDir.y < 0)
        {
            Debug.Log("S");
            transform.rotation = Quaternion.Euler(0, 0, 270);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Seal")
        {
            Debug.Log("Entered seal trigger");
            collision.GetComponentInParent<Seal_SeaBehaviour>().BoatTriggered();
        }
    }
}
