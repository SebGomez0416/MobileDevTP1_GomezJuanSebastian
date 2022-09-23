using UnityEngine;

public class DatosPartida: MonoBehaviour
{  
	public static DatosPartida instance;
	
	public int Players { get; set; }
	public bool Mute   { get; set; }
	public int ScoreP1 { get; set; }
	public int ScoreP2 { get; set; }
	public int cantidadPlayers { get; set; }
	
	public DifficultyController.Difficulty difficulty { get;  set; }

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
		Players = 0;
		ScoreP1 = 0;
		ScoreP2 = 0;
		cantidadPlayers = 0;
		difficulty = DifficultyController.Difficulty.Easy;
	}

}
