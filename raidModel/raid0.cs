using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace raidModel
{
    class raid0: HBA
    {
        const int minHDD = 2;       //min amount of disks in RAID-0
        double arrayCapacity;          //amount of disk space for all disks in array
        HBA array;

        public raid0(disk dType)
        {
            array = new HBA();
            for (int i = 0; i < minHDD; i++)
            {
                array.addDisk(dType);
                arrayCapacity += dType.getSize();
            }
        }

        public raid0()
        {
            array = new HBA();
            arrayCapacity = 0;
        }

        int isEnoughDisks()
        {
            
            if (array.Count() < minHDD)
                return 0;
            else
                return 1;
        }

        public void addDisk(disk nDisk)
        {
            array.addDisk(nDisk);
            arrayCapacity += nDisk.getSize();
        }

        public int writeToArray(List<sbyte> newData)
        {
            DateTime start, end;
            start = DateTime.Now;

            if (isEnoughDisks() == 0)
                return -1;
            if (newData.Capacity > arrayCapacity)
                return -1;
            int i = 0;
            while(i<newData.Count())
            {
                for(int j=0;j<array.Count();j++)
                {
                    if (array.getDiskState(j) == false)
                        return -1;
                    if (array.writeToDisk(j,newData.ElementAt(i))==1)
                        return -1;
                    i++;
                    if (i >= newData.Count)
                        break;
                }
            }

            end = DateTime.Now;
            TimeSpan resultTime = end - start;
            return resultTime.Milliseconds + Convert.ToInt32(array.getWriteLatency(0));
        }

        public int readFromArray(List<sbyte> newData)
        {
            //SOLVE WHAT A FUCK!
            DateTime start, end;
            start = DateTime.Now;

            if (isEnoughDisks() == 0)
                return -1;
            int i = 0;      //disk number in array
            bool stoper = true;
            sbyte b;
            while(stoper)
            {
                for(int j=0;j<array.Count();j++)        //memory slot on disk
                {
                    if (array.getDiskState(j) == false)
                        return -1;
                    b = array.readFromDisk(i,j);
                    if (b == -128)
                        stoper = false;
                    else
                    {
                        newData.Add(b);
                        //i++;
                    }
                }
            }

            end = DateTime.Now;
            TimeSpan resultTime = end - start;
            return resultTime.Milliseconds;
        }
    }
}
