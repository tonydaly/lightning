﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lightning {
    class LightningBolt : ILightning {
        public List<Line> Segments = new List<Line>();
        public Vector2 Start { get { return Segments[0].A; } }
        public Vector2 End { get { return Segments.Last().B; } }
        public bool IsComplete { get { return Alpha <= 0; } }

        public float Alpha { get; set; }
        public float AlphaMultiplier { get; set; }
        public float FadeOutRate { get; set; }
        public Color Tint { get; set; }

        static Random rand = new Random();

        public LightningBolt(Vector2 source, Vector2 dest) : this(source, dest, new Color(0.9f, 0.8f, 1f)) { }

        public LightningBolt(Vector2 source, Vector2 dest, Color color) {
            Segments = CreateBolt(source, dest, 2);

            Tint = color;
            Alpha = 1f;
            AlphaMultiplier = 0.6f;
            FadeOutRate = 0.03f;
        }

        public void Draw(SpriteBatch spriteBatch) {
            if (Alpha <= 0)
                return;

            foreach (var segment in Segments)
                segment.Draw(spriteBatch, Tint * (Alpha * AlphaMultiplier));
        }

        public virtual void Update() {
            Alpha -= FadeOutRate;
        }

        protected static List<Line> CreateBolt(Vector2 source, Vector2 dest, float thickness) {
            var results = new List<Line>();
            Vector2 tangent = dest - source;
            Vector2 normal = Vector2.Normalize(new Vector2(tangent.Y, -tangent.X));
            float length = tangent.Length();

            List<float> positions = new List<float>();
            positions.Add(0);

            for (int i = 0; i < length / 4; i++)
                positions.Add(Rand(0, 1));

            positions.Sort();

            const float Sway = 80;
            const float Jaggedness = 1 / Sway;

            Vector2 prevPoint = source;
            float prevDisplacement = 0;
            for (int i = 1; i < positions.Count; i++) {
                float pos = positions[i];

                // used to prevent sharp angles by ensuring very close positions also have small perpendicular variation.
                float scale = (length * Jaggedness) * (pos - positions[i - 1]);

                // defines an envelope. Points near the middle of the bolt can be further from the central line.
                float envelope = pos > 0.95f ? 20 * (1 - pos) : 1;

                float displacement = Rand(-Sway, Sway);
                displacement -= (displacement - prevDisplacement) * (1 - scale);
                displacement *= envelope;

                Vector2 point = source + pos * tangent + displacement * normal;
                results.Add(new Line(prevPoint, point, thickness));
                prevPoint = point;
                prevDisplacement = displacement;
            }

            results.Add(new Line(prevPoint, dest, thickness));

            return results;
        }

        private static float Rand(float min, float max) {
            return (float)rand.NextDouble() * (max - min) + min;
        }
    }
}
