using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace raidModel
{
    class raid0
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
            
            if (array.Count < minHDD)
                return 0;
            else
                return 1;
        }

        public void addDisk(disk nDisk)
        {
            disk toAdd = new disk(nDisk.getSize(), nDisk.getCashS(), nDisk.getRLat(), nDisk.getWLat());
            array.addDisk(toAdd);
            arrayCapacity += nDisk.getSize();
        }

        public void breakRandDisk()
        {
            Random rand = new Random();
            array.getDisk(rand.Next(0, array.Count - 1)).brake();
        }

        public int writeToArray(List<sbyte> newData)
        {
            DateTime start, end;
            start = DateTime.Now;

            if (isEnoughDisks() == 0)
                return -1;
            if (newData.Capacity > arrayCapacity)
                return -1;
            int mem = 0;
            int hdd = 0;
            
            do
            {
                if (array.getDisk(hdd).getFreeSpace()>=1)
                {
                    if(array.getDiskState(hdd))
                    {
                        if (array.writeToDisk(hdd, newData.ElementAt(mem)) == 1)
                            return -1;
                        hdd++;
                        if (hdd >= array.Count)
                        {
                            hdd = 0;
                            mem++;
                            if (mem >= array.getDisk(0).getSize())
                                return -1;
                        }
                    }
                    else
                        return -1;
                }
                else
                    return -1;
            } while (mem < newData.Count);

            end = DateTime.Now;
            TimeSpan resultTime = end - start;
            return resultTime.Seconds * 1000 + resultTime.Milliseconds + Convert.ToInt32(array.getWriteLatency(0));
        }

        public int readFromArray(List<sbyte> newData)
        {
            DateTime start, end;
            start = DateTime.Now;

            if (isEnoughDisks() == 0)
                return -1;
            int mem = 0;      //memory slot on disk
            int hdd = 0;     //disk number in array
            bool cont = true;
            sbyte b;
            do
            {
                if (array.getDiskState(hdd))
                {
                    b = array.readFromDisk(hdd, mem);
                    if (b == -128)
                        cont = false;
                    else
                    {
                        newData.Add(b);
                        hdd++;
                    }
                    if (hdd >= array.Count)
                    {
                        hdd = 0;
                        mem++;
                    }
                }
                else
                    return -1;
            }while(cont);

            end = DateTime.Now;
            TimeSpan resultTime = end - start;
            return resultTime.Seconds * 1000 + resultTime.Milliseconds + Convert.ToInt32(array.getReadLatency(0));
        }
    }
}
