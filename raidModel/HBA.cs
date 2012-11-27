using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace raidModel
{
    class HBA
    {
        protected List<disk> hdd;     //list of Hard Disk Drives in RAID


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

        protected void addDisk(disk newDisk)
        {
            hdd.Add(newDisk);
        }

        protected int removeDisk()
        {
            if (hdd.Count > 0)
            {
                hdd.RemoveAt(hdd.Count);
                return 0;
            }
            else
                return 1;
        }

        protected int removeDisk(int num)
        {
            if (hdd.Count > 0 && hdd.Count >= num && num >= 0)
            {
                hdd.RemoveAt(num);
                return 0;
            }
            else
                return 1;
        }
    }
}
