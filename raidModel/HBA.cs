using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace raidModel
{
    class HBA
    {
        private List<disk> hdd;     //list of Hard Disk Drives in RAID
        public int Count;


        public HBA(IEnumerable<disk> diskList)
        {
            Count = 0;
            hdd = new List<disk>();
            hdd.InsertRange(0, diskList);
            Count++;
        }

        public HBA(disk nD)
        {
            Count = 0;
            hdd = new List<disk>();
            hdd.Add(nD);
            Count++;
        }

        public HBA(HBA oldH)
        {
            Count = 0;
            hdd = new List<disk>();
            this.hdd.InsertRange(0, oldH.hdd);
            Count += oldH.Count;
        }

        public HBA()
        {
            Count = 0;
            hdd = new List<disk>();
        }

        public disk getDisk(int i)
        {
            return hdd.ElementAt(i);
        }

        public bool getDiskState(int i)
        {
            return hdd.ElementAt(i).getState();
        }

        public void addDisk(disk newDisk)
        {
            hdd.Add(newDisk);
            Count++;
        }

        public int removeDisk()
        {
            if (hdd.Count > 0)
            {
                hdd.RemoveAt(hdd.Count);
                Count--;
                return 0;
            }
            else
                return 1;
        }

        public int removeDisk(int num)
        {
            if (hdd.Count > 0 && hdd.Count >= num && num >= 0)
            {
                hdd.RemoveAt(num);
                Count--;
                return 0;
            }
            else
                return 1;
        }

        public int writeToDisk(int i,sbyte dat)
        {
            return hdd.ElementAt(i).writeToEnd(dat);
        }

        public sbyte readFromDisk(int diskNo, int blockNo)
        {
            return hdd.ElementAt(diskNo).readByte(blockNo);
        }

        public float getWriteLatency(int i)
        {
            return hdd.ElementAt(i).getWLat();
        }

        public float getReadLatency(int i)
        {
            return hdd.ElementAt(i).getRLat();
        }
    }
}
