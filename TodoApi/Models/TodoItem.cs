using System;

namespace TodoApi.Models
{
    public class TodoItem
    {
        public long Id { get; set; } //主キーになっている..はず
        public string Name { get; set; }
        //public bool IsComplete { get; set; }
        public int Asset { get; set; }
        public int numOfPlay { get; set; }
    }
}
