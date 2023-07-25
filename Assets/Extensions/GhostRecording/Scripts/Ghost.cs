using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


	[System.Serializable]
	public class WB_Vector3
	{

		private float x;
		private float y;
		private float z;

		public WB_Vector3()
		{
		}

		public WB_Vector3(Vector3 vec3)
		{
			this.x = vec3.x;
			this.y = vec3.y;
			this.z = vec3.z;
		}

		public static implicit operator WB_Vector3(Vector3 vec3)
		{
			return new WB_Vector3(vec3);
		}

		public static explicit operator Vector3(WB_Vector3 wb_vec3)
		{
			return new Vector3(wb_vec3.x, wb_vec3.y, wb_vec3.z);
		}
	}

	[System.Serializable]
	public class WB_Quaternion
	{

		private float w;
		private float x;
		private float y;
		private float z;

		public WB_Quaternion()
		{
		}

		public WB_Quaternion(Quaternion quat3)
		{
			this.x = quat3.x;
			this.y = quat3.y;
			this.z = quat3.z;
			this.w = quat3.w;
		}

		public static implicit operator WB_Quaternion(Quaternion quat3)
		{
			return new WB_Quaternion(quat3);
		}

		public static explicit operator Quaternion(WB_Quaternion wb_quat3)
		{
			return new Quaternion(wb_quat3.x, wb_quat3.y, wb_quat3.z, wb_quat3.w);
		}
	}

	[System.Serializable]
	public class GhostShot
	{
		public float timeMark = 0.0f; // mark at which the position and rotation are of af a given shot

		private WB_Vector3 _posMark;

		public Vector3 posMark
		{
			get
			{
				if (_posMark == null)
				{
					return Vector3.zero;
				}
				else
				{
					return (Vector3)_posMark;
				}
			}
			set { _posMark = (WB_Vector3)value; }
		}

		private WB_Quaternion _rotMark;

		public Quaternion rotMark
		{
			get
			{
				if (_rotMark == null)
				{
					return Quaternion.identity;
				}
				else
				{
					return (Quaternion)_rotMark;
				}
			}
			set { _rotMark = (WB_Quaternion)value; }
		}

	}

	public class Ghost : MonoBehaviour
	{

		public Transform FrontBumper, FrontSuspension, BackSuspension, Fwheel, Bwheel;
		private List<GhostShot> framesList, framesListFB, framesListFS, framesListBS, framesListFW, framesListBW;

		private List<GhostShot> lastReplayList,
			lastReplayListFB,
			lastReplayListFS,
			lastReplayListBS,
			lastReplayListFW,
			lastReplayListBW;

		private GameObject theGhost;

		private GhostInfo ghostInfo;
		private float replayTimescale = 1;
		private int replayIndex = 0;
		private int levelIndex;

		private float recordTime = 0.0f;
		private float replayTime = 0.0f;

		//Check whether we should be recording or not
		bool startRecording = false, recordingFrame = false, playRecording = false;


		public List<GhostShot> loadFromFile(string nameFile)
		{
			//Check if Ghost file exists. If it does load it
			if (File.Exists(Application.persistentDataPath + "/Ghost" + nameFile + levelIndex))
			{
				BinaryFormatter bf = new BinaryFormatter();
				FileStream file = File.Open(Application.persistentDataPath + "/Ghost" + nameFile + levelIndex,
					FileMode.Open);
//			Debug.Log("Ghost Found");
//			Debug.Log("File Location: " + Application.persistentDataPath + "/Ghost");
				return (List<GhostShot>)bf.Deserialize(file);
				file.Close();

			}
			else
			{
				//	Debug.Log("No Ghost Found");
				return null;
			}
		}

		void FixedUpdate()
		{
			if (startRecording)
			{
				//	Debug.Log("Recording Started");
				StartRecording();
				startRecording = false;
			}
			else if (recordingFrame)
			{
				RecordFrame();
			}

			if (lastReplayList != null && playRecording)
			{

				MoveGhost();
			}
		}

		private void RecordFrame()
		{
			recordTime += Time.smoothDeltaTime * 1000;
			newFrame(transform, framesList);
			newFrame(FrontBumper, framesListFB);
			newFrame(FrontSuspension, framesListFS);
			newFrame(BackSuspension, framesListBS);
			newFrame(Fwheel, framesListFW);
			newFrame(Bwheel, framesListBW);


		}

		public void newFrame(Transform t, List<GhostShot> f)
		{
			GhostShot newFrame = new GhostShot()
			{
				timeMark = recordTime,
				posMark = t.position,
				rotMark = t.rotation
			};

			f.Add(newFrame);
		}

		public void StartRecording()
		{

			levelIndex = ApplicationController.SelectedLevel;
			framesListBW = new List<GhostShot>();
			framesListFS = new List<GhostShot>();
			framesListFW = new List<GhostShot>();
			framesListBS = new List<GhostShot>();
			framesListFB = new List<GhostShot>();

			framesList = new List<GhostShot>();
			replayIndex = 0;
			recordTime = Time.time * 1000;
			recordingFrame = true;
			//playRecording = false;
		}

		public void StopRecordingGhost()
		{
			recordingFrame = false;
			lastReplayList = new List<GhostShot>(framesList);
			lastReplayListFB = new List<GhostShot>(framesListFB);
			lastReplayListFS = new List<GhostShot>(framesListFS);
			lastReplayListBS = new List<GhostShot>(framesListBS);
			lastReplayListFW = new List<GhostShot>(framesListFW);
			lastReplayListBW = new List<GhostShot>(framesListBW);


			//This will overwrite any previous Save
			//Run function if new highscore achieved or change filename in function
			//Save Ghost to file on device/computer
			SaveGhostToFile(lastReplayList, "lastReplayList");
			SaveGhostToFile(lastReplayListFB, "lastReplayListFB");
			SaveGhostToFile(lastReplayListFS, "lastReplayListFS");
			SaveGhostToFile(lastReplayListBS, "lastReplayListBS");
			SaveGhostToFile(lastReplayListFW, "lastReplayListFW");
			SaveGhostToFile(lastReplayListBW, "lastReplayListBW");

			///   Debug.Log("Recording Stopped");

		}

		public void playGhostRecording(Transform t)
		{
			levelIndex = ApplicationController.SelectedLevel;

			lastReplayList = loadFromFile("lastReplayList");
			lastReplayListFB = loadFromFile("lastReplayListFB");
			lastReplayListFS = loadFromFile("lastReplayListFS");
			lastReplayListBS = loadFromFile("lastReplayListBS");
			lastReplayListFW = loadFromFile("lastReplayListFW");
			lastReplayListBW = loadFromFile("lastReplayListBW");

			if (lastReplayList != null)
			{
				CreateGhost(t);
				replayIndex = 0;
				playRecording = true;
				//	Debug.Log("play Recording");

			}
		}

		public void StartRecordingGhost()
		{
			startRecording = true;
		}

		public void MoveGhost()
		{

			replayIndex++;

			replay(lastReplayList, theGhost.transform);
			replay(lastReplayListFB, ghostInfo.FrontBumper);
			replay(lastReplayListFS, ghostInfo.FrontSuspension);
			replay(lastReplayListBS, ghostInfo.BackSuspension);
			replay(lastReplayListFW, ghostInfo.Fwheel);
			replay(lastReplayListBW, ghostInfo.Bwheel);

//        Debug.Log("Playing Recording");

		}
		



		public void replay(List<GhostShot> r, Transform t)
		{
			if (replayIndex < r.Count)
			{
				GhostShot frame = r[replayIndex];
				DoLerp(r[replayIndex - 1], frame, t);


				replayTime += Time.smoothDeltaTime * 1000 * replayTimescale;
			}


		}

		private void DoLerp(GhostShot a, GhostShot b, Transform t)
		{

			t.position = Vector3.Slerp(a.posMark, b.posMark, Mathf.Clamp(replayTime, a.timeMark, b.timeMark));
			t.rotation = Quaternion.Slerp(a.rotMark, b.rotMark, Mathf.Clamp(replayTime, a.timeMark, b.timeMark));

		}

		public void SaveGhostToFile(List<GhostShot> lastFile, string nameFile)
		{
			// Prepare to write
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Create(Application.persistentDataPath + "/Ghost" + nameFile + levelIndex);
			//  Debug.Log("File Location: " + Application.persistentDataPath + "/Ghost"+nameFile +levelIndex);
			// Write data to disk
			bf.Serialize(file, lastFile);
			file.Close();
		}

//    public void SaveGhostToFile(List<Ghost> lastFile)
//    {
//	    // Prepare to write
//	    BinaryFormatter bf = new BinaryFormatter();
//	    FileStream file = File.Create(Application.persistentDataPath + "/Ghost");
//	    Debug.Log("File Location: " + Application.persistentDataPath + "/Ghost");
//	    // Write data to disk
//	    bf.Serialize(file, lastReplayList);
//	    file.Close();
//    }
//    
		public void CreateGhost(Transform parenttransform)
		{
			//Check if ghost exists or not, no reason to destroy and create it everytime.
			if (GameObject.FindWithTag("Ghost") == null)
			{
				//	Debug.Log("Creating Ghost");
				theGhost = Instantiate(Resources.Load("GhostPrefab", typeof(GameObject)),
					parenttransform) as GameObject;
				theGhost = theGhost.transform.GetChild(0).gameObject;
				theGhost.gameObject.tag = "Ghost";
				ghostInfo = theGhost.transform.GetComponentInParent<GhostInfo>();

				//Disable RigidBody
				// theGhost.GetComponent<Rigidbody>().isKinematic = true;

				//  MeshRenderer mr = theGhost.gameObject.GetComponent<MeshRenderer>();
				//  mr.material = Resources.Load("Ghost_Shader", typeof(Material)) as Material;
			}
		}

	}



