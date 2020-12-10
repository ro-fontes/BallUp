using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Audio;

public class MenuManager : MonoBehaviour
{
    GameObject PostFX;
    public AudioMixer masterMixer;
    public Animator menuAnim;
    public Button BotaoJogar, BotaoMultiplayer, BotaoOpcoes, BotaoOpcoes2, CustomizeButton, CustomizeButton2, BotaoSair, BotaoVoltar;
    public GameObject FirstButton, LocationsButton, skinbutton, ColorPickerButton, HomeButtonSelected;
    [Space(20)]
    public GameObject page1, page2, page3, page4, page5;
    [Space(20)]
    public Slider BarraVolume, BarSoundFX, BarSoundMusic, BarFPSLimit;
    public Toggle AnisotropicFiltering, VSync, CaixaModoJanela, MotionBlurBox, BloomBox, DepthOfFieldBox;
    public Dropdown Resolucoes, Qualidades;
    public GameObject ColorPickerUI;
    [Space(20)]
    public Text textoVol, txtStars, txtFragments, txtFPSMax;
    [Space(20)]
    public RectTransform Particle;
    public RectTransform Skins;
    [Space(20)]
    public RectTransform Window, Graphics, Audio, Data, Language;
    [Space(20)]
    public ControllerManager ControllerManager;


    int SettingsPage = 0;
    string nomeDaCena;
    float VOLUME, SFXSound, MusicSound;
    int qualidadeGrafica, modoJanelaAtivo, resolucaoSalveIndex, VSyncEnable, AnisotropicFilteringEnable,
        MotionBlurEnable, BloomEnable, DepthOfFieldEnable, FPSLimit;
    bool telaCheiaAtivada;
    bool OnController = true;

    Resolution[] resolucoesSuportadas;

    DepthOfField DepthOfField = null;
    MotionBlur MotionBlur = null;
    Bloom bloomLayer = null;
    AmbientOcclusion ambientOcclusionLayer = null;
    ColorGrading colorGradingLayer = null;

    [SerializeField]
    private Text[] Score;

    void Awake()
    {
        resolucoesSuportadas = Screen.resolutions;
        ColorPickerUI.GetComponent<ColorPickerUnityUI>().enabled = true;
    }

