using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Conscience.Plugins
{
    public interface IAudioService
    {
        void PlaySound(Stream stream);
        void Stop();
    }
}
