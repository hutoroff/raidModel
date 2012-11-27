using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace raidModel
{
    class raid0: HBA
    {
        const int minHDD = 2;       //min amoun of disks in RAID-1
        int arrayCapacity;           //amount of disk space for all disks in array

        public raid0(disk dType)
        {
            for (int i = 0; i < minHDD; i++)
            {
                addDisk(dType);
                arrayCpacity += dType.getSize();
            }
        }

        public raid0()
        {
            arrayCpacity = 0;
        }

        public int isEnoughDisks()
        {
            if (hdd.Count < minHDD)
                return 0;
            else
                return 1;
        }

        public int writeToArray(List<sbyte> newData)
        {
            if (newData.Capacity > arrayCapacity)
                return 1;

            //TODO: write function

            return 0;
        }
    }
}
