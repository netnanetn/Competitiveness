using System;
using System.Runtime.Serialization;

namespace Falcon.Infrastructure.DependencyManagement
{
    [Serializable]
    public class ComponentRegistrationException : FalconException
    {
        public ComponentRegistrationException(string serviceName)
            : base(String.Format("Component {0} could not be found but is registered in the Nop/engine/components section", serviceName))
        {
        }

        protected ComponentRegistrationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
