using UnityEngine.SceneManagement;

namespace HaikuAP;

public class UnSoftlocker
{
    public static void Init()
    {
        On.IntroSequence.EnablePlayer += _startElsewhere;
    }

    private static void _startElsewhere(On.IntroSequence.orig_EnablePlayer orig, IntroSequence self)
    {
        orig(self);
        UnSoftlock();
    }

    public static void UnSoftlock()
    {
        GameManager.instance.gameLoaded = true;
        CameraBehavior.instance.TransitionState(true);
        SceneManager.LoadScene("G0");
        CameraBehavior.instance.ResumeHideUI();
    }
}