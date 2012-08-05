using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace YouTrack.Web.Helpers
{
    public class TooManyParamsParams
    {
        int _x;
        int _y;
        int _z;
        int _d;

        public TooManyParamsParams(int x, int y, int z, int d)
        {
            _x = x;
            _y = y;
            _z = z;
            _d = d;
        }

        public int X
        {
            get { return _x; }
        }

        public int Y
        {
            get { return _y; }
        }

        public int Z
        {
            get { return _z; }
        }

        public int D
        {
            get { return _d; }
        }
    }

    public class SomeRoutines
    {
        public void IterateOver(IEnumerable<string> inputs)
        {
            if (inputs.Any(input => string.IsNullOrEmpty(input)))
            {
                throw new NoNullAllowedException("Emtpy value detected");
            }
            TooManyParams(new TooManyParamsParams(10, 20, 30, 40));
        }

        void TooManyParams(TooManyParamsParams tooManyParamsParams)
        {
            Debug.WriteLine(String.Format("Coordinates are: {0} {1} {2} {3}", tooManyParamsParams.X, tooManyParamsParams.Y, tooManyParamsParams.Z, tooManyParamsParams.D));
        }
    }
}