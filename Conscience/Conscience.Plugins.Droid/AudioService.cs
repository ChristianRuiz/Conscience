using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Media;
using Java.IO;

namespace Conscience.Plugins.Droid
{
    public class AudioService : IAudioService
    {
        private File _tmpFile;
        private MediaPlayer _player;
        private MediaPlayer Player
        {
            get
            {
                if (_player == null)
                {
                    _player = new MediaPlayer();
                    _player.Prepared += (e, args) =>
                    {
                        _player.Start();
                    };
                }

                return _player;
            }
        }
        
        
        public void PlaySound(System.IO.Stream stream)
        {
            Stop();

            var context = Application.Context;
            
            if (_tmpFile != null)
                _tmpFile.Delete();

            _tmpFile = File.CreateTempFile(Guid.NewGuid().ToString(), ".mp3", context.CacheDir);

            using (var fw = new System.IO.FileStream(_tmpFile.AbsolutePath, System.IO.FileMode.OpenOrCreate))
            {
                stream.CopyTo(fw);
                fw.Flush();
            }
            
            Player.SetDataSource(_tmpFile.AbsolutePath);
            Player.Prepare();
        }

        public void Stop()
        {
            if (Player.IsPlaying)
                Player.Stop();

            Player.Release();

            _player = null;
        }
    }
}