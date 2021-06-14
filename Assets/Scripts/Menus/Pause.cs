using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Audio;
using Photon.Pun;

public class Pause : MonoBehaviour
{
    #region Variables

    [Header("Buttons")]
    [Tooltip("Set Pause button Menu")]
    public Button BotaoRetornarAoJogo, BotaoRestart, BotaoOpcoes, BotaoVoltarAoMenu, BotaoNextLevel;
    public GameObject PauseButton, SettingsButton, EndLevelButton, Options;
    [Space(20)]
    [Header("Sliders")]
    [Tooltip("Set options button Menu")]
    public Slider BarraVolume, BarSoundFX, BarSoundMusic, BarFPSLimit;
    [Header("Toggles")]
    [Tooltip("Set options button Menu")]
    public Toggle CaixaModoJanela, VSync, BloomBox, DepthOfFieldBox, MotionBlurBox, AnisotropicFiltering;
    [Header("Dropdowns")]
    [Tooltip("Set options button Menu")]
    public Dropdown Resolucoes, Qualidades;
    [Space(20)]
    [Header("Others")]
    public AudioMixer masterMixer;
    public Image blur;
    public Text txtFPSMax;
    [Header("SinglePlayer")]
    public int spawnPlayer;
    public int nextSpawn;
    public string nextLevel;

    bool OnController = true;
    DepthOfField DepthOfField = null;
    MotionBlur MotionBlur = null;
    Bloom bloomLayer = null;
    AmbientOcclusion ambientOcclusionLayer = null;
    ColorGrading colorGradingLayer = null;

    GameObject PostFX;
    private int FPSLimit, VSyncEnable, BloomEnable;
    private float VOLUME, SFXSound, MusicSound;
    private int qualidadeGrafica, modoJanelaAtivo, resolucaoSalveIndex, DepthOfFieldEnable, MotionBlurEnable, AnisotropicFilteringEnable;
    private bool telaCheiaAtivada, menuParte1Ativo, menuParte2Ativo;
    private Resolution[] resolucoesSuportadas;

    #endregion

    void Awake()
    {
        resolucoesSuportadas = Screen.resolutions;
        PostFX = GameObject.Find("PostFX");
        PostFX.GetComponent<PostProcessVolume>().profile.TryGetSettings(out DepthOfField);
        PostFX.GetComponent<PostProcessVolume>().profile.TryGetSettings(out MotionBlur);
        PostFX.GetComponent<PostProcessVolume>().profile.TryGetSettings(out bloomLayer);
        PostFX.GetComponent<PostProcessVolume>().profile.TryGetSettings(out ambientOcclusionLayer);
        PostFX.GetComponent<PostProcessVolume>().profile.TryGetSettings(out colorGradingLayer);
    }

