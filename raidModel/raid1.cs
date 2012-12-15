using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            if (array.Count() < minHDD)
                return 0;
            else
                return 1;
        }

        public void addDisk(disk nDisk)
        {
            array.addDisk(nDisk);
            arrayCapacity += nDisk.getSize()/2;
        }

        public int writeToArray(List<sbyte> newData)
        {       //returns time of operation of -1 in case of failure
            DateTime start, end;
            start = DateTime.Now;

            if (newData.Capacity > arrayCapacity)
                return -1;
            int i = 0;
            int j = 0;
            while (i < newData.Count())
            {
                if (array.getDisk(j).getFreeSpace() >= 1)
                {
                    bool failure = false;
                    if (array.getDisk(j).getState())                                //if first disk OK
                        array.getDisk(j).writeToEnd(newData.ElementAt(i));          //write to it
                    else
                        failure = true;

                    if (array.getDisk(j+1).getState())                              //if second disk OK
                        array.getDisk(j+1).writeToEnd(newData.ElementAt(i));        //write mirror to it
                    else
                        failure = true;

                    if (failure)
                        if (array.Count() >= j + 2)
                            j += 2;
                        else
                            return -1;
                    else
                        i++;
                }
                else
                {
                    if (array.Count() >= j + 2 && array.getDisk(j+2).getFreeSpace() >= 1)
                    {
                        j += 2;
                        array.getDisk(j).writeToEnd(newData.ElementAt(i));
                        array.getDisk(j+1).writeToEnd(newData.ElementAt(i));
                        i++;
                    }
                    else
                        return -1;

                }
            }

            end = DateTime.Now;
            TimeSpan resultTime = end - start;
            return resultTime.Seconds * 1000 + resultTime.Milliseconds + Convert.ToInt32(array.getWriteLatency(0));
        }

        public int readFromArray(List<sbyte> newData)
        {       //returns time of operation of -1 in case of failure
            DateTime start, end;
            start = DateTime.Now;

            if (isEnoughDisks() == 0)
                return -1;
            int i = 0;                                      //memory slot number
            int j = 0;                                      //hard disk number in array
            bool stoper = true;                             //false when nothing to read from disk
            sbyte b;
            while (stoper)                                 //while there is data in array to read
            {
                if (array.getDisk(j).getState())
                {
                    b = array.getDisk(j).readByte(i);
                    if (b == -128)                          //if there is no data to read readByter(...) returns -128
                        stoper = false;
                    else
                    {
                        i++;
                        if (i > array.getDisk(j).getFreeSpace())
                        {
                            if (j % 2 == 1)
                                j++;
                            else
                                j += 2;
                        }
                    }
                }
                else
                {
                    if (j % 2 == 1)
                        return -1;
                    j++;
                }
            }

            end = DateTime.Now;
            TimeSpan resultTime = end - start;
            return resultTime.Seconds * 1000 + resultTime.Milliseconds + Convert.ToInt32(array.getReadLatency(0));
        }

    }

}
