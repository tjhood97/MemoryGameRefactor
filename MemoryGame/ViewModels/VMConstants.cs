using System;

namespace MemoryGame.ViewModels
{
    public static class VMConstants
    {
        //Amount of seconds that the slide is peeked at for
        public const int PeekSeconds = 3;

        //Amount of seconds that a slide can be opened for
        public const int OpenSeconds = 5;

        //Amount of pairs of slides that are created
        public const int slideCount = 6;

        //Maximum number of attempts before failure
        public const int MaxAttempts = 4;

        //Points awarded each successful match
        public const int PointAward = 75;
        
        //Points deducted each failed match
        public const int PointDeduction = 15;


    }

}