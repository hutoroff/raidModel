//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace raidModel
//{
//    class raid1: HBA
//    {
//        const int minHDD = 2;       //min amount of disks in RAID-1
//        double arrayCapacity;          //amount of disk space for all disks in array

//        public raid1(disk dType)
//        {
//            for (int i = 0; i < minHDD; i++)
//            {
//                addDisk(dType);
//                arrayCapacity += dType.getSize();
//            }
//            arrayCapacity = arrayCapacity/2;
//        }

//        public raid1()
//        {
//            arrayCapacity = 0;
//        }

//        int isEnoughDisks()
//        {
//            if(hdd.Count%2==1)
//                return 0;
//            if (hdd.Count < minHDD)
//                return 0;
//            else
//                return 1;
//        }

//        public void addDisk(disk nDisk)
//        {
//            hdd.Insert(hdd.Count, nDisk);
//        }

//        public int writeToArray(List<sbyte> newData)
//        {       //returns time of operation of -1 in case of failure
//            DateTime start, end;
//            start = DateTime.Now;

//            if (newData.Capacity > arrayCapacity)
//                return -1;
//            int i = 0;
//            int j = 0;
//            while (i < newData.Count())
//            {
//                if(hdd.ElementAt(j).getFreeSpace()>=1)
//                {
//                    bool failure = false;
//                    if (hdd.ElementAt(j).getState())
//                        hdd.ElementAt(j).writeToEnd(newData.ElementAt(i));
//                    else
//                        failure = true;

//                    if (hdd.ElementAt(j + 1).getState())
//                        hdd.ElementAt(j + 1).writeToEnd(newData.ElementAt(i));
//                    else
//                        failure = true;

//                    if (failure)
//                        if (hdd.Count >= j + 2)
//                            j += 2;
//                        else
//                            return -1;
//                    else
//                        i++;
//                }
//                else
//                {
//                    if(hdd.Count>=j+2&&hdd.ElementAt(j+2).getFreeSpace()>=1)
//                    {
//                        j+=2;
//                        hdd.ElementAt(j).writeToEnd(newData.ElementAt(i));
//                        hdd.ElementAt(j + 1).writeToEnd(newData.ElementAt(i));
//                        i++;
//                    }
//                    else
//                        return -1;
                    
//                }
//            }

//            end = DateTime.Now;
//            TimeSpan resultTime = end - start;
//            return resultTime.Milliseconds;            
//        }

//        public int readFromArray(List<sbyte> newData)
//        {       //returns time of operation of -1 in case of failure
//            DateTime start, end;
//            start = DateTime.Now;

//            if (isEnoughDisks() == 0)
//                return -1;
//            int i = 0;                                      //memory slot number
//            int j = 0;                                      //hard disk number in array
//            bool stoper = true;                             //false when nothing to read from disk
//            sbyte b;
//            while (stoper)                                 //while there is data in array to read
//            {
//                if (hdd.ElementAt(j).getState())
//                {
//                    b = hdd.ElementAt(j).readByte(i);
//                    if (b == -128)                          //if there is no data to read readByter(...) returns -128
//                        stoper = false;
//                    else
//                    {
//                        i++;
//                        if(i>hdd.ElementAt(j).getFreeSpace())
//                        {
//                            if (j % 2 == 1)
//                                j++;
//                            else
//                                j += 2;
//                        }
//                    }
//                }
//                else
//                {
//                    if(j%2==1)
//                        return -1;
//                    j++;
//                }
//            }

//            end = DateTime.Now;
//            TimeSpan resultTime = end - start;
//            return resultTime.Milliseconds;
//        }

//    }
        
//}
