using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	[SerializeField] private GameObject cubePrefab = default;
	[SerializeField] private Transform[] spawnPos = new Transform[2];
	private bool spawnLeft = true;
	[SerializeField] GameObject lastCube = default;
	[SerializeField] GameObject previousCube = default;
	[SerializeField] private Camera camera = default;
	private int combo = 0;
	[SerializeField] private float speed = 3.0f;
	private int score = 0;
	private bool gameOver = false;
	private float baseHue;
	[SerializeField] private Text textScore = default;
	[SerializeField] private Text textHighScore = default;
	[SerializeField] private AudioSource[] audioSources = new AudioSource[2];

	public float Speed => speed;

	public bool GameOver
	{
		set => gameOver = value;
	}

	// Start is called before the first frame update
    void Start()
    {
	    Color.RGBToHSV(lastCube.GetComponent<MeshRenderer>().material.color, out var H, out var S, out var V);
	    baseHue = H;
	    textHighScore.text = "";
    }

    // Update is called once per frame
    void Update()
    {
	    if (Input.GetMouseButtonDown(0) && !gameOver)
	    {
		    if (previousCube)
		    {
			    bool isPerfect = lastCube.GetComponent<CubeManager>().Stop(previousCube);
			    if (isPerfect)
			    {
				    audioSources[0].Play();
				    audioSources[0].pitch += 0.1f;
				    combo++;
			    }
			    else
			    {
				    audioSources[1].Play();
				    audioSources[0].pitch = 1.0f;
				    combo = 0;
			    }
		    }
		    if(!gameOver)
				SpawnCube();
		    score++;
		    if(score >= PlayerPrefs.GetInt("ScoreStackMania"))
				PlayerPrefs.SetInt("ScoreStackMania", score);
		    if (score % 15 == 0)
			    speed = Speed + 1.0f;
	    }
	    
	    textScore.text = "Score : " + score;

	    if (gameOver)
	    {
		    camera.orthographicSize = 1.0f * score;
		    textHighScore.text = "HighScore : " + PlayerPrefs.GetInt("ScoreStackMania");
	    }
    }

    void SpawnCube()
    {
	    if(combo >= 8 && lastCube.transform.localScale.x < 3.0f && lastCube.transform.localScale.z < 3.0f)
		    if (lastCube.transform.localScale.x < lastCube.transform.localScale.z)
			    lastCube.transform.localScale += (Vector3.right * Random.Range(0.1f, 0.2f));
			else
			    lastCube.transform.localScale += (Vector3.forward * Random.Range(0.1f, 0.2f));
	    
	    GameObject currentCube = Instantiate<GameObject>(cubePrefab);
	    currentCube.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.HSVToRGB(baseHue + (score / 100.0f) % 1.0f, 1.0f, 1.0f));
	    currentCube.GetComponent<CubeManager>().Spawn(spawnLeft);
	    currentCube.transform.localScale = lastCube.transform.localScale;
	    currentCube.transform.localPosition = lastCube.transform.position + Vector3.up + (spawnLeft ? spawnPos[0].position : spawnPos[1].position);
	    previousCube = lastCube;
	    lastCube = currentCube;
	    spawnLeft = !spawnLeft;
	    camera.transform.position += Vector3.up;
    }
}