    void Start()
    {
        for (int i = 1; i < Score.Length; i++)
        {
            if (PlayerPrefs.HasKey(i + "LevelTime"))
            {
                Score[i - 1].text = FormatTime(PlayerPrefs.GetFloat(i + "LevelTime"));
            }
            else
            {
                Score[i - 1].text = "00:00.00";
            }
        }

        if (!PostFX)
        {
            PostFX = GameObject.Find("PostFX");
        }


        PostFX.GetComponent<PostProcessVolume>().profile.TryGetSettings(out DepthOfField);
        PostFX.GetComponent<PostProcessVolume>().profile.TryGetSettings(out MotionBlur);
        PostFX.GetComponent<PostProcessVolume>().profile.TryGetSettings(out bloomLayer);
        PostFX.GetComponent<PostProcessVolume>().profile.TryGetSettings(out ambientOcclusionLayer);
        PostFX.GetComponent<PostProcessVolume>().profile.TryGetSettings(out colorGradingLayer);

        DontDestroyOnLoad(PostFX);
        ColorPickerUI.GetComponent<ColorPickerUnityUI>().enabled = false;
        FirstButton.GetComponent<Animator>().updateMode = AnimatorUpdateMode.Normal;
        txtStars.text = PlayerPrefs.GetInt("Stars").ToString();
        txtFragments.text = PlayerPrefs.GetInt("Fragments").ToString();

        ChecarResolucoes();
        AjustarQualidades();

        if (PlayerPrefs.HasKey("RESOLUCAO"))
        {
            int numResoluc = PlayerPrefs.GetInt("RESOLUCAO");
            if (resolucoesSuportadas.Length <= numResoluc)
            {
                PlayerPrefs.DeleteKey("RESOLUCAO");
            }
        }
        
        nomeDaCena = SceneManager.GetActiveScene().name;
        Time.timeScale = 1;

        BarSoundFX.minValue = -80;
        BarSoundFX.maxValue = 5;
        BarSoundMusic.minValue = -80;
        BarSoundMusic.maxValue = 5;
        BarraVolume.minValue = -80;
        BarraVolume.maxValue = 5;

        //=============== SAVES===========//
        if (PlayerPrefs.HasKey("VOLUME"))
        {
            VOLUME = PlayerPrefs.GetFloat("VOLUME");
            BarraVolume.value = VOLUME;
            masterMixer.SetFloat("Master", VOLUME);
        }
        else
        {
            VOLUME = 0;
            PlayerPrefs.SetFloat("VOLUME", VOLUME);
            BarraVolume.value = VOLUME;
        }

        if (PlayerPrefs.HasKey("SFX"))
        {
            SFXSound = PlayerPrefs.GetFloat("SFX");
            BarSoundFX.value = SFXSound;
            masterMixer.SetFloat("SFX", SFXSound);
        }
        else
        {
            SFXSound = 0;
            PlayerPrefs.SetFloat("SFX", SFXSound);
            BarSoundFX.value = SFXSound;
        }

        if (PlayerPrefs.HasKey("Music"))
        {
            MusicSound = PlayerPrefs.GetFloat("Music");
            BarSoundMusic.value = MusicSound;
            masterMixer.SetFloat("Music", MusicSound);
        }
        else
        {
            MusicSound = 0;
            PlayerPrefs.SetFloat("Music", MusicSound);
            BarSoundMusic.value = MusicSound;
        }

        #region Set Start Box

        if (PlayerPrefs.HasKey("FPSLimit"))
        {
            FPSLimit = PlayerPrefs.GetInt("FPSLimit");
            BarFPSLimit.value = FPSLimit;
            Application.targetFrameRate = FPSLimit;
            txtFPSMax.text = FPSLimit.ToString();
        }
        else
        {
            FPSLimit = 60;
            PlayerPrefs.SetInt("FPSLimit", FPSLimit);
            Application.targetFrameRate = FPSLimit;
            txtFPSMax.text = FPSLimit.ToString();
        }

        //=============MODO JANELA===========//
        if (PlayerPrefs.HasKey("modoJanela"))
        {
            modoJanelaAtivo = PlayerPrefs.GetInt("modoJanela");
            if (modoJanelaAtivo == 1)
            {
                Screen.fullScreen = false;
                CaixaModoJanela.isOn = true;
            }
            else
            {
                Screen.fullScreen = true;
                CaixaModoJanela.isOn = false;
            }
        }
        else
        {
            modoJanelaAtivo = 0;
            PlayerPrefs.SetInt("modoJanela", modoJanelaAtivo);
            CaixaModoJanela.isOn = false;
            Screen.fullScreen = true;
        }


        //========RESOLUCOES========//
        if (modoJanelaAtivo == 1)
        {
            telaCheiaAtivada = false;
        }
        else
        {
            telaCheiaAtivada = true;
        }

        //VSync enable/disable
        if (PlayerPrefs.HasKey("VSync"))
        {
            VSyncEnable = PlayerPrefs.GetInt("VSync");
            if(VSyncEnable == 1)
            {
                QualitySettings.vSyncCount = 1;
                VSync.isOn = true;
            }
            else
            {
                QualitySettings.vSyncCount = 0;
                VSync.isOn = false;
            }
        }
        else
        {
            VSyncEnable = 1;
            PlayerPrefs.SetInt("VSync", VSyncEnable);
            VSync.isOn = true;
            QualitySettings.vSyncCount = 1;
        }


        if (PlayerPrefs.HasKey("Bloom"))
        {
            BloomEnable = PlayerPrefs.GetInt("Bloom");
            if (BloomEnable == 1)
            {
                bloomLayer.enabled.value = true;
                BloomBox.isOn = true;
            }
            else
            {
                bloomLayer.enabled.value = false;
                BloomBox.isOn = false;
            }
        }
        else
        {
            BloomEnable = 1;
            PlayerPrefs.SetInt("Bloom", BloomEnable);
            BloomBox.isOn = true;
            bloomLayer.enabled.value = true;
        }



        if (PlayerPrefs.HasKey("DepthOfField"))
        {
            DepthOfFieldEnable = PlayerPrefs.GetInt("DepthOfField");
            if (DepthOfFieldEnable == 1)
            {
                DepthOfField.enabled.value = true;
                DepthOfFieldBox.isOn = true;
            }
            else
            {
                DepthOfField.enabled.value = false;
                DepthOfFieldBox.isOn = false;
            }
        }
        else
        {
            DepthOfFieldEnable = 1;
            PlayerPrefs.SetInt("DepthOfField", DepthOfFieldEnable);
            DepthOfFieldBox.isOn = true;
            DepthOfField.enabled.value = true;
        }



        if (PlayerPrefs.HasKey("MotionBlur"))
        {
            MotionBlurEnable = PlayerPrefs.GetInt("MotionBlur");
            if (MotionBlurEnable == 1)
            {
                MotionBlur.enabled.value = true;
                MotionBlurBox.isOn = true;
            }
            else
            {
                MotionBlur.enabled.value = false;
                MotionBlurBox.isOn = false;
            }
        }
        else
        {
            MotionBlurEnable = 1;
            PlayerPrefs.SetInt("MotionBlur", MotionBlurEnable);
            MotionBlurBox.isOn = true;
            MotionBlur.enabled.value = true;
        }



        if (PlayerPrefs.HasKey("AnisotropicFiltering"))
        {
            AnisotropicFilteringEnable = PlayerPrefs.GetInt("AnisotropicFiltering");
            if(AnisotropicFilteringEnable == 1)
            {
                QualitySettings.anisotropicFiltering = UnityEngine.AnisotropicFiltering.ForceEnable;
                AnisotropicFiltering.isOn = true;
            }
            else
            {
                QualitySettings.anisotropicFiltering = UnityEngine.AnisotropicFiltering.Disable;
                AnisotropicFiltering.isOn = false;
            }
        }
        else
        {
            AnisotropicFilteringEnable = 1;
            PlayerPrefs.SetInt("AnisotropicFiltering", AnisotropicFilteringEnable);
            AnisotropicFiltering.isOn = true;
            QualitySettings.anisotropicFiltering = UnityEngine.AnisotropicFiltering.ForceEnable;
        }



        if (PlayerPrefs.HasKey("RESOLUCAO"))
        {
            resolucaoSalveIndex = PlayerPrefs.GetInt("RESOLUCAO");
            Screen.SetResolution(resolucoesSuportadas[resolucaoSalveIndex].width, resolucoesSuportadas[resolucaoSalveIndex].height, telaCheiaAtivada);
            Resolucoes.value = resolucaoSalveIndex;
        }
        else
        {
            resolucaoSalveIndex = (resolucoesSuportadas.Length - 1);
            Screen.SetResolution(resolucoesSuportadas[resolucaoSalveIndex].width, resolucoesSuportadas[resolucaoSalveIndex].height, telaCheiaAtivada);
            PlayerPrefs.SetInt("RESOLUCAO", resolucaoSalveIndex);
            Resolucoes.value = resolucaoSalveIndex;
        }



        //=========QUALIDADES=========//
        if (PlayerPrefs.HasKey("qualidadeGrafica"))
        {
            qualidadeGrafica = PlayerPrefs.GetInt("qualidadeGrafica");
            QualitySettings.SetQualityLevel(qualidadeGrafica);
            Qualidades.value = qualidadeGrafica;
        }
        else
        {
            QualitySettings.SetQualityLevel((QualitySettings.names.Length - 1));
            qualidadeGrafica = (QualitySettings.names.Length - 1);
            PlayerPrefs.SetInt("qualidadeGrafica", qualidadeGrafica);
            Qualidades.value = qualidadeGrafica;
        }


        #endregion

        // =========SETAR BOTOES==========//
        BotaoJogar.onClick = new Button.ButtonClickedEvent();
        BotaoOpcoes.onClick = new Button.ButtonClickedEvent();
        BotaoOpcoes2.onClick = new Button.ButtonClickedEvent();
        CustomizeButton.onClick = new Button.ButtonClickedEvent();
        CustomizeButton2.onClick = new Button.ButtonClickedEvent();
        BotaoVoltar.onClick = new Button.ButtonClickedEvent();
        BotaoJogar.onClick.AddListener(() => Jogar());
        CustomizeButton.onClick.AddListener(() => Customize());
        CustomizeButton2.onClick.AddListener(() => Customize());
        BotaoOpcoes.onClick.AddListener(() => Settings());
        BotaoOpcoes2.onClick.AddListener(() => Settings());
        BotaoVoltar.onClick.AddListener(() => Back());
    }

