using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace raidModel
{
    class raid5
    {
        const int minHDD = 3;          //min amount of disks in RAID-1
        double arrayCapacity;          //amount of disk space for all disks in array
        HBA array;

        public raid5(disk dType)
        {
            array = new HBA();
            for (int i = 0; i < minHDD; i++)
            {
                addDisk(dType);
                arrayCapacity += dType.getSize();
            }
            arrayCapacity = (array.Count - 1) * array.getDisk(0).getSize();
        }

        public raid5()
        {
            array = new HBA();
            arrayCapacity = 0;
        }

        public void breakRandDisk()
        {
            Random rand = new Random();
            int i = rand.Next(0, array.Count - 1);
            array.getDisk(i).brake();
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

        private sbyte xor(int hdd0,int hdd1,int mem0,int mem1)
        {
            if (hdd0 < 0)
            {
                hdd0 = array.Count - Math.Abs(hdd0);
                mem0--;
            }
            if (hdd1 < 0)
            {
                hdd1 = array.Count - Math.Abs(hdd1);
                mem0--;
            }
            if(hdd0>=array.Count)
            {
                hdd0 = Math.Abs(hdd0) - array.Count;
                mem0++;
            }
            if (hdd1 >= array.Count)
            {
                hdd1 = Math.Abs(hdd1) - array.Count;
                mem1++;
            }
            if (mem0 == -1)
                mem0 = 0;
            if (mem1 == -1)
                mem1 = 0;
            if (mem0 >= array.getDisk(0).getSize() || mem1 >= array.getDisk(0).getSize())
                return -128;
            return Convert.ToSByte(Convert.ToBoolean(array.getDisk(hdd0).readByte(mem0)) ^ Convert.ToBoolean(array.getDisk(hdd1).readByte(mem1)));
        }

        public int writeToArray(List<sbyte> newData)
        {       //returns time of operation of -1 in case of failure
            DateTime start, end;
            start = DateTime.Now;

            if (newData.Capacity > arrayCapacity)
                return -1;
            int hdd = 0;    //disk number
            int mem = 0;    //memory slot number
            int counter = 0;

            do
            {
                if(array.getDisk(hdd).getFreeSpace()>=1)
                {
                    if (array.getDisk(hdd).getState())
                    {
                        if (counter == 2)
                        {
                            array.getDisk(hdd).writeToEnd(xor(hdd - 1, hdd - 2, mem, mem));
                            counter = 0;
                            hdd++;
                        }
                        else
                        {
                            array.getDisk(hdd).writeToEnd(newData.ElementAt(mem));
                            counter++;
                            hdd++;
                        }
                    }
                    else
                        return -1;
                    if (hdd >= array.Count)
                    {
                        hdd = 0;
                        mem++;
                        if (mem >= array.getDisk(0).getSize())
                            return -128;
                    }
                }
            } while (mem < newData.Count);

            end = DateTime.Now;
            TimeSpan resultTime = end - start;
            return resultTime.Seconds*1000 + resultTime.Milliseconds + Convert.ToInt32(array.getWriteLatency(0));
        }

        public int readFromArray(List<sbyte> newData)
        {       //returns time of operation of -1 in case of failure
            DateTime start, end;
            start = DateTime.Now;

            if (isEnoughDisks() == 0)
                return -1;

            int mem = 0;            //memory slot
            int hdd = 0;            //hdd number in array
            int counter = 0;        //helps to find checksum block
            sbyte b=0;
            bool cont = true;

            do
            {
                if (counter != 2)
                {
                    if (array.getDisk(hdd).getState())
                        b = array.getDisk(hdd).readByte(mem);
                    else
                        b = xor(hdd + 2, hdd + 1, mem, mem);
                    counter++;
                }
                else
                    counter = 0;
                if (b == -128)                          //if there is no data to read readByter(...) returns -128
                    cont = false;

                hdd++;
                if (hdd >= array.Count)
                {
                    mem++;
                    hdd = 0;
                }
            } while (cont);

            end = DateTime.Now;
            TimeSpan resultTime = end - start;
            return resultTime.Seconds*1000 + resultTime.Milliseconds + Convert.ToInt32(array.getReadLatency(0));

        }
    }
}
