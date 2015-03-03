using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class ObjectHealth : IObjectHealth
    {
        public uint MaxHealth { get; private set; }
        public uint Health { get; private set; }

        public uint PointsOfDamage
        {
            get
            {
                return MaxHealth - Health;
            }
        }

        public bool FullHealth
        {
            get
            {
                return Health == MaxHealth;
            }
        }

        public bool Destroyed
        {
            get
            {
                return Health == 0;
            }
        }

        public void RepairDamage(uint pointsOfDamage)
        {
            if(Destroyed)
                throw new Exception("Object is destroyed");

            if (Health + pointsOfDamage >= MaxHealth)
            {
                Health = MaxHealth;
            }
            else
            {
                Health += pointsOfDamage;
            }
        }

        public void TakeDamage(uint pointsOfDamage)
        {
            if (pointsOfDamage >= Health)
            {
                Health = 0;
            }
            else
            {
                Health -= pointsOfDamage;
            }
        }

        public void Destroy()
        {
            Health = 0;
        }

        public ObjectHealth(uint maxHealth)
        {
            this.MaxHealth = maxHealth;
            this.Health = maxHealth;
        }
    }
}
