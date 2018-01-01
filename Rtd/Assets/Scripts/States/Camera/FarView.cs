using UnityStandardAssets.Utility;

namespace Assets.Scripts.States.Camera
{
    public class FarView : CameraState
    {
        public override void SetUp(SmoothFollow smth)
        {
            smth.Distance = 25;
            smth.Height = 8;
        }

        public override CameraState NextState()
        {
            return new CloseView();
        }
    }
}