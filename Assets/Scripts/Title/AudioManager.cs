using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;

/// <see cref="https://note.com/calm_otter351/n/need65fbb2810/">【Unity】初心者でもわかる！AudioMixerを使った スライダーで音量調節する機能の作り方</see>

public class AudioManager : MonoBehaviour {
    // AudioMixerの参照を格納
    [SerializeField] private AudioMixer audioMixer;

    // BGMとSE用のスライダーを格納
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider seSlider;

    private const string BgmParameter = "BGM";
    private const string SeParameter = "SE";

    private void Start() {
        InitializeSlider(bgmSlider, BgmParameter);
        InitializeSlider(seSlider, SeParameter);
    }

    /// <summary>
    /// スライダーの初期値をAudioMixerから取得して設定
    /// </summary>
    /// <param name="slider">対象のスライダー</param>
    /// <param name="parameterName">AudioMixerのパラメータ名</param>
    private void InitializeSlider(Slider slider, string parameterName) {
        if (audioMixer.GetFloat(parameterName, out float volume)) {
            slider.value = volume;
        } else {
            Debug.LogWarning($"{parameterName} の取得に失敗しました。");
        }
    }

    /// <summary>
    /// BGMの音量を設定
    /// </summary>
    /// <param name="volume">設定する音量</param>
    public void SetBgmVolume(float volume) {
        SetVolume(BgmParameter, volume);
    }

    /// <summary>
    /// SEの音量を設定
    /// </summary>
    /// <param name="volume">設定する音量</param>
    public void SetSeVolume(float volume) {
        SetVolume(SeParameter, volume);
    }

    /// <summary>
    /// 指定されたパラメータの音量を設定
    /// </summary>
    /// <param name="parameterName">AudioMixerのパラメータ名</param>
    /// <param name="volume">設定する音量</param>
    private void SetVolume(string parameterName, float volume) {
        if (!audioMixer.SetFloat(parameterName, volume)) {
            Debug.LogWarning($"{parameterName} の音量設定に失敗しました。");
        }
    }
}