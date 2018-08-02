﻿// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Taiko.Objects.Drawables.Pieces;

namespace osu.Game.Rulesets.Taiko.Objects.Drawables
{
    public class DrawableDrumRollTick : DrawableTaikoHitObject<DrumRollTick>
    {
        public DrawableDrumRollTick(DrumRollTick tick)
            : base(tick)
        {
            FillMode = FillMode.Fit;
        }

        public override bool DisplayJudgement => false;

        protected override TaikoPiece CreateMainPiece() => new TickPiece
        {
            Filled = HitObject.FirstTick
        };

        protected override void CheckForJudgements(bool userTriggered, double timeOffset)
        {
            if (!userTriggered)
            {
                if (timeOffset > HitObject.HitWindow)
                {
                    ApplyJudgement(HitObject.Judgement, j => j.Result = HitResult.Miss);
                    if (HitObject.IsStrong)
                        ApplyJudgement(HitObject.StrongJudgement, j => j.Result = HitResult.Miss);
                }

                return;
            }

            if (Math.Abs(timeOffset) > HitObject.HitWindow)
                return;

            ApplyJudgement(HitObject.Judgement, j => j.Result = HitResult.Great);
            if (HitObject.IsStrong)
                ApplyJudgement(HitObject.StrongJudgement, j => j.Result = HitResult.Great);
        }

        protected override void UpdateState(ArmedState state)
        {
            switch (state)
            {
                case ArmedState.Hit:
                    this.ScaleTo(0, 100, Easing.OutQuint).Expire();
                    break;
            }
        }

        public override bool OnPressed(TaikoAction action) => UpdateJudgement(true);
    }
}
