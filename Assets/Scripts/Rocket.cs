using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    [SerializeField] private float rotationThrust = 150.0f;
    [SerializeField] private float mainThrust = 50.0f;
    [SerializeField] private AudioClip mainEngine;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip successSound;

    private AudioSource audioSource;
    private Rigidbody rigidBody;

    private int currentSceneNumber;
    private Scene currentScene;

    enum State { Alive, Dying, Transcending }
    State state = State.Alive;

	// Use this for initialization
	void Start ()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update ()
    {
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }

    }

    private void RespondToRotateInput()
    {
        rigidBody.freezeRotation = true;
        float rotationSpeed = rotationThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationSpeed);
        }

        rigidBody.freezeRotation = false;
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(successSound);
        Invoke("LoadNextLevel", 1.5f);
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
        Invoke("LoadLevelAfterDeath", 1.5f);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadLevelAfterDeath()
    {
        currentScene = SceneManager.GetActiveScene();
        currentSceneNumber = currentScene.buildIndex;
        if (currentSceneNumber != 1)
        {
            LoadFirstLevel();
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }
}
