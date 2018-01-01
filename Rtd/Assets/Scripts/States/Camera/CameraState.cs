using UnityStandardAssets.Utility;

namespace Assets.Scripts.States.Camera
{
    public abstract class CameraState
    {
        public abstract void SetUp(SmoothFollow smth);
        public abstract CameraState NextState();
    }
}