using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

public class MenuManager : MonoBehaviour
{
    public Animator menuAnim;
    public Button BotaoJogar, BotaoMultiplayer, BotaoOpcoes, BotaoOpcoes2, CustomizeButton, CustomizeButton2, BotaoSair, BotaoVoltar;
    public GameObject FirstButton, OptionsButton, LocationsButton, skinbutton, ColorPickerButton, HomeButtonSelected;
    [Space(20)]
    public Slider BarraVolume;
    public Toggle CaixaModoJanela;
    public Dropdown Resolucoes, Qualidades;
    public GameObject ColorPickerUI;
    [Space(20)]
    public Text textoVol, txtStars, txtFragments;
    public RectTransform Particle;
    public RectTransform Skins;
    public ControllerManager ControllerManager;
    int Stars, Fragments;
    private string nomeDaCena;
    private float VOLUME;
    private int qualidadeGrafica, modoJanelaAtivo, resolucaoSalveIndex;
    private bool telaCheiaAtivada;
    private Resolution[] resolucoesSuportadas;
    bool OnController = true;

    void Awake()
    {
        resolucoesSuportadas = Screen.resolutions;
        ColorPickerUI.GetComponent<ColorPickerUnityUI>().enabled = true;
    }

    void Start()
    {
        ColorPickerUI.GetComponent<ColorPickerUnityUI>().enabled = false;
        FirstButton.GetComponent<Animator>().updateMode = AnimatorUpdateMode.Normal;
        txtStars.text = PlayerPrefs.GetInt("Stars").ToString();
        txtFragments.text = PlayerPrefs.GetInt("Fragments").ToString();

        Opcoes(false);
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
        Cursor.visible = true;
        Time.timeScale = 1;
        
        BarraVolume.minValue = 0;
        BarraVolume.maxValue = 1;

        //=============== SAVES===========//
        if (PlayerPrefs.HasKey("VOLUME"))
        {
            VOLUME = PlayerPrefs.GetFloat("VOLUME");
            BarraVolume.value = VOLUME;
            AudioListener.volume = VOLUME;
        }
        else
        {
            PlayerPrefs.SetFloat("VOLUME", 1);
            BarraVolume.value = 1;
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

        // =========SETAR BOTOES==========//
        BotaoJogar.onClick = new Button.ButtonClickedEvent();
        BotaoOpcoes.onClick = new Button.ButtonClickedEvent();
        BotaoOpcoes2.onClick = new Button.ButtonClickedEvent();
        CustomizeButton.onClick = new Button.ButtonClickedEvent();
        CustomizeButton2.onClick = new Button.ButtonClickedEvent();
        BotaoSair.onClick = new Button.ButtonClickedEvent();
        BotaoVoltar.onClick = new Button.ButtonClickedEvent();
        BotaoJogar.onClick.AddListener(() => Jogar());
        CustomizeButton.onClick.AddListener(() => Customize());
        CustomizeButton2.onClick.AddListener(() => Customize());
        BotaoOpcoes.onClick.AddListener(() => Settings());
        BotaoOpcoes2.onClick.AddListener(() => Settings());
        BotaoSair.onClick.AddListener(() => Sair());
        BotaoVoltar.onClick.AddListener(() => Back());
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
    private void Opcoes(bool ativarOP)
    {
        //BotaoJogar.gameObject.SetActive(!ativarOP);
        //BotaoMultiplayer.gameObject.SetActive(!ativarOP);
        //BotaoOpcoes.gameObject.SetActive(!ativarOP);
        //BotaoSair.gameObject.SetActive(!ativarOP);
        ////
        //textoVol.gameObject.SetActive(ativarOP);
        //BarraVolume.gameObject.SetActive(ativarOP);
        //CaixaModoJanela.gameObject.SetActive(ativarOP);
        //Resolucoes.gameObject.SetActive(ativarOP);
        //Qualidades.gameObject.SetActive(ativarOP);
        //BotaoVoltar.gameObject.SetActive(ativarOP);
        //BotaoSalvarPref.gameObject.SetActive(ativarOP);
    }

    //=========VOIDS DE SALVAMENTO==========//
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

        PlayerPrefs.SetFloat("VOLUME", BarraVolume.value);
        PlayerPrefs.SetInt("qualidadeGrafica", Qualidades.value);
        PlayerPrefs.SetInt("modoJanela", modoJanelaAtivo);
        PlayerPrefs.SetInt("RESOLUCAO", Resolucoes.value);
        resolucaoSalveIndex = Resolucoes.value;
        AplicarPreferencias();
    }

    private void AplicarPreferencias()
    {
        VOLUME = PlayerPrefs.GetFloat("VOLUME");
        AudioListener.volume = VOLUME;
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("qualidadeGrafica"));
        Screen.SetResolution(resolucoesSuportadas[resolucaoSalveIndex].width, resolucoesSuportadas[resolucaoSalveIndex].height, telaCheiaAtivada);
    }

    //===========VOIDS NORMAIS=========//
    void Update()
    {
        if (SceneManager.GetActiveScene().name != nomeDaCena)
        {
            AudioListener.volume = VOLUME;
            //Destroy(gameObject);
        }
        if(ControllerManager.Mouse_Controller == 0 && OnController == true)
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
        //menuAnim.SetTrigger("Menu");
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
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(OptionsButton);
        }

        Opcoes(true);
        menuAnim.SetBool("Menu", false);
        menuAnim.SetBool("SkinSelectorToSettings", true);
        menuAnim.SetBool("SettingsToSkinSelector", false);
        menuAnim.SetBool("Settings", true);
        menuAnim.SetBool("LocationsToOptions", true);
    }
    private void Sair()
    {
        Application.Quit();
    }
}