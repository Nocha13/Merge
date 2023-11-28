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

    [Header("---BGM---")] //����� ����
    public AudioClip[] bgmClip;
    AudioSource[] bgmPlayer = null;
    public float bgmVol = 1;
    public int bgmChannels;    //Bgmä�� ��
    int bgmChIdx;              //Bgmä�� ��ȣ

    [Header("---SFX---")] //ȿ���� ����
    public AudioClip[] sfxClip;
    AudioSource[] sfxPlayer = null;
    public float sfxVol = 1;
    public int sfxChannels;    //Sfxä�� ��
    int sfxChIdx;              //Sfxä�� ��ȣ

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

    void Init() //�÷��̾� �ʱ�ȭ
    {
        //BGM 
        GameObject bgmObj = new GameObject("BGMPlayer");
        bgmObj.transform.parent = transform;                        //����� �ڽ� ������Ʈ ����
        bgmPlayer = new AudioSource[bgmChannels];                   //ä�� �� �̿�, ������ҽ� �迭 �ʱ�ȭ
        
        for (int idx = 0; idx < bgmPlayer.Length; idx++)
        {
            bgmPlayer[idx] = bgmObj.AddComponent<AudioSource>();    //��� ȿ���� ������ҽ� ����, ���� �ݺ�       
            bgmPlayer[idx].loop = true;                             //�ݺ� ���
            bgmPlayer[idx].playOnAwake = false;                     //���۽� ��� OFF
            bgmPlayer[idx].bypassListenerEffects = true;            //AudioLowPassFilter ���� ���� �ʵ��� ON
            bgmPlayer[idx].volume = bgmVol;                         //����
            bgmPlayer[idx].outputAudioMixerGroup = masterMixer.FindMatchingGroups("BGM")[0];
        }                        

        //SFX 
        GameObject sfxObj = new GameObject("SFXPlayer");
        sfxObj.transform.parent = transform;                        //����� �ڽ� ������Ʈ ����
        sfxPlayer = new AudioSource[sfxChannels];                   //ä�� �� �̿�, ������ҽ� �迭 �ʱ�ȭ

        for (int idx = 0; idx < sfxPlayer.Length; idx++)
        {
            sfxPlayer[idx] = sfxObj.AddComponent<AudioSource>();    //��� ȿ���� ������ҽ� ����, ���� �ݺ�
            sfxPlayer[idx].playOnAwake = false;                     //���۽� ��� OFF
            sfxPlayer[idx].bypassListenerEffects = true;            //AudioLowPassFilter ���� ���� �ʵ��� ON
            sfxPlayer[idx].volume = sfxVol;                         //����          
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
            //ä�� ����ŭ ��ȸ
            int loopIdx = (idx + bgmChIdx) % bgmPlayer.Length;

            if (bgmPlayer[loopIdx].isPlaying)
                continue;    //�ݺ��� [���� ����]�� �ǳʶ�

            bgmChIdx = loopIdx;
            bgmPlayer[loopIdx].clip = bgmClip[(int)bgm];
            bgmPlayer[loopIdx].Play();
            break;          //ȿ���� ��� �� �ݺ��� ����
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
            //ä�� ����ŭ ��ȸ
            int loopIdx = (idx + sfxChIdx) % sfxPlayer.Length;

            if (sfxPlayer[loopIdx].isPlaying)
                continue;    //�ݺ��� [���� ����]�� �ǳʶ�

            int ranIdx = 0; //���� �̸� ȿ���� ���� ���
            if (sfx == SFX.LevelUp)
            {
                ranIdx = Random.Range(0, 3);
            }

            sfxChIdx = loopIdx;
            sfxPlayer[loopIdx].clip = sfxClip[(int)sfx + ranIdx];
            sfxPlayer[loopIdx].Play();
            break;          //ȿ���� ��� �� �ݺ��� ����
        }
    }
}
