using System;

namespace DataBase.DbLogger
{
    public class DbSetLoggerAttribute: Attribute
    {
        public LogLevel Level { get; set; }

        public bool SaveData { get; set; }

        public string ConfigXml { get; set; }
    }
    

    public enum LogLevel
    {
        All,//全部
        Insert,//新增
        Update,//修改
        Delete,//删除
        InsertUpdate,//新增和修改
        InsertDelete,//新增和删除
        UpDateDelete//修改和删除
    }
    
}
