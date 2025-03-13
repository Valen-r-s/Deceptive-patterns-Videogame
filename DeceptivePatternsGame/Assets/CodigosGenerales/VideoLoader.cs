using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Audio;

public class VideoLoader : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Arrastra aquí el VideoPlayer desde el Inspector
    public AudioSource audioSource; // Arrastra aquí el AudioSource desde el Inspector
    public string videoFileName = "VideoOutro.mp4"; // Cambia por el nombre de tu video
    public AudioClip backgroundMusic; // Arrastra aquí el clip de audio desde el Inspector

    void Start()
    {
        string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);

        // Configura el VideoPlayer
        videoPlayer.url = videoPath;
        videoPlayer.Play(); // Reproduce el video automáticamente al iniciar

        // Configura el evento para cuando el video termine
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        // Reproduce la música en bucle
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.Play();
    }
}
