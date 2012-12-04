using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace raidModel
{
    class disk
    {
        int mainSize;               //size of disk space in bytes
        int freeSpace;              //free disk space
        int cacheSize;              //size of disk cache memory in bytes
        bool state;                 //condition of the disk: true - OK, false - failure
        List<sbyte> data;           //data, saved on this disk
        List<sbyte> cache;          //cache memory of the disk
        int latency;                //latency of time between operations in ms

        #region Constructors
        public disk(int nS)
        {
            mainSize = nS;
            cacheSize = (16 * 1024 * 1024); //16MB by default
            freeSpace = mainSize;
            latency = 0;                    //no extra latency by default
            state = true;

        }

        public disk(int nS, IEnumerable<sbyte> nDat)
        {
            mainSize = nS;
            cacheSize = (16 * 1024 * 1024); //16MB by default
            latency = 0;                    //no extra latency by default
            data.InsertRange(0, nDat);
            freeSpace = mainSize - nDat.Count();
        }

        public disk(int mSz, int cSz, int let, IEnumerable<sbyte> nDat)
        {
            mainSize = mSz;
            cacheSize = cSz;
            latency = let;
            data.InsertRange(0, nDat);
            freeSpace = mainSize - nDat.Count();
        }
        #endregion

        public void brake()
        {   //makes disk unavalible for use
            state = false;
        }

        public void repair()
        {   //makes disk avalible for use
            state = true;
        }

        public int getSize()
        {
            return mainSize;
        }

        public int getFreeSpace()
        {
            return freeSpace;
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
            System.Threading.Thread.Sleep(latency);
            if (state)
                return 0;
            return 1;
        }

        public int writeToEnd(IEnumerable<sbyte> nDat)
        {
            if (nDat.Count() > this.freeSpace)
                return 1;
            data.InsertRange(data.Count(), nDat);
            System.Threading.Thread.Sleep(latency);
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
            if (f > data.Count())
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
