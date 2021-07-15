using UnityEngine;
using UnityEngine.EventSystems;

public class QuizView : MonoBehaviour
{

    public QuizController quizController;

    //detecting the tap on the options
    public void Tap()
    {
        string ObName = EventSystem.current.currentSelectedGameObject.name;
        quizController.TapResult(ObName);
    }


}
