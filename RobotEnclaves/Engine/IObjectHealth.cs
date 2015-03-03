using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public interface IObjectHealth
    {
        uint MaxHealth { get; }
        uint Health { get; }
        uint PointsOfDamage { get; }
        bool FullHealth { get; }
        bool Destroyed { get; }

        void RepairDamage(uint pointsOfDamage);
        void TakeDamage(uint pointsOfDamage);
        void Destroy();
    }
}
