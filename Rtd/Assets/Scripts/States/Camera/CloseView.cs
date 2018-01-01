using UnityStandardAssets.Utility;

namespace Assets.Scripts.States.Camera
{
    public class CloseView : CameraState
    {
        public override void SetUp(SmoothFollow smth)
        {
            smth.Distance = 10;
            smth.Height = 5;
        }

        public override CameraState NextState()
        {
            return new MediumView();
        }
    }
}