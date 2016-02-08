using System;
using System.Windows.Media;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Wma;
using Un4seen.Bass.AddOn.Fx;
using Un4seen.Bass.Misc;
using Common;
using Status = Common.Common.Status;

namespace Player
{
    public class Player : IPlayer
    {
        private int _stream;
        private int _fxEQ;
        public Player()
        {
            BassNet.Registration("xxxddr3@gmail.com", "2X441017152222");
            if(!(Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero)))
            {
                throw new Exception("Unable to initialize instance of Player class");
            }
            SetBFX_EQ(_stream);
        }
        private void SetBFX_EQ(int channel)
        {
            _fxEQ = Bass.BASS_ChannelSetFX(channel, BASSFXType.BASS_FX_BFX_PEAKEQ, 0);
            BASS_BFX_PEAKEQ eq = new BASS_BFX_PEAKEQ();
            eq.fQ = 2f;
            eq.fBandwidth = 2.5f;
            eq.lChannel = BASSFXChan.BASS_BFX_CHANALL;

            float[] InsertedValues = new float[18]{31f,63f,87f,125f,175f,250f,350f,500f,700f,
                1000f,1400f,2000f,2800f,4000f,5600f,8000f,11200f,16000f};
            for (int i = 0; i < 18; ++i)
            {
                eq.lBand = i;
                eq.fCenter = InsertedValues[i];
                Bass.BASS_FXSetParameters(_fxEQ, eq);
                UpdateFX(i, 0f);
            }
        }
        private void UpdateFX(int band, float gain)
        {
            BASS_BFX_PEAKEQ eq = new BASS_BFX_PEAKEQ();
            eq.lBand = band;
            Bass.BASS_FXGetParameters(_fxEQ, eq);
            eq.fGain = gain;
            Bass.BASS_FXSetParameters(_fxEQ, eq);
        }
        public Status SetSource(Song playedSong)
        {
            if (playedSong.Downloaded)
            {
                _stream = Bass.BASS_StreamCreateFile(playedSong.DownloadedUri.LocalPath, 0, 0, BASSFlag.BASS_DEFAULT);
            }
            else
            {
                _stream = Bass.BASS_StreamCreateURL(playedSong.Uri.ToString(), 0, 0, null, new IntPtr(0));
            }
            if (_stream != 0)
            {
                return Status.OK;
            }
            else
            {
                if (playedSong.Downloaded)
                {
                    playedSong.Downloaded = false;
                    _stream = Bass.BASS_StreamCreateURL(playedSong.Uri.ToString(), 0, 0, null, new IntPtr(0));
                }
                if (_stream != 0)
                {
                    return Status.OK;
                }
                else
                {
                    return Status.Error;
                }
            }
        }
        public Status Play()
        {
            if(Bass.BASS_ChannelPlay(_stream, false))
            {
                return Status.OK;
            }
            else
            {
                return Status.Error;
            }
        }
        public Status Stop()
        {
            if (Bass.BASS_ChannelStop(_stream))
            {
                return Status.OK;
            }
            else
            {
                return Status.Error;
            }
        }
        public Status Pause()
        {
            if (Bass.BASS_ChannelPause(_stream))
            {
                return Status.OK;
            }
            else
            {
                return Status.Error;
            }
        }
        public Status AdjustSound()
        {
            return Status.OK;
        }
        ~Player()
        {
            Bass.BASS_StreamFree(_stream);
            Bass.BASS_Free();
        }
    }
}
