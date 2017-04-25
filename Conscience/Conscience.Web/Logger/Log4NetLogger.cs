using log4net;
using log4net.Config;
using System;
using System.Threading.Tasks;
using log4net.Core;

namespace Conscience.Web.Logger
{
    public class Log4NetLogger
    {
        private static Log4NetLogger _current;
        private static readonly ILog logger = LogManager.GetLogger("Conscience");
        
        public Log4NetLogger()
        {
            XmlConfigurator.Configure();
        }

        public static Log4NetLogger Current
        {
            get
            {
                if (_current == null)
                    _current = new Log4NetLogger();
                return _current;
            }
        }


        public void WriteCritical(string message, Exception ex)
        {
            WriteCritical(message + Environment.NewLine + ex.ToString());
        }

        public void WriteCritical(string message)
        {
            logger.Fatal(message);
        }
        
        public void WriteError(string message, Exception ex)
        {
            WriteError(message + Environment.NewLine + ex.ToString());
        }

        public void WriteError(string message)
        {
            logger.Error(message);
        }
        
        public void WriteWarning(string message, Exception ex)
        {
            WriteWarning(message + Environment.NewLine + ex.ToString());
        }

        public void WriteWarning(string message)
        {
            logger.Warn(message);
        }
        
        public void WriteDebug(string message)
        {
            logger.Debug(message);
        }
    }
}