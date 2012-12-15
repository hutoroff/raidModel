using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace raidModel
{
    class disk
    {
        double mainSize;               //size of disk space in bytes
        double freeSpace;              //free disk space
        double cacheSize;              //size of disk cache memory in bytes
        bool state;                 //condition of the disk: true - OK, false - failure
        List<sbyte> data;           //data, saved on this disk
        List<sbyte> cache;          //cache memory of the disk
        float latencyRead;            //latency of time between operations of reading in ms
        float latencyWrite;           //latency of time between operations of writing in ms
        private double p1;
        private int p2;
        private double p3;
        private int p4;

        #region Constructors
        public disk(double nS)
        {
            data = new List<sbyte>();
            cache = new List<sbyte>();
            mainSize = nS;
            cacheSize = (16 * 1024 * 1024); //16MB by default
            freeSpace = mainSize;
            latencyRead = 0;                    //no extra latency by default
            latencyWrite = 0;
            state = true;

        }

        public disk(double nS, IEnumerable<sbyte> nDat)
        {
            data = new List<sbyte>();
            cache = new List<sbyte>();
            mainSize = nS;
            cacheSize = (16 * 1024 * 1024); //16MB by default
            latencyRead = 0;                    //no extra latency by default
            latencyWrite = 0;
            data.InsertRange(0, nDat);
            freeSpace = mainSize - nDat.Count();
            state = true;
        }

        public disk(double mSz, double cSz, float letR, float letW, IEnumerable<sbyte> nDat)
        {
            data = new List<sbyte>();
            cache = new List<sbyte>();
            mainSize = mSz;
            cacheSize = cSz;
            latencyRead = letR;
            latencyWrite = letW;
            data.InsertRange(0, nDat);
            freeSpace = mainSize - nDat.Count();
            state = true;
        }

        public disk(double mSz, double cSz, float letR, float letW)
        {
            data = new List<sbyte>();
            cache = new List<sbyte>();
            mainSize = mSz;
            cacheSize = cSz;
            latencyRead = letR;
            latencyWrite = letW;
            freeSpace = mainSize;
            state = true;
        }

        //public disk(double p1, int p2, double p3, int p4)
        //{
        //    // TODO: Complete member initialization
        //    this.p1 = p1;
        //    this.p2 = p2;
        //    this.p3 = p3;
        //    this.p4 = p4;
        //}
        #endregion

        public void brake()
        {   //makes disk unavalible for use
            state = false;
        }

        public void repair()
        {   //makes disk avalible for use
            state = true;
        }

        public double getSize()
        {
            return mainSize;
        }

        public double getFreeSpace()
        {
            return freeSpace;
        }

        public float getWLat()
        {
            return latencyWrite;
        }

        public float getRLat()
        {
            return latencyRead;
        }
        public bool getState()
        {
            return state;
        }

        public int writeToBeg(IEnumerable<sbyte> nDat)
        {
            if (nDat.Count() > this.freeSpace)
                return 1;
            data.InsertRange(0, nDat);
            TimeSpan toSleep = new TimeSpan(0, 0, 0, Convert.ToInt32(latencyWrite), Convert.ToInt32((latencyWrite - Convert.ToInt32(latencyWrite))*100));
            System.Threading.Thread.Sleep(toSleep);
            if (state)
                return 0;
            return 1;
        }

        public int writeToEnd(IEnumerable<sbyte> nDat)
        {
            if (nDat.Count() > this.freeSpace)
                return 1;
            data.InsertRange(data.Count(), nDat);
            TimeSpan toSleep = new TimeSpan(0, 0, 0, Convert.ToInt32(latencyWrite), Convert.ToInt32((latencyWrite - Convert.ToInt32(latencyWrite)) * 100));
            System.Threading.Thread.Sleep(toSleep);
            if (state)
                return 0;
            return 1;
        }

        public int writeToEnd(sbyte nDat)
        {
            if (this.freeSpace == 0)
                return 1;
            data.Insert(data.Count(), nDat);
            return 0;
        }

        public int readAll(IEnumerable<sbyte> nDat)
        {
            nDat.Concat(data);
            return nDat.Count();
        }

        public int readFrom(int f, IEnumerable<sbyte> nDat)
        {
            List<sbyte> rawDat = new List<sbyte>();
            rawDat.InsertRange(f, data);
            nDat.Concat(rawDat);
            return nDat.Count();
        }

        public sbyte readByte(int f)
        {
            
            if (f >= data.Count())
                return -128;
            return data.ElementAt(f);
        }

        public int readFromTo(int f, int t, IEnumerable<sbyte> nDat)
        {
            List<sbyte> rawDat = new List<sbyte>();
            rawDat.InsertRange(f, data);
            int length = t - f;
            rawDat.RemoveRange(length, rawDat.Count() - length);
            nDat.Concat(rawDat);
            return nDat.Count();
        }
    }
}
