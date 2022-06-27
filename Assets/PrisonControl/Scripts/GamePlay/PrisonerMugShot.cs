using UnityEngine;

namespace PrisonControl
{
    public class PrisonerMugShot : MonoBehaviour
    {
        public System.Action _movedIn;
        public System.Action _movedOut;

        public void MovedIn()
        {
            _movedIn?.Invoke();
        }

        public void MovedOut()
        {
            _movedOut?.Invoke();
        }
    }
}
