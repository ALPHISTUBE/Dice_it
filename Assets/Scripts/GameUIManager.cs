using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [Header("Value")]
    [SerializeField] private float offset;
    [SerializeField] private float time;
    [SerializeField] private BoxController boxController;

    [Header("UI Element")]
    [SerializeField] private Transform settingB;
    [SerializeField] private GameObject settingUI;
    [SerializeField] private Transform skipB;
    [SerializeField] private Transform resetB;
    [SerializeField] private TextMeshPro levelText;
    [SerializeField] private TMP_Dropdown qualitySetting;
    [SerializeField] private Toggle postEffect;
    [SerializeField] private Slider music;
    [SerializeField] private Slider sfx;

    [Header("VFX")]
    [SerializeField] private Volume vfx;
    [SerializeField] private VolumeProfile main;
    [SerializeField] private VolumeProfile blur;

    private void Start()
    {
        //levelText.text = 0.ToString();
    }

    public void DoSetting()
    {
        StartCoroutine(SettingAnim());
        settingUI.SetActive(true);
    }

    public void CloseSetting()
    {
        vfx.profile = main;
        settingUI.SetActive(false);
    }

    public void DoSkip()
    {
        StartCoroutine(boxController.SkipLevel());
        StartCoroutine(SkipAnim());
    }

    public void DoResetLevel()
    {
        boxController.ResetLevel();
        StartCoroutine(ResetLevelAnim());
    }

    public void DoMainManu()
    {
        SceneManager.LoadScene(0);
    }

    public void DoQualityChange()
    {
        QualitySettings.SetQualityLevel(qualitySetting.value);
    }

    public void DoPostEffect()
    {
        vfx.enabled = postEffect.isOn;
    }

    public void DoMusicVolume()
    {

    }

    public void DoSfxVolume()
    {

    }

    //Animation
    public IEnumerator SettingAnim()
    {
        settingB.position = Vector3.down * offset + settingB.position;
        yield return new WaitForSeconds(time);
        settingB.position = Vector3.up * offset + settingB.position;
    }

    public IEnumerator SkipAnim()
    {
        skipB.position = Vector3.down * offset + skipB.position;
        yield return new WaitForSeconds(time);
        skipB.position = Vector3.up * offset + skipB.position;
    }

    public IEnumerator ResetLevelAnim()
    {
        resetB.position = Vector3.down * offset + resetB.position;
        yield return new WaitForSeconds(time);
        resetB.position = Vector3.up * offset + resetB.position;
    }
}
