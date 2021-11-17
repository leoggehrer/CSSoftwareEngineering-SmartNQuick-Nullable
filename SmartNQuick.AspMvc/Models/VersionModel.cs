//@BaseCode
//MdStart
using System;
using System.Text;

namespace SmartNQuick.AspMvc.Models
{
    public class VersionModel : IdentityModel, Contracts.IVersionable
	{
		public byte[] RowVersion { get; set; }

        /// <summary>
        /// Gets and sets the the row stamp as string.
        /// </summary>
        public string RowVersionString
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                if (this.RowVersion != null)
                {
                    foreach (var item in this.RowVersion)
                    {
                        if (sb.Length > 0)
                            sb.Append('.');

                        sb.Append((int)item);
                    }
                }
                return sb.ToString();
            }
            set
            {
                if (value != null)
                {
                    string[] data = value.Split('.');
                    Byte[] ts = new byte[data.Length];

                    for (int i = 0; i < data.Length; i++)
                    {
                        ts[i] = Convert.ToByte(data[i]);
                    }
                    this.RowVersion = ts;
                }
            }
        }
    }
}
//MdEnd
