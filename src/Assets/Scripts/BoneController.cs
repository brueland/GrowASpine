using UnityEngine;
using UnityEngine.SceneManagement;

public class BoneController : MonoBehaviour
{
    public int maxCycles = 5;
    public float forceMultiplier = 1f;
    public float learningRate = 0.0001f;
    public int cycleTruncate = 10000;

    public hudController hud;

    private int currentCycle = 0;
    private int lastLearningCycle = 0;
    private int totalCycles = 0;

    private bool isPaused = false;
    private bool truncated = false;

    private Vector3 initialGravity = Physics.gravity;
    private Vector3 storedVelocity = Vector3.zero;
    private Vector3 storedAngularVelocity = Vector3.zero;

    private float weight;

    private Rigidbody rb;
    private Collider col;

    private float magnitude = 0f;
    private Vector3 position = Vector3.zero;
    private Vector3 direction = Vector3.zero;

    private float maxHeight = 0f;
    private float maxMagnitude = 0f;
    private Vector3 maxDirection = Vector3.zero;

    private Vector3 maxPosition = Vector3.zero;
    private Quaternion maxRotation = Quaternion.identity;

    private float magnitudeModifier = 0f;
    private float directionModifier = 0f;

    void Awake(){
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        isPaused = false;
        truncated = false;
        rb.isKinematic = false;
        Physics.gravity = initialGravity;
        currentCycle = 0;
        lastLearningCycle = 0;
        totalCycles = 0;
    }

    void Start()
    {
        weight = rb.mass*Physics.gravity.magnitude;
        isPaused = false;
        truncated = false;
        rb.isKinematic = false;
        Physics.gravity = initialGravity;
        currentCycle = 0;
        lastLearningCycle = 0;
        totalCycles = 0;
    }

    void Update()
    {
        quitCheck();
        resetCheck();
        pauseCheck();
        updateHUD();

        if ((magnitudeModifier >= 1f || directionModifier >= 1f || (totalCycles - lastLearningCycle) >= cycleTruncate) && !truncated)
        {
            truncated = true;
            Debug.Log("Training truncated after " + totalCycles + " total cycles.");
            isPaused = true;
            toggleGravityAndVelocity();
            GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            capsule.transform.position = maxPosition;
            capsule.transform.rotation = maxRotation;
            DebugDrawArrow(maxPosition, maxMagnitude * maxDirection * weight, Color.red);
        }
        
        if (currentCycle < maxCycles && !isPaused){
            currentCycle++;
            totalCycles++;
        }
        else if (!isPaused){
            currentCycle = 0;
            totalCycles++;

            if (rb.position.y >= maxHeight)
            {
                maxHeight = rb.position.y;
                Debug.Log("Max Height: " + maxHeight + " after " + totalCycles + " total cycles.");
                maxMagnitude = magnitude;
                maxDirection = direction;

                maxPosition = rb.position;
                maxRotation = rb.rotation;

                magnitudeModifier += learningRate;
                directionModifier += learningRate;
                lastLearningCycle = totalCycles;
            }
            ApplyForceAtCenterOfMass();
        }
        DebugDrawArrow(position, magnitude * direction * weight, Color.green);
    }

    void ApplyForceAtCenterOfMass()
    {
        magnitude = Random.Range(0f, 1f) * (1 - magnitudeModifier) + (maxMagnitude * magnitudeModifier);
        magnitude = magnitude * forceMultiplier;

        position = rb.position;

        direction = (
            new Vector3(
                Random.Range(0f, 1f),
                Random.Range(0f, 1f),
                Random.Range(0f, 1f)
            ) * 
            (1 - directionModifier) +
            (maxDirection * directionModifier)
        );

        rb.AddForce(magnitude * direction * weight, ForceMode.Impulse);
    }

    (float, Vector3, Vector3) ApplyForceAtRandomPosition()
    {
        magnitude = Random.Range(0f, 1f) * forceMultiplier;

        position = new Vector3(
            Random.Range(col.bounds.min.x, col.bounds.max.x),
            Random.Range(col.bounds.min.y, col.bounds.max.y),
            Random.Range(col.bounds.min.z, col.bounds.max.z)
        );

        direction = new Vector3(
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            Random.Range(0f, 1f)
        );

        rb.AddForceAtPosition(magnitude * direction * weight, position);
        return (magnitude, position, direction);
    }

    void DebugDrawArrow(Vector3 start, Vector3 direction, Color color)
    {
        if (direction == Vector3.zero)
        {
            return; // Exit the method if direction is zero.
        }

        Debug.DrawRay(start, direction, color);

        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + 30, 0) * new Vector3(0, 0, 1);
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - 30, 0) * new Vector3(0, 0, 1);
        
        Debug.DrawRay(start + direction, right * 0.25f, color);
        Debug.DrawRay(start + direction, left * 0.25f, color);
    }

    void pauseCheck(){
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isPaused = !isPaused; // toggle pause
            toggleGravityAndVelocity();
        }
    }

    void toggleGravityAndVelocity(){
        if (Physics.gravity == Vector3.zero && !isPaused)
        {
            Physics.gravity = initialGravity;
            rb.isKinematic = false;
            rb.velocity = storedVelocity;
            rb.angularVelocity = storedAngularVelocity;
        }
        else
        {
            Physics.gravity = Vector3.zero;
            storedVelocity = rb.velocity;
            storedAngularVelocity = rb.angularVelocity;
            rb.isKinematic = true;
        } 
    }

    void quitCheck(){
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();

            // If you are in the editor it won't quit the application,
            // so we use this line to stop playing the scene
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }

    void resetCheck(){
        if (Input.GetKeyDown(KeyCode.R))
        {
            isPaused = false;
            truncated = false;
            rb.isKinematic = false;
            Physics.gravity = initialGravity;
            currentCycle = 0;
            lastLearningCycle = 0;
            totalCycles = 0;
            SceneManager.LoadScene("Main");
        }
    }

    void updateHUD(){
        if (truncated) {
            hud.state = "Terminated";
        } 
        else if (isPaused) {
            hud.state = "Paused";
        }
        else {
            hud.state = "Running";
        }

        hud.generation = totalCycles;
        hud.maxHeight = maxHeight;
        hud.magnitudeModifier = magnitudeModifier;
        hud.directionModifier = directionModifier;
    }
}
