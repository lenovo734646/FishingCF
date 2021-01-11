
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : DDOLSingleton<AudioManager>
{
    private float bgmVolumScale = 1f;                                          //背景音乐声音大小比例
    private float effVolumScale = 0.5f;                                        //音效声音大小比例
    public List<AudioSource> audioSourceList = new List<AudioSource>();        //音效播放器
    private int audioSourceNum = 20;                                           //音效播放器个数
    private AudioSource musicAudio;                                            //背景音乐播放器
    private Dictionary<string, AudioData> audioDic;                            //音效文件缓存

    public override void Init()
    {

    }

    private void Awake()
    {
        audioDic = new Dictionary<string, AudioData>();
        gameObject.AddComponent<AudioListener>();
        if (PlayerPrefs.HasKey("bgmVolumScale"))
            bgmVolumScale = PlayerPrefs.GetFloat("bgmVolumScale");
        if (PlayerPrefs.HasKey("effVolumScale"))
            effVolumScale = PlayerPrefs.GetFloat("effVolumScale");
        musicAudio = gameObject.AddComponent<AudioSource>();
        musicAudio.playOnAwake = false;
        musicAudio.loop = true;
        musicAudio.volume = bgmVolumScale;
        initAudioSources();
    }

    //初始化as组件播放器
    private void initAudioSources()
    {
        for (int i = 0; i < audioSourceNum; i++)
        {
            var obj = new GameObject("AudioSource" + (i + 1));
            obj.transform.SetParent(transform);
            var audio = obj.AddComponent<AudioSource>();
            audio.playOnAwake = false;
            audio.loop = false;
            audioSourceList.Add(audio);
        }
    }

    //解暂停
    public void UnPause()
    {
        musicAudio.UnPause();
        foreach (var item in audioSourceList)
            item.UnPause();
    }

    //暂停所有声音
    public void Pause()
    {
        musicAudio.Pause();
        foreach (var item in audioSourceList)
            item.Pause();
    }

    //停止所有声音
    public void Stop()
    {
        musicAudio.Stop();
        foreach (var item in audioSourceList)
            item.Stop();
    }

    //设置静音
    public void SetMute(bool isMute)
    {
        musicAudio.mute = isMute;
        foreach (var item in audioSourceList)
            item.mute = isMute;
    }

    /// <summary>
    /// 设置背景音乐
    /// </summary>
    /// <param name="scale">音乐大小[0,1]</param>
    public void SetBGMVolumScale(float scale)
    {
        bgmVolumScale = scale;
        musicAudio.volume = bgmVolumScale;
        PlayerPrefs.SetFloat("bgmVolumScale", bgmVolumScale);
    }

    /// <summary>
    /// 设置音效
    /// </summary>
    /// <param name="scale">音效大小[0,1]</param>
    public void SetEFFVolumScale(float scale)
    {
        effVolumScale = scale;
        PlayerPrefs.SetFloat("effVolumScale", effVolumScale);
    }

    //获取背景音乐音量[0,1]
    public float GetBGMVolumScale()
    {
        return bgmVolumScale;
    }

    //获取音效音量[0,1]
    public float GetEFFVolumScale()
    {
        return effVolumScale;
    }

    //播放背景音乐
    public void PlayMusic(string music)
    {
        if (IsPlayMusic(music))
            return;
        var data = GetClipByName(music, false);
        if (ReferenceEquals(null, data)) return;
        //AudioClip clip = GetClipByName(music, false);
        float vol = 0.6f + data.Volume * 0.02f * 0.2f;
        PlayMusic(data.Clip, vol);
    }

    //是否正在播放music
    public bool IsPlayMusic(string music)
    {
        if (musicAudio.isPlaying)
            return musicAudio.clip.name == music;
        return false;
    }

    //是否正在播放背景音乐
    public bool IsPlayMusic()
    {
        return musicAudio.isPlaying;
    }

    //是否正在播放音效
    public bool IsPlaySoundEff(string eff)
    {
        for (int i = 0; i < audioSourceList.Count; i++)
        {
            var audio = audioSourceList[i];
            if (audio.isPlaying && audio.clip.name == eff)
                return true;
        }
        return false;
    }

    //停止背景音乐
    public void StopMusic()
    {
        musicAudio.Stop();
    }

    //停止所有音效
    public void StopAllSoudEff()
    {
        foreach (var item in audioSourceList)
            item.Stop();
    }

    /// <summary>
    /// 停止某音效
    /// </summary>
    /// <param name="eff">音效名称</param>
    public void StopSoundEff(string eff)
    {
        foreach (var item in audioSourceList)
        {
            if (item.isPlaying && item.clip.name == eff)
                item.Stop();
        }
    }

    /// <summary>
    /// 停止某音效
    /// </summary>
    /// <param name="clip">音效</param>
    public void StopSoundEff(AudioClip clip)
    {
        StopSoundEff(clip.name);
    }

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="clip">音频</param>
    public void PlayMusic(AudioClip clip, float volume = 1)
    {
        musicAudio.clip = clip;
        musicAudio.volume = bgmVolumScale * volume;
        musicAudio.Play();
    }

    /// <summary>
    /// 播放2d音效
    /// </summary>
    /// <param name="eff">音效名称</param>
    /// <param name="loop">是否循环</param>
    public void PlaySoundEff2D(string eff, bool loop = false, float volume = 1)
    {
        var data = GetClipByName(eff, true);
        if (ReferenceEquals(null, data)) return;
        //AudioClip clip = GetClipByName(eff, true);
        //float vol = volume * (0.6f + data.Volume * 0.02f * 0.2f);
        PlaySoundEff2D(data.Clip, loop, 0.6f + 0.2f * data.Volume * 0.02f);
    }

    /// <summary>
    /// 播放2d音效
    /// </summary>
    /// <param name="clip">音频</param>
    /// <param name="loop">是否循环</param>
    public void PlaySoundEff2D(AudioClip clip, bool loop = false, float volume = 1)
    {
        if (clip == null)
            return;
        AudioSource audioSrc = GetCurrentAudioSource();
        audioSrc.loop = loop;
        audioSrc.clip = clip;
        audioSrc.volume = effVolumScale * volume;
        audioSrc.Play();
    }

    //获取当前可用播放器
    private AudioSource GetCurrentAudioSource()
    {
        AudioSource audioSrc = null;
        for (int i = 0, imax = audioSourceList.Count; i < imax; i++)
        {
            if (audioSourceList[i].isPlaying == false)
                audioSrc = audioSourceList[i];
        }
        if (ReferenceEquals(audioSrc, null))
            audioSrc = audioSourceList[0];
        return audioSrc;
    }

    /// <summary>
    /// 根据字符串找到clip
    /// </summary>
    /// <param name="path">音效地址</param>
    /// <param name="cache">是否缓存</param>
    /// <returns></returns>
    public AudioData GetClipByName(string path, bool cache)
    {
        if (audioDic.ContainsKey(path))
            return audioDic[path];

        var obj = ResManager.Instance.LoadPrefab(path);
        if (obj)
        {
            var data = ResManager.Instance.LoadPrefab(path).GetComponent<AudioData>();
            audioDic.Add(path, data);
            return data;
        }
        else
            return null;
    }
}
