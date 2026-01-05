using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.src.Utils
{
    internal class AudioManagement
    {
        private SoundPlayer? _player;
        public bool IsPlaying { get; private set; } 

        public AudioManagement(string filePath)
        {
            IsPlaying = false;
            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    _player = new SoundPlayer(filePath);
                } else
                {
                    _player = null;
                }
            }
            catch { _player = null; }
        }

        public void Play()
        {
            if (_player != null)
            {
                try
                {
                    _player.PlayLooping();
                    IsPlaying = true;
                }
                catch { IsPlaying = false; }
            }
        }

        public void Stop()
        {
            if (_player != null)
            {
                _player.Stop();
                IsPlaying = false;
            }
        }

        // Hàm chuyển đổi trạng thái (Bật -> Tắt và ngược lại)
        public bool Toggle()
        {
            if (IsPlaying)
                Stop();
            else
                Play();

            return IsPlaying;
        }
    }
}
