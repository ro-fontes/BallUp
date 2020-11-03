using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class Pause : MonoBehaviour
{
    public Button BotaoRetornarAoJogo, BotaoRestart, BotaoOpcoes, BotaoVoltarAoMenu;
    [Space(20)]
    public Slider BarraVolume;
    public Toggle CaixaModoJanela;
    public Dropdown Resolucoes, Qualidades;
    public Button BotaoVoltar, BotaoSalvarPref;
    [Space(20)]
    public Text textoVol;
    public Image blur;
    public string nomeCenaMenu = "Menu";
    private float VOLUME;
    private int qualidadeGrafica, modoJanelaAtivo, resolucaoSalveIndex;
    private bool telaCheiaAtivada, menuParte1Ativo, menuParte2Ativo;
    private Resolution[] resolucoesSuportadas;

    void Awake()
    {
        resolucoesSuportadas = Screen.resolutions;
    }

    void Start()
    {
        Opcoes(false, false);
        ChecarResolucoes();
        AjustarQualidades();
        Time.timeScale = 1;
        AudioListener.volume = 1;
        BarraVolume.minValue = 0;
        BarraVolume.maxValue = 1;
        menuParte1Ativo = menuParte2Ativo = false;

        if (PlayerPrefs.HasKey("RESOLUCAO"))
        {
            int numResoluc = PlayerPrefs.GetInt("RESOLUCAO");
            if (resolucoesSuportadas.Length <= numResoluc)
            {
                PlayerPrefs.DeleteKey("RESOLUCAO");
            }
        }
        //=============== SAVES===========//
        if (PlayerPrefs.HasKey("VOLUME"))
        {
            VOLUME = PlayerPrefs.GetFloat("VOLUME");
            BarraVolume.value = VOLUME;
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
        BotaoVoltarAoMenu.onClick = new Button.ButtonClickedEvent();
        BotaoRestart.onClick = new Button.ButtonClickedEvent();
        BotaoOpcoes.onClick = new Button.ButtonClickedEvent();
        BotaoRetornarAoJogo.onClick = new Button.ButtonClickedEvent();
        BotaoVoltar.onClick = new Button.ButtonClickedEvent();
        BotaoSalvarPref.onClick = new Button.ButtonClickedEvent();
        //
        BotaoVoltarAoMenu.onClick.AddListener(() => VoltarAoMenu());
        BotaoRestart.onClick.AddListener(() => Restart());
        BotaoOpcoes.onClick.AddListener(() => Opcoes(false, true));
        BotaoRetornarAoJogo.onClick.AddListener(() => Opcoes(false, false));
        BotaoVoltar.onClick.AddListener(() => Opcoes(true, false));
        BotaoSalvarPref.onClick.AddListener(() => SalvarPreferencias());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuParte1Ativo == false && menuParte2Ativo == false)
            { 
                menuParte1Ativo = true;
                menuParte2Ativo = false;
                Opcoes(true, false);
                Time.timeScale = 0;
                AudioListener.volume = 0;
            }
            else if (menuParte1Ativo == true && menuParte2Ativo == false)
            {
                menuParte1Ativo = menuParte2Ativo = false;
                Opcoes(false, false);
                Time.timeScale = 1;
                AudioListener.volume = VOLUME;
            }
            else if (menuParte1Ativo == false && menuParte2Ativo == true)
            {
                menuParte1Ativo = true;
                menuParte2Ativo = false;
                Opcoes(true, false);
                Time.timeScale = 0;
                AudioListener.volume = 0;
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

    private void Opcoes(bool ativarOP, bool ativarOP2)
    {
        BotaoVoltarAoMenu.gameObject.SetActive(ativarOP);
        BotaoRestart.gameObject.SetActive(ativarOP);
        BotaoOpcoes.gameObject.SetActive(ativarOP);
        BotaoRetornarAoJogo.gameObject.SetActive(ativarOP);

        textoVol.gameObject.SetActive(ativarOP2);
        BarraVolume.gameObject.SetActive(ativarOP2);
        CaixaModoJanela.gameObject.SetActive(ativarOP2);
        Resolucoes.gameObject.SetActive(ativarOP2);
        Qualidades.gameObject.SetActive(ativarOP2);
        BotaoVoltar.gameObject.SetActive(ativarOP2);
        BotaoSalvarPref.gameObject.SetActive(ativarOP2);
        if (ativarOP == true && ativarOP2 == false)
        {
            menuParte1Ativo = true;
            menuParte2Ativo = false;
        }
        else if (ativarOP == false && ativarOP2 == true)
        {
            menuParte1Ativo = false;
            menuParte2Ativo = true;
        }
        else if (ativarOP == false && ativarOP2 == false)
        {
            menuParte1Ativo = false;
            menuParte2Ativo = false;
            Time.timeScale = 1;
            AudioListener.volume = VOLUME;
        }
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
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("qualidadeGrafica"));
        Screen.SetResolution(resolucoesSuportadas[resolucaoSalveIndex].width, resolucoesSuportadas[resolucaoSalveIndex].height, telaCheiaAtivada);
    }

    public void VoltarAoMenu()
    {
        GameObject destroyPlayer = GameObject.Find("Player");
        GameObject destroyParticle = GameObject.Find("BallParticle");
        Destroy(destroyParticle);
        Destroy(destroyPlayer);
        SceneManager.LoadScene(nomeCenaMenu);
    }

    public void Restart()
    {
        GameObject.Find("Player").transform.position = PlayerSelect.Instance.SpawnPlayer[SceneManager.GetActiveScene().buildIndex - 1];
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    public void LoadNextLevel()
    {
        GameObject.Find("Player").transform.position = PlayerSelect.Instance.SpawnPlayer[SceneManager.GetActiveScene().buildIndex];
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}