using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rendering.Graphics
{
    using Common;
    using Engine;
    using Engine.Items;
    using VectorMath;

    public class CollectableItemSprite : ObjectMapSprite
    {
        private CollectableItem Item;

        public override bool Visible
        {
            get
            {
                return !Item.Collected;
            }
        }

        private Color Color {
            get
            {
                if (Item.World.Robots.Any(r => r.Crane.ItemInRange(Item)))
                    return Color.Green;

                return Item.Discovered ? Color.White : Color.Black;
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

            if (Item.Discovered)
            {
                renderEngine.DrawText(ObjectPosition, Item.ToString(), this.Color);
            }
        }
    }
}
