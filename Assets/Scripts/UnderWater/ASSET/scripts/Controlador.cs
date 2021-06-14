using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

// Va em Edit > Project Settings > Graphics e no always adicione "GlassStainedBumpDistort"

// SCRIPT FEITO POR MARCO SCHULTZ - ACESSE WWW.SCHULTZGAMES.COM

[RequireComponent(typeof(Fisheye))]
[RequireComponent(typeof(Blur))]
[RequireComponent(typeof(SphereCollider))] 
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(EdgeDetection))]
[RequireComponent(typeof(Vortex))]
[RequireComponent(typeof(AudioSource))]

public class Controlador : MonoBehaviour{

	private bool revert, revert2, saiuDaAgua, temSunShafts;
	private float cronometro,cronometro2, cronometroGotas;

	public Texture TexturaPingos;
	public string TagAgua = "Water";
	public Color corAgua = new Color32 (15, 150, 125, 0);
	[Range(0.5f,1.5f)]
	public float Distorcao = 1;
	[Range(0.0f,0.2f)]
	public float velocidadeDistorc = 0.15f;
	[Range(0.0f, 0.9f)]
	public float intensidadeCor = 0.3f;
	[Range(0,5)]
	public int DistanciaDeVisib = 2;
	[Range(0,10)]
	public float Visibilidade = 7;
	[Range(0,5)]
	public float IntensidadeSol = 3;
	public AudioClip somEntrando, somSaindo, somSubmerso;

	private GameObject objetoSomSub;
	private GameObject planoGotas;
	private AudioSource _audSourc;
	private Fisheye _fisheye;
	private Blur _blur;
	private EdgeDetection _edge;
	private Vortex _vortex;
	private SunShafts _sunShafts;
	private float intSolInicial;

	void Start (){
		gameObject.transform.parent = null;
		objetoSomSub = new GameObject ();
		objetoSomSub.AddComponent (typeof(AudioSource));
		objetoSomSub.GetComponent<AudioSource> ().loop = true;
		objetoSomSub.transform.localPosition = new Vector3 (0, 0, 0);
		objetoSomSub.GetComponent<AudioSource> ().clip = somSubmerso;
		objetoSomSub.transform.parent = transform;
		objetoSomSub.SetActive (false);
		//
		_fisheye = GetComponent<Fisheye> ();
		_blur = GetComponent<Blur> ();
		_edge = GetComponent<EdgeDetection> ();
		_vortex = GetComponent<Vortex> ();
		//
		GetComponent<SphereCollider> ().radius = 0.005f;
		GetComponent<SphereCollider> ().isTrigger = true;
		GetComponent<Rigidbody> ().isKinematic = true;
		GetComponent<Camera> ().nearClipPlane = 0.01f;
		//
		_blur.iterations = 5 - DistanciaDeVisib;
		_blur.blurSpread = 1-(Visibilidade/10);
		//
		_vortex.radius = new Vector2 (1, 1);
		_vortex.center = new Vector2 (0.5f, 0.5f);
		//
		_edge.mode = EdgeDetection.EdgeDetectMode.TriangleLuminance;
		_edge.lumThreshold = 0;
		_edge.sampleDist = 0;
		_edge.edgesOnly = intensidadeCor;
		_edge.edgesOnlyBgColor = corAgua;
		//
		_blur.enabled = false;
		_fisheye.enabled = false;
		_edge.enabled = false;
		_vortex.enabled = false;
		//
		//planoGotas = GameObject.CreatePrimitive(PrimitiveType.Plane);
		//Destroy (planoGotas.GetComponent<MeshCollider> ());
		////planoGotas.transform.localScale = new Vector3 (0.02f, 0.02f, 0.02f);
		//planoGotas.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);
		//planoGotas.transform.parent = transform;
		//planoGotas.transform.localPosition = new Vector3 (0, 0, 0.05f);
		//planoGotas.transform.localEulerAngles = new Vector3 (90, 180, 0);
		//planoGotas.GetComponent<Renderer>().material.shader = Shader.Find("FX/Glass/Stained BumpDistort");
		//planoGotas.GetComponent<Renderer> ().material.SetTexture ("_BumpMap", TexturaPingos);
		//planoGotas.GetComponent<Renderer> ().material.SetFloat ("_BumpAmt", 0);
		//
		if (GetComponent<SunShafts> () != null) {
			temSunShafts = true;
			_sunShafts = GetComponent<SunShafts> ();
			intSolInicial = _sunShafts.sunShaftIntensity;
		} else {
			temSunShafts = false;
		}
		//
		_audSourc = GetComponent<AudioSource> ();
	}

	void Update (){
		if (revert == false) {
			cronometro += Time.deltaTime*velocidadeDistorc;
		}
		if (cronometro > 0.5f) {
			revert = true;
		}
		if (revert == true) {
			cronometro -= Time.deltaTime*velocidadeDistorc;
		}
		if (cronometro < 0) {
			revert = false;
		}
		//
		if (revert2 == false) {
			cronometro2 += Time.deltaTime*velocidadeDistorc*2;
		}
		if (cronometro2 > 2) {
			revert2 = true;
		}
		if (revert2 == true) {
			cronometro2 -= Time.deltaTime*velocidadeDistorc*2;
		}
		if (cronometro2 < -1) {
			revert2 = false;
		}

		_vortex.center = new Vector2(cronometro2,0.5f);
		_vortex.angle = ((cronometro * 20) - 10)*(Distorcao*2);

		_fisheye.strengthX = (cronometro/2)*Distorcao;
		_fisheye.strengthY = 0.5f-cronometro*Distorcao;

		// PROPRIEDADES DO SHADER
		//if (saiuDaAgua == true) {
		//	cronometroGotas -= Time.deltaTime*20;
		//	planoGotas.GetComponent<Renderer> ().material.SetTextureOffset ("_BumpMap", new Vector2 (0, -cronometroGotas/100));
		//	planoGotas.GetComponent<Renderer> ().material.SetFloat ("_BumpAmt", cronometroGotas);
		//	if(cronometroGotas < 0){
		//		cronometroGotas = 0;
		//		saiuDaAgua = false;
		//		planoGotas.GetComponent<Renderer> ().material.SetFloat ("_BumpAmt", 0);
		//	}
		//}
	}
	void OnTriggerEnter (Collider colisor){
		if (colisor.gameObject.CompareTag (TagAgua)) {
			_blur.enabled = true;
			_fisheye.enabled = true;
			_edge.enabled = true;
			_vortex.enabled = true;
			saiuDaAgua = false;
			//planoGotas.GetComponent<Renderer> ().material.SetFloat ("_BumpAmt", 0);
			if(temSunShafts == true){
				_sunShafts.sunShaftIntensity = _sunShafts.sunShaftIntensity*IntensidadeSol;
			}
			_audSourc.PlayOneShot(somEntrando);
			objetoSomSub.SetActive (true);
		}
	}
	void OnTriggerExit (Collider colisor){
		if (colisor.gameObject.CompareTag (TagAgua)) {
			_blur.enabled = false;
			_fisheye.enabled = false;
			_edge.enabled = false;
			_vortex.enabled = false;
			saiuDaAgua = true;
			cronometroGotas = 40;
			if(temSunShafts == true){
				_sunShafts.sunShaftIntensity = intSolInicial;
			}
			_audSourc.PlayOneShot(somSaindo);
			objetoSomSub.SetActive (false);
		}
	}
}