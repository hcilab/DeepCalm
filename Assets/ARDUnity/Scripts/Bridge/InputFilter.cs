using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Ardunity
{
	[AddComponentMenu("ARDUnity/Bridge/Input/InputFilter")]
    [HelpURL("https://sites.google.com/site/ardunitydoc/references/bridge/inputfilter")]
	public class InputFilter : ArdunityBridge, IWireInput<float>
	{
		public bool invert = false;
		public bool amplify = false;
		[Range(0f, 1f)]
		public float minValue = 0f;
		[Range(0f, 1f)]
		public float maxValue = 1f;
		public bool smooth = false;
		[Range(0f, 1f)]
		public float sensitivity = 0.5f;
		
        private AnalogInput _analogInput;
		private List<float> _sourceValues = new List<float>();
		private List<float> _resultValues = new List<float>();
		private List<float> squared_vector = new List<float> ();
		private List<float> normalized_vector = new List<float> ();
		private List<float> values = new List<float> ();

		private float _minSourceValue;
		private float _maxSourceValue;
		private float _sensitivity;
		private int _sampleNum = 300;
		private int normalized_vector_size = 300;

		private float _sourceValue;
		private float _resultValue;
		private bool _connected;
		// Kalman filter's parameter
		private float _Q;
		private float _R;
		private float _P;
		private float _X;
		private float _K;		

		private float current = 0.0f;
		private float previous = 0.0f;

		bool started = false;
		bool stopped = false;
		bool calibration = false;

		float y_multiplier = 1000.0f;
		float y_multiplier_step = 1.0f;
		float y_down_shift = 0;
		float average = 0;
		float min = 100000.0f;
		float max = 0.0f;
		float processed_min = 10000.0f;
		float processed_max = 0.0f;
		float range = 0.0f;

		float target_range = 20.0f;

		bool is_calibrating = false;
		bool done_calibrating = false;
		bool calibration_values_full = false;

		float start_time = 0.0f;
		int start_frame = 0;
		private List<float> calibration_values = new List<float> ();
		int average_num_frames = 8;
		int average_value_window = 16;
		private List<float> average_values = new List<float>();

		Transform player;
		// Use this for initialization
		void Start ()
		{
			player = GameObject.FindGameObjectWithTag ("player").transform;
		}

		void Update ()
		{
			/*
			if (calibration && started) {
				if (Input.GetKeyDown (KeyCode.KeypadPlus)) {
					y_multiplier += y_multiplier_step;
					Debug.Log(string.Format("y_multiplier: " + y_multiplier));
				} else if(Input.GetKeyDown(KeyCode.KeypadMinus)) {
					y_multiplier -= y_multiplier_step;
					Debug.Log(string.Format("y_multiplier: " + y_multiplier));
				}
			}
			*/

			if (is_calibrating && !smooth) {
				if (start_time + 8 < Time.time) {
					calibration_values_full = true;
					Debug.Log ("Getting values");
					string msg = "";
					min = calibration_values.Min ();
					max = calibration_values.Max ();

					// processed_min = ProcessFilter (min);
					// processed_max = ProcessFilter (max);
					// scale 
					msg += string.Format ("min: {0}\tmax: {1}", min, max);
					start_time = Time.time;
					Debug.Log (msg);
				}
				if (calibration_values_full) {
					calibration_values.RemoveAt (0);
				}
				calibration_values.Add (_sourceValue);
				if ((start_frame - Time.frameCount) % average_num_frames == 0 && calibration_values_full) {
					Debug.Log ("_sourceValues.Count: " + _sourceValues.Count);
					Debug.Log ("_sourceValues.Count - average_value_window: " + (_sourceValues.Count - average_value_window));
					average_values = _sourceValues.GetRange ((_sourceValues.Count - average_value_window), average_value_window);
					average = average_values.Average ();
					Debug.Log ("Updating average to: " + average);
				}
			}

			if (is_calibrating && smooth) {
				if (start_time + 10 < Time.time) {
					Debug.Log ("Getting values");
					string msg = "";
					min = ProcessFilter(calibration_values.Min ());
					max = ProcessFilter(calibration_values.Max ());
					// scale 
					msg += string.Format ("min: {0}\tmax: {1}", min, max);
					calibration_values.Clear ();
					start_time = Time.time;
					Debug.Log (msg);
				} else {
					calibration_values.Add (_sourceValue);
				}
			}

			/*
			if (Input.GetMouseButtonDown (0)) {
				if (calibration && !is_calibrating) {
					is_calibrating = true;
					start_time = Time.time;
					start_frame = Time.frameCount;
					previous = _resultValue;
					Debug.Log ("range: " + range);
				} else if(calibration && !started && is_calibrating){
					done_calibrating = true;
					started = true;
				}
			}
			*/

			string u_msg = "\nUpdate()\n";
			if(_analogInput != null)
			{
				if(_analogInput.connected)
				{
					if (!is_calibrating) {
						is_calibrating = true;
						start_time = Time.time;
						start_frame = Time.frameCount;
						previous = _resultValue;
						started = true;
					}


					if(!_connected && _analogInput.connected)
						Reset();

					// get the raw value
					_sourceValue = _analogInput.Value;
					_minSourceValue = Mathf.Min(_minSourceValue, _sourceValue);
					_maxSourceValue = Mathf.Max(_maxSourceValue, _sourceValue);

					float result = ProcessFilter(_sourceValue);

					/*
					if (started) {
						current = result;
						float diff = current - previous;
						u_msg += string.Format ("\tdiff: {0}\n", diff);
						if (diff <= 0) {
							u_msg += "\tDECREASING\n";
						} else {
							u_msg += "\tINCREASING\n";
						}
						u_msg += string.Format ("\tresult: {0}\n", result);
						u_msg += string.Format ("\tcurrent player position: {0}\n", GameObject.FindGameObjectWithTag ("player").transform.position);
						GameObject.FindGameObjectWithTag ("player").GetComponent<CharacterController> ().Move (new Vector3 (0,
							(y_multiplier * diff), 0));
						u_msg += string.Format ("\tUpdated player position: {0}\n", GameObject.FindGameObjectWithTag ("player").transform.position);
					}
					*/

					if (started && !smooth) {

						if (current < min) {
							current = min;
						}
						if (current > max) {
							current = max;
						}

						current = Mathf.Abs(average - min) / Mathf.Abs(max - min);

						//Debug.Log ("min: " + min + " max: " + max);
						//Debug.Log ("current: " + current);

						Debug.Log ("current after clamping " + current);
						current =  current * 22;
						Debug.Log ("current after scaling: " + current);
						Debug.Log ("going to plot player at: " + (current - 11));
						player.position = new Vector3 (player.position.x, current - 11 , player.position.z);

						u_msg += string.Format ("\tUpdated player position: {0}\n", GameObject.FindGameObjectWithTag ("player").transform.position);
					}

					/*
					if (started && smooth) {
						current = result;
						if (current < min) {
							current = min;
						}
						if (current > max) {
							current = max;
						}

						current = Mathf.Abs(average - min) / (max - min);
						current *= scale_up;
						current -= y_down_shift;


						GameObject.FindGameObjectWithTag ("player").transform.position = new Vector3 (0, current - 11, 0);
						u_msg += string.Format ("\tUpdated player position: {0}\n", GameObject.FindGameObjectWithTag ("player").transform.position);
					}
					*/


					if(result != _resultValue)
					{
						_resultValue = result;

						if(OnWireInputChanged != null)
							OnWireInputChanged(Value);
					}

					/*
					if (normalized_vector.Count >= normalized_vector_size) {
						squared_vector.RemoveAt (0);
						normalized_vector.RemoveAt (0);
					}
					*/

					if(_sourceValues.Count >= _sampleNum)
					{
						_sourceValues.RemoveAt(0);
						_resultValues.RemoveAt(0);
					}
					_sourceValues.Add(_sourceValue);
					_resultValues.Add(_resultValue);

					/*
					if (started) {
						squared_vector.Add (_resultValue * _resultValue);
						normalized_vector.Add (_resultValue / Mathf.Sqrt (squared_vector.Sum ()));
						values.Add (_resultValue);

					}*/

					/*
					if (stopped) {
						//Print_Float_List (normalized_vector, "normalized_vector");

						List<float> calibration_list = new List<float> ();
						foreach (float f in values) {
							calibration_list.Add (f - values.Min ());
							squared_vector.Add (f * f);
						}
						float sqrt_sum = Mathf.Sqrt(squared_vector.Sum ());
						foreach (float f in calibration_list) {
							normalized_vector.Add (f / sqrt_sum);
						}
						Print_Float_List (normalized_vector, "normalized_vector");
					}
					*/
				}
				previous = current;
				_connected = _analogInput.connected;
			}
			//Debug.Log (u_msg);
		}

		public void On_Level_Started(){

		}

		public void On_Level_Ended(){

		}

		void Print_Float_List(List<float> list, string name){
			string s = name + " values:";
			foreach (float f in list) {
				s += "\n\t" + f;
			}
			Debug.Log (s);
		}
		
		public float Value
		{
			get
			{
				return _resultValue;
			}
		}
		
		public float minSourceValue
		{
			get
			{
				return _minSourceValue;
			}
		}
		
		public float maxSourceValue
		{
			get
			{
				return _maxSourceValue;
			}
		}
		
		public float[] sourceValues
		{
			get
			{
				return _sourceValues.ToArray();
			}
		}
		
		public float[] resultValues
		{
			get
			{
				return _resultValues.ToArray();
			}
		}
		
		public void ResetFilter()
		{
			if(_analogInput == null)
				return;
			
			_sourceValues.Clear();
			_resultValues.Clear();
			_minSourceValue = 1f;
			_maxSourceValue = 0f;
			_sourceValue = _analogInput.Value;
			SmoothReset();
			_resultValue = ProcessFilter(_sourceValue);
			_sourceValues.Add(_sourceValue);
			_resultValues.Add(_resultValue);
		}
		
		private float ProcessFilter(float source)
		{
			float result = source;
			
			if(amplify)
			{
				result = Mathf.Clamp(result, minValue, maxValue);
				result = (result - minValue) / (maxValue - minValue);
			}
			
			if(smooth)
			{
				if(_sensitivity != sensitivity)
					SmoothReset();
				
				_K = (_P + _Q) / (_P + _Q + _R);
				_P = _R * (_P + _Q) / (_R + _P + _Q);
				result = _X + (result - _X) * _K;
				_X = result;
			}
			
			if(invert)
				result = 1f - result;
			
			return result;
		}
		
		private void SmoothReset()
		{
			_sensitivity = sensitivity;
			_Q = 0.00001f + (0.001f * _sensitivity);
			_R = 0.01f;
			_P = 1f;
			_X = 0f;
			_K = 0f;
		}
		
		public event WireEventHandler<float> OnWireInputChanged;

        float IWireInput<float>.input
        {
			get
			{
				return Value;
			}
        }
		
		protected override void AddNode(List<Node> nodes)
        {
			base.AddNode(nodes);
			
            nodes.Add(new Node("analogInput", "AnalogInput", typeof(AnalogInput), NodeType.WireFrom, "AnalogInput"));
			nodes.Add(new Node("result", "Result", typeof(IWireInput<float>), NodeType.WireTo, "Input<float>"));
        }
        
        protected override void UpdateNode(Node node)
        {
            if(node.name.Equals("analogInput"))
            {
				node.updated = true;

                if(node.objectTarget == null && _analogInput == null)
                    return;
                
                if(node.objectTarget != null)
                {
                    if(node.objectTarget.Equals(_analogInput))
                        return;
                }
                
                _analogInput = node.objectTarget as AnalogInput;
                if(_analogInput != null)
                {
                    Reset();
                    _connected = _analogInput.connected;
                }
                else
                    node.objectTarget = null;
                
                return;
            }
			else if(node.name.Equals("result"))
            {
                node.updated = true;
                return;
            }
                        
            base.UpdateNode(node);
        }
	}
}