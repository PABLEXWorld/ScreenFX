using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace ScreenFX
{
    public partial class MainWindow : Form
    {

        public MainWindow()
        {
            InitializeComponent();
            Magnification.MagInitialize();
            Magnification.MagSetFullscreenTransform(1.001f, 0, 0);
            Magnification.MagSetFullscreenTransform(1, 0, 0);
            if (Magnification.IsBitmapSmoothingSupported())
            {
                if (checkBox3.Checked)
                {
                    Magnification.MagSetFullscreenUseBitmapSmoothing(1);
                }
            }
            else
            {
                checkBox3.Checked = false;
                checkBox3.Enabled = false;
            }
            beatinterval = 60000f / (float)numericUpDown1.Value;
            zoombase = (float)numericUpDown2.Value;
            baseTimer3Interval = timer3.Interval;
            timer3.Interval = baseTimer3Interval / (int)numericUpDown5.Value;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (checkBox3.Checked)
            {
                Magnification.MagSetFullscreenUseBitmapSmoothing(0);
            }
            Magnification.MagUninitialize();
        }

        private void SetZoom(float z)
        {
            zoom = z;
            if (timer3.Enabled)
            {
                if (zoom < 1.01f)
                {
                    zoom = zoomin;
                }
            }
            int x = Screen.PrimaryScreen.Bounds.Left;
            int y = Screen.PrimaryScreen.Bounds.Top;
            float w = Screen.PrimaryScreen.Bounds.Width;
            float h = Screen.PrimaryScreen.Bounds.Height;
            float w2 = w * zoom;
            float h2 = h * zoom;
            float w3 = w2 - w;
            float h3 = h2 - h;
            float w4 = w3 / zoom / 2;
            float h4 = h3 / zoom / 2;
            float w5 = w4;
            float h5 = h4;
            if (timer3.Enabled)
            {
                w5 += offsetx;
                h5 += offsety;
            }
            Magnification.MagSetFullscreenTransform(zoom, x + (int)w5, y + (int)h5);
        }

        private float QuadraticEaseOut(float t, float b, float c, float d)
        {
            t /= d;
            return -c * t * (t - 2) + b;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!stopped & !paused)
            {
                t = stopwatch1.ElapsedMilliseconds % beatinterval;
                float t2 = t / beatinterval;
                if (t2 > 1)
                {
                    t2 = 1;
                }
                zoom = QuadraticEaseOut(t2, zoombase, 1.0f - zoombase, 1);
                if (zoom > 1.01f)
                {
                    SetZoom(zoom);
                }
                else
                {
                    if (scheduledStop)
                    {
                        stopwatch1.Stop();
                        timer1.Stop();
                        timer1.Enabled = false;
                        scheduledStop = false;
                        stopped = true;
                    }
                    if (scheduledPause)
                    {
                        timer1.Stop();
                        timer1.Enabled = false;
                        scheduledPause = false;
                        paused = true;
                    }
                    zoom = 1f;
                    SetZoom(1f);
                }
                if (zoom > prevzoom & enableStepDisco)
                {
                    if (enableStepDisco)
                    {
                        grados += 90;
                        if (grados >= 360)
                        {
                            grados %= 360;
                        }
                        SetScreenEffect.SetScreenRot(grados);
                    }
                }
                prevzoom = zoom;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            grados += (float)timer2.Interval / 4f * (float)numericUpDown4.Value;
            if (grados >= 360)
            {
                grados %= 360;
            }
            SetScreenEffect.SetScreenRot(grados);
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            int w = SystemInformation.VirtualScreen.Width;
            int h = SystemInformation.VirtualScreen.Height;
            float w2 = w * zoomin;
            float h2 = h * zoomin;
            float w3 = w2 - w;
            float h3 = h2 - h;
            float w4 = w3 / zoomin / 2;
            float h4 = h3 / zoomin / 2;
            float w5 = w4 * (float)numericUpDown3.Value;
            float h5 = h4 * (float)numericUpDown3.Value;
            int w6 = (int)Math.Round(w5, 0, MidpointRounding.AwayFromZero);
            int h6 = (int)Math.Round(h5, 0, MidpointRounding.AwayFromZero);
            int w7 = (int)Math.Round(w4, 0, MidpointRounding.AwayFromZero);
            int h7 = (int)Math.Round(h4, 0, MidpointRounding.AwayFromZero);
            offsetx = rand.Next(w7 - w6, w7 + w6 + 2);
            offsety = rand.Next(h7 - h6, h7 + h6 + 2);
            if (stopped | paused)
            {
                Magnification.MagSetFullscreenTransform(zoomin, offsetx, offsety);
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            beatinterval = 60000f / (float)numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            zoombase = (float)numericUpDown2.Value;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                stopped = false;
                scheduledStop = false;
                timer1.Enabled = true;
                timer1.Start();
                stopwatch1.Restart();
            }
            else
            {
                scheduledStop = true;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                scheduledPause = true;
            }
            else
            {
                paused = false;
                scheduledPause = false;
                timer1.Enabled = true;
                timer1.Start();
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            Magnification.MagSetFullscreenUseBitmapSmoothing(checkBox3.Checked ? 1 : 0);
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                if (!checkBox6.Checked)
                {
                    timer2.Enabled = true;
                    timer2.Start();
                }
            }
            else
            {
                if (!checkBox6.Checked)
                {
                    timer2.Stop();
                    timer2.Enabled = false;
                }
                grados = 0;
                SetScreenEffect.SetScreenRot(grados);
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
           if (checkBox5.Checked)
            {
                timer3.Enabled = true;
                timer3.Start();
            }
            else
            {
                timer3.Stop();
                timer3.Enabled = false;
                offsetx = 0;
                offsety = 0;
                if (stopped | paused)
                {
                    Magnification.MagSetFullscreenTransform(1f, offsetx, offsety);
                }
            }
        }

        bool enableStepDisco;
        Stopwatch stopwatch1 = new Stopwatch();
        float beatinterval;
        bool scheduledStop = false;
        bool stopped = true;
        bool scheduledPause = false;
        bool paused = false;
        float zoom;
        float prevzoom;
        float t;
        float zoombase;
        const float zoomin = 1.0100001f;
        int baseTimer3Interval;
        int offsetx;
        int offsety;
        float grados = 0;
        Random rand = new Random();

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            timer3.Interval = baseTimer3Interval / (int)numericUpDown5.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            numericUpDown1.Value *= 2;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            numericUpDown1.Value /= 2;
        }

        bool opacitySet;

        private void trackBar4_ValueChanged(object sender, EventArgs e)
        {
            if (!opacitySet)
            {
                opacitySet = true;
                numericUpDown9.Value = trackBar4.Value;
                UpdateOpacity(trackBar4.Value);
            }
        }

        private void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {
            if (!opacitySet)
            {
                opacitySet = true;
                trackBar4.Value = (int)numericUpDown9.Value;
                UpdateOpacity((float)numericUpDown9.Value);
            }
        }

        private void UpdateOpacity(float op)
        {
            SetScreenEffect.SetScreenOp(op / 100f);
            opacitySet = false;
        }

        bool brightnessSet;

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (!brightnessSet)
            {
                brightnessSet = true;
                numericUpDown6.Value = trackBar1.Value;
                UpdateBrightness(trackBar1.Value);
            }
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            if (!brightnessSet)
            {
                brightnessSet = true;
                trackBar1.Value = (int)numericUpDown6.Value;
                UpdateBrightness((float)numericUpDown6.Value);
            }
        }

        private void UpdateBrightness(float bright)
        {
            SetScreenEffect.SetScreenBright((bright - 50f) / 50f);
            brightnessSet = false;
        }

        bool contrastSet;

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            if (!contrastSet)
            {
                contrastSet = true;
                numericUpDown7.Value = trackBar2.Value;
                UpdateContrast(trackBar2.Value);
            }
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            if (!contrastSet)
            {
                contrastSet = true;
                trackBar2.Value = (int)numericUpDown7.Value;
                UpdateContrast((float)numericUpDown7.Value);
            }
        }

        private void UpdateContrast(float con)
        {
            SetScreenEffect.SetScreenCon(con * 2f / 100f);
            contrastSet = false;
        }

        bool saturationSet;

        private void trackBar3_ValueChanged(object sender, EventArgs e)
        {
            if (!saturationSet)
            {
                saturationSet = true;
                numericUpDown8.Value = trackBar3.Value;
                UpdateSaturation(trackBar3.Value);
            }
        }

        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            if (!saturationSet)
            {
                saturationSet = true;
                trackBar3.Value = (int)numericUpDown8.Value;
                UpdateSaturation((float)numericUpDown8.Value);
            }
        }

        private void UpdateSaturation(float sat)
        {
            SetScreenEffect.SetScreenSat(sat * 2f / 100f);
            saturationSet = false;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
            {
                timer2.Stop();
                timer2.Enabled = false;
            }
            else if (checkBox4.Checked)
            {
                timer2.Enabled = true;
                timer2.Start();
            }
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            enableStepDisco = checkBox7.Checked;
            if (!checkBox7.Checked & !timer2.Enabled)
            {
                grados = 0;
                SetScreenEffect.SetScreenRot(grados);
            }
        }
    }
}
