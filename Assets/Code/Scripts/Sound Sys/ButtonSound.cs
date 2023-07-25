
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent (typeof(Button))]
public class ButtonSound : MonoBehaviour, IPointerClickHandler
{

	public AudioLibrary Type;
	public bool Gameplay;

	public void OnPointerClick (PointerEventData data)
	{
		if (Gameplay)
		{
			GameplaySoundController.instance.playFromPool(Type);
		}
		else
		{
			MenuSoundController.instance.playFromPool(Type);
		}
	}

	

}
