﻿using SQLBuilder.Attributes;

namespace SQLBuilder.UnitTest
{
    [Table("Base_Student")]
    public class Student
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AccountId { get; set; }
        public string Name { get; set; }
    }
}
