using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Bosch_ImportData
{
    public class PictureConvert
    {
        public static class IPictureDispConverter
        {
            [DllImport("OleAut32.dll", EntryPoint = "OleCreatePictureIndirect", ExactSpelling = true, PreserveSig = false)]
            private static extern stdole.IPictureDisp OleCreatePictureIndirect([MarshalAs(UnmanagedType.AsAny)] object picdesc, ref Guid iid, [MarshalAs(UnmanagedType.Bool)] bool fOwn);

            public static Bitmap ToBitmap(stdole.IPictureDisp picture)
            {
                Guid iid = typeof(stdole.IPictureDisp).GUID;
                object rawImage = OleCreatePictureIndirect(picture, ref iid, false);
                return (Bitmap)rawImage;
            }
        }
    }
}
