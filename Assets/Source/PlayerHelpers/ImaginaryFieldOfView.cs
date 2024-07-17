using Characters;

namespace PlayerHelpers
{
    public class ImaginaryFieldOfView
    {
        private readonly IFieldOfView _fieldOfView;
        private readonly ISwitchable<ITarget> _switchable;

        public ImaginaryFieldOfView(IFieldOfView fieldOfView)
        {
            _fieldOfView = fieldOfView;
            _switchable = (ISwitchable<ITarget>)_fieldOfView;
        }

        public bool CanView(ITarget target)
        {
            _switchable.Switch(target);
            return _fieldOfView.CanView() == true && _fieldOfView.IsBlockingByWall() == false;
        }
    }
}