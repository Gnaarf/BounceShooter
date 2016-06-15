using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace GameProject2D
{
    interface Updateable
    {
        void Update(float deltaTime);
    }
}
