using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rendering.Graphics
{
    using Engine;
    using Engine.Items;
    using VectorMath;

    class CollectableItemSprite : ObjectMapSprite
    {
        private CollectableItem Item;
        private Color Color {
            get
            {
                return Color.Black;
            }
        }

        public CollectableItemSprite(CollectableItem item)
            : base(item.Position)
        {
            Item = item;
        }

        public override void Render(IRenderEngine renderEngine)
        {
            renderEngine.DrawVector(ObjectPosition - new Vector2(5, 5), new Vector2(10, 10), this.Color, 2f);
            renderEngine.DrawVector(ObjectPosition - new Vector2(5, -5), new Vector2(10, -10), this.Color, 2f);
        }
    }
}
