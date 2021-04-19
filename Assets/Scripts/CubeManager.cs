using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
	private Vector3 startPos = default;
	private bool spawnLeft = true;
	private bool goBack = false;
	[SerializeField] private bool stop = false;
	[SerializeField] private float littleAnchor = 0.1f;
	private GameManager gameManager = default;
	
    // Start is called before the first frame update
    void Start()
    {
	    startPos = transform.position;
	    gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
	    if (!stop)
	    {
		    if (spawnLeft)
		    {
			    if (goBack)
			    {
				    transform.Translate(Vector3.left * (Time.deltaTime * gameManager.Speed));
				    if (transform.position.x < startPos.x)
					    goBack = false;
			    }
			    else
			    {
				    transform.Translate(Vector3.right * (Time.deltaTime * gameManager.Speed));
				    if (transform.position.x > -startPos.x)
					    goBack = true;
			    }
		    }
		    else
		    {
			    if (goBack)
			    {
				    transform.Translate(Vector3.forward * (Time.deltaTime * gameManager.Speed));
				    if (transform.position.z > startPos.z)
					    goBack = false;
			    }
			    else
			    {
				    transform.Translate(Vector3.back * (Time.deltaTime * gameManager.Speed));
				    if (transform.position.z < -startPos.z)
					    goBack = true;
			    }
		    }
	    }
    }

    public void Spawn(bool left)
    {
	    spawnLeft = left;
    }

    public bool Stop(GameObject lastCube)
    {
	    stop = true;
	    if (spawnLeft)
	    {
		    float factor = transform.position.x - lastCube.transform.position.x;

		    if (Mathf.Abs(factor) > lastCube.transform.localScale.x)
		    {
			    gameManager.GameOver = true;
			    GetComponent<Rigidbody>().isKinematic = false;
			    return false;
		    }
		    
		    if (littleAnchor > Mathf.Abs(factor))
		    {
			    transform.position = lastCube.transform.position + Vector3.up;
			    return true;
		    }

		    float ScaleXBefore = transform.localScale.x;
		    
		    transform.localScale += Mathf.Abs(factor) * Vector3.left;
		    transform.Translate((factor / 2.0f) * Vector3.left);

		    GameObject fallCube = Instantiate(gameObject);
		    fallCube.GetComponent<Rigidbody>().isKinematic = false;
		    fallCube.transform.localScale = new Vector3((ScaleXBefore - Mathf.Abs(transform.localScale.x)), 1.0f, transform.localScale.z);
		    fallCube.transform.position = transform.position;
		    fallCube.transform.Translate(transform.localScale.x * Vector3.right * (factor / Mathf.Abs(factor)));
		    
		    return false;
	    }
		else 
	    {
		    float factor = transform.position.z - lastCube.transform.position.z;

		    if (Mathf.Abs(factor) > lastCube.transform.localScale.z)
		    {
			    gameManager.GameOver = true;
			    GetComponent<Rigidbody>().isKinematic = false;
			    return false;
		    }
		    
		    if (littleAnchor > Mathf.Abs(factor))
		    {
			    transform.position = lastCube.transform.position + Vector3.up;
			    return true;
		    }

		    float ScaleZBefore = transform.localScale.z;
		    
		    transform.localScale += Mathf.Abs(factor) * Vector3.back;
		    transform.Translate((factor / 2.0f) * Vector3.back );

		    GameObject fallCube = Instantiate(gameObject);
		    fallCube.GetComponent<Rigidbody>().isKinematic = false;
		    fallCube.transform.localScale = new Vector3(transform.localScale.x, 1.0f, (ScaleZBefore - Mathf.Abs(transform.localScale.z)));
		    fallCube.transform.position = transform.position;
		    fallCube.transform.Translate(transform.localScale.z * Vector3.forward * (factor / Mathf.Abs(factor)));
		    
		    return false;
	    }
    }
}
