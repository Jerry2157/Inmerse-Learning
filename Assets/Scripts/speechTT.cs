using UnityEngine;
using System.Collections;
using IBM.Watson.DeveloperCloud.Logging;
using IBM.Watson.DeveloperCloud.Services.SpeechToText.v1;
using IBM.Watson.DeveloperCloud.Utilities;
using IBM.Watson.DeveloperCloud.DataTypes;
using System.Collections.Generic;
using UnityEngine.UI;

public class speechTT : MonoBehaviour
{
	#region PLEASE SET THESE VARIABLES IN THE INSPECTOR
	[Space(10)]
	[Tooltip("The service URL (optional). This defaults to \"https://stream.watsonplatform.net/speech-to-text/api\"")]
	[SerializeField]
	private string _serviceUrl="https://stream.watsonplatform.net/speech-to-text/api";
	[Tooltip("Text field to display the results of streaming.")]
	public Text ResultsField;
	[Header("CF Authentication")]
	[Tooltip("The authentication username.")]
	[SerializeField]
	private string _username="e27e8d0a-7884-4ee7-85a2-31320a0fc730";
	[Tooltip("The authentication password.")]
	[SerializeField]
	private string _password="zsBaHjrGzpl7";
	[Header("IAM Authentication")]
	[Tooltip("The IAM apikey.")]
	[SerializeField]
	private string _iamApikey="zsBaHjrGzpl7";
	[Tooltip("The IAM url used to authenticate the apikey (optional). This defaults to \"https://iam.bluemix.net/identity/token\".")]
	[SerializeField]
	private string _iamUrl="https://iam.bluemix.net/identity/token";
	#endregion

	private int _recordingRoutine = 0;
	private string _microphoneID = null;
	private AudioClip _recording = null;
	private int _recordingBufferSize = 1;
	private int _recordingHZ = 22050;

	private string msj; //variable mensaje 
	private SpeechToText _service;


    public textToS texttos;

	void Start()
	{
		LogSystem.InstallDefaultReactors();
		Runnable.Run(CreateService());
		msj="";

	}

	private IEnumerator CreateService()
	{
		//  Create credential and instantiate service
		Credentials credentials = null;
		if (!string.IsNullOrEmpty(_username) && !string.IsNullOrEmpty(_password))
		{
			//  Authenticate using username and password
			credentials = new Credentials(_username, _password, _serviceUrl);
		}
		else if (!string.IsNullOrEmpty(_iamApikey))
		{
			//  Authenticate using iamApikey
			TokenOptions tokenOptions = new TokenOptions()
			{
				IamApiKey = _iamApikey,
				IamUrl = _iamUrl
			};

			credentials = new Credentials(tokenOptions, _serviceUrl);

			//  Wait for tokendata
			while (!credentials.HasIamTokenData())
				yield return null;
		}
		else
		{
			throw new WatsonException("Please provide either username and password or IAM apikey to authenticate the service.");
		}

		_service = new SpeechToText(credentials);
		_service.StreamMultipart = true;



		Active = true;
		//StartRecording();
	}

	public bool Active
	{
		get { return _service.IsListening; }
		set
		{
			if (value && !_service.IsListening)
			{
				_service.DetectSilence = true;
				_service.EnableWordConfidence = true;
				_service.EnableTimestamps = true;
				_service.SilenceThreshold = 0.01f;
				_service.MaxAlternatives = 0;
				_service.EnableInterimResults = true;
				_service.OnError = OnError;
				_service.InactivityTimeout = -1;
				_service.ProfanityFilter = false;
				_service.SmartFormatting = true;
				_service.SpeakerLabels = false;
				_service.WordAlternativesThreshold = null;
				_service.StartListening(OnRecognize, OnRecognizeSpeaker);
			}
			else if (!value && _service.IsListening)
			{
				_service.StopListening();
			}
		}
	}

	public void StartRecording()
	{
        Invoke("StopRecording", 5.0f);
		if (_recordingRoutine == 0)
		{
			UnityObjectUtil.StartDestroyQueue();
			_recordingRoutine = Runnable.Run(RecordingHandler());
		}
	}

	public void StopRecording()
	{
        
		if (_recordingRoutine != 0)
		{
			Microphone.End(_microphoneID);
			Runnable.Stop(_recordingRoutine);
			_recordingRoutine = 0;
		}
        reconoceDespliega();
    }

	private void OnError(string error)
	{
		Active = false;

		Log.Debug("ExampleStreaming.OnError()", "Error! {0}", error);
	}

