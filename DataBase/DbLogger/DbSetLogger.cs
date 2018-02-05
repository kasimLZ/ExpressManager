using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.DbLogger
{
    public class DbSetLogger
    {
        public static Action<string> WriteAction = a =>
        {
            var sw = new System.IO.StreamWriter(@"d:\Data.log") { AutoFlush = true };
            sw.Write(DateTime.Now.ToString());
        };

        public static void Write (string str)
        {
            string DefalutName = "NoName";

            ///LevelRangeFilter  
            log4net.Filter.LevelRangeFilter levfilter = new log4net.Filter.LevelRangeFilter();
            levfilter.LevelMax = log4net.Core.Level.Fatal;
            levfilter.LevelMin = log4net.Core.Level.Error;
            levfilter.ActivateOptions();
            //Appender1  
            log4net.Appender.RollingFileAppender appender1 = new log4net.Appender.RollingFileAppender();
            appender1.AppendToFile = true;
            appender1.File = "ErrLog.log";
            appender1.ImmediateFlush = true;
            appender1.MaxFileSize = 32 * 1024;
            appender1.LockingModel = new log4net.Appender.FileAppender.MinimalLock();
            appender1.Encoding = Encoding.UTF8;

            appender1.Name = DefalutName;
            appender1.AddFilter(levfilter);

            log4net.Layout.PatternLayout layout = new log4net.Layout.PatternLayout("%date [%thread] %-5level - %message%newline ");
            layout.Header = "------ New session ------" + Environment.NewLine;
            layout.Footer = "------ End session ------" + Environment.NewLine;
            appender1.Layout = layout;
            appender1.ActivateOptions();

            log4net.Repository.ILoggerRepository repository = LogManager.GetAllRepositories().FirstOrDefault(a => a.Name == DefalutName);
            if (repository == null)
            {
                repository = LogManager.CreateRepository(DefalutName);
                log4net.Config.BasicConfigurator.Configure(repository, appender1);
            }

            ILog logger = log4net.LogManager.GetLogger(repository.Name, DefalutName);
            logger.Error("zzddff");

        }
    }
}