    void Start()
    {
        Opcoes(false, false);

        Time.timeScale = 1;

        BarSoundFX.minValue = -80;
        BarSoundFX.maxValue = 5;
        BarSoundMusic.minValue = -80;
        BarSoundMusic.maxValue = 5;
        BarraVolume.minValue = -80;
        BarraVolume.maxValue = 5;
        menuParte1Ativo = menuParte2Ativo = false;

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
            if (VSyncEnable == 1)
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
            if (AnisotropicFilteringEnable == 1)
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
        BotaoVoltarAoMenu.onClick = new Button.ButtonClickedEvent();
        BotaoRestart.onClick = new Button.ButtonClickedEvent();
        BotaoOpcoes.onClick = new Button.ButtonClickedEvent();
        BotaoRetornarAoJogo.onClick = new Button.ButtonClickedEvent();
        //
        BotaoVoltarAoMenu.onClick.AddListener(() => VoltarAoMenu());
        BotaoNextLevel.onClick.AddListener(() => LoadNextLevel(nextLevel, nextSpawn));
        BotaoRestart.onClick.AddListener(() => Restart(spawnPlayer));
        BotaoOpcoes.onClick.AddListener(() => Opcoes(false, true));
        BotaoRetornarAoJogo.onClick.AddListener(() => Opcoes(false, false));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Select"))
        {
            if (menuParte1Ativo == false && menuParte2Ativo == false)
            {
                menuParte1Ativo = true;
                menuParte2Ativo = false;
                Opcoes(true, false);
                Time.timeScale = 0;
                masterMixer.SetFloat("Master", -80);
            }
            else if (menuParte1Ativo == true && menuParte2Ativo == false)
            {
                menuParte1Ativo = menuParte2Ativo = false;
                Opcoes(false, false);
                Time.timeScale = 1;
                masterMixer.SetFloat("Master", VOLUME);
            }
            else if (menuParte1Ativo == false && menuParte2Ativo == true)
            {
                menuParte1Ativo = true;
                menuParte2Ativo = false;
                Opcoes(true, false);
                Time.timeScale = 0;
                masterMixer.SetFloat("Master", -80);
            }
        }
        if (menuParte1Ativo == true || menuParte2Ativo == true)
        {
            blur.gameObject.SetActive(true);
        }
        else
        {
            blur.gameObject.SetActive(false);
        }

        if (GameManager.Instance.completeLevelUI.activeSelf == true)
        {
            if(OnController == true)
            {
                EventSystem.current.SetSelectedGameObject(null);

                EventSystem.current.SetSelectedGameObject(EndLevelButton);
                OnController = false;
            }
        }
        else
        {
            OnController = true;
        }

        if (BarraVolume.gameObject.activeSelf)
        {
            if (Input.GetButtonDown("B"))
            {
                menuParte1Ativo = true;
                menuParte2Ativo = false;
                Opcoes(true, false);
            }
        }
        else if (BotaoVoltarAoMenu.gameObject.activeSelf)
        {
            if (Input.GetButtonDown("B"))
            {
                menuParte1Ativo = false;
                menuParte2Ativo = false;
                Opcoes(false, false);
            }
        }
        SalvarPreferencias();
    }

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

    private void Opcoes(bool ativarOP, bool ativarOP2)
    {
        BotaoVoltarAoMenu.gameObject.SetActive(ativarOP);
        BotaoRestart.gameObject.SetActive(ativarOP);
        BotaoOpcoes.gameObject.SetActive(ativarOP);
        BotaoRetornarAoJogo.gameObject.SetActive(ativarOP);
        Options.gameObject.SetActive(ativarOP2);

        if (ativarOP == true && ativarOP2 == false)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(PauseButton);
            menuParte1Ativo = true;
            menuParte2Ativo = false;
        }
        else if (ativarOP == false && ativarOP2 == true)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(SettingsButton);
            menuParte1Ativo = false;
            menuParte2Ativo = true;
        }
        else if (ativarOP == false && ativarOP2 == false)
        {
            menuParte1Ativo = false;
            menuParte2Ativo = false;
            Time.timeScale = 1;
        }
    }

    private void SalvarPreferencias()
    {
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

        if (VSync.isOn == true)
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

    public void VoltarAoMenu()
    {
        //GameObject destroyPlayer = Destroy(GameObject.FindGameObjectWithTag("Player"));
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        Destroy(GameObject.Find("BallParticle"));
        //Destroy(destroyParticle);
        //Destroy(destroyPlayer);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("Menu");
    }

    public void Restart(int spawn)
    {
        GameObject.FindGameObjectWithTag("Player").transform.position = PlayerSelect.Instance.SpawnPlayer[spawn];
        GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().velocity = new Vector3(0,0,0); 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    public void LoadNextLevel(string nextLevel, int nextSpawn)
    {
        GameObject.FindGameObjectWithTag("Player").transform.position = PlayerSelect.Instance.SpawnPlayer[nextSpawn];
        SceneManager.LoadScene(nextLevel);
    }
}
