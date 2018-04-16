﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BubbleGhostGame2D
{
    public class Camera : GameObject
    {
        public Camera() : base( (int)RenderLayer.None )
        {
            AddBehaviour<CameraManager>(CameraManager.Instance);
        }
    }
}
