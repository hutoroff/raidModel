using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace raidModel
{
    class raid1
    {
        const int minHDD = 2;          //min amount of disks in RAID-1
        double arrayCapacity;          //amount of disk space for all disks in array
        HBA array;

        public raid1(disk dType)
        {
            array = new HBA();
            for (int i = 0; i < minHDD; i++)
            {
                addDisk(dType);
                arrayCapacity += dType.getSize();
            }
            arrayCapacity = arrayCapacity / 2;
        }

        public raid1()
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

        public void breakRandDisk()
        {
            Random rand = new Random();
            array.getDisk(rand.Next(0, array.Count - 1)).brake();
        }

        public void addDisk(disk nDisk)
        {
            disk toAdd = new disk(nDisk.getSize(), nDisk.getCashS(), nDisk.getRLat(), nDisk.getWLat());
            array.addDisk(toAdd);
            arrayCapacity += nDisk.getSize();
        }

        public int writeToArray(List<sbyte> newData)
        {       //returns time of operation of -1 in case of failure
            DateTime start, end;
            start = DateTime.Now;

            if (newData.Capacity > arrayCapacity)
                return -1;
            int mem = 0;
            int hdd = 0;
            while (mem < newData.Count())
            {
                if (array.getDisk(hdd).getFreeSpace() >= 1)
                {
                    bool failure = false;
                    if (array.getDisk(hdd).getState())                                //if first disk OK
                        array.getDisk(hdd).writeToEnd(newData.ElementAt(mem));          //write to it
                    else
                        failure = true;

                    if (array.getDisk(hdd+1).getState())                              //if second disk OK
                        array.getDisk(hdd+1).writeToEnd(newData.ElementAt(mem));        //write mirror to it
                    else
                        failure = true;

                    if (failure)
                        if (array.Count >= hdd + 2)
                            hdd += 2;
                        else
                            return -1;
                    else
                        mem++;
                }
                else
                {
                    if (array.Count >= hdd + 2 && array.getDisk(hdd+2).getFreeSpace() >= 1)
                    {
                        hdd += 2;
                        array.getDisk(hdd).writeToEnd(newData.ElementAt(mem));
                        array.getDisk(hdd+1).writeToEnd(newData.ElementAt(mem));
                        mem++;
                    }
                    else
                        return -1;

                }
            }

            end = DateTime.Now;
            TimeSpan resultTime = end - start;
            return (resultTime.Seconds * 1000 + resultTime.Milliseconds + Convert.ToInt32(array.getWriteLatency(0)))*4;
        }

        public int readFromArray(List<sbyte> newData)
        {       //returns time of operation of -1 in case of failure
            DateTime start, end;
            start = DateTime.Now;

            if (isEnoughDisks() == 0)
                return -1;
            int mem = 0;                                      //memory slot number
            int hdd = 0;                                      //hard disk number in array
            bool cont = true;                             //false when nothing to read from disk
            sbyte b;
            while (cont)                                 //while there is data in array to read
            {
                if (array.getDisk(hdd).getState())
                {
                    b = array.getDisk(hdd).readByte(mem);
                    if (b == -128)                          //if there is no data to read readByter(...) returns -128
                        cont = false;
                    else
                    {
                        mem++;
                        if (mem > array.getDisk(hdd).getFreeSpace())
                        {
                            if (hdd % 2 == 1)
                                hdd++;
                            else
                                hdd += 2;
                        }
                    }
                }
                else
                {
                    if (hdd % 2 == 1)
                        return -1;
                    hdd++;
                }
            }

            end = DateTime.Now;
            TimeSpan resultTime = end - start;
            return Convert.ToInt32((resultTime.Seconds * 1000 + resultTime.Milliseconds + Convert.ToInt32(array.getReadLatency(0)))*3.5);
        }

    }

}
