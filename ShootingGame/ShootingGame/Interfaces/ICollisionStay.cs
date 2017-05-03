using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    interface ICollisionStay
    {
        void OnCollisionStay(Collider other);
    }
}
