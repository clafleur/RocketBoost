using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    public bool collisionsDisabled = false;
    [SerializeField] private float rotationThrust = 150.0f;
    [SerializeField] private float mainThrust = 50.0f;
    [SerializeField] private AudioClip mainEngine;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip successSound;
    [SerializeField] private float levelLoadDelay = 1.5f;

    [SerializeField] private ParticleSystem mainEnginParticle;
    [SerializeField] private ParticleSystem crashParticle;
    [SerializeField] private ParticleSystem successParticle;

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

        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(new Vector3(0f,0f, -horizontalInput) * rotationSpeed);

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
            mainEnginParticle.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        mainEnginParticle.Play();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || collisionsDisabled)
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
        successParticle.Play();
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
        crashParticle.Play();

        Invoke("LoadLevelAfterDeath", levelLoadDelay);
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
