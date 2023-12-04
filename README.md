Merge
===


게임 장르 : Merge(합치기)
---

게임 소개 : 
더 크게 합쳐보자!
---


개발 목적 : 편리한 조작감 구현
---

사용 엔진 : UNITY 2021.3.17f1
---


개발 기간 : 2023.09 ~ 2023.11
---


포트폴리오 영상
---
[유튜브 영상 링크](링크예정)


빌드 파일
---
[구글 드라이브 다운로드 링크](https://drive.google.com/file/d/1JRSGiZR3P5XjKuaMydYr0ob5zu1n1pqp/view?usp=drive_link)

* 시작 시 음향이 클 수 있으므로 주의 바랍니다.
* 모바일 버전입니다. Apk 파일 형식입니다.

다른 포트폴리오 보기
---
[Project Ark](https://github.com/Nocha13/Ark_2DPortfolio.git)

주요 활용 기술
---
* #01)([스크립트](https://github.com/Nocha13/Merge_2DPortfolio/blob/main/Merge/Assets/02.Code/InGame/Merge.cs#L123)) [오브젝트 기본 로직(합치기)]
<details>
<summary>예시 코드</summary>
  
```csharp
public void 함수 이름()
{
    void OnCollisionStay2D(Collision2D coll)
    {
        if(coll.gameObject.tag == "Merge")
        {
            Merge other = coll.gameObject.GetComponent<Merge>();

            if(level == other.level && !isMerge && !other.isMerge && level < 7)
            {//Start Merge
                float myX = transform.position.x;
                float myY = transform.position.y;
                float otherX = other.transform.position.x;
                float otherY = other.transform.position.y;

                //아래 있을 때
                //동일 높일 때, 오른쪽, 왼쪽에 있을 때
                if(myY < otherY || (myY == otherY && myX > otherX && myX < otherX))
                {//상대방 숨기기
                    other.Hide(transform.position);
                    LevelUp();
                }
            }
        }
    }
}
```
</details>

---
* #02)([스크립트](https://github.com/Nocha13/Merge_2DPortfolio/blob/main/Merge/Assets/02.Code/InGame/GameManager.cs#L67)) [오브젝트 풀링 생성]

<details>
<summary>예시 코드</summary>
  
```csharp
 // ORDER : #02) 오브젝트 풀링 생성
    void NextMerge()
    {
        if (isOver)
            return;

        //생성시 조건 정보
        lastMerge = PoolMgr.poolInst.GetMerge();
        lastMerge.Inst = this;
        lastMerge.level = Random.Range(0, maxLevel);
        lastMerge.gameObject.SetActive(true);
        AudioMgr.Inst.PlaySfx(AudioMgr.SFX.Next);
        isStart = true;
        StartCoroutine(WaitNext());
    }

    IEnumerator WaitNext()
    {
        while (lastMerge != null)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f); //0.5초마다 생성
        NextMerge();
    }
```
</details>
