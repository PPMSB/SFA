using Microsoft.Dynamics.BusinessConnectorNet;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNet.pod.Utils
{
    public class AxaptaTTSUtility
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public static void TryAbort(Axapta ax)
        {
            try
            {
                ax.TTSAbort();
            }
            catch (Exception ex)
            {
                logger.Warn(ex, "TTSAbort failed, possibly due to invalid session.");
            }
        }
    }
}