using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MusicPlayer_KJS : MonoBehaviour
{
    [Header("UI Components")]
    public TMP_Dropdown musicDropdown; // TextMeshPro Dropdown
    public string musicFolderPath = "Music"; // Default folder relative to project or executable
    public AudioSource audioSource; // AudioSource to play selected music

    private string[] musicFiles;

    void Start()
    {
        PopulateDropdown();
        musicDropdown.onValueChanged.AddListener(OnMusicSelected);
    }

    private void PopulateDropdown()
    {
        // Ensure the directory exists
        string fullPath = Path.Combine(Application.dataPath, musicFolderPath);
        if (!Directory.Exists(fullPath))
        {
            Debug.LogWarning($"Music folder not found at path: {fullPath}");
            return;
        }

        // Get all .mp3 and .wav files in the directory
        musicFiles = Directory.GetFiles(fullPath, "*.*", SearchOption.TopDirectoryOnly)
            .Where(file => file.EndsWith(".mp3") || file.EndsWith(".wav"))
            .ToArray();

        // Clear existing options
        musicDropdown.ClearOptions();

        // Add file names (without paths) to the dropdown
        var options = musicFiles.Select(file => Path.GetFileName(file)).ToList();
        musicDropdown.AddOptions(options);
    }

    private void OnMusicSelected(int index)
    {
        if (musicFiles == null || musicFiles.Length <= index)
            return;

        string selectedFile = musicFiles[index];

        // Load and play the selected music file
        StartCoroutine(LoadAndPlayMusic(selectedFile));
    }

    private IEnumerator LoadAndPlayMusic(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError($"File not found: {filePath}");
            yield break;
        }

        // Load audio file as a UnityWebRequest (works for local files too)
        using (var www = UnityEngine.Networking.UnityWebRequestMultimedia.GetAudioClip("file://" + filePath, AudioType.UNKNOWN))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityEngine.Networking.UnityWebRequest.Result.ConnectionError ||
                www.result == UnityEngine.Networking.UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error loading audio file: {www.error}");
                yield break;
            }

            AudioClip audioClip = UnityEngine.Networking.DownloadHandlerAudioClip.GetContent(www);

            // Play the loaded audio clip
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }
}
