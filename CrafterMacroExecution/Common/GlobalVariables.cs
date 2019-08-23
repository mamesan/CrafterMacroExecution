using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrafterMacroExecution.Common
{
    public interface IGlobalVariables
    {
        int Count { get; set; }
        int AllCount { get; set; }
        bool LoopFlg { get; set; }
        bool LoopFlg_Go { get; set; }
        bool EndFlg { get; set; }
        bool RepairingFlg { get; set; }
        DateTime MedicineDateTime { get; set; }
        DateTime FoodDateTime { get; set; }
        bool RoenaFlg { get; set; }
        int FoodTime { get; set; }
        int MedicineTime { get; set; }
    }
    class GlobalVariables : IGlobalVariables
    {
        private int count;
        private int allCount;
        private bool loopFlg;
        private bool loopFlg_Go;
        private bool endFlg;
        private bool repairingFlg;
        private DateTime medicineDateTime;
        private DateTime foodDateTime;
        private bool roenaFlg;
        private int foodTime;
        private int medicineTime;

        public int Count
        {
            get { return count; }
            set { count = value; }
        }
        public int AllCount
        {
            get { return allCount; }
            set { allCount = value; }
        }
        public bool LoopFlg
        {
            get { return loopFlg; }
            set { loopFlg = value; }
        }
        public bool LoopFlg_Go
        {
            get { return loopFlg_Go; }
            set { loopFlg_Go = value; }
        }
        public bool EndFlg
        {
            get { return endFlg; }
            set { endFlg = value; }
        }
        public bool RepairingFlg
        {
            get { return repairingFlg; }
            set { repairingFlg = value; }
        }
        public DateTime MedicineDateTime
        {
            get { return medicineDateTime; }
            set { medicineDateTime = value; }
        }
        public DateTime FoodDateTime
        {
            get { return foodDateTime; }
            set { foodDateTime = value; }
        }
        public bool RoenaFlg
        {
            get { return roenaFlg; }
            set { roenaFlg = value; }
        }
        public int FoodTime
        {
            get { return foodTime; }
            set { foodTime = value; }
        }
        public int MedicineTime
        {
            get { return medicineTime; }
            set { medicineTime = value; }
        }
    }
}