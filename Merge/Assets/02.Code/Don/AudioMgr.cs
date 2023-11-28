using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMgr : MonoBehaviour
{
    public enum BGM
    {
        Title = 0,
        //InGameBGM = 1,
    }

    public enum SFX
    {
        LevelUp,    //A,B,C
        Next = 3,
        Button,
        Attach,
        GameOver
    }

    public static AudioMgr Inst;

    [Header("---BGM---")] //배경음 관련
    public AudioClip[] bgmClip;
    AudioSource[] bgmPlayer = null;
    public float bgmVol = 1;
    public int bgmChannels;    //Bgm채널 수
    int bgmChIdx;              //Bgm채널 번호

    [Header("---SFX---")] //효과음 관련
    public AudioClip[] sfxClip;
    AudioSource[] sfxPlayer = null;
    public float sfxVol = 1;
    public int sfxChannels;    //Sfx채널 수
    int sfxChIdx;              //Sfx채널 번호

    [Header("---Object")]
    public AudioMixer masterMixer;
    [HideInInspector] public bool isBgmOnOff = true;
    [HideInInspector] public bool isSfxOnOff = true;

    void Awake()
    {
        Inst = this;
        Init();
    }

    public void BGMOnOff(bool a_bgmOnOff = true)
    {
        bool a_bgmMuteOnOff = !a_bgmOnOff;

        for(int idx = 0; idx < bgmChannels; idx++)
        {
            if (bgmPlayer[idx] != null)
                bgmPlayer[idx].mute = a_bgmMuteOnOff;
        }
        isBgmOnOff = a_bgmOnOff;
    }

    public void SFXOnOff(bool a_sfxOnOff = true)
    {
        bool a_sfxMuteOff = !a_sfxOnOff;

        for (int idx = 0; idx < sfxChannels; idx++)
        {
            if (sfxPlayer[idx] != null)
            {
                sfxPlayer[idx].mute = a_sfxMuteOff;
            }
        }

        isSfxOnOff = a_sfxOnOff;
    }

    void Init() //플레이어 초기화
    {
        //BGM 
        GameObject bgmObj = new GameObject("BGMPlayer");
        bgmObj.transform.parent = transform;                        //배경음 자식 오브젝트 생성
        bgmPlayer = new AudioSource[bgmChannels];                   //채널 값 이용, 오디오소스 배열 초기화
        
        for (int idx = 0; idx < bgmPlayer.Length; idx++)
        {
            bgmPlayer[idx] = bgmObj.AddComponent<AudioSource>();    //모든 효과음 오디오소스 생성, 저장 반복       
            bgmPlayer[idx].loop = true;                             //반복 재생
            bgmPlayer[idx].playOnAwake = false;                     //시작시 재생 OFF
            bgmPlayer[idx].bypassListenerEffects = true;            //AudioLowPassFilter 영향 받지 않도록 ON
            bgmPlayer[idx].volume = bgmVol;                         //볼륨
            bgmPlayer[idx].outputAudioMixerGroup = masterMixer.FindMatchingGroups("BGM")[0];
        }                        

        //SFX 
        GameObject sfxObj = new GameObject("SFXPlayer");
        sfxObj.transform.parent = transform;                        //배경음 자식 오브젝트 생성
        sfxPlayer = new AudioSource[sfxChannels];                   //채널 값 이용, 오디오소스 배열 초기화

        for (int idx = 0; idx < sfxPlayer.Length; idx++)
        {
            sfxPlayer[idx] = sfxObj.AddComponent<AudioSource>();    //모든 효과음 오디오소스 생성, 저장 반복
            sfxPlayer[idx].playOnAwake = false;                     //시작시 재생 OFF
            sfxPlayer[idx].bypassListenerEffects = true;            //AudioLowPassFilter 영향 받지 않도록 ON
            sfxPlayer[idx].volume = sfxVol;                         //볼륨          
            sfxPlayer[idx].outputAudioMixerGroup = masterMixer.FindMatchingGroups("SFX")[0];
        }

        int bgmOnOff = PlayerPrefs.GetInt("BgmOnOff", 1);
        int sfxOnOff = PlayerPrefs.GetInt("SfxOnOff", 1);

        if (bgmOnOff == 1)
            BGMOnOff(true);
        else
            BGMOnOff(false);

        if (sfxOnOff == 1)
            SFXOnOff(true);
        else
            SFXOnOff(false);
    }

    public void PlayBgm(BGM bgm)
    {
        for (int idx = 0; idx < bgmPlayer.Length; idx++)
        {
            //채널 수만큼 순회
            int loopIdx = (idx + bgmChIdx) % bgmPlayer.Length;

            if (bgmPlayer[loopIdx].isPlaying)
                continue;    //반복문 [다음 루프]로 건너뜀

            bgmChIdx = loopIdx;
            bgmPlayer[loopIdx].clip = bgmClip[(int)bgm];
            bgmPlayer[loopIdx].Play();
            break;          //효과음 재생 후 반복문 종료
        }
    }

    public void PlayBgm(bool isPlay)
    {
        for (int idx = 0; idx < bgmPlayer.Length; idx++)
        {
            if (isPlay)
                bgmPlayer[idx].Play();

            else
                bgmPlayer[idx].Stop();
        }
    }

    public void PlaySfx(SFX sfx)
    {
        for (int idx = 0; idx < sfxPlayer.Length; idx++)
        {
            //채널 수만큼 순회
            int loopIdx = (idx + sfxChIdx) % sfxPlayer.Length;

            if (sfxPlayer[loopIdx].isPlaying)
                continue;    //반복문 [다음 루프]로 건너뜀

            int ranIdx = 0; //같은 이름 효과음 랜덤 재생
            if (sfx == SFX.LevelUp)
            {
                ranIdx = Random.Range(0, 3);
            }

            sfxChIdx = loopIdx;
            sfxPlayer[loopIdx].clip = sfxClip[(int)sfx + ranIdx];
            sfxPlayer[loopIdx].Play();
            break;          //효과음 재생 후 반복문 종료
        }
    }
}
