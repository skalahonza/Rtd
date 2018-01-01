using UnityStandardAssets.Utility;

namespace Assets.Scripts.States.Camera
{
    public class MediumView : CameraState {
        public override void SetUp(SmoothFollow smth)
        {
            smth.Distance = 20;
            smth.Height = 5;
        }

        public override CameraState NextState()
        {
            return new FarView();
        }
    }
}