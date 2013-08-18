﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Lightning {
    static class Art {
        public static Texture2D LightningSegment, HalfCircle, Pixel;

        public static void Load(ContentManager content) {
            LightningSegment = content.Load<Texture2D>("line-middle");
            HalfCircle = content.Load<Texture2D>("line-end");
            //Pixel = content.Load<Texture2D>("Pixel");
        }
    }
}
