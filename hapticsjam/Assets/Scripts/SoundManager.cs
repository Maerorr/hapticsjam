using System.Collections;
using DG.Tweening;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource soundSource;
    public AudioSource sonarSource;
    public AudioSource deafSource;
    public AudioSource rocketLaunchSource;
    public AudioSource rocketMineHitSource;
    public AudioSource rocketMissSource;
    public AudioSource nextLevelSound;

    private float boatX = 7.5f;

    public void CheckSound(Vector2Int hit, Vector2Int minePos)
    {
        float distance = Mathf.Abs(hit.x - minePos.x) + Mathf.Abs(hit.y - minePos.y);
        distance /= 19.0f;
        soundSource.pitch = 1.0f - (0.75f * distance) + 0.1f;
        soundSource.Play();
    }

    public void SonarSound(int x)
    {
        sonarSource.panStereo = CalculatePanFromX(x);
        sonarSource.Play();
    }

    public void DeafSound()
    {
        deafSource.Play();
    }

    public void LaunchRocketSound(int x, bool hit)
    {
        float pan = CalculatePanFromX(x);
        rocketLaunchSource.Play();
        DOTween.To(() => rocketLaunchSource.panStereo, x => rocketLaunchSource.panStereo = x, pan, 1.0f);
        StartCoroutine(RocketHitDelay(hit, pan));
    }

    IEnumerator RocketHitDelay(bool hit, float pan)
    {
        yield return new WaitForSeconds(1.5f);
        if (hit)
        {
            rocketMineHitSource.panStereo = pan;
            rocketMineHitSource.Play();
        }
        else
        {
            rocketMissSource.panStereo = pan;
            rocketMissSource.Play();
        }
    }

    private float CalculatePanFromX(int x)
    {
        float distance = (x - boatX) / boatX;
        if (distance < 0)
        {
            distance = -Mathf.Sqrt(-distance);
        }
        else
        {
            distance = Mathf.Sqrt(distance);
        }
        return distance;
    }

    public void NextLevelSound()
    {
        nextLevelSound.Play();
    }
}
