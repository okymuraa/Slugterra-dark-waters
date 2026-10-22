using UnityEngine;
using UnityEngine.SceneManagement;

[AddComponentMenu("2D Toolkit/Demo/tk2dDemoReloadController")]
public class tk2dDemoReloadController : MonoBehaviour
{
	private void Reload()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
