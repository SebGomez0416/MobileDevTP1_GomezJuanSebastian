using UnityEngine;

public class DatosPartida: MonoBehaviour
{  
	public static DatosPartida instance;
    
	public int ScoreP1 { get; set; }
	public int ScoreP2 { get; set; }
	public int cantidadPlayers { get; set; }

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			if (instance != this)
			{
				Destroy(gameObject);
			}
		}
	}

	public void Init()
	{
		ScoreP1 = 0;
		ScoreP2 = 0;
		cantidadPlayers = 0;
	}

}
