using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace raidModel
{
    class raid0: HBA
    {
        const int minHDD = 2;       //min amount of disks in RAID-1
        int arrayCapacity;          //amount of disk space for all disks in array

        public raid0(disk dType)
        {
            for (int i = 0; i < minHDD; i++)
            {
                addDisk(dType);
                arrayCapacity += dType.getSize();
            }
        }

        public raid0()
        {
            arrayCapacity = 0;
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
            int i = 0;
            while(i<newData.Count())
            {
                for(int j=0;j<hdd.Count();j++)
                {
                    if (hdd.ElementAt(j).writeToEnd(newData.ElementAt(i))==1)
                        return 1;
                    i++;
                    if (i >= newData.Count())
                        break;
                }
            }
            return 0;
        }

        public int readFromArray(List<sbyte> newData)
        {
            int i = 0;
            bool stoper = true;
            sbyte b;
            while(stoper)
            {
                for(int j=0;j<hdd.Count();j++)
                {
                    if (hdd.ElementAt(j).getState() == false)
                        return 1;
                    b = hdd.ElementAt(j).readByte(i);
                    if (b == -128)
                        stoper = false;
                    else
                        i++;
                }
            }
            return 0;
        }
    }
}
