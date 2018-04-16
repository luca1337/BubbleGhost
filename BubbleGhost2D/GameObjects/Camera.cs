using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BehaviourEngine.Test
{
    public class Camera : GameObject
    {
        public Camera()
        {
            AddBehaviour<CameraManager>(CameraManager.Instance);
        }
    }
}