    string FormatTime(float time)
    {
        int intTime = (int)time;
        int minutes = intTime / 60;
        int seconds = intTime % 60;
        float fraction = time * 1000;
        fraction = (fraction % 1000);
        string timeText = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, fraction);
        return timeText;
    }

    //=========VOIDS DE CHECAGEM==========//
    private void ChecarResolucoes()
    {
        Resolution[] resolucoesSuportadas = Screen.resolutions;
        Resolucoes.options.Clear();
        for (int y = 0; y < resolucoesSuportadas.Length; y++)
        {
            Resolucoes.options.Add(new Dropdown.OptionData() { text = resolucoesSuportadas[y].width + "x" + resolucoesSuportadas[y].height });
        }
        Resolucoes.captionText.text = "Resolucao";
    }

    private void AjustarQualidades()
    {
        string[] nomes = QualitySettings.names;
        Qualidades.options.Clear();
        for (int y = 0; y < nomes.Length; y++)
        {
            Qualidades.options.Add(new Dropdown.OptionData() { text = nomes[y] });
        }
        Qualidades.captionText.text = "Qualidade";
    }

    //=========VOIDS DE SALVAMENTO==========//
    private void SalvarPreferencias()
    {
        #region SetPostFX

        if (CaixaModoJanela.isOn == true)
        {
            modoJanelaAtivo = 1;
            telaCheiaAtivada = false;
        }
        else
        {
            modoJanelaAtivo = 0;
            telaCheiaAtivada = true;
        }

        if (BarFPSLimit.value != FPSLimit)
        {
            FPSLimit = Convert.ToInt32(BarFPSLimit.value);
            PlayerPrefs.SetInt("FPSLimit", Convert.ToInt32(BarFPSLimit.value));
            txtFPSMax.text = FPSLimit.ToString();
        }


        if(VSync.isOn == true)
        {
            VSyncEnable = 1;
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            VSyncEnable = 0;
            QualitySettings.vSyncCount = 0;
        }


        if (DepthOfFieldBox.isOn == true)
        {
            DepthOfFieldEnable = 1;
            DepthOfField.enabled.value = true;
        }
        else
        {
            DepthOfFieldEnable = 0;
            DepthOfField.enabled.value = false;
        }


        if (MotionBlurBox.isOn == true)
        {
            MotionBlurEnable = 1;
            MotionBlur.enabled.value = true;
        }
        else
        {
            MotionBlurEnable = 0;
            MotionBlur.enabled.value = false;
        }



        if (BloomBox.isOn == true)
        {
            BloomEnable = 1;
            bloomLayer.enabled.value = true;
        }
        else
        {
            BloomEnable = 0;
            bloomLayer.enabled.value = false;
        }



        if (AnisotropicFiltering.isOn == true)
        {
            AnisotropicFilteringEnable = 1;
            QualitySettings.anisotropicFiltering = UnityEngine.AnisotropicFiltering.ForceEnable;
        }
        else
        {
            AnisotropicFilteringEnable = 0;
            QualitySettings.anisotropicFiltering = UnityEngine.AnisotropicFiltering.Disable;
        }
        #endregion

        PlayerPrefs.SetFloat("VOLUME", BarraVolume.value);
        PlayerPrefs.SetFloat("SFX", BarSoundFX.value);
        PlayerPrefs.SetFloat("Music", BarSoundMusic.value);
        PlayerPrefs.SetInt("FPSLimit", Convert.ToInt32(BarFPSLimit.value));
        PlayerPrefs.SetInt("VSync", VSyncEnable);
        PlayerPrefs.SetInt("AnisotropicFiltering", AnisotropicFilteringEnable);
        PlayerPrefs.SetInt("Bloom", BloomEnable);
        PlayerPrefs.SetInt("DepthOfField", DepthOfFieldEnable);
        PlayerPrefs.SetInt("MotionBlur", MotionBlurEnable);
        PlayerPrefs.SetInt("qualidadeGrafica", Qualidades.value);
        PlayerPrefs.SetInt("modoJanela", modoJanelaAtivo);
        PlayerPrefs.SetInt("RESOLUCAO", Resolucoes.value);

        resolucaoSalveIndex = Resolucoes.value;
        AplicarPreferencias();
    }

    private void AplicarPreferencias()
    {
        SFXSound = PlayerPrefs.GetFloat("SFX");
        VOLUME = PlayerPrefs.GetFloat("VOLUME");
        MusicSound = PlayerPrefs.GetFloat("Music");
        FPSLimit = PlayerPrefs.GetInt("FPSLimit");

        Application.targetFrameRate = FPSLimit;

        masterMixer.SetFloat("Master", VOLUME);
        masterMixer.SetFloat("SFX", SFXSound);
        masterMixer.SetFloat("Music", MusicSound);

        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("qualidadeGrafica"));
        Screen.SetResolution(resolucoesSuportadas[resolucaoSalveIndex].width, resolucoesSuportadas[resolucaoSalveIndex].height, telaCheiaAtivada);
    }

    //===========VOIDS NORMAIS=========//
    void Update()
    {
        if (SceneManager.GetActiveScene().name != nomeDaCena)
        {
            masterMixer.SetFloat("Master", VOLUME);
            masterMixer.SetFloat("SFX", SFXSound);
            masterMixer.SetFloat("Music", MusicSound);
        }

        if (ControllerManager.Mouse_Controller == 0 && OnController == true)
        {
            Back();
            OnController = false;
        }
        else if(ControllerManager.Mouse_Controller == 1)
        {
            OnController = true;
        }

        if (Input.GetButtonDown("B") || Input.GetKeyDown(KeyCode.Escape))
        {
            Back();
        }
        if (Input.GetButtonDown("Select"))
        {
            EventSystem.current.SetSelectedGameObject(null);

            EventSystem.current.SetSelectedGameObject(HomeButtonSelected);
            ColorPickerUI.GetComponent<ColorPickerUnityUI>().enabled = false;
        }
        if(ControllerManager.Xbox_One_Controller == 1)
        {
            if (menuAnim.GetBool("Settings") == true)
            {
                switch (SettingsPage)
                {
                    case 0:
                        Window.SetAsLastSibling();
                        break;
                    case 1:
                        Graphics.SetAsLastSibling();
                        break;
                    case 2:
                        Audio.SetAsLastSibling();
                        break;
                    case 3:
                        Data.SetAsLastSibling();
                        break;
                    case 4:
                        Language.SetAsLastSibling();
                        break;
                }

                if (Input.GetButtonDown("RB"))
                {
                    SettingsPage++;

                    switch (SettingsPage)
                    {
                        case 0:
                            EventSystem.current.SetSelectedGameObject(null);
                            EventSystem.current.SetSelectedGameObject(page1);
                            break;
                        case 1:
                            EventSystem.current.SetSelectedGameObject(null);
                            EventSystem.current.SetSelectedGameObject(page2);
                            break;
                        case 2:
                            EventSystem.current.SetSelectedGameObject(null);
                            EventSystem.current.SetSelectedGameObject(page3);
                            break;
                        case 3:
                            EventSystem.current.SetSelectedGameObject(null);
                            EventSystem.current.SetSelectedGameObject(page4);
                            break;
                        case 4:
                            EventSystem.current.SetSelectedGameObject(null);
                            EventSystem.current.SetSelectedGameObject(page5);
                            break;
                    }


                    if (SettingsPage > 4)
                    {
                        SettingsPage = 4;
                    }
                }

                if (Input.GetButtonDown("LB"))
                {
                    SettingsPage--;
                    switch (SettingsPage)
                    {
                        case 0:
                            EventSystem.current.SetSelectedGameObject(null);
                            EventSystem.current.SetSelectedGameObject(page1);
                            break;
                        case 1:
                            EventSystem.current.SetSelectedGameObject(null);
                            EventSystem.current.SetSelectedGameObject(page2);
                            break;
                        case 2:
                            EventSystem.current.SetSelectedGameObject(null);
                            EventSystem.current.SetSelectedGameObject(page3);
                            break;
                        case 3:
                            EventSystem.current.SetSelectedGameObject(null);
                            EventSystem.current.SetSelectedGameObject(page4);
                            break;
                        case 4:
                            EventSystem.current.SetSelectedGameObject(null);
                            EventSystem.current.SetSelectedGameObject(page5);
                            break;
                    }

                    if (SettingsPage < 0)
                    {
                        SettingsPage = 0;
                    }
                }
            }
            else
            {
                SettingsPage = 0;
            }
        }

        if (menuAnim.GetBool("SkinSelector") == true)
        {
            if (Input.GetButtonDown("X"))
            {
                EventSystem.current.SetSelectedGameObject(null);
                ColorPickerUI.GetComponent<ColorPickerUnityUI>().enabled = true;
            }
            if (Input.GetButtonDown("Y"))
            {
                EventSystem.current.SetSelectedGameObject(null);

                EventSystem.current.SetSelectedGameObject(skinbutton);
                ColorPickerUI.GetComponent<ColorPickerUnityUI>().enabled = false;

            }
            if (Input.GetButtonDown("RB"))
            {
                Skins.gameObject.SetActive(false);
                Particle.gameObject.SetActive(true);
                Particle.SetAsLastSibling();
            }
            if (Input.GetButtonDown("LB"))
            {
                Skins.gameObject.SetActive(true);
                Particle.gameObject.SetActive(false);
                Skins.SetAsLastSibling();
            }
        }

        txtStars.text = PlayerPrefs.GetInt("Stars").ToString();
        txtFragments.text = PlayerPrefs.GetInt("Fragments").ToString();
        SalvarPreferencias();
    }
    
    public void ClearAllData()
    {
        PlayerPrefs.DeleteAll();
    }

    private void Jogar()
    {
        menuAnim.SetBool("MenuToLocations", true);
        menuAnim.SetBool("Menu", false);
        if (ControllerManager.Mouse_Controller == 1)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);

            EventSystem.current.SetSelectedGameObject(LocationsButton);
        }

    }
    private void Back()
    {
        FirstButton.GetComponent<Animator>().updateMode = AnimatorUpdateMode.Normal;
        if (ControllerManager.Mouse_Controller == 1)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);

            EventSystem.current.SetSelectedGameObject(FirstButton);
        }

        ColorPickerUI.GetComponent<ColorPickerUnityUI>().enabled = false;
        menuAnim.SetBool("Menu", true);
        menuAnim.SetBool("SkinSelectorToSettings", false);
        menuAnim.SetBool("Settings", false);
        menuAnim.SetBool("SettingsToSkinSelector", false);
        menuAnim.SetBool("SkinSelector", false);
        menuAnim.SetBool("MenuToLocations", false);
        menuAnim.SetBool("LocationsToSkinSelector", false);
        menuAnim.SetBool("LocationsToOptions", false);
    }
    void Customize()
    {
        Invoke("ActiveColorUI", 0.7f);
        if (ControllerManager.Mouse_Controller == 1)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(skinbutton);
        }

        if (Input.GetButtonDown("X"))
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(ColorPickerButton);
        }
        if (Input.GetButtonDown("Y"))
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(skinbutton);
        }
        menuAnim.SetBool("Menu", false);
        menuAnim.SetBool("SkinSelector", true);
        menuAnim.SetBool("SkinSelectorToSettings", false);
        menuAnim.SetBool("SettingsToSkinSelector", true);
        menuAnim.SetBool("LocationsToSkinSelector", true);
    }
    void ActiveColorUI()
    {
        ColorPickerUI.GetComponent<ColorPickerUnityUI>().enabled = true;
    }
    void Settings()
    {
        if (ControllerManager.Mouse_Controller == 1)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {

            //EventSystem.current.SetSelectedGameObject(null);
            //EventSystem.current.SetSelectedGameObject(OptionsButton);
        }
        menuAnim.SetBool("Menu", false);
        menuAnim.SetBool("SkinSelectorToSettings", true);
        menuAnim.SetBool("SettingsToSkinSelector", false);
        menuAnim.SetBool("Settings", true);
        menuAnim.SetBool("LocationsToOptions", true);
    }
    public void Sair()
    {
        Application.Quit();
    }
}