using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace raidModel
{
    class disk
    {
        int mainSize;               //size of disk space in bytes
        int cacheSize;              //size of disk cache memory in bytes
        bool state;                 //condition of the disk: true - OK, false - failure
        List<sbyte> data;           //data, saved on this disk
        List<sbyte> cache;          //cache memory of the disk
        int letency;                //letency of time beteween operations in ms

        public disk(int nS)
        {
            mainSize = nS;
            cacheSize = (16 * 1024 * 1024); //16MB by default
            letency = 0;                    //no extra letency by default
            state = true;
            
        }

        public disk(int nS, IEnumerable<sbyte> nDat)
        {
            mainSize = nS;
            cacheSize = (16 * 1024 * 1024); //16MB by default
            letency = 0;                    //no extra letency by default
            data.InsertRange(0, nDat);
        }

        public disk(int mSz, int cSz, int let, IEnumerable<sbyte> nDat)
        {
            mainSize = mSz;
            cacheSize = cSz;
            letency = let;
            data.InsertRange(0, nDat);
        }

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

        public int write(IEnumerable<sbyte> nDat)
        {
            return 0;
            //TODO: make function
        }
    }
}
