using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IVMenuScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 30;
        #if DEDICATED_SERVER
        IVUtil.MLAPIMode = LBMLAPIMode.DEDICATED_SERVER;
        LoadGameScene();
        #endif
    }

    public void StartHost()
    {
        IVUtil.MLAPIMode = LBMLAPIMode.HOST;
        LoadGameScene();
    }

    public void StartServer()
    {
        IVUtil.MLAPIMode = LBMLAPIMode.DEDICATED_SERVER;
        LoadGameScene();
    }

    public void StartClient()
    {
        IVUtil.MLAPIMode = LBMLAPIMode.CLIENT;
        LoadGameScene();
    }

    private void LoadGameScene()
    {
        SceneManager.LoadSceneAsync("GameScene");
    }
}