	private IEnumerator RecordingHandler()
	{
		Log.Debug("ExampleStreaming.RecordingHandler()", "devices: {0}", Microphone.devices);
		_recording = Microphone.Start(_microphoneID, true, _recordingBufferSize, _recordingHZ);
		yield return null;      // let _recordingRoutine get set..

		if (_recording == null)
		{
			StopRecording();
			yield break;
		}

		bool bFirstBlock = true;
		int midPoint = _recording.samples / 2;
		float[] samples = null;

		while (_recordingRoutine != 0 && _recording != null)
		{
			int writePos = Microphone.GetPosition(_microphoneID);
			if (writePos > _recording.samples || !Microphone.IsRecording(_microphoneID))
			{
				Log.Error("ExampleStreaming.RecordingHandler()", "Microphone disconnected.");

				StopRecording();
				yield break;
			}

			if ((bFirstBlock && writePos >= midPoint)
				|| (!bFirstBlock && writePos < midPoint))
			{
				// front block is recorded, make a RecordClip and pass it onto our callback.
				samples = new float[midPoint];
				_recording.GetData(samples, bFirstBlock ? 0 : midPoint);

				AudioData record = new AudioData();
				record.MaxLevel = Mathf.Max(Mathf.Abs(Mathf.Min(samples)), Mathf.Max(samples));
				record.Clip = AudioClip.Create("Recording", midPoint, _recording.channels, _recordingHZ, false);
				record.Clip.SetData(samples, 0);

				_service.OnListen(record);

				bFirstBlock = !bFirstBlock;
			}
			else
			{
				// calculate the number of samples remaining until we ready for a block of audio, 
				// and wait that amount of time it will take to record.
				int remaining = bFirstBlock ? (midPoint - writePos) : (_recording.samples - writePos);
				float timeRemaining = (float)remaining / (float)_recordingHZ;

				yield return new WaitForSeconds(timeRemaining);
			}

		}

		yield break;
	}

	private void OnRecognize(SpeechRecognitionEvent result, Dictionary<string, object> customData)
	{
		if (result != null && result.results.Length > 0)
		{
			foreach (var res in result.results)
			{
				foreach (var alt in res.alternatives)
				{
					string text = string.Format("{0} ({1}, {2:0.00})\n", alt.transcript, res.final ? "Final" : "Interim", alt.confidence); //TU mandas lo saldado y es JSON
					Log.Debug("ExampleStreaming.OnRecognize()", text);
					//Log.Debug ("Aqui tenemos el texto que requerimos"+  text);
					msj=alt.transcript; //Asignacion de
					ResultsField.text = text; //
				}

				if (res.keywords_result != null && res.keywords_result.keyword != null)
				{
					foreach (var keyword in res.keywords_result.keyword)
					{
						Log.Debug("ExampleStreaming.OnRecognize()", "keyword: {0}, confidence: {1}, start time: {2}, end time: {3}", keyword.normalized_text, keyword.confidence, keyword.start_time, keyword.end_time);
					}
				}

				if (res.word_alternatives != null)
				{
					foreach (var wordAlternative in res.word_alternatives)
					{
						Log.Debug("ExampleStreaming.OnRecognize()", "Word alternatives found. Start time: {0} | EndTime: {1}", wordAlternative.start_time, wordAlternative.end_time);
						foreach(var alternative in wordAlternative.alternatives)
							Log.Debug("ExampleStreaming.OnRecognize()", "\t word: {0} | confidence: {1}", alternative.word, alternative.confidence);
					}
				}
			}
			//reconoceDespliega ();
		}

	}
    

	public void reconoceDespliega(){
		Debug.Log ("Mensaje: "+msj); //imrpimimos el mensaje
		if(msj.Contains("méxico")){
			Debug.Log ("pais ubicado en el norte del contienente americano, independenica declara el 15 de septimebre de 1810, sus pobladores hablan español");
            texttos.conviertVoz("pais ubicado en el norte del contienente americano, independenica declara el 15 de septimebre de 1810, sus pobladores hablan español");
        }
		if(msj.Contains("tundra")){
            texttos.conviertVoz("Ecosistemas mas frios, es practicamente un desierto polar, caracterizado principalmente por un clima sumamente frio, fuertes vientos, pocas precipitaciones.");
		}
		if(msj.Contains("globalizacion")){
            texttos.conviertVoz("proceso economico, tecnologico, politico, social, empresarial y cultural a escala mundial que consiste en la creciente comunicacion e interdependencia" +
				" entre los paises del mundo uniendo sus mercados, sociedades y culturas.");
		}
		if(msj.Contains("maya")){
            texttos.conviertVoz("civilizacion mesoamericana, se desarrollo en la region que abarca el sureste de Mexico, correspondiente a los estados de yucatan, campeche, tabasco.");
		}
		if(msj.Contains("innovacion")){
            texttos.conviertVoz("cambio que introduce novededas, que se refiere a moficiar elementos ya existentes con el fin de mejorarlos o renovarlos");
		}
		if(msj.Contains("acento")){
            texttos.conviertVoz("Es la mayor intensidad o fuerza de voz con que se pronuncia determinada silaba de una palabra");
		}
		if(msj.Contains("agudas")){
            texttos.conviertVoz("Palabras que llevan acento en la ultima silaba, llevaran tilde si terminan n s o en vocal");
		}
		if(msj.Contains("azteca")){
            texttos.conviertVoz("cultura prehispanica, que se ubicaba en el centro del Valle de Mexico.");
		}
		if(msj.Contains("internet")){
            texttos.conviertVoz("conjunto descentralizado de redes de comunicacion interconectadas.");
		}
		if(msj.Contains("españa")){
            texttos.conviertVoz("pais trascontinental, miembro de la union europea, su capital es madrid y su idioma oficial el castellano");
		}
	}

	private void OnRecognizeSpeaker(SpeakerRecognitionEvent result, Dictionary<string, object> customData)
	{
		if (result != null)
		{
			foreach (SpeakerLabelsResult labelResult in result.speaker_labels)
			{
				Log.Debug("ExampleStreaming.OnRecognize()", string.Format("speaker result: {0} | confidence: {3} | from: {1} | to: {2}", labelResult.speaker, labelResult.from, labelResult.to, labelResult.confidence));
			}
		}
	}
}
