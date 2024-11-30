using Firebase;
using Firebase.Auth;
using UnityEngine;
using System.Threading.Tasks;

public class MetaFireAuth : MonoBehaviour
{
    public static MetaFireAuth instance;

    private FirebaseAuth auth;
    private FirebaseUser user;

    // �ӽ� �α��� ����
    private const string TEMP_EMAIL = "hxh@gmail.com";
    private const string TEMP_PASSWORD = "123456"; // ���� ��ϵ� ��й�ȣ�� �Է��ϼ���.

    public bool IsLoggedIn => user != null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // �ٸ� ������ �̵��ص� ��ü ����
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeFirebase();
    }

    private void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += OnAuthStateChanged;
        OnAuthStateChanged(this, null);

        // Firebase �ʱ�ȭ �� �ڵ� �α��� �õ�
        SignInWithTemporaryAccount();
    }

    private void OnAuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("User signed out.");
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("User signed in: " + user.DisplayName);
            }
        }
    }

    public void SignInWithTemporaryAccount()
    {
        auth.SignInWithEmailAndPasswordAsync(TEMP_EMAIL, TEMP_PASSWORD).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailPassword was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailPassword encountered an error: " + task.Exception);
                return;
            }

            user = auth.CurrentUser;
            Debug.Log("User signed in successfully: " + user.Email);
        });
    }
}
