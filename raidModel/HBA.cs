using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace raidModel
{
    class HBA
    {
        private List<disk> hdd;     //list of Hard Disk Drives in RAID


        public HBA(IEnumerable<disk> diskList)
        {
            hdd.InsertRange(0, diskList);
        }

        public HBA(disk nD)
        {
            hdd.Add(nD);
        }

        public HBA(HBA oldH)
        {
            this.hdd.InsertRange(0, oldH.hdd);
        }

        public HBA()
        {
        }

        public disk getDisk(int i)
        {
            return hdd.ElementAt(i);
        }

        public bool getDiskState(int i)
        {
            return hdd.ElementAt(i).getState();
        }

        public int Count()
        {
            return hdd.Count;
        }

        public void addDisk(disk newDisk)
        {
            hdd.Add(newDisk);
        }

        public int removeDisk()
        {
            if (hdd.Count > 0)
            {
                hdd.RemoveAt(hdd.Count);
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
                return 0;
            }
            else
                return 1;
        }

        public int writeToDisk(int i,sbyte dat)
        {
            return hdd.ElementAt(i).writeToEnd(dat);
        }

        public sbyte readFromDisk(int i, int j)
        {
            return hdd.ElementAt(i).readByte(j);
        }
    }
}
