using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooperGUI
{
    class Violation
    {

        public String Callsign;
        public String Reason;
        public String Cause;
        public double UnixStart;
        public double UnixEnd;
        public long Length;
        public bool Pardoned;
        public bool Expired;

        public Violation(String callsign, String reason, String cause, long seconds) {
            Callsign = callsign;
            Reason = reason;
            Cause = cause;
            UnixStart = (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
            UnixEnd = UnixStart + (seconds * 1000);
            Length = seconds;
            Pardoned = true;
        }

        public Violation()
        {
            Callsign = null;
            Reason = null;
            Cause = null;
            UnixStart = 0;
            UnixEnd = 0;
            Pardoned = true;
            Expired = true;
        }

        public String GetCallsign()
        {
            return Callsign;
        }

        public String GetReason()
        {
            return Reason;
        }

        public String GetCause()
        {
            return Cause;
        }

        public double GetStartTime()
        {
            return UnixStart;
        }

        public double GetEndTime()
        {
            return UnixEnd;
        }

        public void SetEndTime(long NewEndTime)
        {
            UnixEnd = NewEndTime;
        }

        public bool IsActive()
        {
            if (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds - UnixStart > UnixEnd * 1000)
            {
                Expired = true;
            } else
            {
                Expired = false;
            }
            if (Pardoned)
            {
                return false;
            } else
            {
                if (Expired)
                    return true;
                else
                    return false;
            }
        }

        public void Pardon()
        {
            Pardoned = false;
        }

        public void Reactivate()
        {
            Pardoned = true;
        }
    }
}
